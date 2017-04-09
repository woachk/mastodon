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
        string id;
        string uri;
        string url;
        public AccountClass account;
        string in_reply_to_id;
        string in_reply_to_account_id;
        StatusClass reblog;
        public string content;
        string created_at;
        string reblogs_count;
        string favorites_count;
        string reblogged;
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
            int i = 0;
            while (!reader.Read())
            {
                if (reader.Value.ToString() == "StartArray")
                {
                    Debugger.Break();
                }
                if (reader.Value.ToString() == "id")
                {
                    reader.Read();
                    names[i].id = reader.Value.ToString();
                }
                if (reader.Value.ToString() == "uri")
                {
                    reader.Read();
                    names[i].uri = reader.Value.ToString();
                }
                if (reader.Value.ToString() == "url")
                {
                    reader.Read();
                    names[i].url = reader.Value.ToString();

                }
                if (reader.Value.ToString() == "account")
                {
                    reader.Read();
                    while(!reader.Read())
                    {
                        if (reader.Value.ToString() == "acct")
                        {
                            reader.Read();
                            names[i].account.acct = reader.Value.ToString();
                        }
                        if (reader.Value.ToString() == "display_name")
                        {
                            reader.Read();
                            names[i].account.display_name = reader.Value.ToString();
                        }
                        if (reader.Value.ToString() == "avatar")
                        {
                            reader.Read();
                            names[i].account.avatar = reader.Value.ToString();
                        }
                        if (reader.Value.ToString() == "statuses_count")
                        {
                            reader.Read();
                            names[i].account.statuses_count = reader.Value.ToString();
                            break;
                        }
                    }
                }
                if (reader.Value.ToString() == "content")
                {
                    reader.Read();
                    names[i].content = reader.Value.ToString();
                }
                if (reader.Value.ToString() == "content")
                {
                    reader.Read();
                    names[i].content = reader.Value.ToString();
                }
                if (reader.Value.ToString() == "reblogged")
                {
                    reader.Read();
                    names[i].content = reader.Value.ToString();
                    i++;
                }
                // ugly code warning
                if (reader.Value.ToString() == "application")
                {
                    reader.Read();
                    // name
                    reader.Read();
                    names[i].application.name = reader.Value.ToString();
                    reader.Read();
                    // website
                    reader.Read();
                    names[i].application.website = reader.Value.ToString();
                    i++;
                }
            }
            return names;
        }
    }
}
