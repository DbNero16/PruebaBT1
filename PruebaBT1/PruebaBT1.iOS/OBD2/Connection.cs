using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PruebaBT1.OBD2;
using System.Runtime.CompilerServices;

namespace PruebaBT1.Droid.OBD2
{
    /**class Connection : IConnection
    {
        
       // BluetoothSocket socket = null;
        
       
        //public DataResponse ConsultParameters(BluetoothSocket socket)
        
            /**
             * Los métodos de esta clase, una vez se acople este módulo al proyecto, estarán en hilos que se ejecutarán constamente mientras se encuentra uno en la 
             * opción de "Smart Monitoring" para evaluar en TR. Probablemente exista la necesidad de usar un semáforo o algo similar para evitar llamadas paralelas por el único socket
           
            

        public DataResponse ConsultParameters()
        {
            DataResponse result=ConsultParameters(Parameters.PID.RPM);
            return new OBD2.DataResponse(result.Response.ToString(), Parameters.PID.RPM, Parameters.ConsultMode.CurrentData);
            
        }

       // public DataResponse ConsultParameters(BluetoothSocket socket, Parameters.PID pid)
        public DataResponse ConsultParameters(Parameters.PID pid)
        {
            string result;
            string send = (Convert.ToUInt32(Parameters.ConsultMode.CurrentData).ToString("X2") + Convert.ToUInt32(pid).ToString("X2")+"/r");
            byte[] cmd = Encoding.ASCII.GetBytes(send);
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(1000);
            string data = Read();
            Thread.Sleep(1000);
            DataResponse dr = new DataResponse(data, pid, Parameters.ConsultMode.CurrentData);
            string speed = (Speed(dr).ToString());

            if (pid == Parameters.PID.Speed)
            {
                result = (Speed(dr)).ToString(); 
            }
            if(pid== Parameters.PID.RPM)
            {
                result = (RMP(dr)).ToString();
            }
            if (pid == Parameters.PID.EngineTemperature)
            {
                result = (EngineTemperature(dr)).ToString();
            }
            if (pid == Parameters.PID.FuelPressure)
            {
                result = (FuelPressure(dr)).ToString();
            }
            if (pid == Parameters.PID.ThrottlePosition)
            {
                result = (FuelPressure(dr)).ToString();
            }
            return new DataResponse(speed, pid, Parameters.ConsultMode.CurrentData);            
        }



        // public DataResponse DiagnosticCar(BluetoothSocket socket)
        public DataResponse DiagnosticCar()
        {

            DataResponse response = null;
            string send = (Convert.ToUInt32(Parameters.ConsultMode.DiagnosticTroubleCodes).ToString("X2"));
            byte[] cmd = Encoding.ASCII.GetBytes(send);
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(1000);
           
            string data = Read();
            Thread.Sleep(1000);
            if (data != null) { 
            socket.OutputStream.Flush();
            DataResponse dr = new DataResponse(data, Parameters.ConsultMode.CurrentData);
            List<DiagnosticTroubleCode>list=DiagnosticTroubleCodes(dr);
            string result = "";
            foreach(DiagnosticTroubleCode dtc in list)
            {
                result= result + "\n" + dtc.ToString();
            }
            response = new DataResponse(result, Parameters.ConsultMode.DiagnosticTroubleCodes);
                }
            return response;

        }

        /**
         * Serie de comandos de inicialización
         
        public  void Initialization(BluetoothSocket socket)
        {
            this.socket = socket;

            byte[] cmd = Encoding.ASCII.GetBytes("ATD" + "\r");
        
            Console.WriteLine("Enviando comando ATD");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                throw new System.Exception();
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);


            cmd = Encoding.ASCII.GetBytes("ATZ" + "\r");
            Console.WriteLine("Enviando comando ATZ");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                throw new System.Exception();
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);

            cmd = Encoding.ASCII.GetBytes("AT E0"+"/r");
            Console.WriteLine("Enviando comando AT E0");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);

            cmd = Encoding.ASCII.GetBytes("AT L0"+"/r");
            Console.WriteLine("Enviando comando AT L0");

            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);

            Console.WriteLine("Enviando comando AT S0");
            cmd = Encoding.ASCII.GetBytes("AT S0" + "/r");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);
            Console.WriteLine("Enviando comando AT H0");
            cmd = Encoding.ASCII.GetBytes("AT H0" + "/r");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            socket.OutputStream.Flush();
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);
            cmd = Encoding.ASCII.GetBytes("AT SP 0" + "/r");
            Console.WriteLine("Enviando comando AT SP 0");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(3000);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(3000);

        }

        /**
         * Método de lectura de la respuesta del dispositivo BT
         
        [MethodImpl(MethodImplOptions.Synchronized)]
        private  string Read()
        {
            //También probado con BufferedReader.ready() para comprobar si el stream está listo para leer; mismo resultado.


           
            string data = ""; 
            try
                
            {
                
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                char c;
                byte a;
                byte b = 0;
                //Este tipo de socket no soporta Timeouts

                if (socket.InputStream.IsDataAvailable())
                {
                    char[] chr = new char[100];
                    byte[] bytes = new byte[20];
                    
                    // socket.InputStream.Read(bytes,0,0);
                 //   if (!socket.InputStream.CanRead || !socket.InputStream.IsDataAvailable()) continue;
                    while (true)
                  //  for (int i = 0; i < bytes.Length; i++)
                    {
                        
                        a = (byte)socket.InputStream.ReadByte();
                        // a = buffer[i];
                       

                      c = (char)a;
                        if(b==47 && a == 114)
                        {
                            Thread.Sleep(2000);
                            data=Read();
                            
                        }
                        builder.Append(c);
                        if (c.Equals('\r')) //Comprobado \r es el último caracter, no llega a  ETX
                       {
                          
                            break;

                       }

                        b = a;
                       
                    }
                    data = builder.ToString();
                    Console.WriteLine("Información: " + data);

                    socket.InputStream.Flush();
                    



                }

                else
                {
                    Console.WriteLine("Input Stream no disponible");
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error de lectura :"+e.Message);
                
            }

            

            return data;


        }

        
         Serie de métodos que calculan los parámetros requeridos en base a la respuesta de OBDII
         
        public static uint Speed(DataResponse dr) {
            return (dr.Value.Length >= 1) ? Convert.ToUInt32(dr.Value.First()) : 0;
          }

        public static int EngineTemperature(DataResponse dr)
        {
            return (dr.Value.Length >= 1) ? Convert.ToInt32(dr.Value.First()) - 40 : 0;
        }

        public static uint RMP(DataResponse dr)
        {
            if (dr.Value.Length < 2)
            {
                throw new System.Exception();
            }

            uint rpm1 = Convert.ToUInt32(dr.Value.First());
            uint rpm2 = Convert.ToUInt32(dr.Value.ElementAt(1));
            return ((rpm1 * 256) + rpm2) / 4;
        }

        public static uint ThrottlePosition(DataResponse dr)
        {
            return (dr.Value.Length>=1) ? (Convert.ToUInt32(dr.Value.First()) * 100) / 255 : 0;
        }

        public static uint CalculatedEngineLoadValues(DataResponse dr)
        {
            return (dr.Value.Length >= 1) ? (Convert.ToUInt32(dr.Value.First()) * 100) / 255 : 0;
        }

        public static uint FuelPressure(DataResponse dr)
        {
            return (dr.Value.Length >= 1) ? Convert.ToUInt32(dr.Value.First()) * 3 : 0;
        }
        public static List<DiagnosticTroubleCode> DiagnosticTroubleCodes(DataResponse dr)
        {
            // issue the request for the actual DTCs
     
            var fetchedCodes = new List<DiagnosticTroubleCode>();
            for (int i = 1; i < dr.Value.Length; i += 3) // each error code got a size of 3 bytes
            {
                byte[] troubleCode = new byte[3];
                Array.Copy(dr.Value, i, troubleCode, 0, 3);

                if (!troubleCode.All(b => b == default(System.Byte))) 
                    fetchedCodes.Add(new DiagnosticTroubleCode(troubleCode));
            }

            return fetchedCodes;
        }
    }
}**/

}