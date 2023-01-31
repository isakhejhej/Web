using System.Data.SqlClient;

namespace Web2.Data
{
    public class DB
    {
        private readonly IConfiguration _config;

        public string ConnectionStringName { get; set; } = "MainDB";
        public DB(IConfiguration config)

        {
            _config = config;
        }

        public SqlConnection Conn => new SqlConnection(_config.GetConnectionString(ConnectionStringName));
        public string ConnString()
        {
            return _config.GetConnectionString(ConnectionStringName);
        }
    }
}
