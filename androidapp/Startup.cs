using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using androidapp.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using IdentityServer4.Models;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace androidapp
{
    public class Startup
    {
        public readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = "yourdomain.com",
               ValidAudience = "yourdomain.com",
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR-PRIVATE-RSA-KEY"))
           };
       });
           
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });
           

            services.AddMvc();
            
          

            services.AddDbContext<DataBaseContext>(options =>  options.UseSqlServer(@"Data Source=DESKTOP-653TN87\LOCALSERVER;Initial Catalog=Training;Persist Security Info=True;User ID=TestUser;Password=11111"));
            //services.AddSwaggerGen(c =>
            //{
            //   // options.OperationFilter<TFilter>(new AuthorizationHeaderParameterOperationFilter());

            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            //    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
            //    { "Bearer", Enumerable.Empty<string>() },


            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OAuth2Scheme { Flow = "password", Description = "Please enter JWT with Bearer into field", TokenUrl = ("http://localhost:50302/api/auth/auth2  "), Type = "oauth2" });
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                //{ "Bearer", Enumerable.Empty<string>() },
                //    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //    {
                //    { "oauth2", new[] { "readAccess", "writeAccess" } }
                c.DescribeAllEnumsAsStrings();

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                     { "Bearer", new string[]{ } }

                });

            });

            



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           

            app.UseAuthentication();

            app.UseStaticFiles();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.

            // UseOpenIdConnectAuthentication(app);

          

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
              //  c.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");

                // c.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
            });
            //  UseOpenIdConnectAuthentication(app);
            app.UseSwagger();

            app.UseMvc();
           
        }
        
    }
}
