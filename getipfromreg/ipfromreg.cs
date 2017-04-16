using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Microsoft.Win32;

namespace getipfromreg
{
    [Cmdlet(VerbsCommon.Get, "IpFromReg")]
    public class Ipfromreg : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Get IP from Registry of local or remote computer")]
        public string[] Computername { get; set; }
       

        protected override void ProcessRecord()
        {

            //base.ProcessRecord();
            string rkeys = "SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces";
            WriteObject(Getmeregistry(rkeys));
        }

        public IEnumerable<IpSm> Getmeregistry(string rkeys)
        {
            IList<IpSm> iplist = new List<IpSm>();

            
            foreach (string computer in Computername)
            {
                using (RegistryKey r = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, computer).OpenSubKey(rkeys))
                {
                    foreach (string subkey in r.GetSubKeyNames())
                    {
                       // Console.WriteLine(subkey);
                        using(RegistryKey rr = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, computer).OpenSubKey(rkeys + "\\" + subkey))
                        {
                            foreach(string values in rr.GetValueNames())
                            {
                              if(values== "IPAddress" )
                                {
                                    
                                    iplist.Add(new IpSm {Ipaddress= string.Join(",", (string[])rr.GetValue("IPAddress")) ,Subnetmask = string.Join(",", (string[])rr.GetValue("SubnetMask")) ,Gateway= string.Join(",", (string[])rr.GetValue("DefaultGateway")), Server =computer });


                                }
                                    //
                                    //
                                    //Console.WriteLine(ips);
                                    
                            }
                        }
                    }

                }
            }
           
            return iplist;
        }

    }
    public class IpSm
    {
        public string Ipaddress { get; set; }
        public string Subnetmask { get; set; }
        public string Gateway { get; set; }
        public string Server { get; set; }

        public override string ToString()
        {
            return this.Ipaddress + "          " + this.Subnetmask + "          " + this.Gateway + "          " + this.Server;
        }
    }
}
