
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;



namespace PruebaBT1
{
    public partial class MainPage : ContentPage
    {
        List<string> devices;


        public MainPage()
        {
            InitializeComponent();
            InitializeBluetooth();
            
        }




        //INICIALIZAR
        private void InitializeBluetooth()
        {
            bool res = false;


            var typeB = DependencyService.Get<IBluetooth>();

            res = typeB.IsOn();

            if (res == false)
            {
                DisplayAlert("Title", "El dispositivo no dispone de BT", "OK");

            }
            else
            {
                // DisplayAlert("Title", "BT active", "OK");
                //scanDevices();
            }



        }
        //CLICKED
        private async void scanDevices(object sender1, EventArgs e1)
        {

            var scan = DependencyService.Get<IBluetooth>();
            //como hacer que espere
            devices = await scan.scanDevices();
            devices.Distinct().ToString();
            var listView = new ListView(ListViewCachingStrategy.RecycleElement);
            listView.ItemsSource = devices.Distinct().ToList();

            //   listView.ItemSelected += (object sender, ItemClickEventArgs e) => { String selectedFromList = lv.GetItemAtPosition(e.Position); };
            // Content = listView;
            if (devices.Count != 0)
            {
                // DisplayAlert("Title", devices.ElementAt(0), "OK");
                Content = Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = { listView }
                };
            }
            else
            {
                await DisplayAlert("Title", "No hay dispositivos", "OK");
            }
            listView.ItemSelected += (sender, e) => openConnectionDevice(e.SelectedItem.ToString());
        }

        private void openConnectionDevice(String MAC)
        {

            var scan = DependencyService.Get<IBluetooth>();
            bool result = scan.openConnection(MAC);
            if (result == true)
            {
                inConnection(MAC);

            } else
            {
                DisplayAlert("Title", "No se estableció la conexión con el dispositivo", "OK");
                var listView = new ListView(ListViewCachingStrategy.RecycleElement);
                listView.ItemsSource = devices;

                Content = Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = { listView }
                };
            }


        }

        private void inConnection(String MAC)
        {
            var scan = DependencyService.Get<IBluetooth>();
            var database = DependencyService.Get<ISQLite>();
            database.GetConnection();
            database.initBBDD();
            Label label = new Label();
            string nameDevice = scan.getDevice(MAC);
            // scan.initializedOBD2();
            Button consultTR = new Button();
            consultTR.Text = " Consultar Parámetros";
            consultTR.Clicked += (sender, e) => consultParameters();
            Button diagnostic = new Button();
            diagnostic.Text = "Diagnóstico";
            diagnostic.Clicked += (sender, e) => diagnosticCar();
            label.Text = ("Connected to " + nameDevice + " MAC: " + MAC);
            Content = Content = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { label, consultTR, diagnostic }
            };
        }

        private void consultParameters()
        {
            var scan = DependencyService.Get<IBluetooth>();
            // string parameter = scan.consultParameters();
            scan.consultParameters();
            var database = DependencyService.Get<ISQLite>();
            var connection=  database.GetConnection();
            scan.consultParameters();

            while (true)
            {
                
                int speed= connection.ExecuteScalar<int>("SELECT speed FROM SpeedData ORDER BY ID DESC LIMIT 1");
                int rpm = connection.ExecuteScalar<int>("SELECT rpm FROM RPMData ORDER BY ID DESC LIMIT 1");
                /*int rpm = connection.Execute("SELECT rpm FROM RPMData ORDER BY ID DESC LIMIT 1");
                int EngineTemperature = connection.Execute("SELECT rpm FROM RPMData ORDER BY ID DESC LIMIT 1");
                int fuelPresurre = connection.Execute("SELECT rpm FROM RPMData ORDER BY ID DESC LIMIT 1");
                int throttlePosition = connection.Execute("SELECT throttlePosition FROM ThrottlePosition ORDER BY ID DESC LIMIT 1");*/


            }
            

           // DisplayAlert("Consult Parameters", "Su velocidad es"+parameter, "OK");
        
    }

        private void diagnosticCar()
        {
            var scan = DependencyService.Get<IBluetooth>();
            string troubles = scan.diagnostic();
            var array = troubles.ToCharArray();
            List<string> troublesCode = new List<string>();
            string res = "";
            string espacio = " ";
            for (int i = 0; i < array.Length; i++)
            {
                var current = array.ElementAt(i);
                bool esEspacio = current.ToString().Equals(espacio);
                if (!esEspacio)
                {
                    res = (res + current.ToString());
                }
                else
                {
                    troublesCode.Add(res);
                    res = "";
                }
            }
            if (troublesCode.Count == 0)
            {
                DisplayAlert("Trouble Code", "No se encuentran errores en su ECU", "OK");
            }
            else
            {
                var listView = new ListView(ListViewCachingStrategy.RecycleElement);
                listView.ItemsSource = troublesCode;
                Content = Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = { listView }
                };
            }
        }
    }
}




