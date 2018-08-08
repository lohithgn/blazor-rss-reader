using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BlazorRssReader.Pages
{
    public class IndexModel : BlazorComponent
    {
        [Inject] private ILogger<IndexModel> Logger { get; set; }
        [Inject] private FeedService FeedService { get; set; }
        [Inject] private IUriHelper UriHelper { get; set; }

        protected override void OnInit()
        {
            List<Models.Feed> feeds = FeedService.GetFeeds();
            if (feeds.Count > 0)
            {
                UriHelper.NavigateTo($"/feed/{feeds[0].Id}");
            }
        }
    }
}