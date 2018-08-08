using Microsoft.AspNetCore.Blazor.Components;
using System.Threading.Tasks;
using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.Logging;

namespace BlazorRssReader.Pages.Add
{
    public class AddFeedModel : BlazorComponent
    {
        [Inject] NotificationService NotificationService { get; set; }
        [Inject] FeedService FeedService { get; set; }
        [Inject] IUriHelper UriHelper { get; set; }
        [Inject] ILogger<AddFeedModel> Logger { get; set; }

        public string FeedUrl { get; set; }
        public bool IsBusy { get; set; }
        public bool IsError { get; set; }
        public Feed Feed { get; set; } = null;
        public async Task OnCheckFeed()
        {
            if (string.IsNullOrEmpty(FeedUrl)) return;

            IsError = false;
            IsBusy = true;
            Feed = null;
            try
            {
                Feed = await FeedService.GetFeedMetadata(FeedUrl);
            }
            catch
            {
                IsError = true;
            }
            finally
            {
                IsBusy=false;
            }
        }

        public async Task OnFollowFeed()
        {
            await FeedService.AddFeed(Feed);
            NotificationService.NotifyFeedChange();
            UriHelper.NavigateTo($"/feed/{Feed.Id}");
        }
    }
}