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
    public class NotificationClass
    {
        public string id;
        public string type;
        public string created_at;
        public AccountClass account;
        public StatusClass status;
        static public List<NotificationClass> getNotifications(AuthenticateClass token)
        {
            List<NotificationClass> notifications = new List<NotificationClass>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/notifications");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            NotificationClass notification = new NotificationClass();
            notification.status = new StatusClass();
            notification.account = new AccountClass();
            notification.status.account = new AccountClass();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        notification.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "type")
                    {
                        reader.Read();
                        notification.type = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "created_at")
                    {
                        reader.Read();
                        notification.created_at = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    notification.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    notification.account.display_name = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    notification.account.avatar = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                        if (notification.type == "follow")
                        {
                            if (notification.status.account.display_name == null)
                            {
                                notification.status.account.display_name = notification.status.account.acct;
                            }
                            notifications.Add(notification);
                            notification = new NotificationClass();
                            notification.status = new StatusClass();
                            notification.account = new AccountClass();
                            notification.status.account = new AccountClass();
                        }
                    }
                    else if (reader.Value.ToString() == "status" && notification.type != "follow")
                    {
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            { 
                                if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    notification.status.id = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "uri")
                                {
                                    reader.Read();
                                    notification.status.uri = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "url")
                                {
                                    reader.Read();
                                    notification.status.url = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "content")
                                {
                                    reader.Read();
                                    notification.status.content = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "reblogged")
                                {
                                    reader.Read();
                                    if (reader.Value != null)
                                    {
                                        notification.status.reblogged = reader.Value.ToString();
                                    }
                                    if (notification.status.account.display_name == null)
                                    {
                                        notification.status.account.display_name = notification.status.account.acct;
                                    }
                                    notifications.Add(notification);
                                    notification = new NotificationClass();
                                    notification.status = new StatusClass();
                                    notification.account = new AccountClass();
                                    notification.status.account = new AccountClass();
                                    break;
                                }
                                else if (reader.Value.ToString() == "account")
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.Value != null)
                                        {
                                            if (reader.Value.ToString() == "acct")
                                            {
                                                reader.Read();
                                                notification.status.account.acct = reader.Value.ToString();
                                            }
                                            else if (reader.Value.ToString() == "display_name")
                                            {
                                                reader.Read();
                                                notification.status.account.display_name = reader.Value.ToString();
                                            }
                                            else if (reader.Value.ToString() == "avatar")
                                            {
                                                reader.Read();
                                                notification.status.account.avatar = reader.Value.ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return notifications;
        }
    }
}
