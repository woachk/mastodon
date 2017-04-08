using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MastodonAPI;

namespace AuthenticateApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string client_id = "50198a2087135caa6c6f9a1222209d5ef44f663c69d071c40e5eb89d51e23790";
            string client_secret = "6cb8a25a4dda237a4aa9337ab30a5b1510aa11107084779464ed97b8903fb651";
            ApplicationTokenClass token = new ApplicationTokenClass(client_id, client_secret, "woafre.tk");
            AuthenticateClass usertoken = new AuthenticateClass(token, "username", "password");
        }
    }
}
