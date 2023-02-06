using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Web2.Data;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]

    public class SwishPaymentController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public SwishPaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("swishpayment")]
        
        public string Post(SwishCallback swishCallback)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
            SqlDataAdapter da = new SqlDataAdapter("INSERT INTO dbo.SwishPayments (Id) VALUES('00000000-0000-0000-0000-000000000000')", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Debug.WriteLine("GOT SWISH PAYMENT!");
            return "yas";
        }

    }
}
