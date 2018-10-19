using BL.Configuration;
using BL.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Exceptions;
using RestAPI.MiddlewareHandler;
using RestAPI.Security;

namespace RestAPI
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
            AutoMapperInstaller.Init();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connection = @"Server=(localdb)\mssqllocaldb;Database=UserAPISample";
            services.ConfigureServices(connection)
                .ConfigureFacades()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            if (c.Exception is SecurityTokenExpiredException)
                                throw new UnauthorizedException(ErrorMessages.SessionExpired);
                            else if (c.Exception is UnauthorizedException)
                                throw c.Exception;
                            else
                                throw new UnauthorizedException(ErrorMessages.Unauthorized);
                        },
                        OnChallenge = c =>
                        {
                            c.HandleResponse();
                            throw new UnauthorizedException(ErrorMessages.Unauthorized);
                        }
                    };

                    options.TokenValidationParameters = JwtTokenHelper.ValidationParameters;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ErrorHandler));
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
