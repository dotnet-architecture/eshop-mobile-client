using eShopOnContainers.Core.Models.Marketing;
using eShopOnContainers.Core.Services.Dependency;
using eShopOnContainers.Core.Services.Marketing;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels;
using eShopOnContainers.Core.ViewModels.Base;
using eShopOnContainers.UnitTests.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace eShopOnContainers.UnitTests.ViewModels
{
    public class MarketingViewModelTests
    {
        public MarketingViewModelTests()
        {
            ViewModelLocator.UpdateDependencies(true);
        }

        [Fact]
        public void GetCampaignsIsNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());
            var campaignViewModel = new CampaignViewModel();
            Assert.Null(campaignViewModel.Campaigns);
        }

        [Fact]
        public async Task GetCampaignsIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());
            var campaignViewModel = new CampaignViewModel();

            await campaignViewModel.InitializeAsync(null);

            Assert.NotNull(campaignViewModel.Campaigns);
        }

        [Fact]
        public void GetCampaignDetailsCommandIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());
            var campaignViewModel = new CampaignViewModel();

            Assert.NotNull(campaignViewModel.GetCampaignDetailsCommand);
        }

        [Fact]
        public void GetCampaignDetailsByIdIsNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());
            var campaignViewModel = new CampaignDetailsViewModel();
            Assert.Null(campaignViewModel.Campaign);
        }

        [Fact]
        public async Task GetCampaignDetailsByIdIsNotNullTest()
        {
            Xamarin.Forms.DependencyService.RegisterSingleton<ISettingsService>(new MockSettingsService());
            Xamarin.Forms.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());
            var campaignDetailsViewModel = new CampaignDetailsViewModel();

            await campaignDetailsViewModel.InitializeAsync(new Dictionary<string, string> { { nameof(Campaign.Id), "1" } });

            Assert.NotNull(campaignDetailsViewModel.Campaign);
        }
    }
}