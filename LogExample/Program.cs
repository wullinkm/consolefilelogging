using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace LogExample
{
    class Program
    {
        static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                var worker = new MyClass(logger);
                worker.Start();
                logger.Information("The work finished at: {DateTime}", DateTime.Now);
                return 0;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "The application crashed");
                return 1;
            }
            finally
            {
                logger.Dispose();
            }
        }
    }

    public class MyClass
    {
        private readonly ILogger _logger;

        public MyClass(ILogger logger) => _logger = logger;

        public void Start()
        {
            _logger.Information("The worker has started at: {DateTime}", DateTime.Now);
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
