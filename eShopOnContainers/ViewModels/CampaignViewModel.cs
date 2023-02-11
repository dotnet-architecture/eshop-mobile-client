using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

public partial class CampaignViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ObservableCollectionEx<CampaignItem> _campaigns = new ();

    public IReadOnlyList<CampaignItem> Campaigns => _campaigns;

    public CampaignViewModel(
        IAppEnvironmentService appEnvironmentService,
        INavigationService navigationService, ISettingsService settingsService)
        : base(navigationService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;
    }

    public override async Task InitializeAsync()
    {
        await IsBusyFor(
            async () =>
            {
                // Get campaigns by user
                var campaigns = await _appEnvironmentService.CampaignService.GetAllCampaignsAsync(_settingsService.AuthAccessToken);
                _campaigns.ReloadData(campaigns);
            });
    }

    [RelayCommand]
    private async Task GetCampaignDetailsAsync(CampaignItem campaign)
    {
        if (campaign is null)
        {
            return;
        }

        await NavigationService.NavigateToAsync(
            "CampaignDetails",
            new Dictionary<string, object> { { nameof(Campaign.Id), campaign.Id } });
    }
}