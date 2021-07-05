using HLSSafariProxy_ASP.NET.Services;
using HLSSafariProxy_ASP.NET.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;

namespace HLSSafariProxy_ASP.NET
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFetcherService, FetcherService>();
            services.AddScoped<ISecondLevelManifestModifierService, SecondLevelManifestModifierService>();
            services.AddScoped<ITopManifestModifierService, TopManifestModifierService>();
            services.AddScoped<IUrlService, UrlService>();

            services.AddHttpClient();
            
            services.AddSwaggerDocument(opt =>
            {
                opt.PostProcess = document =>
                {
                    document.Info.Description = "An API converting the manifest in order to make it able to play AES encrypted videos within iOS and older Android devices";
                    document.Info.Title = "HLSSafariProxy ASP.NET Core API";
                    document.Info.Version = "v1";
                    document.Info.Contact = new OpenApiContact
                    {
                        Email = "marcingadomski94@gmail.com",
                        Name = "Marcin Gadomski",
                        Url = string.Empty,
                        ExtensionData = null
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = "https://github.com/MarcinGadomski94/HLSSafariProxy-ASP.NET/blob/master/LICENSE"
                    };
                };
            });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}