﻿using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Diagnostics;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SwishPaymentController : ControllerBase
    {
        [HttpPost("swishpayment")]
        
        public string Post(SwishCallback swishCallback)
        {
            Debug.WriteLine("GOT SWISH PAYMENT!");
            return "yas";
        }

    }
}
