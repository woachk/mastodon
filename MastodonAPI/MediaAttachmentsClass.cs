using System;
using System.Collections.Generic;
using System.Text;

namespace MastodonAPI
{
    public class MediaAttachmentsClass
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string remote_url { get; set; }
        public string text_url { get; set; }
        public string preview_url { get; set; }
    }
}
