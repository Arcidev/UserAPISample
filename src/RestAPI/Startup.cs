using BL.Configuration;
using BL.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Exceptions;
using RestAPI.Middleware;
using RestAPI.Responses;
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
            JwtTokenHelper.Secret = Configuration["SymmetricKeys:JwtTokenKey"];

            services.AddMvc(opt => opt.EnableEndpointRouting = false).AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.ConfigureDatabase(Configuration.GetConnectionString("UserAPISampleDatabase"))
                .ConfigureServices()
                .ConfigureFacades()
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResultCustom(context.ModelState, ErrorMessages.InvalidInput);
                })
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = c =>
                        {
                            c.HandleResponse();
                            if (c.AuthenticateFailure is SecurityTokenExpiredException)
                                throw new UnauthorizedException(ErrorMessages.SessionExpired, c.AuthenticateFailure);

                            throw new UnauthorizedException(ErrorMessages.Unauthorized, c.AuthenticateFailure);
                        }
                    };

                    options.TokenValidationParameters = JwtTokenHelper.ValidationParameters;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseNotFoundMiddleware();
            app.UseMiddleware(typeof(ErrorHandler));
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
