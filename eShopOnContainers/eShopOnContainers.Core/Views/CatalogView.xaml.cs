using eShopOnContainers.Core.ViewModels;
using eShopOnContainers.Core.ViewModels.Base;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace eShopOnContainers.Core.Views
{
    public partial class CatalogView : ContentPageBase
    {
        private FiltersView _filterView = new FiltersView();

        public CatalogView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            Products.Scrolled -= Products_Scrolled;
            Products.Scrolled += Products_Scrolled;
        }

        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();
            Products.Scrolled -= Products_Scrolled;
        }

        private void Products_Scrolled (object sender, ScrolledEventArgs e)
        {
            var modifiedScrollDistance = (e.ScrollY / 3d);
            if (modifiedScrollDistance <  CampaignButton.Height)
            {
                CampaignButton.TranslationY = Math.Max(0, modifiedScrollDistance);
                CampaignButton.Opacity = 1.0d - (CampaignButton.TranslationY / CampaignButton.Height);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _filterView.BindingContext = BindingContext;
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            Navigation.ShowPopup (_filterView);
        }
    }
}