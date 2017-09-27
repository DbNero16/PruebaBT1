using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PruebaBT1
{
   public interface IBluetooth
    {
       bool IsOn();
        Task<List<string>> scanDevices();

      bool openConnection(String MAC);

        string getDevice(String MAC);

        string diagnostic();

        void consultParameters();

        void initializedOBD2();

    }
}
