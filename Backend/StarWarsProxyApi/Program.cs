using Serilog;
using Serilog.Events;
using StarWarsProxyApi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .WriteTo.File("logs/log.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day);
            });


}


//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging(configure => configure
//            .AddConsole()) ; // Add Console logger


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader());
//});

//// Add services to the container.
//builder.Services.AddHttpClient("starWarsClient", httpClient =>
//{
//    httpClient.BaseAddress = new Uri("https://swapi.dev/api/");
//    httpClient.Timeout = new TimeSpan(0, 0, 30);
//    httpClient.DefaultRequestHeaders.Clear();
//});


//builder.Services.AddScoped(typeof(IStarWarService<>), typeof(StarWarService<>));

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proxy window for Star Wars", Version = "v1" });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proxy Api"));
//}
//app.UseHttpsRedirection();
//app.UseRouting();
//app.UseAuthorization();
//app.UseCors("AllowAll");
//app.MapControllers();

//app.Run();
