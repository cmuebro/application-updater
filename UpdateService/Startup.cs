using Common.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UpdateStorage;
using UpdateStorage.Strategies;

namespace UpdateService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<IUpdateStorageSystem, UpdateStorageSystem>();
        services.AddScoped<FileSystemUpdateStorageStrategy>();
        services.AddScoped<DatabaseUpdateStorageStrategy>();


        //services.AddScoped<IUpdateStorageStrategy>(provider =>
        //{
        //    // Konfig auslesen
        //    var updateStorageType = configuration.GetValue<UpdateStorageTypeEnum>("UpdateStorageType");

        //    switch (updateStorageType)
        //    {
        //        case UpdateStorageTypeEnum.FileSystem:
        //            return provider.GetService<FileSystemUpdateStorageStrategy>() ?? throw new InvalidOperationException();

        //        case UpdateStorageTypeEnum.Database:
        //            return provider.GetService<DatabaseUpdateStorageStrategy>() ?? throw new InvalidOperationException();

        //        default:
        //            throw new InvalidOperationException();
        //    }

        //    // abhängig vom Wert in der Konfiguration die entsprechende Strategie zurückgeben
        //});

    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // TODO: hier ggf. noch die Authentifizierung aktivieren
        //app.UseAuthorization(); 

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
