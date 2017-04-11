using MastodonAPI;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Notifications;

namespace MastodonUWA
{
    class SyncBackgroundTask : IBackgroundTask
    {
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
                        NotificationClass notification = NotificationClass.parseNotification(stdata);
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
