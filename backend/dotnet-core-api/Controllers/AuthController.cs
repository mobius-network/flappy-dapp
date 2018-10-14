using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stellar = stellar_dotnet_sdk;
using Auth = Mobius.Library.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotnet_core_api.Models;

namespace dotnet_core_api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private IConfiguration Configuration;
        public AuthController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            string APP_KEY = Configuration.GetValue("APP_KEY", "string");

            return new Mobius.Library.Auth.Challenge().Call(APP_KEY);
        }

        [HttpPost]
        public ActionResult<string> Post([FromQuery] string xdr = null, [FromQuery] string public_key = null)
        {
            TokenRequest request = new TokenRequest();
            if (request.Xdr == null && xdr == null) 
                return BadRequest("xdr cannot be null");

            if (request.Public_Key == null && public_key == null) 
                return BadRequest("public_key cannot be null");

            string APP_KEY = Configuration.GetValue("APP_KEY", "string");
            string APP_DOMAIN = Configuration.GetValue("APP_DOMAIN", "string");

            xdr = xdr.Length > 0 ? xdr : request.Xdr;
            public_key = public_key.Length > 0 ? public_key : request.Public_Key;

            try
            {
                Auth.Token token = new Auth.Token(APP_KEY, xdr, public_key);
                token.Validate();

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue("APP_KEY", "string")));
			    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                Claim[] claims = new[]
				{
					new Claim(ClaimTypes.NameIdentifier, public_key),
					new Claim("public_key", public_key )
				};

                Stellar.TimeBounds timebounds = token.TimeBounds();

                JwtSecurityToken payload = new JwtSecurityToken(
					issuer: APP_DOMAIN,
					audience: APP_DOMAIN,
					claims: claims,
					expires: DateTimeOffset.FromUnixTimeSeconds(timebounds.MaxTime).UtcDateTime,
					signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(payload);
            } 
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}
