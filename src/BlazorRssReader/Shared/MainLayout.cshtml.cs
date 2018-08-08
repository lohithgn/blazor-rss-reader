using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Layouts;
using System;
using System.Collections.Generic;

namespace BlazorRssReader.Shared
{
    public class MainLayoutModel : BlazorLayoutComponent, IDisposable
    {
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private FeedService FeedService { get; set; }
        public List<Feed> MenuItems { get; set; }

        protected override void OnInit()
        {
            NotificationService.OnFeedUpdated += UpdateFeeds;
            LoadMenuItems();
        }

        private void UpdateFeeds()
        {
            LoadMenuItems();
            StateHasChanged();
        }

        private void LoadMenuItems()
        {
            MenuItems = FeedService.GetFeeds();
        }

        public void Dispose()
        {
            NotificationService.OnFeedUpdated -= UpdateFeeds;
        }
    }
}
