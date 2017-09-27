using System;
using SQLite;

namespace PruebaBT1.BBDD
{
    public class ThrottlePosition : Table
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double throttlePosition { get; set; }
        public DateTime CreatedOn { get; set; }

        public ThrottlePosition(){}
    }
}
