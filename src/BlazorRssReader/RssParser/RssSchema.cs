using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Parsers
{
    /// <summary>
    /// Implementation of the RssSchema class.
    /// </summary>
    public class RssSchema : SchemaBase
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets summary.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets content.
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// Gets or sets image Url.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets extra Image Url.
        /// </summary>
        public string ExtraImageUrl { get; set; }

        /// <summary>
        /// Gets or sets media Url.
        /// </summary>
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets feed Url.
        /// </summary>
        public string FeedUrl { get; set; }

        /// <summary>
        /// Gets or sets author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets publish Date.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets item's categories.
        /// </summary>
        public IEnumerable<string> Categories { get; set; }
    }
}
