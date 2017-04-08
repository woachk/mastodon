using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MastodonAPI
{
    public class AuthenticateClass
    {
        ApplicationTokenClass apptoken;
        public string token;
        public string appname;
        public string server;
        public AuthenticateClass(ApplicationTokenClass app_token, string username, string password)
        {
            HttpClient client = new HttpClient();
            string json;
            apptoken = app_token;
            server = apptoken.server;
            string request = "client_id=" + apptoken.client_id + "&client_secret=" + apptoken.client_secret + "&grant_type=password" + "&username=" + username + "&password=" +password;
            HttpContent content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + server + "/oauth/token", content);
            msg.Wait();
            HttpResponseMessage message = msg.Result;
            json = (message.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
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
    }
}
