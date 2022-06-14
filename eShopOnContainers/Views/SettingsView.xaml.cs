using eShopOnContainers.ViewModels;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
