using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PruebaBT1.UWP.Bluetooth))]
namespace PruebaBT1.UWP
{
    public class Bluetooth : IBluetooth
    {
        public bool IsOn()
        {
            
            return true;
        }
    }
}
