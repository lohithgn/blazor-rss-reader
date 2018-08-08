using System;

namespace BlazorRssReader.Models
{
    public class FeedItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Descriiption { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public string Creator { get; set; }
        public string Category { get; set; }
        public string Encoded { get; set; }
    }
}
