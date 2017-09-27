using SQLite;

namespace PruebaBT1
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();

        void initBBDD();
    }
}
