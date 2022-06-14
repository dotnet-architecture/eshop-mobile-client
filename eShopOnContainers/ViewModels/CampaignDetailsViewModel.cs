using eShopOnContainers.Extensions;
using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using CommunityToolkit.Mvvm.Input;

namespace eShopOnContainers.ViewModels
{
    [QueryProperty(nameof(CampaignId), nameof(Models.Marketing.Campaign.Id))]
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
}