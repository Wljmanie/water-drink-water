using blazor.wa.tbd.Services;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using viewmodels;

namespace blazor.wa.tbd.Pages;

public partial class Friends
{
    [Inject] FriendsService FriendsService { get; set; } = null!;

    private Modal _modal = null!;
    private Modal _joinModal = null!;
    private string _groupName = string.Empty;
    private string _groupCode = string.Empty;
    private bool _groupCodeIsValid = true;

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

    private async Task OnShowJoinModalClick()
    {
        await _joinModal.ShowAsync();

        _groupCodeIsValid = true;
    }

    private async Task OnHideJoinModalClick()
    {
        await _joinModal.HideAsync();
    }

    private async Task OnJoinGroupClick()
    {
        _groupCodeIsValid = await FriendsService.JoinGroup(_groupCode);

        if (_groupCodeIsValid)
        {
            Groups = await FriendsService.GetGroups();

            await _joinModal.HideAsync();
        }
    }
}