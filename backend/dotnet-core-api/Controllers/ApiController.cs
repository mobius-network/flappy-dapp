using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Mobius.Library;
using Mobius.Library.App;
using Stellar = stellar_dotnet_sdk;
using DotNetCore.API.Models;

namespace DotNetCore.API.Controllers
{
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiController : Controller
    {
        private IConfiguration Configuration;
        private string APP_KEY {get; set;}
        public ApiController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.APP_KEY = Configuration.GetValue("APP_KEY", "string");
        }

        [HttpGet("test")]
        public ActionResult<dynamic> Test()
        {
            return User.Claims.Select(c => new {
                Issuer = c.Issuer,
                PublicKey = c.Value,
                Type = c.Type
            }).FirstOrDefault();
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            try 
            {
                string userPublicKey = User.Claims.FirstOrDefault().Value;
                App dapp = await new AppBuilder().Build(this.APP_KEY, userPublicKey);

                return Ok(dapp.UserBalance());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/values
        [HttpPost("charge")]
        public async Task<IActionResult> Charge([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0) return BadRequest("Invalid Amount");

            try 
            {
                string userPublicKey = User.Claims.FirstOrDefault().Value;
                App dapp = await new AppBuilder().Build(this.APP_KEY, userPublicKey);

                var response = await dapp.Charge(request.Amount, request.TargetAddress);

                return Ok(new {
                    status = "Ok",
                    tx_hash = response.Hash,
                    balance = dapp.UserBalance()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0) return BadRequest("Invalid Amount");
            if (request.TargetAddress == null) return BadRequest("Invalid Target Address");

            try 
            {
                string userPublicKey = User.Claims.FirstOrDefault().Value;
                App dapp = await new AppBuilder().Build(this.APP_KEY, userPublicKey);

                var response = await dapp.Transfer(request.Amount, request.TargetAddress);

                return Ok(new {
                    status = "Ok",
                    tx_hash = response.Hash,
                    balance = dapp.UserBalance()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payout")]
        public async Task<IActionResult> Payout([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0) return BadRequest("Invalid Amount");

            try 
            {
                string userPublicKey = User.Claims.FirstOrDefault().Value;
                App dapp = await new AppBuilder().Build(this.APP_KEY, userPublicKey);

                var response = await dapp.Payout(request.Amount, request.TargetAddress);

                return Ok(new {
                    status = "Ok",
                    tx_hash = response.Hash,
                    balance = dapp.UserBalance()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
