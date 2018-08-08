using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.Logging;

namespace BlazorRssReader.Pages.Organize
{
    public class EditModel : BlazorComponent
    {
        [Parameter] protected string FeedId { get; set; }

        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private IUriHelper UriHelper { get; set; }
        [Inject] private ILogger<EditModel> Logger { get; set; }  

        protected bool IsBusy { get; set; } = false;

        [Inject] private FeedService FeedService { get; set; }

        public string FeedTitle { get; set; }

        private Feed feed;

        protected override void OnInit()
        {
            IsBusy = true;
            LoadFeed();
            IsBusy = false;
        }

        private void LoadFeed()
        {
            feed = FeedService.GetFeedDetails(FeedId);
            Logger.LogInformation("Feed {0}",feed);
            FeedTitle = feed.Title;
        }

        public void OnSaveClick()
        {
            FeedService.UpdateFeed(FeedId, FeedTitle);
            NotificationService.NotifyFeedChange();
            UriHelper.NavigateTo("/organize");
        }
    }
}