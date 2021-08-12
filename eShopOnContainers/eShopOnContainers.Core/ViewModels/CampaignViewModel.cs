using eShopOnContainers.Core.Models.Marketing;
using eShopOnContainers.Core.Services.Marketing;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels
{
    public class CampaignViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ICampaignService _campaignService;

        private ObservableCollection<CampaignItem> _campaigns;

        public CampaignViewModel()
        {
            _settingsService = DependencyService.Get<ISettingsService> ();
            _campaignService = DependencyService.Get<ICampaignService> ();
        }

        public ObservableCollection<CampaignItem> Campaigns
        {
            get => _campaigns;
            set
            {
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public ICommand GetCampaignDetailsCommand => new Command<CampaignItem>(async (item) => await GetCampaignDetailsAsync(item));

        public override async Task InitializeAsync (IDictionary<string, string> query)
        {
            IsBusy = true;
            // Get campaigns by user
            Campaigns = await _campaignService.GetAllCampaignsAsync (_settingsService.AuthAccessToken);
            IsBusy = false;
        }

        private async Task GetCampaignDetailsAsync(CampaignItem campaign)
        {
            await NavigationService.NavigateToAsync(
                "CampaignDetails",
                new Dictionary<string, string> { { nameof (Campaign.Id), campaign.Id.ToString () } });
        }
    }
}