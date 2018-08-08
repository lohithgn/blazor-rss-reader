using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.Toolkit.Parsers
{
    public class RssParser : IParser<RssSchema>
    {

        /// <summary>
        /// Parse an RSS content string into RSS Schema.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>Strong type.</returns>

        public IEnumerable<RssSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            var doc = XDocument.Parse(data);
            var type = BaseRssParser.GetFeedType(doc);
            BaseRssParser rssParser;
            if (type == RssType.Rss)
            {
                rssParser = new Rss2Parser();
            }
            else
            {
                rssParser = new AtomParser();
            }
            return rssParser.LoadFeed(doc);
        }
    }
}
