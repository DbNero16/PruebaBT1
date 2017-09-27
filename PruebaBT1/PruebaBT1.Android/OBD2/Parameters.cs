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
            FuelSystemStatus=0x03,
            CalculatedEngineLoadValue = 0x04,
            EngineTemperature = 0x05,
            FuelTrim_Bank1_Short=0x06,
            FuelTrim_Banl1_Long = 0x07,
            FuelTrim_Bank2_Short = 0x08,
            FuelTrim_Bank2_Long = 0x09,
            FuelPressure = 0x0A,
            IntakeManifoldAbsolutePressure=0x0B,
            RPM = 0x0C,
            Speed = 0x0D,
            TimingAdvance=0x0E,
            IntakeTemperature=0x0F,
            ThrottlePosition = 0x11,
            OxygenSensor1=0x14,
            OxygenSensor2 = 0x15,
            OxygenSensor3 = 0x16,
            OxygenSensor4 = 0x17,
            OxygenSensor5 = 0x18,
            OxygenSensor6 = 0x19,
            OxygenSensor7 = 0x1A,
            OxygenSensor8 = 0x1B,
            EngineStartTime=0x1F,
            //Otras funcionalidades del sensor de oxigeno, considerar
            FuelTankLevel=0x2F,
            EvapSystemVaporPressure=0x32,
            AbsolutBarometricPressure=0x33,
            CatalystTemperature_Bank1_Sensor1=0x3C,
            CatalystTemperature_Bank2_Sensor1 = 0x3D,
            CatalystTemperature_Bank1_Sensor2 = 0x3E,
            CatalystTemperature_Bank2_Sensor2 = 0x3F,
            RelativeThrottlePosition=0x45,
            EngineOilTemperature=0x5C,




        };

        //Funciones

       
    }
}
