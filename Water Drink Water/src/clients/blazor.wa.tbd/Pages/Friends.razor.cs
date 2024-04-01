using blazor.wa.tbd.Services;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using viewmodels;

namespace blazor.wa.tbd.Pages;

public partial class Friends
{
    [Inject] FriendsService FriendsService { get; set; } = null!;

    private Modal _modal = null!;
    private string _groupName = string.Empty;

    private IEnumerable<GroupViewModel> Groups { get; set; } = Array.Empty<GroupViewModel>();

    protected override async Task OnInitializedAsync()
    {
        Groups = await FriendsService.GetGroups();
    }

    private async Task OnAddGroupClick()
    {
        if (await FriendsService.AddGroup(_groupName))
        {
            Groups = await FriendsService.GetGroups();
            
            await _modal.HideAsync();
        }
    }

    private async Task OnShowModalClick()
    {
        await _modal.ShowAsync();
    }

    private async Task OnHideModalClick()
    {
        await _modal.HideAsync();
    }
}