using System;
using SQLite;

namespace PruebaBT1.BBDD
{

    public class RPMData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int rpm { get; set; }
        public DateTime CreatedOn { get; set; }

        public RPMData() { }
    }
}
