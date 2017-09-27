using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaBT1.OBD2
{
    class Parameters
    {
        public enum ConsultMode
        {
            Unknown = 0x00,
            CurrentData = 0x01,           
            DiagnosticTroubleCodes = 0x03
        }

        //Incluir Datos de Interés
        public enum PID
        {
            Unknown = 0x0,
            MIL = 0x01,
            DTCCount = 0x01,
            Speed = 0x0D,
            EngineTemperature = 0x05,
            RPM = 0x0C,
            ThrottlePosition = 0x11,
            CalculatedEngineLoadValue = 0x04,
            FuelPressure = 0x0A
        };

        //Funciones

       
    }
}
