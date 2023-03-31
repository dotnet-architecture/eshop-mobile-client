using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

[QueryProperty(nameof(CampaignId), "Id")]
public partial class CampaignDetailsViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    [ObservableProperty]
    private CampaignItem _campaign;

    [ObservableProperty]
    private bool _isDetailsSite;

    [ObservableProperty]
    private int _campaignId;

    public CampaignDetailsViewModel(
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
                // Get campaign by id
                Campaign = await _appEnvironmentService.CampaignService.GetCampaignByIdAsync(CampaignId, _settingsService.AuthAccessToken);
            });
    }

    [RelayCommand]
    private void EnableDetailsSite()
    {
        IsDetailsSite = true;
    }
}