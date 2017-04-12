using MastodonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MastodonUWA
{
    class GetToken
    {
        public static string getServerName()
        {
            var serverfile = ApplicationData.Current.LocalFolder.GetFileAsync("server.txt");
            serverfile.AsTask().Wait();
            var serverfile_o = serverfile.GetResults();
            var ioop = FileIO.ReadTextAsync(serverfile_o);
            ioop.AsTask().Wait();
            return ioop.GetResults();
        }
        public static string getToken()
        {
            var serverfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            serverfile.AsTask().Wait();
            var serverfile_o = serverfile.GetResults();
            var ioop = FileIO.ReadTextAsync(serverfile_o);
            ioop.AsTask().Wait();
            return ioop.GetResults();
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
