using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorRssReader.Pages.Organize
{
    public class IndexModel : BlazorComponent
    {
        [Inject] NotificationService NotificationService { get; set; }
        [Inject] FeedService Service { get; set; }
        [Inject] ILogger<IndexModel> Logger { get; set; }

        public bool IsBusy { get; set; }

        public List<Feed> Feeds { get; set; }

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadFeedsAsync();
            IsBusy = false;
        }

        private async Task LoadFeedsAsync()
        {
            await Task.Run(() =>
                  {
                        Feeds = Service.GetFeeds();
                  });
        }

        public async Task OnUnfollowFeed(Guid feedId)
        {
            Logger.LogInformation("feed id {feedId}", feedId);
            await Service.DeleteFeed(feedId);
            NotificationService.NotifyFeedChange();
            await LoadFeedsAsync();
        }
    }
}