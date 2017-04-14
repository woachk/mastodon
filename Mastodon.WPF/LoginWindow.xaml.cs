using MastodonAPI;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Mastodon.WPF
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string servername = ServerBox.Text;

            ApplicationTokenClass apptoken = new ApplicationTokenClass(servername, "Mastodon for Windows", "read write follow", "urn:ietf:wg:oauth:2.0:oob");

            AuthenticateClass oauth_token = new AuthenticateClass(apptoken, UserNameBox.Text, PasswordHereBox.Password);
            if (apptoken.client_id == null)
            {
                FailedTextBox.Text = "Login failed.";

                return;

            }

            if (oauth_token.token == null)
            {
                FailedTextBox.Text = "Login failed.";
                return;
            }
            var rkey = (Registry.CurrentUser.OpenSubKey("Software", true)).OpenSubKey("Mastodon");
            rkey.SetValue("ServerName", servername);
            rkey.SetValue("Token", oauth_token.token);
        }
    }
}
