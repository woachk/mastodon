using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MastodonAPI;

namespace Mastodon.WPF
{
    class GetToken
    {
        public static string getServerName()
        {
            var rkey = (Registry.CurrentUser.OpenSubKey("Software", false)).OpenSubKey("Mastodon");
            return rkey.GetValue("ServerName").ToString();
        }
        public static string getToken()
        {
            var rkey = (Registry.CurrentUser.OpenSubKey("Software", false)).OpenSubKey("Mastodon");
            return rkey.GetValue("Token").ToString();
        }
        public static AuthenticateClass getAuthClass()
        {
            AuthenticateClass token = new AuthenticateClass();
            token.server = getServerName();
            token.token = getToken();
            return token;
        }
    }
}
