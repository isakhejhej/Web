using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Web2.Models;
using System.Diagnostics;


namespace Web2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class ProductsController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "GetProducts")]
        public string Get()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM dbo.Products", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Product> productList = new List<Product>();
            if(dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    Product product = new Product();

                    product.Id = (Guid)dt.Rows[i]["Id"];
                    product.Name = (string)dt.Rows[i]["Name"];
                    product.Price = Convert.ToInt32(dt.Rows[i]["Price"]);

                    productList.Add(product);
                }
                return JsonConvert.SerializeObject(productList);
            }
            else
            {
                return "None";
            }
        }
    }
}
