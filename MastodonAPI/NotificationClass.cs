using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MastodonAPI
{
    public class NotificationClass_new
    {
        static public dynamic getNotifications(AuthenticateClass token)
        {
            List<NotificationClass> notifications = new List<NotificationClass>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/notifications");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            return obj;
        }
        static public dynamic parseNotification(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            return obj;
        }
    }
}
