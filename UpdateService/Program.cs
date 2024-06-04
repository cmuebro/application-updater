namespace UpdateService;

internal class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            //.ConfigureLogging(
            //    (context, logging) =>
            //    {
            //        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            //        logging.AddSimpleConsole(options => { options.SingleLine = false; });
            //    });

}
