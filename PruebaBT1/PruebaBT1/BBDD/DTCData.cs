using System;
using SQLite;

namespace PruebaBT1.BBDD
{
    public class DTCData : Table
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string dtcFound { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
