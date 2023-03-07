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
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using System;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]

    public class SwishPaymentController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public SqlConnection con => new SqlConnection(_configuration.GetConnectionString("MainDB").ToString());
        public SwishPaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("swishpayment")]
        
        public void SwishPayment(SwishCallback swishCallback)
        {
            string queryy = "INSERT INTO dbo.Debug (Log) VALUES ('" + swishCallback.Status +"')";
            SqlDataAdapter da = new SqlDataAdapter(queryy, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string query = @"UPDATE dbo.SwishStatus SET Status=@status WHERE InstructionUUID=@id";
            con.QuerySingleAsync<SwishStatus>(query, new { swishCallback.Id, swishCallback.Status });
        }

        [HttpPost("request-payment")]
        public async Task<string> PostAsync()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();

            X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            var certs = new X509Certificate2Collection();
            certs.Import(@"C:\Certs\Swish_Merchant_TestCertificate_1234679304.p12", "swish", X509KeyStorageFlags.DefaultKeySet );

            foreach(X509Certificate2 cert in certs)
            {
                if(cert.HasPrivateKey)
                {
                    clientHandler.ClientCertificates.Add(cert);
                } else
                {
                    store.Add(cert);
                }
            }

            HttpClient client = new HttpClient(clientHandler);
            
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            var parameters = new Dictionary<string, string> { { "callbackUrl", "https://woizservice.xyz/api/api/swishpayment" }, { "currency", "SEK" }, { "amount", "100" }, { "payeeAlias", "1231181189" }, { "payeePaymentReference", "HEJ" } };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonData = JsonConvert.SerializeObject(parameters);
            var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var url = "https://mss.cpc.getswish.net/swish-cpcapi/api/v2/paymentrequests/" + guid;
            var response = await client.PutAsync(url, contentData);

            NewSwishStatus(guid);

            return url;
        }

        private void NewSwishStatus(string guid)
        {
            string query = @"INSERT INTO dbo.SwishStatus (InstructionUUID, Status) VALUES(@guid, 'PENDING')";
            con.QuerySingleAsync<SwishStatus>(query, new {guid});
        }

        [HttpGet("payment-status")]
        public async Task<SwishStatus> PaymentStatus(string instructionUUID)
        {
            string query = @"SELECT * FROM dbo.SwishStatus WHERE InstructionUUID=@instructionUUID";
            return await con.QuerySingleAsync<SwishStatus>(query, new { instructionUUID });
        }

    }
}
