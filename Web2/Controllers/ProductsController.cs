using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using API.Models;
using System.Net.Http;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Web2.Controllers
{
    [ApiController]
    [Route("api")]


    public class ProductsController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public SqlConnection con => new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("add-to-cart/{productId:guid}")]
        public string AddToCart(Guid productId, AddToCart addToCart)
        {
            //TODO
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            //SqlDataAdapter da = new SqlDataAdapter("UPDATE dbo.Products SET Quantity = Quantity - '" + cartProduct.Quantity + "' WHERE Id = '" + productId + "'", con);
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");

            string query = "IF EXISTS (SELECT * FROM dbo.CartProducts WHERE ProductId = '" + productId + "') " +
                "BEGIN " +
                "UPDATE dbo.CartProducts SET Quantity = Quantity + '" + addToCart.Quantity + "' WHERE ProductId = '" + productId +"' " +
                "END " +
                "ELSE " +
                "BEGIN " +
                "INSERT INTO dbo.CartProducts (UserId, ProductId, Quantity) VALUES ('" + userId + "', '" + productId + "', '" + addToCart.Quantity + "') " +
                "END";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            StatusCode sc = new StatusCode
            {
                Code = 200,
                Message = "Added product to cart, quantity " + addToCart.Quantity
            };
            return JsonConvert.SerializeObject(sc);
        }

        [HttpPost("new-order")]
        public string NewOrder()
        {
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");
            string query = "BEGIN " +
                "IF NOT EXISTS (SELECT * FROM dbo.Orders WHERE UserId = '" + userId + "' AND Paid = 0) " +
                "BEGIN " +
                "INSERT INTO dbo.Orders (UserId, Created) VALUES ('" + userId + "', '" + DateTime.Now + "') SELECT SCOPE_IDENTITY() AS OrderId " +
                "END " +
                "END";
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                Dictionary<string, int> orderReturn = new Dictionary<string, int>();
                orderReturn.Add("orderId", Convert.ToInt32(dt.Rows[0]["OrderId"]));
                return JsonConvert.SerializeObject(orderReturn);
            } 
            else
            {
                StatusCode sc = new StatusCode
                {
                    Code = 404,
                    Message = "Already has order"
                };
                return JsonConvert.SerializeObject(sc);
            }
        }

        [HttpGet("order-status-old")]
        public string OrderStatus()
        {
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");
            string query = "SELECT * FROM dbo.Orders WHERE UserId = '" + userId + "' AND Paid = '" + 0 + "' OR Paid = '" + 1 +"'";
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                OrderStatus orderStatus = new OrderStatus
                {
                    Id = (Guid)dt.Rows[0]["Id"],
                    OrderId = (int)dt.Rows[0]["OrderId"],
                    Created = (DateTime)dt.Rows[0]["Created"],
                    Paid = (Byte)dt.Rows[0]["Paid"]
                };
                return JsonConvert.SerializeObject(orderStatus);
            }
            else
            {
                StatusCode sc = new StatusCode
                {
                    Code = 404,
                    Message = "Order status not found"
                };
                return JsonConvert.SerializeObject(sc);
            }
        }

        [HttpGet("order-status")]
        public async Task<OrderStatus> OrderStatus2()
        {
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");
            string query = @"SELECT * FROM dbo.Orders WHERE UserId=@userId AND Paid = '0' OR Paid = '1'";
            return await con.QuerySingleAsync<OrderStatus>(query, new {userId});
        }

        [HttpGet("order")]
        public string Order()
        {
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");
            string query = "SELECT dbo.CartProducts.Id, dbo.Products.Name, dbo.CartProducts.Quantity, dbo.Products.Price * dbo.CartProducts.Quantity AS Price FROM dbo.CartProducts " +
                "INNER JOIN dbo.Products " +
                "ON dbo.CartProducts.ProductId = dbo.Products.Id " +
                "WHERE UserId = '" + userId + "'";
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                List<OrderProduct> orderList = new List<OrderProduct>();
                int totalPrice = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    OrderProduct orderProduct = new OrderProduct
                    {
                        Id = (Guid)dt.Rows[i]["Id"],
                        Name = (string)dt.Rows[i]["Name"],
                        Price = Convert.ToInt32(dt.Rows[i]["Price"]),
                        Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"])
                    };
                    totalPrice += orderProduct.Price;
                    orderList.Add(orderProduct);
                }
                data.Add("order", orderList);
                data.Add("totalPrice", totalPrice);
                return JsonConvert.SerializeObject(data);
            }
            else
            {
                StatusCode sc = new StatusCode
                {
                    Code = 404,
                    Message = "No order"
                };
                return JsonConvert.SerializeObject(sc);
            }
        }

        [HttpGet("product-info-old/{productId:guid}")]
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

        [HttpGet("product-info/{productId:guid}")]
        public async Task<Product> ProductInfo2(Guid productId)
        {
            string query = @"SELECT * FROM dbo.Products WHERE Id=@productId";
            return await con.QuerySingleAsync<Product>(query, new {productId});
        }

        [HttpGet("products")]
        public async Task<IEnumerable<Product>> Products2()
        {
            Guid userId = Guid.Parse("f60e7c57-d272-45c0-aaec-ca8a6020b471");
            string query = @"SELECT * FROM dbo.Products";
            return await con.QueryAsync<Product>(query);
        }

        [HttpGet("products-old")]
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
