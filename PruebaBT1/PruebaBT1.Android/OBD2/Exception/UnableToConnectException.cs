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
using PruebaBT1.Droid.OBD2.Exception;

namespace PruebaBT1.Droid.OBD2
{
    class UnableToConnectException : ResponseException
    {
        public UnableToConnectException(string message) : base("UNABLE TO CONNECT")
        {
        }
    }
}