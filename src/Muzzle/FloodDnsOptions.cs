using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzle
{
    [Verb("dns-flood", HelpText = "Attack DNS servers (flood)")]
    internal class FloodDnsOptions
    {
        [Option('d', "domain", Required = true)]
        public string Domain { get; set; }

        [Option('t', "threads", Required = false, Default = 200)]
        public int ThreadsPerServer { get; set; }
    }
}
