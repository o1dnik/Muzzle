using CommandLine;
using CommandLine.Text;
using System.Net;

namespace Muzzle
{
    class Program
    {
        public static async Task Main(params string[] args)
        {
            Console.WriteLine($"{HeadingInfo.Default}");
            
            // Set the maximum number of connections per server to 4.  
            ServicePointManager.DefaultConnectionLimit = 1000;

            var parsedArguments = Parser.Default.ParseArguments<SniffOptions, FloodDnsOptions, FloodHttpOptions>(args);
            parsedArguments.WithParsed<SniffOptions>(options => SniffCommand.Execute(options));
            parsedArguments.WithParsed<FloodDnsOptions>(options => FloodDnsCommand.Execute(options));
            parsedArguments.WithParsed<FloodHttpOptions>(options => FloodHttpCommand.Execute(options));
        }
    }
}