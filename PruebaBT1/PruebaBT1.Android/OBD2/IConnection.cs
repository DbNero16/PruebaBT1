using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PruebaBT1.OBD2;
using Android.Bluetooth;
using System.Threading.Tasks;

namespace PruebaBT1.Droid.OBD2
{
    interface IConnection
    {
        /*DataResponse ConsultParameters( Parameters.PID pid);

        DataResponse  ConsultParameters();
        DataResponse  DiagnosticCar();*/

        void ConsultParameters( Parameters.PID pid);

        void  ConsultParameters();
       string  DiagnosticCar();
        void Initialization(BluetoothSocket socket);
    }
}