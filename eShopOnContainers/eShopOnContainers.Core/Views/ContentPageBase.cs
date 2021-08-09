using System;
using eShopOnContainers.Core.ViewModels.Base;
using Xamarin.Forms;

namespace eShopOnContainers.Core.Views
{
    public abstract class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            NavigationPage.SetBackButtonTitle (this, string.Empty);
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing ();

            if (BindingContext is ViewModelBase vmb)
            {
                if (!vmb.IsInitialized || vmb.MultipleInitialization)
                {
                    await vmb.InitializeAsync (null);
                }
            }
        }
    }
}
