using Blazored.Storage;
using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Parsers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BlazorRssReader.Pages.Listing
{
    public class IndexModel : BlazorComponent
    {
        [Parameter] protected string FeedId { get; set; } = "";
        [Inject] ILogger<IndexModel> Logger { get; set; }
        [Inject] FeedService FeedService { get; set; }
        [Inject] IUriHelper UriHelper { get; set; }
        [Inject] Session Session { get; set; }
        [Inject] ILocalStorage Storage { get; set; }

        public Feed Feed { get; set; } = null;
        public List<RssSchema> FeedItems { get; set; } = null;
        public bool IsBusy { get; set; } = false;
        public string ViewType { get; set; } 

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(FeedId)) return;

            IsBusy = true;
            ClearFeedItems();
            SetViewType();
            LoadFeedDetails();
            await LoadFeedItems();
            IsBusy = false;
        }

        private void ClearFeedItems()
        {
            FeedItems = null;
            Session.SelectedFeedItems = null;
        }

        private void SetViewType()
        {
            var viewType = Storage.GetItem<string>("blazor.rss.viewtype");
            ViewType = string.IsNullOrEmpty(viewType) ? "Title" : viewType;
        }

        public void OnArticleViewClick()
        {
            Storage.SetItem("blazor.rss.viewtype","Article");
            ViewType = "Article";
        }

        public void OnTitleViewClick()
        {
            Storage.SetItem("blazor.rss.viewtype", "Title");

            ViewType = "Title";
        }

        public void OnCardsViewClick()
        {
            Storage.SetItem("blazor.rss.viewtype", "Cards");

            ViewType = "Cards";
        }

        public void OnMagazineViewClick()
        {
            Storage.SetItem("blazor.rss.viewtype", "Magazine");

            ViewType = "Magazine";
        }
        
        public void OnFeedItemClick(RssSchema item)
        {
            Session.SelectedEntry = item;
            Session.SelectedFeed = Feed;
            Session.SelectedFeedItems = FeedItems;
            UriHelper.NavigateTo($"/entry/{HttpUtility.UrlEncode(item.InternalID)}");
        }

        public async Task OnRefreshFeed()
        {
            IsBusy = true;
            await LoadFeedItems();
            IsBusy = false;
        }

        private async Task LoadFeedItems()
        {
            if(Session.SelectedFeedItems != null)
            {
                FeedItems = Session.SelectedFeedItems;
            }
            else
            {
                FeedItems = await FeedService.GetFeedItems(Feed);
            }
        }

        private void LoadFeedDetails()
        {
            Feed = FeedService.GetFeedDetails(FeedId);
        }

      
    }
}
