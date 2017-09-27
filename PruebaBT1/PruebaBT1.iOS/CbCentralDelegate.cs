/**using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PruebaBT1.iOS
{
   public class CbCentralDelegate : CBCentralManagerDelegate
    {
       public override void UpdatedState(CBCentralManager central)
        {
            if (central.State == CBCentralManagerState.PoweredOn)
            {
                System.Console.WriteLine("Powered On");
                //CBUUID[] cbuuids = null;
                central.ScanForPeripherals((CBUUID[]) null); //Initiates async calls of DiscoveredPeripheral
                //Timeout after 30 seconds
                var timer = new Timer(15*1000);
                timer.Elapsed += (sender, e) => central.StopScan();
            }
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            BluetoothIOS.getScanDevices(peripheral);
        }

        public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            base.FailedToConnectPeripheral(central, peripheral, error);
        }

        public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            base.DisconnectedPeripheral(central, peripheral, error);
        }

        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            // base.ConnectedPeripheral(central, peripheral);


            Console.WriteLine("ConnectedPeripheral: " + peripheral.Name);
            //central.ConnectPeripheral(peripheral);
        }

        

    }
}**/
