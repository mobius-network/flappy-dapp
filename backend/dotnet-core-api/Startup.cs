using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stellar = stellar_dotnet_sdk;

namespace dotnet_core_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options => {
            //         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("SecurityKey").GetValue<string>("Key")));

            //         options.TokenValidationParameters = new TokenValidationParameters {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = "mobius.network",
            //             ValidAudience = "mobius.network",
            //             IssuerSigningKey = key
            //         };
            //     });

            services.AddCors(o => o.AddPolicy("DevelopmentCORS", builder => {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            // services.AddCors(o => o.AddPolicy("ProductionCORS", builder => {
            //     builder
            //         .AllowAnyOrigin()
            //         .AllowAnyMethod()
            //         .AllowAnyHeader()
            //         .AllowCredentials();
            // }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors("DevelopmentCORS");
                Stellar.Network.UseTestNetwork();
            }
            else
            {
                app.UseHsts();
                // app.UseHttpsRedirection();
                // app.UseCors("ProductionCORS");
                Stellar.Network.UsePublicNetwork();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
