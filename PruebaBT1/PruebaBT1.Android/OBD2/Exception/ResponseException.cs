﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PruebaBT1.Droid.OBD2.Exception
{
    public class ResponseException
    {
        private string message;

        protected ResponseException(string message)
        {
            this.message = message;
        }
    }
}