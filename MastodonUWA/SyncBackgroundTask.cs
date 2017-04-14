using MastodonAPI;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Notifications;
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
    class SyncBackgroundTask : IBackgroundTask
    {
        static async public void NotificationSetup()
        {
            PushNotificationChannel channel = null;

            try
            {
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            }

            catch (Exception exception)
            {
                // Could not create a channel. 
            }

            String serverUrl = "http://www.contoso.com";

            // Create the web request.
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            byte[] channelUriInBytes = System.Text.Encoding.UTF8.GetBytes("ChannelUri=" + channel.Uri);

            // Write the channel URI to the request stream.
            Stream requestStream = await webRequest.GetRequestStreamAsync();
            requestStream.Write(channelUriInBytes, 0, channelUriInBytes.Length);
            try
            {
                // Get the response from the server.
                WebResponse response = await webRequest.GetResponseAsync();
                StreamReader requestReader = new StreamReader(response.GetResponseStream());
                String webResponse = requestReader.ReadToEnd();
            }

            catch (Exception ex)
            {
                // Could not send channel URI to server.
            }
        }
        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            AuthenticateClass token = new AuthenticateClass();
            token.appname = null;
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = MainPage.getServerName();
            string baseuri = "https://" + token.server;
            baseuri = baseuri + "/api/v1/streaming/user";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            var msg = client.GetStreamAsync(baseuri);
            msg.Wait();
            var msga = msg.Result;
            while (1 == 1)
            {
                var st = new StreamReader(msga);
                string text = st.ReadLine();
                if ((text.ToArray())[0] != ':')
                {
                    string[] text2 = text.Split(':');
                    string text3 = st.ReadLine();
                    string stdata = new string((text3.Skip(5)).ToArray());
                    if (text2[1] == " notification")
                    {
                        NotificationClass_new notification = NotificationClass_new.parseNotification(stdata);
                        if (notification.id != null)
                        {
                            if (notification.type == "favourite")
                            {
                                    
                            }
                            if (notification.type == "reblog")
                            {
                                   
                            }
                            if (notification.type == "follow")
                            {
                                if (notification.account.avatar[0] == 'h')
                                {
                                    notification.account.avatar = "https://" + token.server + notification.account.avatar;
                                }
                                ToastVisual visual = new ToastVisual()
                                {
                                    BindingGeneric = new ToastBindingGeneric()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = notification.account.display_name + " followed you."
                                            },
                                            new AdaptiveImage()
                                            {
                                                Source = notification.account.avatar
                                            }
                                        }
                                    }
                                };
                                ToastContent toastContent = new ToastContent()
                                {
                                    Visual = visual
                                };
                                var toast = new ToastNotification(toastContent.GetXml());
                                toast.ExpirationTime = DateTime.Now.AddDays(1);
                                ToastNotificationManager.CreateToastNotifier().Show(toast);
                            }
                            if (notification.type == "mention")
                            {
                                    
                            }
                        }
                    }
                }
            }
        }
    }
}
