using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzle
{
    [Verb("sniff", HelpText = "Print out list of servers")]
    internal class SniffOptions
    {
        [Option('d', "domain", Required = true)]
        public string Domain { get; set; }
    }
}
