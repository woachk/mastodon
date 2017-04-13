using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MastodonAPI
{
    public class HttpConnectionClass
    {
        public HttpClient client;
        public AuthenticateClass auth;
        public HttpConnectionClass(AuthenticateClass authen)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authen.token);
            auth = authen;
        }
    }
}
