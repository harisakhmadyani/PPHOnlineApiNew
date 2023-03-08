using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using newplgapi.Repository.Implements;
using newplgapi.Repository.Interfaces;
using Newtonsoft.Json.Serialization;

namespace newplgapi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "newplgapi", Version = "v1" });
            });
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDapperContext, DapperContext>();
            // services.AddSingleton<ILog4NetRepository, Log4NetRepository>();
            services.AddCors(option => option.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://192.168.12.60:99", "http://192.168.12.55:99", "http://facebook.com", "http://darimana.com", "http://192.168.3.120")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }
            ));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    // using Microsoft.IdentityModel.Tokens;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAcces", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // string[] dt = {"Admin", "admin"};
                    // policy.RequireClaim(ClaimTypes.Role, dt);
                });

                options.AddPolicy("RequireAdminRole", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // string[] dt = {"Admin", "admin"};
                    // policy.RequireClaim(ClaimTypes.Role, dt);
                });
            });

            // services.AddScoped<IUnitOfWorks, CountryRepositoryGUI>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddSingleton<ILog, ILog>();
            services.AddControllers(
                opt =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                }
            ).AddNewtonsoftJson(
                options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg4NDc4QDMxMzgyZTM0MmUzME51QlRwNmMwams3eGFYTzBZKzlYR2tJdGtmUlNjdFNWeU5ROXdYYzFxbTA9");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "newplgapi v1"));
            }
            // else{
            //     app.UseDeveloperExceptionPage();
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "plgapi v1"));
            // }


            // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
