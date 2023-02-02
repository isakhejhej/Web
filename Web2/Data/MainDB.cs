using API.Models;
using Dapper;

namespace Web2.Data
{
    public class MainDB
    {
        DB _DB { get; init; }
        public MainDB(DB _DB)
        {
            this._DB = _DB;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var sql = "SELECT * FROM dbo.Products";
            using var cn = _DB.Conn;
            return await cn.QueryAsync<Product>(sql);
        }

    }
}
