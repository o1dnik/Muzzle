using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzle
{
    [Verb("http-flood", HelpText = "Attack web servers (flood)")]
    internal class FloodHttpOptions
    {
        [Option('d', "domain", Required = true)]
        public string Domain { get; set; }

        [Option('t', "threads", Required = false, Default = 100)]
        public int ThreadsPerServer { get; set; }
    }
}
