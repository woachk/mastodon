using System;
using System.Collections.Generic;
using System.Text;

namespace MastodonAPI
{
    public class StatusContext
    {
        public StatusClass_new[] ancestors { get; set; }
        public StatusClass_new[] descendants { get; set; }
    }
}
