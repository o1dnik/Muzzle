using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whois;

namespace Muzzle
{
    internal class DomainTools
    {
        private static Regex domainRegex = new Regex(@"^([a-zA-Z0-9\-]{1,63}\.)+[A-Za-z]{2,6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidTarget(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                Console.WriteLine($"Missing target");
                return false;
            }
            if (!domainRegex.IsMatch(target))
            {
                Console.WriteLine($"Target must be a valid domain such as huylo.ru or targan.by");
                return false;
            }

            return true;
        }

        public static List<IPAddress> GetDnsServers(string domain)
        {
            var whois = new WhoisLookup();

            var response = whois.Lookup(domain);
            var nameServers = response.NameServers;

            // Merge host names
            var fullNameServers = nameServers.Select(x =>
                x.EndsWith(".")
                ? x.Substring(0, x.Length - 1)
                : x + "." + domain);


            var dnsServers = new List<IPAddress>();

            foreach (var nameServer in fullNameServers)
            {
                var ipAddresses = GetHostAddresses(nameServer);
                dnsServers.AddRange(ipAddresses);
            }

            return dnsServers;
        }

        public static IEnumerable<IPAddress> GetHostAddresses(string host)
        {
            return GetHostAddresses(host, new List<IPAddress>());
        }

        public static List<IPAddress> GetHostAddresses(string host, List<IPAddress> dnsServers)
        {
            var extendedDnsList = new List<IPAddress>(dnsServers);
            extendedDnsList.Add(IPAddress.Parse("1.1.1.1"));

            var result = new List<IPAddress>();
            var dnsClient = new LookupClient(extendedDnsList.ToArray());
            var dnsResponse = dnsClient.Query(host, QueryType.A, QueryClass.IN);
            var aRecords = dnsResponse.Answers.ARecords();
            foreach (var aRecord in aRecords)
                result.Add(aRecord.Address);
            return result.Distinct().ToList();
        }
    }
}
