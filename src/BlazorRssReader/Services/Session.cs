using BlazorRssReader.Models;
using Microsoft.Toolkit.Parsers;
using System.Collections.Generic;

namespace BlazorRssReader.Services
{
    public class Session
    {
        public Feed SelectedFeed { get; set; } = null;
        public List<RssSchema> SelectedFeedItems { get; set; } = null;
        public RssSchema SelectedEntry { get; set; } = null;
    }
}
