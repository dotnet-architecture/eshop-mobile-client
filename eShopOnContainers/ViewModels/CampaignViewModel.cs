using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

public class CampaignViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ObservableCollectionEx<CampaignItem> _campaigns;

    public IList<CampaignItem> Campaigns => _campaigns;

    public ICommand GetCampaignDetailsCommand { get; }

    public CampaignViewModel(
        IAppEnvironmentService appEnvironmentService,
        IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;

        _campaigns = new ObservableCollectionEx<CampaignItem>();

        GetCampaignDetailsCommand = new AsyncRelayCommand<CampaignItem>(GetCampaignDetailsAsync);
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