using System;

using Android.App;
using Android.Content.PM;

using Android.OS;
using Android.Content;
using Android.Bluetooth;



namespace PruebaBT1.Droid
{
    [Activity(Label = "PruebaBT1", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            BroadcastReceiverClass broadcastReceiver = new BroadcastReceiverClass();
            
            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(BluetoothDevice.ActionFound);
            intentFilter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            
            RegisterReceiver(broadcastReceiver, intentFilter);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

      
    }
}

