using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using foodynet_api_anais.Models;
using MySQL.Data.Entity.Extensions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace foodynet_api_anais
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Cross Domain
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin",
            //        builder => builder.WithOrigins("http://192.168.1.40:5000"));
            //});
            // Add framework services.
            services.AddMvc();
            

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var sqlConnectionString = Configuration.GetConnectionString("SampleConnection");

            services.AddDbContext<MyDbContext>(options =>
                options.UseMySQL(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("foodynet_api_anais")
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var secretKey = "foodynetKey!ASPnetcore123";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            //---

            // Add JWT generation endpoint:
            var options = new TokenProviderOptions
            {
                Audience = "aspnet-core",
                Issuer = "http://localhost:5001",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            //---

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:5001",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "aspnet-core",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMvc();
        }
    }
}
