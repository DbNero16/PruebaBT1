using PruebaBT1.Droid;
using Android.Bluetooth;
using Xamarin.Forms;
using System.Collections.Generic;
using Android.Content;
using Android.App;
using System;
using System.Linq;
using Java.IO;
using Android.OS;
using PruebaBT1.Droid.OBD2;
using System.Threading.Tasks;
using System.Threading;

[assembly: Dependency(typeof(BluetoothAndroid))]
namespace PruebaBT1.Droid
{
    public class BluetoothAndroid : IBluetooth
    {
        
        static BluetoothAdapter bluetoothAdapter;
        static List<BluetoothDevice> discoveredDevices = new List<BluetoothDevice>();
        BluetoothSocket socket = null;
        static Connection OBD2Connection = null;
        BluetoothDevice device = null;


        /**
         * Método para obtener el "gestor" BT 
         * **/
        static public BluetoothAdapter getBluetoothAdapter()
        {
            return bluetoothAdapter;
        }

        /**
         * Método para comprobar que el BT está activo, y en caso contrario, solicitar activarlo
         * **/
        public bool IsOn()
        {
            bool res;
            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (bluetoothAdapter == null)
            {
                res = false;
            }
            else
            {
                if (!bluetoothAdapter.IsEnabled)
                {
                    Intent visible = new Intent(BluetoothAdapter.ActionRequestEnable);
                    ((Activity)Forms.Context).StartActivityForResult(visible, 0);
                    
                    res = true;
                   
                }
                else
                {
                    
                    res = true;
                }
            }

            return res;
        }


        /**
         * Método para lanzar el escaneo de dispositivos BT
         * **/
        public async Task<List<string>> scanDevices()
        {

            if (bluetoothAdapter.IsDiscovering)
            {
                bluetoothAdapter.CancelDiscovery();
            }


            bluetoothAdapter.StartDiscovery();
            System.Console.WriteLine("Comienza el descubrimiento");
            Thread.Sleep(2000);
            List<string> list = new List<string>();

            await Task.Delay(8000);
            foreach (BluetoothDevice currentDevice in discoveredDevices)
            {

             // if (currentDevice.Name.Equals("OBDII")){
                list.Add(currentDevice.Address.ToString());
               // }

            }

         
            return list;



        }
        /**
         * Método para  añadir un dispositivo descubierto a la lista
         * **/
        internal static void getScanDevices(BluetoothDevice bd)
        {
            discoveredDevices.Add(bd);
            discoveredDevices = discoveredDevices.Distinct().ToList();
        }

        /**
         * Método para comenzar una conexión con un dispositivo BT
         * **/
        public bool openConnection(String MAC)
        {
            if (bluetoothAdapter.IsDiscovering)
            {
                bluetoothAdapter.CancelDiscovery();
                System.Console.WriteLine("Sigue DESCUBRIENDO");
            }
            bluetoothAdapter.CancelDiscovery(); //para abrir conexion hay que parar https://developer.android.com/reference/android/bluetooth/BluetoothSocket.html#connect()
            device = bluetoothAdapter.GetRemoteDevice(MAC);


            if (device.BondState == Bond.None)
            {


                bondedDevice(device);



            }

            return Connect(device);
        }


        /**
         * Método para vincular un dispositivo BT al dispositivo móvil
         * **/
        public void bondedDevice(BluetoothDevice device)
        {
            bluetoothAdapter.CancelDiscovery(); //para abrir conexion hay que parar https://developer.android.com/reference/android/bluetooth/BluetoothSocket.html#connect()

            bool res = device.CreateBond();
            Thread.Sleep(10000);

           }


        /**
         * Método para establecer la conexión con el dispositivo BT
         * **/
        protected bool Connect(BluetoothDevice device)
        {
            
            ParcelUuid[] uuids = null;
            bool connected = false;
            if (device.FetchUuidsWithSdp())
            {
                uuids = device.GetUuids();
            }
            if ((uuids != null) && (uuids.Length > 0))
            {
                // Check if there is no socket already
                foreach (var uuid in uuids)
                {

                    // if (bs == null)
                    if (!connected)
                    {
                         try
                         {
                             socket = device.CreateRfcommSocketToServiceRecord(uuid.Uuid);
                            
                         }
                         catch (IOException ex)
                         {
                             throw ex;
                         }
                         //  }

                         try
                         {
                             System.Console.WriteLine("Attempting to connect...");

                             // Create a socket connection
                             socket.Connect();

                             connected = true;
                         }
                         catch
                         {
                             System.Console.WriteLine("Connection failed...");
                             connected = false;
                             System.Console.WriteLine("Attempting to connect...");
                        try
                        {
                            socket = device.CreateInsecureRfcommSocketToServiceRecord(uuid.Uuid);
                                socket.Connect();
                                
                            connected = true;
                        }
                        catch
                        {
                            System.Console.WriteLine("Connection failed...");
                            connected = false;

                        }
                    }
                }
            }



            }
            if (connected)
            {
                System.Console.WriteLine("Client socket is connected!");
                initializedOBD2();
                
            }
            return connected;
        }

        /**
         * Método para obtener un dispositivo reconocido por el móvil mediante su MAC
         * **/
        public string getDevice(string MAC)
        {
            return bluetoothAdapter.GetRemoteDevice(MAC).Name;
        }

        /**
         * Método para lanzar el diagnóstico
         * **/
        public string diagnostic()
        {
            if (OBD2Connection == null)
            {
                initializedOBD2();
            }


            return OBD2Connection.DiagnosticCar();

        }
        /**
         * Método para lanzar la consulta de parámetros
         * **/
        public void consultParameters()
        {
            if (OBD2Connection == null)
            {
                initializedOBD2();
            }
            OBD2Connection.ConsultParameters();
            // return OBD2Connection.ConsultParameters().Value.ToString();
          //  return OBD2Connection.ConsultParameters().Response.ToString() ;
             
            
        }

        /**
         * Método para lanzar la inicialización del OBDII
         * **/
        public void initializedOBD2()
        {
            OBD2Connection = new Connection();
           OBD2Connection.Initialization(socket);
        }


    }

}

