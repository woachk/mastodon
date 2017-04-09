using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
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
namespace MastodonAPI
{
    public class ApplicationTokenClass
    {
        string appname;
        public string client_id;
        public string client_secret;
        string redirect_url;
        string scopes;
        public string server;
        string request;
        string json;
        public ApplicationTokenClass(string srv,string app_name, string scope, string redirecturl)
        {
            HttpClient client = new HttpClient();
            server = srv;
            appname = app_name;
            scopes = scope;
            redirect_url = redirecturl;
            request = "client_name=" + appname + "&redirect_uris=" + redirect_url + "&scopes=" + scopes;
            HttpContent content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + srv + "/api/v1/apps", content);
            msg.Wait();
            HttpResponseMessage message = msg.Result;
            json = (message.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "client_id")
                    {
                        reader.Read();
                        client_id = reader.Value.ToString();
                    }
                    if (reader.Value.ToString() == "client_secret")
                    {
                        reader.Read();
                        client_secret = reader.Value.ToString();
                    }
                }
            }
        }
        public ApplicationTokenClass (string clientid, string clientsecret, string srv)
        {
            client_id = clientid;
            client_secret = clientsecret;
            srv = server;
        }
    }
}
