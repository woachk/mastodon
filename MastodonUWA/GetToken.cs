using MastodonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
/*  Copyright (C) 2017  my123 (@never_released)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
