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
using System.Text.RegularExpressions;

namespace PruebaBT1.Droid.OBD2
{
    class DataResponse
    {
        string response;
        private Parameters.PID pid;
        private Parameters.ConsultMode mode;

        public string Response
        {
            get
            {
                return response;
            }

            set
            {
                response = value;
            }
        }

        internal Parameters.PID Pid
        {
            get
            {
                return pid;
            }

            set
            {
                pid = value;
            }
        }

        internal Parameters.ConsultMode Mode
        {
            get
            {
                return mode;
            }

            set
            {
                mode = value;
            }
        }



        public DataResponse (string response, Parameters.PID pid, Parameters.ConsultMode mode)
        {
            this.Response = response;
            this.Pid = pid;
            this.Mode = mode;
        }
        public DataResponse(string response,  Parameters.ConsultMode mode)
        {
            this.Response = response;
            this.Mode = mode;
        }

        /*public Byte[] Value
        {
            get
            {
                byte[] bArray = null;
                if (Pid != Parameters.PID.Unknown && Mode != Parameters.ConsultMode.Unknown)
                {
                    Match matchedPattern = Regex.Match(Response, @"\n([0-9a-fA-F ]{5})([0-9a-fA-F ]+)\r\n>");
                    if (matchedPattern.Groups.Count > 2)
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Groups[2].Value.Replace("", ""));
                    }
                    else
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Value);

                    }
                }
                else if (Pid == Parameters.PID.Unknown)
                {
                    Match matchedPattern = Regex.Match(Response, @"\n([0-9a-fA-F]{2})([0-9a-fA-F ]+)\r\n>");
                    if (matchedPattern.Groups.Count > 2)
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Groups[2].Value.Replace("", ""));
                    }
                    else
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Value);
                    }


                }
                else
                {
                    Match matchedPattern = Regex.Match(Response, @"\n([0-9a-fA-F ]+)\r\n>");

                    if (matchedPattern.Groups.Count > 1)
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Groups[1].Value.Replace(" ", ""));
                    }
                    else
                    {
                        bArray = Encoding.ASCII.GetBytes(matchedPattern.Value);
                    }
                }
                return bArray;
            }*/
        }
    }
