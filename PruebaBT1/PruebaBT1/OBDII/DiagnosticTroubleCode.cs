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

namespace PruebaBT1.Droid.OBD2
{
    class DiagnosticTroubleCode 
    {
        public enum Type
        {
            Powertrain = 0x00,
            Chassis = 0x01,
            Body = 0x02,
            Network = 0x03
        }

        public Byte[] Code { get; set; }

        public string TextRepresentation
        {
            get
            {
                string representation = "";
                switch (ErrorType)
                {
                    case Type.Powertrain: { representation = "P"; break; }
                    case Type.Chassis: { representation = "C"; break; }
                    case Type.Body: { representation = "B"; break; }
                    case Type.Network: { representation = "U"; break; }
                }

                Byte firstByte = Code.First();
                representation += (firstByte >> 4) & 3;
                representation += Convert.ToInt32(((firstByte >> 0) & 15).ToString(), 16);

                Byte secondByte = Code.ElementAt(1);
                representation += Convert.ToInt32(((secondByte >> 4) & 15).ToString(), 16);
                representation += Convert.ToInt32(((secondByte >> 0) & 15).ToString(), 16);

                return representation;
            }
        }

        public Type ErrorType
        {
            get
            {
                Byte firstByte = Code.First();
                return (Type)((firstByte >> 6) & 3);
            }
        }

        public DiagnosticTroubleCode(string code)
        {
            Code = Encoding.Unicode.GetBytes(code);
        }

        public DiagnosticTroubleCode(Byte[] code)
        {
            Code = code;
        }
    }

}
