using CommunityToolkit.Mvvm.Messaging;
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

        WeakReferenceMessenger.Default
            .Register<Messages.AddProductMessage>(
                this,
                (r, m) =>
                {
                    this.Dispatcher.DispatchAsync(
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

        WeakReferenceMessenger.Default
            .Unregister<Messages.AddProductMessage>(this);
    }
}