using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.ViewModels;

[QueryProperty(nameof(CampaignId), "Id")]
public class CampaignDetailsViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    private CampaignItem _campaign;
    private bool _isDetailsSite;

    public int CampaignId { get; set; }

    public ICommand EnableDetailsSiteCommand { get; }

    public CampaignDetailsViewModel(
        IAppEnvironmentService appEnvironmentService,
        IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        _appEnvironmentService = appEnvironmentService;
        _settingsService = settingsService;

        EnableDetailsSiteCommand = new RelayCommand(EnableDetailsSite);
    }

    public CampaignItem Campaign
    {
        get => _campaign;
        set => SetProperty(ref _campaign, value);
    }

    public bool IsDetailsSite
    {
        get => _isDetailsSite;
        set => SetProperty(ref _isDetailsSite, value);
    }

    public override async Task InitializeAsync ()
    {
        await IsBusyFor(
            async () =>
            {
                // Get campaign by id
                Campaign = await _appEnvironmentService.CampaignService.GetCampaignByIdAsync(CampaignId, _settingsService.AuthAccessToken);
            });
    }

    private void EnableDetailsSite()
    {
        IsDetailsSite = true;
    }
}