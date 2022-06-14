using eShopOnContainers.ViewModels;
using eShopOnContainers.ViewModels.Base;
using System;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class CatalogView : ContentPageBase
    {
        private FiltersView _filtersView;

        public CatalogView(CatalogViewModel viewModel)
        {
            this.BindingContext = viewModel;

            _filtersView =
                new FiltersView
                {
                    BindingContext = viewModel,
                };

            InitializeComponent();
        }

        private async void OnFilterChanged(object sender, EventArgs e)
        {
            //TODO: This can probably be adjusted
            await this.Navigation.PushModalAsync(_filtersView, true);
        }
    }
}