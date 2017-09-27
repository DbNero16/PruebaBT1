using System;
using SQLite;

namespace PruebaBT1.BBDD
{
    public class SpeedData : Table
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;}
        public int speed { get; set; }
        public DateTime CreatedOn { get; set; }

        public SpeedData() { }
    }
}
