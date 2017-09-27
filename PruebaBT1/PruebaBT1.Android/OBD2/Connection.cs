using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PruebaBT1.OBD2;
using Android.Bluetooth;
using Java.Lang;
using System.IO;
using System.Runtime.CompilerServices;
using PruebaBT1.Droid.BBDD;
using SQLite;
using PruebaBT1.BBDD;

namespace PruebaBT1.Droid.OBD2
{
    class Connection : IConnection
    {
        
        BluetoothSocket socket = null;
        SQLAndroid sql;
        SQLiteConnection dataBase = null;

        public Connection()
        {
            sql = new SQLAndroid();
            dataBase=sql.GetConnection();
        }
        //public DataResponse ConsultParameters(BluetoothSocket socket)
        
            /**
             * Los métodos de esta clase, una vez se acople este módulo al proyecto, estarán en hilos que se ejecutarán constamente mientras se encuentra uno en la 
             * opción de "Smart Monitoring" para evaluar en TR. Probablemente exista la necesidad de usar un semáforo o algo similar para evitar llamadas paralelas por el único socket
           
             * */

        public void ConsultParameters()
        {
            //DataResponse result=
            //ConsultParameters(Parameters.PID.RPM);
            //new Thread(ConsultParametersThread).Start();

            Thread t = new Thread(ConsultParametersThread);
            t.Start();
        }

        public void ConsultParametersThread() {
            while (true)
            {
                ConsultParameters(Parameters.PID.Speed);
                ConsultParameters(Parameters.PID.RPM);
                ConsultParameters(Parameters.PID.EngineTemperature);
                ConsultParameters(Parameters.PID.FuelPressure);
                ConsultParameters(Parameters.PID.ThrottlePosition);
                ConsultParameters(Parameters.PID.CalculatedEngineLoadValue);

            //    int RPM= dataBase.ExecuteScalar<int>("SELECT rpm FROM RPMData ORDER BY ID DESC LIMIT 1");
                
                //Console.WriteLine("RPM Actuales " + RPM);
                
            }
        }


       // public DataResponse ConsultParameters(BluetoothSocket socket, Parameters.PID pid)
        public void ConsultParameters(Parameters.PID pid)
        {

            //En alternativa (void), aqui se guardarían los resultados en la base de datos
            string result = "";
            string send = (Convert.ToUInt32(Parameters.ConsultMode.CurrentData).ToString("X2") + Convert.ToUInt32(pid).ToString("X2")+"/r");
            byte[] cmd = Encoding.ASCII.GetBytes(send);
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            
            string data = Read();
            string dataFilter = "";
            for(int i = 0; i < data.Length; i++)
            {
                byte a = (byte)data[i];
                if (a == 13)
                {
                    break;
                }
                dataFilter = dataFilter + data[i];

            }         
            

            DataResponse dr = new DataResponse(dataFilter, pid, Parameters.ConsultMode.CurrentData);
          

            if (pid == Parameters.PID.Speed)
            {
                result = (Speed(dr)).ToString(); 
                
            } else 
            if(pid== Parameters.PID.RPM)
            {
                result = (RMP(dr)).ToString();
            }
            else
            if (pid == Parameters.PID.EngineTemperature)
            {
                result = (EngineTemperature(dr)).ToString();
            }
            else
            if (pid == Parameters.PID.FuelPressure)
            {
                result = (FuelPressure(dr)).ToString();
            }
            else
            if (pid == Parameters.PID.ThrottlePosition)
            {
                result = (FuelPressure(dr)).ToString();
            }
            
            Thread.Sleep(200);
            // return new DataResponse(result, pid, Parameters.ConsultMode.CurrentData);            
        }



        // public DataResponse DiagnosticCar(BluetoothSocket socket)
        public string DiagnosticCar()
        {
            DataResponse response = null;
            string send = (Convert.ToUInt32(Parameters.ConsultMode.DiagnosticTroubleCodes).ToString("X2"));
            byte[] cmd = Encoding.ASCII.GetBytes(send);
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(1000);
            string result = "";
            string data = Read();
            Thread.Sleep(1000);
            if (data != null)
            {
                socket.OutputStream.Flush();
                DataResponse dr = new DataResponse(data, Parameters.ConsultMode.CurrentData);
                List<DiagnosticTroubleCode> list = DiagnosticTroubleCodes(dr);
                
                foreach (DiagnosticTroubleCode dtc in list)
                {
                    result = result + "\n" + dtc.ToString();
                }
                response = new DataResponse(result, Parameters.ConsultMode.DiagnosticTroubleCodes);
            }
           return result;

        }

        /**
         * Serie de comandos de inicialización
         * */
        public  void Initialization(BluetoothSocket socket)
        {
            this.socket = socket;

            byte[] cmd = Encoding.ASCII.GetBytes("ATD" + "\r");

            Console.WriteLine("Enviando comando ATD");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                throw new System.Exception();
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);


            cmd = Encoding.ASCII.GetBytes("ATZ" + "\r");
            Console.WriteLine("Enviando comando ATZ");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                throw new System.Exception();
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);

            cmd = Encoding.ASCII.GetBytes("AT E0" + "\r");
            Console.WriteLine("Enviando comando AT E0");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);

            cmd = Encoding.ASCII.GetBytes("AT L0" + "\r");
            Console.WriteLine("Enviando comando AT L0");

            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);

            Console.WriteLine("Enviando comando AT S0");
            cmd = Encoding.ASCII.GetBytes("AT S0" + "\r");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);
            Console.WriteLine("Enviando comando AT H0");
            cmd = Encoding.ASCII.GetBytes("AT H0" + "\r");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            socket.OutputStream.Flush();
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);

            //Estandar de operación; 0 Automático, A se corresponde con las operaciones de J1939
            cmd = Encoding.ASCII.GetBytes("AT SP 0" + "\r");
            Console.WriteLine("Enviando comando AT SP 0");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);

            cmd = Encoding.ASCII.GetBytes("0100" + "\r");
            Console.WriteLine("Enviando comando AT SP 0");
            socket.OutputStream.Write(cmd, 0, cmd.Length);
            Thread.Sleep(100);
            if (Read().Equals(""))
            {
                //return;
            }
            socket.OutputStream.Flush();
            Thread.Sleep(100);
        }

        /**
         * Método de lectura de la respuesta del dispositivo BT
         * */
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
                        Console.WriteLine(a);
                        if (a < -1)
                        {
                            break;
                        }

                        c = (char)a;
                       if (c.Equals('>')) 
                        {

                            break;

                        }

                      

                    }
                    if(data.Equals("UNABLE TO CONNECT"))
                    {
                        throw new System.Exception();
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
                Console.WriteLine("Error de lectura :" + e.Message);

            }



            return data;


        }

        /**
         * Serie de métodos que calculan los parámetros requeridos en base a la respuesta de OBDII
         * */
        public int Speed(DataResponse dr) {
            // throw new NotImplementedException();
            string data = dr.Response.Substring(dr.Response.Length - 1);
            
            int numSpeed = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            var rowAdded=dataBase.Insert(new SpeedData()
            {
                CreatedOn = DateTime.Now,
                speed = numSpeed

            });
            Console.WriteLine("Velocidad añadida: " + numSpeed + " rowAdded: " + rowAdded) ;
            return numSpeed;
            // return (dr.Value.Length >= 1) ? Convert.ToUInt32(dr.Value.First()) : 0;
        }

        public  int EngineTemperature(DataResponse dr)
        {
            //throw new NotImplementedException();
            string data = dr.Response.Substring(dr.Response.Length - 1);
            
            int engineTemperature = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            engineTemperature= engineTemperature - 40;
            var rowAdded = dataBase.Insert(new EngineTemperatureData()
            {
                CreatedOn = DateTime.Now,
                temperature = engineTemperature

            });
            Console.WriteLine("Temperatura añadida: " + engineTemperature + " rowAdded: " + rowAdded);
            return engineTemperature;
            //return (dr.Value.Length >= 1) ? Convert.ToInt32(dr.Value.First()) - 40 : 0;
        }

        public  int RMP(DataResponse dr)
        {
            /*  if (dr.Value.Length < 2)
              {
                  throw new System.Exception();
              }

              uint rpm1 = Convert.ToUInt32(dr.Value.First());
              uint rpm2 = Convert.ToUInt32(dr.Value.ElementAt(1));
              return ((rpm1 * 256) + rpm2) / 4;**/
            string data=dr.Response.Substring(dr.Response.Length - 3);
            string rpm1 = "";
            string rpm2 = "";
            

            for(int i = 0; i < data.Length; i++)
            {
                if (i <= 1)
                {
                    rpm1 = rpm1 + data.ElementAt(i);
                }
                else
                {
                    rpm2 = rpm2 + data.ElementAt(i);
                }
            }

            int numRpm1 = Int32.Parse(rpm1, System.Globalization.NumberStyles.HexNumber);
            int numRpm2 = Int32.Parse(rpm2, System.Globalization.NumberStyles.HexNumber);
            
            int res=(256*numRpm1+numRpm2)/ 4;

            var rowAdded = dataBase.Insert(new RPMData()
            {
                CreatedOn = DateTime.Now,
                rpm = res
                
            });
            Console.WriteLine("RMP añadida: " + res + " rowAdded: " + rowAdded);
            return res;

        }

        public  double ThrottlePosition(DataResponse dr)
        {
            string data = dr.Response.Substring(dr.Response.Length - 1);

            int throttlePosition = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            double throttlePositionRes = throttlePosition * 100 / 255;


            var rowAdded = dataBase.Insert(new ThrottlePosition()
            {
                CreatedOn = DateTime.Now,
                throttlePosition = throttlePositionRes

            });
            Console.WriteLine("ThrottlePosition añadida: " + throttlePosition + " rowAdded: " + rowAdded);
            return throttlePositionRes;
            // return (dr.Value.Length>=1) ? (Convert.ToUInt32(dr.Value.First()) * 100) / 255 : 0;
        }

        public  double CalculatedEngineLoadValues(DataResponse dr)
        {
            string data = dr.Response.Substring(dr.Response.Length - 1);

            int calculatedEngineLoad = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            double calculatedEngineLoadRes = calculatedEngineLoad * 100 / 255;
            var rowAdded = dataBase.Insert(new CalculatedEngineLoadValuesData()
            {
                CreatedOn = DateTime.Now,
                calculatedEngineLoadValue = calculatedEngineLoadRes

            });
            Console.WriteLine("EngineLoad añadida: " + calculatedEngineLoadRes + " rowAdded: " + rowAdded);
            return calculatedEngineLoadRes;


            // return (dr.Value.Length >= 1) ? (Convert.ToUInt32(dr.Value.First()) * 100) / 255 : 0;
        }

        public  int FuelPressure(DataResponse dr)
        {
            // throw new NotImplementedException();
            string data = dr.Response.Substring(4);

            int fuelPressure = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            fuelPressure = fuelPressure * 3;


            var rowAdded = dataBase.Insert(new FuelPressureData()
            {
                CreatedOn = DateTime.Now,
                fuelPressure = fuelPressure

            });
            Console.WriteLine("FuelPressure añadida: " + fuelPressure + " rowAdded: " + rowAdded);

            return fuelPressure;
            //return (dr.Value.Length >= 1) ? Convert.ToUInt32(dr.Value.First()) * 3 : 0;
        }
        public  List<DiagnosticTroubleCode> DiagnosticTroubleCodes(DataResponse dr)
        {
            throw new NotImplementedException();
            // issue the request for the actual DTCs

           /* var fetchedCodes = new List<DiagnosticTroubleCode>();
            for (int i = 1; i < dr.Value.Length; i += 3) // each error code got a size of 3 bytes
            {
                byte[] troubleCode = new byte[3];
                Array.Copy(dr.Value, i, troubleCode, 0, 3);

                if (!troubleCode.All(b => b == default(System.Byte))) 
                    fetchedCodes.Add(new DiagnosticTroubleCode(troubleCode));
            }

            return fetchedCodes;*/
        }
    }
}

