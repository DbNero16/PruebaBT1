using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaBT1.BBDD
{
    public class CalculatedEngineLoadValuesData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double calculatedEngineLoadValue { get; set; }
        public DateTime CreatedOn { get; set; }

        public CalculatedEngineLoadValuesData() { }
    }
}
