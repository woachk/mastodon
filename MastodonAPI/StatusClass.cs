using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public class StatusClass
    {
        public string id;
        public string uri;
        public string url;
        public AccountClass account;
        string in_reply_to_id;
        string in_reply_to_account_id;
        StatusClass reblog;
        public string content;
        string created_at;
        string reblogs_count;
        string favorites_count;
        public string reblogged;
        public string favourited;
        string sensitive;
        string spoiler_text;
        string visiblity;
        //string media_attachments;
        ApplicationClass application;
        static public List<StatusClass> getTimeline(AuthenticateClass token)
        {
            List<StatusClass> names = new List<StatusClass>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/timelines/home");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = "1";
                        }
                        names.Add(status);
                        status = new StatusClass();
                        status.account = new AccountClass();
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = "1";
                        }
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        } 
                    }
                    /* else if (reader.Value.ToString() == "application")
                    {
                        reader.Read();
                        reader.Read();
                        status.application.name = reader.Value.ToString();
                        reader.Read();
                    } */
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return names;
        }
        static public List<StatusClass> getPublicTimeline(AuthenticateClass token)
        {
            List<StatusClass> names = new List<StatusClass>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/timelines/public");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = reader.Value.ToString();
                        }
                        names.Add(status);
                        status = new StatusClass();
                        status.account = new AccountClass();
                    }
                    /* else if (reader.Value.ToString() == "application")
                    {
                        reader.Read();
                        reader.Read();
                        status.application.name = reader.Value.ToString();
                        reader.Read();
                    } */
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return names;
        }
        static public List<StatusClass> getLocalPublicTimeline(AuthenticateClass token)
        {
            List<StatusClass> names = new List<StatusClass>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/timelines/public?local=true");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = reader.Value.ToString();
                        }
                        names.Add(status);
                        status = new StatusClass();
                        status.account = new AccountClass();
                    }
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return names;
        }
        static public StatusClass postStatus(AuthenticateClass token, string content, string in_reply_to_id, string media_ids, string sensitive, string spoiler_text)
        {
            StatusClass status = new StatusClass();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            string request = "status=" + content;
            if (content == null)
            {
                return null;
            }
            if (in_reply_to_id != null)
            {
                request = request + "&in_reply_to_id=" + in_reply_to_id;
            }
            if (media_ids != null)
            {
                request = request + "&media_ids=" + media_ids;
            }
            if (sensitive != null)
            {
                request = request + "&sensitive=" + sensitive;
            }
            if (spoiler_text != null)
            {
                request = request + "&spoiler_text=" + spoiler_text;
            }
            HttpContent request_content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + token.server + "/api/v1/statuses", request_content);
            HttpResponseMessage message = msg.Result;
            string json = (message.Content).ReadAsStringAsync().Result;
            status.content = content;
            status.in_reply_to_id = in_reply_to_id;
            status.sensitive = sensitive;
            status.spoiler_text = spoiler_text;
            return status;
        }
        static public StatusClass parseToot(string json)
        {
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return status;
        }
        static public List<StatusClass>[] GetStatusContext(AuthenticateClass token, StatusClass ogstatus)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/statuses/" + ogstatus.id + "/context");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            List<StatusClass>[] statuses = new List<StatusClass>[2];
            statuses[0] = new List<StatusClass>();
            statuses[1] = new List<StatusClass>();
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            int i = 0;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = reader.Value.ToString();
                        }
                        statuses[i].Add(status);
                        status = new StatusClass();
                        status.account = new AccountClass();
                    }
                    /* else if (reader.Value.ToString() == "application")
                    {
                        reader.Read();
                        reader.Read();
                        status.application.name = reader.Value.ToString();
                        reader.Read();
                    } */
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    if (reader.Value != null)
                    {
                        if (reader.Value.ToString() == "descendants")
                        {
                            i++;
                            status = new StatusClass();
                            status.account = new AccountClass();
                        }
                    }
                }
            }
            return statuses;
        }
        static public void favourite_toot(string id, AuthenticateClass token)
        {
            StatusClass status = new StatusClass();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            string request = "";
            HttpContent request_content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + token.server + "/api/v1/statuses/" + id + "/favourite", request_content);
            HttpResponseMessage message = msg.Result;
            string json = (message.Content).ReadAsStringAsync().Result;
            return;
        }
        static public void boost_toot(string id, AuthenticateClass token)
        {
            StatusClass status = new StatusClass();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            string request = "";
            HttpContent request_content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(request));
            Task<HttpResponseMessage> msg = client.PostAsync("https://" + token.server + "/api/v1/statuses/" + id + "/reblog", request_content);
            HttpResponseMessage message = msg.Result;
            string json = (message.Content).ReadAsStringAsync().Result;
            return;
        }
        static public StatusClass GetStatus(AuthenticateClass token, StatusClass ogstatus)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            Task<HttpResponseMessage> message = client.GetAsync("https://" + token.server + "/api/v1/statuses/" + ogstatus.id + "/context");
            message.Wait();
            HttpResponseMessage msg = message.Result;
            String json = (msg.Content).ReadAsStringAsync().Result;
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            bool t = false;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "id")
                    {
                        reader.Read();
                        status.id = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "uri")
                    {
                        reader.Read();
                        status.uri = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        reader.Read();
                        status.url = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "content")
                    {
                        reader.Read();
                        status.content = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "mentions")
                    {
                        while (true)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndArray)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.Value.ToString() == "reblogged")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.reblogged = reader.Value.ToString();
                        }
                    }
                    else if (reader.Value.ToString() == "favourited")
                    {
                        reader.Read();
                        if (reader.Value != null)
                        {
                            status.favourited = reader.Value.ToString();
                        }
                    }
                    /* else if (reader.Value.ToString() == "application")
                    {
                        reader.Read();
                        reader.Read();
                        status.application.name = reader.Value.ToString();
                        reader.Read();
                    } */
                    else if (reader.Value.ToString() == "account")
                    {
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                if (reader.Value.ToString() == "acct")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                else if (reader.Value.ToString() == "id")
                                {
                                    reader.Read();
                                    status.account.acct = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "display_name")
                                {
                                    reader.Read();
                                    status.account.display_name = reader.Value.ToString();
                                }
                                if (reader.Value.ToString() == "avatar")
                                {
                                    reader.Read();
                                    status.account.avatar = reader.Value.ToString();
                                    break;
                                }
                                if (reader.Value.ToString() == "statuses_count")
                                {
                                    reader.Read();
                                    status.account.statuses_count = reader.Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return status;
        }
    }
}

