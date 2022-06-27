using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RapidPay.Data;
using RapidPay.Data.Mapping;
using RapidPay.Data.Repositories;
using RapidPay.ServiceHost.Handlers;
using RapidPay.Services.CardManagement;
using RapidPay.Services.PaymentFee;
using RapidPay.Services.UserAuthentication;

namespace RapidPay.ServiceHost
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

            // Basic authentication config
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Swagger config with Basic Authentication
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RapidPay", Version = "v1" });

                options.AddSecurityDefinition("basicAuthentication",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Description = "Basic Authorization header using the username:password base64 encoded. Example: \"basic {username:password}\"",
                        Scheme = "basic",
                        In = ParameterLocation.Header
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basicAuthentication" }
                        }, new string[]{ }
                    }
                });

            });

            //Database config
            services.AddDbContext<RapidPayContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("RapidPayDb"));
            });

            //AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            //DI - Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICardManagementService, CardManagementService>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentFeeRepository, PaymentFeeRepository>();
            services.AddSingleton<IUFEService, UFEService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RapidPay");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

