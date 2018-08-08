using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using BlazorRssReader.Models;
using System.Collections.Generic;
using Blazored.Storage;
using Microsoft.Toolkit.Parsers;

namespace BlazorRssReader.Services
{
    public class FeedService
    {
        readonly HttpClient _client;
        readonly ILogger<FeedService> _logger;
        readonly ILocalStorage _localStorage;
        public FeedService(HttpClient httpClient, ILocalStorage localStorage, ILogger<FeedService> logger)
        {
            _client = httpClient;
            _logger = logger;
            _localStorage = localStorage;
        }

        public List<Feed> GetFeeds()
        {
            var feeds = _localStorage.GetItem<List<Feed>>("blazor.rss.feeds") ?? new List<Feed>();
            return feeds;
        }

        public Feed GetFeedDetails(string feedId)
        {
            var feeds = GetFeeds();
            var feed = feeds.SingleOrDefault(f => f.Id.ToString() == feedId);
            return feed;
        }
        public async Task<Feed> GetFeedMetadata(string feedUrl)
        {
            try
            {
                Feed feed = null;
                SyndicationFeed syndicationFeed = await GetSyndicationFeed(feedUrl);
                if (syndicationFeed != null)
                {
                    feed = new Feed
                    {
                        Id = Guid.NewGuid(),
                        Description = syndicationFeed.Description?.Text,
                        ImageUrl = syndicationFeed.ImageUrl?.AbsoluteUri,
                        Items = syndicationFeed.Items.Take(3).Select(i => new FeedItem
                        {
                            Title = i.Title.Text,
                            Descriiption = i.Summary.Text,
                            Url = i.Links[0].Uri.AbsoluteUri
                        }).ToList(),
                        Title = syndicationFeed.Title?.Text,
                        Url = feedUrl,
                        WebsiteUrl = syndicationFeed.Links.SingleOrDefault(l => l.RelationshipType == "alternate")?.Uri.AbsoluteUri,
                        LastUpdate = syndicationFeed.LastUpdatedTime.DateTime
                    };
                }
                return feed;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error Parsing Feed", null);
                throw;
            }
        }

        public async Task<List<RssSchema>> GetFeedItems(Feed selectedFeed)
        {
            try
            {
                var rawContent = await GetRawContent(selectedFeed.Url);
                var feedContent = GetFeedContent(rawContent);
                List<RssSchema> feedItems = new RssParser().Parse(feedContent).ToList();
                return feedItems;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error Parsing Feed", null);
                throw;
            }
        }

        public async Task AddFeed(Feed feed)
        {
            await Task.Run(() =>
            {
                feed.Items = null;
                var existingFeeds = GetFeeds();
                existingFeeds.Add(feed);
                _localStorage.SetItem("blazor.rss.feeds", existingFeeds);
            }).ConfigureAwait(false);
        }

        public void UpdateFeed(string feedId, string feedTitle)
        {
            var feeds = GetFeeds();
            var feed = feeds.SingleOrDefault(f => f.Id.ToString() == feedId);
            feed.Title = feedTitle;
            _localStorage.SetItem("blazor.rss.feeds", feeds);
        }

        public async Task DeleteFeed(Guid feedId)
        {
            await Task.Run(() =>
            {
                var existingFeeds = GetFeeds();
                var feedToDelete = existingFeeds.SingleOrDefault(f => f.Id == feedId);
                existingFeeds.Remove(feedToDelete);
                _localStorage.SetItem("blazor.rss.feeds", existingFeeds);
            }).ConfigureAwait(false);
        }

        private async Task<SyndicationFeed> GetSyndicationFeed(string feedUrl)
        {
            var rawContent = await GetRawContent(feedUrl);
            var feedContent = GetFeedContent(rawContent);
            var syndicationFeed = SyndicationFeed.Load(XmlReader.Create(new StringReader(feedContent)));
            return syndicationFeed;
        }

        private string GetFeedContent(string rawContent)
        {
            var xDoc = XDocument.Load(new StringReader(rawContent));
            var node = xDoc.XPathSelectElement("//query/results/*[1]");
            var reader = node.CreateReader();
            reader.MoveToContent();
            var feedString = reader.ReadOuterXml();
            return feedString;
        }

        private async Task<string> GetRawContent(string feedUrl)
        {
            var url = $"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20xml%20where%20url%20%3D%20'{feedUrl}'&format=xml";
            var rawContent = await _client.GetStringAsync(url);
            return rawContent;
        }
    }
}
