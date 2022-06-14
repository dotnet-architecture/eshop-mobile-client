using eShopOnContainers.ViewModels;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class CampaignView : ContentPageBase
    {
        public CampaignView(CampaignDetailsViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}