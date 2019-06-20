using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BadApiCodingChallenge.Models
{
    public class TweetModel
    {
        public string Id { get; set; }
        public DateTime Stamp { get; set; }
        public string Text { get; set; }

        public TweetModel() { }
    }
}