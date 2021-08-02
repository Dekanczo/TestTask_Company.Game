using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Authentication.Cookies;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Game.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Game.API.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;

namespace Game.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Infrastructure.ProjectsConfigurations/sharedsettings.json", optional: true)
                .AddJsonFile(@Directory.GetCurrentDirectory() + "./sharedsettings.json", optional: true)
                .AddJsonFile(@Directory.GetCurrentDirectory() + "./appsettings.json")
                .Build();

                var connectionString = configuration.GetConnectionString(
                    configuration.GetValue<string>("CurrentEnvironment"));
                options
                    .UseSqlServer(connectionString);
            });

            services
                .AddControllers()
                .AddNewtonsoftJson(t =>
                {
                    t.SerializerSettings.MaxDepth = 3;
                    t.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            #region Swagger configuration

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game.API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT(Bearer) place",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer"
                            },
                            new List<string>()
                        }
                    });
            });

            #endregion

            #region Auth configuration


            services.AddScoped<UserStore<User, Role, ApplicationContext, Guid>, OwnUserStore>();
            services.AddScoped<OwnUserStore, OwnUserStore>();
            services.AddScoped<UserManager<User>>();
            services.AddScoped<UserManager<User>, UserManager<User>>();
            services.AddScoped<RoleManager<Role>>();

            services.AddTransient<AuthService, AuthService>();
            services.AddTransient<AuthService>();

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthConfiguration.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthConfiguration.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthConfiguration.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

            services
            .AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            #endregion

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game.API v1"));
            };

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(s =>
            {
                s.WithOrigins("https://localhost:44328");
                s.AllowAnyOrigin();
                s.AllowAnyMethod();
                s.AllowAnyHeader();
            });

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();
        }
    }
}
