using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
