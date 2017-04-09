using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MastodonUWA
{
    public sealed partial class Toot : UserControl
    {
        public string toot_id;
        public Toot()
        {
            this.InitializeComponent();
        }
        public Toot(string username, string display_name, string contents, string avatar, string id)
        {
            this.InitializeComponent();
            TootContents.NavigateToString(contents);
            UserName.Text = display_name + "\n" + "@" + username;
            toot_id = id;
            UserImage.Source = new BitmapImage(new Uri(avatar));
        }
    }
}
