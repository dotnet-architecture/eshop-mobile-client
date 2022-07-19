using eShopOnContainers.Models.Catalog;

namespace eShopOnContainers.Views;

public partial class CatalogView : ContentPageBase
{
    public CatalogView(CatalogViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MessagingCenter.Subscribe<CatalogViewModel>(
            this,
            MessengerKeys.AddProduct,
            (sender) =>
            {
                MainThread.BeginInvokeOnMainThread(
                    async () =>
                    {
                        await badge.ScaleTo(1.2);
                        await badge.ScaleTo(1.0);
                    });
            });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        MessagingCenter.Unsubscribe<CatalogViewModel, CatalogItem>(this, MessengerKeys.AddProduct);
    }
}