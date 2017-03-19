using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace foodynet_webapp
{
    public class Program
    {
        static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            Console.WriteLine("************* WEBAPP *******************");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
