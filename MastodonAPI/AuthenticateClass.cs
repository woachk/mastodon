using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    public class AuthenticateClass
    {
        ApplicationTokenClass apptoken;
        public string token;
        public string appname;
        public string server;
        public AuthenticateClass(ApplicationTokenClass app_token, string code)
        {
            HttpClient client = new HttpClient();
            string json;
            apptoken = app_token;
            server = apptoken.server;
            string request = "grant_type=authorization_code" + "&client_id=" + apptoken.client_id + "&client_secret=" + apptoken.client_secret + "&code=" + code + "&scope=read write follow" + "&redirect_uri=urn:ietf:wg:oauth:2.0:oob";
            HttpContent content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + server + "/oauth/token", content);
            msg.Wait();
            HttpResponseMessage message = msg.Result;
            json = (message.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            try
            {
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.Value.ToString() == "access_token")
                        {
                            reader.Read();
                            token = reader.Value.ToString();
                        }
                    }
                }
            }
            catch
            {
                token = null;
            }
        }
        public AuthenticateClass()
        {
            return;
        }
    }
}
