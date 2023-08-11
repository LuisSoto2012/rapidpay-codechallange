using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RapidPay.Api.Handlers;
using RapidPay.Api.Helpers;
using RapidPay.Api.Validators;
using RapidPay.Api.Validators.Factory;
using RapidPay.Data;
using RapidPay.Data.Mapping;
using RapidPay.Data.Repositories;
using RapidPay.Domain.Dto;
using RapidPay.Domain.Dto.Request;
using RapidPay.Services.CardManagement;
using RapidPay.Services.PaymentFee;
using RapidPay.Services.UserAuthentication;

namespace RapidPay.Api
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
            
            services.AddFluentValidationAutoValidation();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    RequireExpirationTime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            //Swagger config with Basic Authentication
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RapidPay", Version = "v1" });

                //JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
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
            
            services.AddSingleton<IRequestValidatorFactory, RequestValidatorFactory>();
            // Register your individual validators
            services.AddTransient<IValidator<CreateCardRequest>, CreateCardRequestValidator>();
            services.AddTransient<IValidator<DoPaymentRequest>, DoPaymentRequestValidator>();
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
            
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

