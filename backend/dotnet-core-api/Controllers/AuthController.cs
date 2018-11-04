using System;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Stellar = stellar_dotnet_sdk;
using Auth = Mobius.Library.Auth;
using DotNetCore.API.Models;

namespace DotNetCore.API.Controllers
{
    [Route("auth")]
    [EnableCors("All")]
    public class AuthController : ControllerBase
    {
        private IConfiguration Configuration;
        private string APP_KEY {get; set;}
        private string APP_DOMAIN {get; set;}
        public AuthController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.APP_KEY = Configuration.GetValue("APP_KEY", "string");
            this.APP_DOMAIN = Configuration.GetValue("APP_DOMAIN", "string");
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return new Mobius.Library.Auth.Challenge().Call(this.APP_KEY);
        }

        [HttpPost]
        public ActionResult<string> Post(
            [FromForm] TokenRequest request = null, 
            [FromQuery] string xdr = null, 
            [FromQuery] string public_key = null
        )
        {
            if (request.Xdr == null && xdr == null) 
                return BadRequest("xdr cannot be null");

            if (request.PublicKey == null && public_key == null) 
                return BadRequest("public_key cannot be null");

            xdr = xdr != null ? xdr : request.Xdr;
            public_key = public_key != null ? public_key : request.PublicKey;

            try
            {
                var token = new Auth.Token(this.APP_KEY, xdr, public_key);
                token.Validate();

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.APP_KEY));
			    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                Claim[] claims = new[]
				{
					new Claim(ClaimTypes.NameIdentifier, public_key),
					new Claim("public_key", public_key ),
                    new Claim(JwtRegisteredClaimNames.Jti, token.Hash())
				};

                var timebounds = token.TimeBounds();

                JwtSecurityToken payload = new JwtSecurityToken(
					issuer: this.APP_DOMAIN,
					audience: this.APP_DOMAIN,
					claims: claims,
					expires: DateTimeOffset.FromUnixTimeSeconds(timebounds.MaxTime).UtcDateTime,
					signingCredentials: creds);
                
                string signedToken = new JwtSecurityTokenHandler().WriteToken(payload);

                return Ok(signedToken);
            } 
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}
