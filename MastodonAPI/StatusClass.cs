﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class StatusClass_new
    {
        public string id { get; set; }
        public string uri { get; set; }
        public string url { get; set; }
        public AccountClass account { get; set; }
        public MediaAttachmentsClass[] media_attachments { get; set; }
        public string in_reply_to_id { get; set; }
        public string in_reply_to_account_id { get; set; }
        //public StatusClass reblog { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public string reblogs_count { get; set; }
        public string favorites_count { get; set; }
        public string reblogged { get; set; }
        public string favourited { get; set; }
        public string sensitive { get; set; }
        public string spoiler_text { get; set; }
        public string visiblity { get; set; }
        public ApplicationClass application { get; set; }
        static public StatusClass_new parseToot(string json)
        {
            return JsonConvert.DeserializeObject<StatusClass_new>(json);
        }
        static public StatusClass_new GetStatus(HttpConnectionClass token, StatusClass_new ogstatus)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/statuses/" + ogstatus.id);
            message.Wait();
            HttpResponseMessage msg = message.Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusClass_new();
            }
            String json = (msg.Content).ReadAsStringAsync().Result;
            return parseToot(json);
        }
        static public StatusClass_new[] GetTimeline(HttpConnectionClass token)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/timelines/home");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusClass_new[0];
            }
            StatusClass_new[] obj = JsonConvert.DeserializeObject<StatusClass_new[]>(json);
            return obj;
        }
        static public StatusClass_new[] GetAccountStatuses(HttpConnectionClass token, AccountClass account)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/accounts/"+ account.id + "/statuses");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusClass_new[0];
            }
            StatusClass_new[] obj = JsonConvert.DeserializeObject<StatusClass_new[]>(json);
            return obj;
        }
        static public StatusClass_new[] GetPublicTimeline(HttpConnectionClass token)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/timelines/public/?local=true");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusClass_new[0];
            }
            StatusClass_new[] obj = JsonConvert.DeserializeObject<StatusClass_new[]>(json);
            return obj;
        }
        static public StatusClass_new[] GetPublicLocalTimeline(HttpConnectionClass token)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/timelines/public");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusClass_new[0];
            }
            StatusClass_new[] obj = JsonConvert.DeserializeObject<StatusClass_new[]>(json);
            return obj;
        }
        static public StatusContext GetStatusContext(HttpConnectionClass token, StatusClass_new ogstatus)
        {
            Task<HttpResponseMessage> message = token.client.GetAsync("https://" + token.auth.server + "/api/v1/statuses/" + ogstatus.id + "/context");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            if (!msg.IsSuccessStatusCode)
            {
                return new StatusContext
                {
                    ancestors = new StatusClass_new[0],
                    descendants = new StatusClass_new[0]
                };
            }
            String json = (msg.Content).ReadAsStringAsync().Result;
            StatusContext obj = JsonConvert.DeserializeObject<StatusContext>(json);
            return obj;
        }
    }
}
