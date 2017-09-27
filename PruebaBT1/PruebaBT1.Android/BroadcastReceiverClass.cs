using System;
using Android.Content;
using Android.Bluetooth;
using Android.App;

namespace PruebaBT1.Droid
{
    [BroadcastReceiver(Enabled = true, Label = "Receiver Label")]
    [IntentFilter(new[] { BluetoothDevice.ActionFound })]
    [IntentFilter(new[] { BluetoothAdapter.ActionDiscoveryFinished })]
    public class BroadcastReceiverClass : BroadcastReceiver
    {
        
        BluetoothDevice device;
        BluetoothAdapter ba = BluetoothAdapter.DefaultAdapter;
       

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;

            if (BluetoothDevice.ActionFound.Equals(action))
            {
                
                device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                Console.WriteLine(device.Name);
         // if (device.Name.Equals("OBDII"))
           //  {
                    
                   
                    BluetoothAndroid.getScanDevices(device);
            //}
            }
              
            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
                {

                Console.WriteLine("DescubrimientoFinalizado");
                //BluetoothAndroid.setDiscovery(true);
                }
            if (BluetoothAdapter.ActionDiscoveryStarted.Equals(action))
            {

                Console.WriteLine("DescubrimientoIniciado");
              //  BluetoothAndroid.setDiscovery(false);
            }

            if (BluetoothAdapter.ActionStateChanged.Equals(action))
            {
                if(!BluetoothAndroid.getBluetoothAdapter().IsEnabled)
                Console.Write("No se dispone de Bluetooth activo");
                return;
            }

         if (BluetoothDevice.ActionPairingRequest.Equals(action))
            {
                device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                int extraPairingVariant = intent.GetIntExtra(BluetoothDevice.ExtraPairingVariant, 0);
                int pin = intent.GetIntExtra(BluetoothDevice.ExtraPairingKey, 0);
                switch (extraPairingVariant)
                {
                    case BluetoothDevice.PairingVariantPin: // 0
                        TrySetPin(device, "1234");
                        //device.SetPairingConfirmation(false);
                        break;
                    case BluetoothDevice.PairingVariantPasskeyConfirmation: // 2
                                                                            //we don't care for this scenario
                        break;
                }
            }
               
            if (BluetoothAdapter.ActionConnectionStateChanged.Equals(action))
                {

                    Console.WriteLine("Llega a comprobar la acción de cambio");
                }
            if (BluetoothDevice.ActionAclConnected.Equals(action))
            {
                Console.WriteLine("Dispositivo conectado");
            }
            }

        private static bool TrySetPin(BluetoothDevice device, string pin)
        {
            try
            {
                return device.SetPin(PinToByteArray(pin));
            }
            catch
            {
                return false;
            }
        }

        private static byte[] PinToByteArray(string pin)
        {
            return System.Text.Encoding.UTF8.GetBytes(pin);
        }


    }
    }
