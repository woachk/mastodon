using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MastodonAPI;
using System.Net;

namespace createapp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string scopes = "read write follow";
            string redirecturl = "urn:ietf:wg:oauth:2.0:oob";
            ApplicationTokenClass app = new ApplicationTokenClass("woafre.tk", "dotnettest", scopes, redirecturl);
            System.Console.WriteLine(app.client_id);
            System.Console.WriteLine(app.client_secret);
            System.Console.ReadLine();
        }
    }
}
