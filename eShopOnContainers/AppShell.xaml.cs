using System;
using System.Collections.Generic;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using eShopOnContainers.Views;
using Microsoft.Maui;

namespace eShopOnContainers
{
    public partial class AppShell : Shell
    {
        public AppShell (ISettingsService settingsService)
        {
            InitializeRouting ();
            InitializeComponent ();

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
