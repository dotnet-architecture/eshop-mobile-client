using System;
using System.Collections.Generic;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.ViewModels.Base;
using eShopOnContainers.Core.Views;
using Xamarin.Forms;

namespace eShopOnContainers.Core
{
    public partial class AppShell : Shell
    {
        public AppShell ()
        {
            InitializeRouting ();
            InitializeComponent ();

            var settingsService = ViewModelLocator.Resolve<ISettingsService> ();

            if (string.IsNullOrEmpty (settingsService.AuthAccessToken))
            {
                this.GoToAsync ("//Login");
            }
        }

        private void InitializeRouting()
        {
            Routing.RegisterRoute ("Basket", typeof (BasketView));
            Routing.RegisterRoute ("Settings", typeof (SettingsView));
            Routing.RegisterRoute ("OrderDetail", typeof (OrderDetailView));
            Routing.RegisterRoute ("CampaignDetails", typeof(CampaignDetailsView));
            Routing.RegisterRoute ("Checkout", typeof (CheckoutView));
        }


    }
}
