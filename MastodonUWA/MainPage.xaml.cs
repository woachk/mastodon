﻿using System;
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

namespace MastodonUWA
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string getServerName()
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
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "PublicTimeline");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var settings = (string)e.Parameter;
            AuthenticateClass token = new AuthenticateClass();
            token.appname = null;
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = getServerName();
            List<StatusClass> tootlist = null;
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
            IAsyncAction tootrefresh = ThreadPool.RunAsync(async (source) =>
             {
                 while (1 == 1)
                 {
                     HttpResponseMessage msg = await client.GetAsync(baseuri);
                     string text = await msg.Content.ReadAsStringAsync();
                     if ((text.ToArray())[0] != ':')
                     {
                         string[] text2 = text.Split('\n');
                         string[] text3 = text2[0].Split(':');
                         string stdata = (text2[1].Skip(5)).ToString();

                     }
                 }
             }
                );

            if (settings != "notifications")
            {
                if (settings == "PublicTimeline")
                {
                    tootlist = StatusClass.getPublicTimeline(token);
                }
                else if (settings == "LocalPublicTimeline")
                {
                    tootlist = StatusClass.getLocalPublicTimeline(token);
                }
                else
                {
                    tootlist = StatusClass.getTimeline(token);
                }
                for (int i = 0; i < tootlist.Count; i++)
                {
                    Toot toot;
                    if (tootlist[i].account.acct != null)
                    {
                        toot = new Toot(tootlist[i].account.acct, tootlist[i].account.display_name, tootlist[i].content, tootlist[i].account.avatar, tootlist[i].id);
                        TootContainer.Items.Add(toot);
                    }
                }
            }
            else
            {
                List<NotificationClass> notifications = NotificationClass.getNotifications(token);
                for (int i = 0; i < notifications.Count; i++)
                {
                    if (notifications[i].id != null)
                    {
                        if (notifications[i].type == "favourite")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " favourited your post.";
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(block);
                            TootContainer.Items.Add(toot);
                        }
                        if (notifications[i].type == "reblog")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " boosted your post.";
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(block);
                            TootContainer.Items.Add(toot);
                        }
                        if (notifications[i].type == "follow")
                        {
                            TextBlock block = new TextBlock();
                            block.Text = notifications[i].account.display_name + " now follows you.";
                            TootContainer.Items.Add(block);
                        }
                        if (notifications[i].type == "mention")
                        {
                            Toot toot;
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(toot);
                        }
                    }
                }
            }
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "LocalPublicTimeline");
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "notifications");
        }

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "Timeline");
        }

        private void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
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
