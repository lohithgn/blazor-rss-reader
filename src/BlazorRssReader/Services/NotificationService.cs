using System;

namespace BlazorRssReader.Services
{
    public class NotificationService
    {
        public event Action OnFeedUpdated;
        public void NotifyFeedChange()
        {
            OnFeedUpdated?.Invoke();
        }
    }
}
