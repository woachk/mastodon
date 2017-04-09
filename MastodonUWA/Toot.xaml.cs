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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MastodonUWA
{
    public sealed partial class Toot : UserControl
    {
        public Toot()
        {
            this.InitializeComponent();
        }
        public Toot(string username, string display_name, string contents, string avatar)
        {
            this.InitializeComponent();
            TootContents.Text = contents;
            UserName.Text = display_name;
            UserImage.Source = new BitmapImage(new Uri(avatar));

        }
    }
}
