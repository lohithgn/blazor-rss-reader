using BlazorRssReader.Services;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorRssReader.Pages.Details
{
    public class DetailsModel : BlazorComponent
    {
        [Parameter] protected string EntryId { get; set; } 
        [Inject] protected Session Session { get; set; } 
    }
}