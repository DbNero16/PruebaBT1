using System;
using System.Collections.Generic;
using CoreBluetooth;
using CoreFoundation;
using Foundation;
using System.Linq;
using ExternalAccessory;
using System.Threading.Tasks;
using PruebaBT1.Droid.OBD2;
namespace PruebaBT1.iOS
{
    class BluetoothIOS : IBluetooth
    {
        CBCentralManager bluetoothManager;
        static readonly CBUUID PeripheralUUID = CBUUID.FromPartial(0x180D);
        static CBPeripheral CurrentPeripheral = null;
        static List<CBPeripheral> discoveredDevices = new List<CBPeripheral>();
        static List<CBPeripheral> retrievedDevices = new List<CBPeripheral>();
        EAAccessoryManager ea = EAAccessoryManager.SharedAccessoryManager;
        //CbCentralDelegate centralDelegate;
      

        public bool IsOn()
        {
            //throw new NotImplementedException();
               bluetoothManager = new CBCentralManager(DispatchQueue.DefaultGlobalQueue);
            /**                                             new CBCentralInitOptions { ShowPowerAlert = true });
             * 
             *
           centralDelegate = new CbCentralDelegate();
           bluetoothManager.UpdatedState += (object sender, EventArgs e) => centralDelegate.UpdatedState(bluetoothManager);
           bluetoothManager.DiscoveredPeripheral += (object sender, CBDiscoveredPeripheralEventArgs e) => centralDelegate.DiscoveredPeripheral(bluetoothManager, e.Peripheral, e.AdvertisementData , e.RSSI);
           bluetoothManager.ConnectedPeripheral += (object sender, CBPeripheralEventArgs e) => centralDelegate.ConnectedPeripheral(bluetoothManager, e.Peripheral);
           bluetoothManager.FailedToConnectPeripheral += (object sender, CBPeripheralErrorEventArgs e) => centralDelegate.FailedToConnectPeripheral(bluetoothManager, e.Peripheral, e.Error);
           bluetoothManager.DisconnectedPeripheral += (object sender, CBPeripheralErrorEventArgs e) => centralDelegate.DisconnectedPeripheral(bluetoothManager, e.Peripheral, e.Error);**/
            this.bluetoothManager.RetrievedPeripherals += CentralManagerOnRetrievedPeripherals;
            this.bluetoothManager.DiscoveredPeripheral += CentralManagerOnDiscoveredPeripheral;
            this.bluetoothManager.UpdatedState += CentralManagerOnUpdatedState;
            return bluetoothManager.State == CBCentralManagerState.PoweredOn;
        }

        private void CentralManagerOnUpdatedState(object sender, EventArgs e)
        {
            if (bluetoothManager.State == CBCentralManagerState.PoweredOn)
            {
                //var options = new NSDictionary(CBCentralManager.ScanOptionAllowDuplicatesKey, true);
                bluetoothManager.ScanForPeripherals(null, new PeripheralScanningOptions { AllowDuplicatesKey = true });
            }
        }

        private void CentralManagerOnDiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            discoveredDevices.Add(e.Peripheral);
            if (e.AdvertisementData.ContainsKey(CBAdvertisement.DataManufacturerDataKey))
            {
                // For debugging
                var data = e.AdvertisementData[CBAdvertisement.DataManufacturerDataKey];
            }
            Console.WriteLine(e.AdvertisementData);
        }

        private void CentralManagerOnRetrievedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            foreach(CBPeripheral p in e.Peripherals)
            {
                if (!retrievedDevices.Contains(p))
                {
                    retrievedDevices.Add(p);
                    Console.WriteLine(p.Name);
                }
            }
            Console.WriteLine("Retrieved Peripherals");
        }

        internal static void getScanDevices(CBPeripheral cbp)
        {

            CurrentPeripheral = cbp;
           discoveredDevices.Add(CurrentPeripheral);
            discoveredDevices = discoveredDevices.Distinct().ToList();
        }


        public async Task<List<string>> scanDevices()
        {
            //throw new NotImplementedException();
            //ea.ShowBluetoothAccessoryPicker(null, null);
            Console.WriteLine("Start scan request!");
            var u = CBUUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
            var a = new CBUUID[] { u };
            if (!bluetoothManager.IsScanning)
            {
                //bluetoothManager.StopScan();

                bluetoothManager.RetrievePeripherals(a);
                PeripheralScanningOptions option = new PeripheralScanningOptions(NSDictionary.FromObjectAndKey(CBCentralManager.ScanOptionSolicitedServiceUUIDsKey, CBUUID.FromString("00001101-0000-1000-8000-00805F9B34FB")));
                bluetoothManager.ScanForPeripherals(a, option);
            }
            await Task.Delay(8000);

            /** bluetoothManager.ScanForPeripherals(null, option);
             List<string> devices = new List<string>();
             bluetoothManager.RetrievePeripherals(CBUUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));


             bluetoothManager.DiscoveredPeripheral += (sender, e) =>
              {

                  var name = e.Peripheral.UUID;
                  string uuid = name.ToString();
                  devices.Add(uuid);
                  if (e.AdvertisementData.ContainsKey(CBAdvertisement.DataLocalNameKey))
                  {
                     // iOS caches the peripheral name, so it can become stale (if changing)
                     // keep track of the local name key manually
                     name = ((NSString)e.AdvertisementData.ValueForKey(CBAdvertisement.)).ToString();

                  }
              };**/
            List<string> ids = new List<string>();
            /**foreach (CBUUID uuid in a)
            {
                ids.Add(uuid.ToString());
            }***/
            foreach(CBPeripheral p in retrievedDevices)
            {
                ids.Add(p.UUID.ToString());
            }
            return ids;


        }

        public bool openConnection(string ids)
        {
            throw new NotImplementedException();
#pragma warning disable CS0162 // Se ha detectado código inaccesible
            foreach (CBPeripheral p in discoveredDevices)
#pragma warning restore CS0162 // Se ha detectado código inaccesible
            {
                if (p.UUID.Equals(CBUUID.FromString(ids)))
                {
                    CurrentPeripheral = p;
                }
            }
            if (CurrentPeripheral != null)
            {
                bluetoothManager.ConnectPeripheral(CurrentPeripheral);
            }
            //    List<CBPeripheral> peri= bluetoothManager.RetrieveConnectedPeripherals();
         //  CBPeripheral peripheral = bluetoothManager.RetrievePeripherals(CBUUID.FromString(ids));
            //bluetoothManager.ConnectPeripheral
        }

        public string getDevice(string MAC)
        {
            throw new NotImplementedException();
        }

        public string diagnostic()
        {
            throw new NotImplementedException();
        }

        public string consultParameters()
        {
            throw new NotImplementedException();
        }

        public void initializedOBD2()
        {
            Connection OBD2Connection = new Connection();
            //OBD2Connection.Initialization(socket);
            
            
        }

       

        /** public async Task<bool> SetupTransport()
         {
             Console.WriteLine("Setting up transport!");

             //Wait for state update...
             if (await WaitForCentralState(CBCentralManagerState.PoweredOn, TimeSpan.FromSeconds(2)))
             {

                 //Failed detecting a powered-on BLE interface
                 Console.WriteLine("Failed detecting usable BLE interface!");
                 return false;
             }

             //Kick scanning... 
             StartScanning();

             //Wait for discovery and connection to a valid peripheral, or timeout trying...
             await Task.Delay(10000);

             return true;
         }**/
    }
    }
