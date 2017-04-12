﻿using System;
using System.Collections.Generic;
using System.Text;
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
    public class AccountClass
    {
        public string id { get; set; }
        public string username { get; set; }
        public string acct { get; set; }
        public string display_name { get; set; }
        public string note { get; set; }
        public string url { get; set; }
        public string avatar { get; set; }
        public string header { get; set; }
        public string locked { get; set; }
        public string created_at { get; set; }
        public string followers_count { get; set; }
        public string following_count { get; set; }
        public string statuses_count { get; set; }

    }
}
