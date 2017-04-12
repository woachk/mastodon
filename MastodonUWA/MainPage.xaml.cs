using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
using MastodonAPI;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Background;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace MastodonUWA
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<object> TootCollectionBind; 
        IAsyncAction tootrefresh;
        public static string getServerName()
        {
            var serverfile = ApplicationData.Current.LocalFolder.GetFileAsync("server.txt");
            serverfile.AsTask().Wait();
            var serverfile_o = serverfile.GetResults();
            var ioop = FileIO.ReadTextAsync(serverfile_o);
            ioop.AsTask().Wait();
            return ioop.GetResults();
        }
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            tootrefresh.Cancel();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "PublicTimeline");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            if (isDark)
            {
                SPanel.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }
            TootCollectionBind = new ObservableCollection<object>();
            TootContainer.DataContext = TootCollectionBind;
            base.OnNavigatedTo(e);
            var settings = (string)e.Parameter;
            AuthenticateClass token = GetToken.getAuthClass();
            //List<StatusClass> tootlist = null;
            StatusClass_new[] tootlist;
            string baseuri = "https://" + token.server;
            if (settings == "notifications")
            {
                baseuri = baseuri + "/api/v1/streaming/user";
            }
            else if (settings == "PublicTimeline")
            {
                baseuri = baseuri + "/api/v1/streaming/public";
            }
            else if (settings == "LocalPublicTimeline")
            {
                baseuri = baseuri + "/api/v1/streaming/public?local=true";
            }
            else
            {
                baseuri = baseuri + "/api/v1/streaming/user";
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            tootrefresh = ThreadPool.RunAsync(async (source) =>
             {
             var msg = await client.GetStreamAsync(baseuri);
             while (1 == 1)
             {
                 var st = new StreamReader(msg);
                     string text = st.ReadLine();
                     if (text == null)
                     { 
                        msg = await client.GetStreamAsync(baseuri); // reset the connection
                     }
                     else if ((text.ToArray())[0] != ':')
                     {
                        string[] text2 = text.Split(':');
                        var intermediate = (text.Split('\n'));
                        string text3 = intermediate.Aggregate((a, b) => a + b);
                         string stdata = st.ReadLine();
                         stdata = new string(stdata.Skip(5).ToArray());
                         if (text2[1] == " delete")
                         {
                             st.DiscardBufferedData();
                         }
                         if (text2[1] == " update")
                         {
                             while (stdata.Last() != '}')
                             {
                                 stdata = stdata + st.ReadLine();
                             }
                             dynamic status = StatusClass_new.parseToot(stdata);
                             // Insert at the beginning
                             await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                                {
                                    Toot toot = new Toot(status);
                                    TootCollectionBind.Add(toot);
                                }));
                        }
                        else if (text2[1] == " notification")
                         {
                             dynamic notification = NotificationClass_new.parseNotification(stdata);
                             if (notification.id != null)
                             {
                                 if (notification.type == "favourite")
                                 {
                                     await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                                     {
                                         TextBlock block = new TextBlock();
                                         Toot toot;
                                         block.Text = notification.account.display_name + " favourited your post.";
                                         toot = new Toot(notification.status);
                                         TootCollectionBind.Add(block);
                                         TootCollectionBind.Add(toot);
                                     }));
                                 }
                                 if (notification.type == "reblog")
                                 {
                                     await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                                     {
                                         TextBlock block = new TextBlock();
                                         Toot toot;
                                         block.Text = notification.account.display_name + " boosted your post.";
                                         toot = new Toot(notification.status);
                                         TootCollectionBind.Add(block);
                                         TootCollectionBind.Add(toot);
                                     }));
                                 }
                                 if (notification.type == "follow")
                                 {
                                     await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                                     {
                                         TextBlock block = new TextBlock();
                                         block.Text = notification.account.display_name + " now follows you.";
                                         TootCollectionBind.Add(block);
                                     }));
                                 }
                                 if (notification.type == "mention")
                                 { 
                                     await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                                     {
                                         Toot toot;
                                         toot = new Toot(notification.status);
                                         TootCollectionBind.Add(toot);
                                     }));
                                 }
                             }
                         }
                     }
                 }
             }
                );

            if (settings != "notifications")
            {
                if (settings == "PublicTimeline")
                {
                    //tootlist = StatusClass.getPublicTimeline(token);
                    tootlist = StatusClass_new.GetPublicTimeline(token);
                }
                else if (settings == "LocalPublicTimeline")
                {
                    tootlist = StatusClass_new.GetPublicLocalTimeline(token);
                }
                else
                {
                    tootlist = StatusClass_new.GetTimeline(token);
                }
                for (int i = tootlist.Length -1 ; i >= 0; i--)
                {
                    Toot toot;
                    toot = new Toot(tootlist[i]);
                    TootCollectionBind.Add(toot);
                }
            }
            else
            {
                dynamic notifications = NotificationClass_new.getNotifications(token);
                for (int i = notifications.Count - 1; i >= 0; i--)
                {
                    if (notifications[i].id != null)
                    {
                        if (notifications[i].type == "favourite")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " favourited your post.";
                            toot = new Toot(notifications[i].status);
                            TootCollectionBind.Add(block);
                            TootCollectionBind.Add(toot);
                        }
                        if (notifications[i].type == "reblog")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " boosted your post.";
                            toot = new Toot(notifications[i].status);
                            TootCollectionBind.Add(block);
                            TootCollectionBind.Add(toot);
                        }
                        if (notifications[i].type == "follow")
                        {
                            TextBlock block = new TextBlock();
                            block.Text = notifications[i].account.display_name + " now follows you.";
                            TootCollectionBind.Add(block);
                        }
                        if (notifications[i].type == "mention")
                        {
                            Toot toot;
                            toot = new Toot(notifications[i].status);
                            TootCollectionBind.Add(toot);
                        }
                    }
                }
            }
        }
        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            tootrefresh.Cancel();
            rootFrame.Navigate(typeof(MainPage), "LocalPublicTimeline");
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            tootrefresh.Cancel();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "notifications");
        }

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            tootrefresh.Cancel();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "Timeline");
        }

        private void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            tootrefresh.Cancel();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(SettingsPage), e);
        }

        private async void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new WritingToot();
            await contentDialog.ShowAsync();
        }
    }
}
