using eShopOnContainers.ViewModels;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class BasketView : ContentPageBase
    {
        public BasketView(BasketViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}