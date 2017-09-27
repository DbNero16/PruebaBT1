using SQLite;

namespace PruebaBT1.BBDD
{
    class DataBaseRepository
    {
        private readonly DataBaseContext dataBase;
        private static object locker = new object();

        public DataBaseRepository (DataBaseContext dataBase)
        {
            this.dataBase = dataBase;
        }

      

        
    }
}
