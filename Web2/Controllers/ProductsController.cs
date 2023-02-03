using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using API.Models;
using System.Net.Http;

namespace Web2.Controllers
{
    [ApiController]
    [Route("api")]


    public class ProductsController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("add-to-cart/{productId:guid}")]
        public string AddToCart(Guid productId, CartProduct cartProduct)
        {
            //TODO
            StatusCode sc = new StatusCode
            {
                Code = 200,
                Message = "Added product to cart, quantity " + cartProduct.Quantity
            };
            return JsonConvert.SerializeObject(sc);
        }

        [HttpGet("product-info/{productId:guid}")]
        public string ProductInfo(Guid productId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM dbo.Products WHERE Id = '"+ productId + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Product product = new Product
                {
                    Id = productId,
                    Name = dt.Rows[0]["Name"].ToString(),
                    Price = (int)dt.Rows[0]["Price"],
                    Quantity = (int)dt.Rows[0]["Quantity"]
                };
               return JsonConvert.SerializeObject(product);
            } 
            else
            {
                StatusCode sc = new StatusCode
                {
                    Code = 404,
                    Message = "Product not found"
                };
                return JsonConvert.SerializeObject(sc);
            }
        }

        [HttpGet("products")]
        public string Products()
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
                    product.Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]);

                    productList.Add(product);
                }
                return JsonConvert.SerializeObject(productList);
            }
            else
            {
                StatusCode sc = new StatusCode
                {
                    Code = 404,
                    Message = "Something went wrong"
                };
                return JsonConvert.SerializeObject(sc);
            }
        }
    }
}
