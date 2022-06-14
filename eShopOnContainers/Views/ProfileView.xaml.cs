using eShopOnContainers.ViewModels;
using Microsoft.Maui;
using PCLCrypto;

namespace eShopOnContainers.Views
{
    public partial class ProfileView : ContentPageBase
    {
        private readonly ProfileViewModel _viewModel;

        public ProfileView(ProfileViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(_viewModel.IsInitialized)
            {
                _viewModel.RefreshCommand.Execute(null);
            }
        }
    }
}