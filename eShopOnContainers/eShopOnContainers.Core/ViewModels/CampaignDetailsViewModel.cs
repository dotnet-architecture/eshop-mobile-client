using eShopOnContainers.Core.Extensions;
using eShopOnContainers.Core.Models.Marketing;
using eShopOnContainers.Core.Services.Marketing;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels
{
    public class CampaignDetailsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ICampaignService _campaignService;

        private CampaignItem _campaign;
        private bool _isDetailsSite;

        public ICommand EnableDetailsSiteCommand => new Command(EnableDetailsSite);

        public CampaignDetailsViewModel()
        {
            _settingsService = DependencyService.Get<ISettingsService> ();
            _campaignService = DependencyService.Get<ICampaignService> ();
        }

        public CampaignItem Campaign
        {
            get => _campaign;
            set
            {
                _campaign = value;
                RaisePropertyChanged(() => Campaign);
            }
        }

        public bool IsDetailsSite
        {
            get => _isDetailsSite;
            set
            {
                _isDetailsSite = value;
                RaisePropertyChanged(() => IsDetailsSite);
            }
        }

        public override async Task InitializeAsync (IDictionary<string, string> query)
        {
            var campaignId = query.GetValueAsInt (nameof (Campaign.Id));

            if (campaignId.ContainsKeyAndValue)
            {
                IsBusy = true;
                // Get campaign by id
                Campaign = await _campaignService.GetCampaignByIdAsync(campaignId.Value, _settingsService.AuthAccessToken);
                IsBusy = false;
            }
        }

        private void EnableDetailsSite()
        {
            IsDetailsSite = true;
        }
    }
}