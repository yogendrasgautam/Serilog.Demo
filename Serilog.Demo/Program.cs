
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Logger without adding configuration in appsettings.json
            builder.Host.UseSerilog((context, configuration) => configuration
                            .MinimumLevel.Warning()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                            .MinimumLevel.Override("System", LogEventLevel.Fatal)
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                                                theme: SystemConsoleTheme.Colored)
                            .WriteTo.File("logs/log.txt",
                                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day, 
                                            rollOnFileSizeLimit: true, fileSizeLimitBytes: 500000)
                            .WriteTo.Seq("http://localhost:5341"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
