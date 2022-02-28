using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whois;

namespace Muzzle
{
    internal class SniffCommand
    {
        public static void Execute(SniffOptions options)
        {   
            if (!DomainTools.IsValidTarget(options.Domain))
                return;

            Console.WriteLine($"Target: {options.Domain}");

            Console.WriteLine();
            Console.WriteLine("Sniffing DNS servers...");
            var dnsServers = DomainTools.GetDnsServers(options.Domain);
            if (!dnsServers.Any())
            {
                Console.WriteLine("No DNS servers found");
            }
            else
            {
                foreach (var ip in dnsServers)
                {
                    Console.WriteLine(ip);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Sniffing web servers...");
            var webServers = DomainTools.GetHostAddresses(options.Domain, dnsServers); 
            if (!webServers.Any())
            {
                Console.WriteLine("No web servers found");
            }
            else
            {
                foreach (var ip in webServers)
                {
                    Console.WriteLine(ip);
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Done sniffing");
        }
    }
}
