using System;
using eShopOnContainers.Core.ViewModels.Base;
using Xamarin.Forms;

namespace eShopOnContainers.Core.Views
{
    public class ContentPageBase : ContentPage
    {
        public ContentPageBase ()
        {
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing ();

            if(BindingContext is ViewModelBase vmb)
            {
                if (!vmb.IsInitialized)
                {
                    await vmb.InitializeAsync (null /*TODO: This will be cleaned up with shell nav parameters later*/);
                    vmb.IsInitialized = true;
                }
            }
        }
    }
}
