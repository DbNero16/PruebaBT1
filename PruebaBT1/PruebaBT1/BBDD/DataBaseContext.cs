using System;
using Xamarin.Forms;
using SQLite;
namespace PruebaBT1.BBDD
{
    class DataBaseContext
    {

        public SQLiteConnection GetConnection()
        {
            return DependencyService.Get<ISQLite>().GetConnection();
        }
        public bool CreateDataBase()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.CreateTable<RPMData>();
                    connection.CreateTable<DTCData>();
                    connection.CreateTable<EngineTemperatureData>();
                    connection.CreateTable<FuelPressureData>();
                    connection.CreateTable<SpeedData>();
                    connection.CreateTable<ThrottlePosition>();
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
