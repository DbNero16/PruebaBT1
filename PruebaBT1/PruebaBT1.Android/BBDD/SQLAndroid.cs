using Xamarin.Forms;
using PruebaBT1.Droid.BBDD;
using System.IO;
using SQLite;
using PruebaBT1.BBDD;

[assembly: Dependency(typeof(SQLAndroid))]
namespace PruebaBT1.Droid.BBDD
{

    class SQLAndroid : ISQLite
    {

        SQLiteConnection connection;
        public SQLiteConnection GetConnection()
        {
            var fileName = "SmartMonitoring.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
                     
            connection = new SQLiteConnection(path);
            if (connection != null)
            {
                initBBDD();
            }
            return connection;
        }

        //Estos métodos hay que ver en que clase deberían ir

        public void initBBDD()
        {
            connection.CreateTable<RPMData>();
            connection.CreateTable<DTCData>();
            connection.CreateTable<EngineTemperatureData>();
            connection.CreateTable<FuelPressureData>();
            connection.CreateTable<SpeedData>();
            connection.CreateTable<ThrottlePosition>();
        }

        
        
    }
}