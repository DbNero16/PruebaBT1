using System;
using SQLite;

namespace PruebaBT1.BBDD
{
    public class EngineTemperatureData : Table
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int temperature { get; set; }
        public DateTime CreatedOn { get; set; }

        public EngineTemperatureData() { }
    }
}
