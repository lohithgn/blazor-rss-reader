using BlazorRssReader.Models;
using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Layouts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorRssReader.Shared
{
    public class MainLayoutModel : BlazorLayoutComponent, IDisposable
    {
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private FeedService FeedService { get; set; }
        public List<Feed> MenuItems { get; set; }

        protected override async Task OnInitAsync()
        {
            NotificationService.OnFeedUpdated += UpdateFeeds;
            await LoadMenuItems();
        }

        private async Task UpdateFeeds()
        {
            await LoadMenuItems();
            StateHasChanged();
        }

        private async Task LoadMenuItems()
        {
            MenuItems = await FeedService.GetFeeds();
        }

        public void Dispose()
        {
            NotificationService.OnFeedUpdated -= UpdateFeeds;
        }
    }
}
