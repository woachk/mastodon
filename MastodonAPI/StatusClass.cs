using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
        string favourited;
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
                            status.reblogged = reader.Value.ToString();
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
    }
}

