using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class NotificationClass_new
    {
        public string id { get; set; }
        public string type { get; set; }
        public string created_at { get; set; }
        public AccountClass account { get; set; }
        public StatusClass_new status { get; set; }
        static public NotificationClass_new[] getNotifications(HttpConnectionClass token)
        {
            List<NotificationClass> notifications = new List<NotificationClass>();
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/notifications");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            NotificationClass_new[] obj = JsonConvert.DeserializeObject<NotificationClass_new[]>(json);
            return obj;
        }
        static public NotificationClass_new parseNotification(string json)
        {
            NotificationClass_new obj = JsonConvert.DeserializeObject<NotificationClass_new>(json);
            return obj;
        }
    }
}
