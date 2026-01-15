using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class DatabaseContext
    {
        // SSL komutlarını sildik. Artık hata vermeyecek, varsayılan ayarla bağlanacak.
        private string connectionString = "Server=172.21.54.253;Port=3306;Database=26_132430080;Uid=26_132430080;Pwd=İnif123.;Charset=utf8mb4;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}