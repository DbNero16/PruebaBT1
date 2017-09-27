using System;
using SQLite;

namespace PruebaBT1.BBDD
{
    public class FuelPressureData : Table
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int fuelPressure { get; set; }
        public DateTime CreatedOn { get; set; }

        public FuelPressureData() { }
    }
}
