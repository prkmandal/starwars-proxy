using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StarWars.Services.Implementation;
using StarWarsServices.Interfaces;

namespace StarWarsProxyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
             {
                 options.AddPolicy("AllowAll", builder =>
                     builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
             });

            string baseuri = _configuration.GetValue<string>("StarWarsBaseUrl:baseUri");

            services.AddHttpClient("starWarsClient", httpClient =>
             {
                 httpClient.BaseAddress = new Uri(baseuri);
                 httpClient.Timeout = new TimeSpan(0, 0, 30);
                 httpClient.DefaultRequestHeaders.Clear();
             });

            services.AddTransient(typeof(IStarWarService<>), typeof(StarWarService<>));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proxy window for Star Wars", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proxy Api"));
            }
            else
            {
                app.UseHsts();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

