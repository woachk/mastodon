﻿using MastodonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mastodon.WPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HttpConnectionClass token = null;
            int logged_in = 1;
            try
            {
                token = new HttpConnectionClass(GetToken.getAuthClass());
            }
            catch
            {
                logged_in = 0;
                LoginWindow window = new LoginWindow();
                window.Show();
            }
            if (logged_in == 1)
            {
                string baseuri = "https://" + token.auth.server;
                var tootlist = StatusClass_new.GetPublicTimeline(token);
                for (int i = 0; i < tootlist.Length - 1; i++)
                {
                    Toot toot;
                    toot = new Toot(tootlist[i]);
                    Toots.Items.Insert(0, toot);
                }
            }
        }
        public override void BeginInit()
        {
            base.BeginInit();
        }   
    }
}
