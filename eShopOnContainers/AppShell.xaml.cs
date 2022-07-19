using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Views;

namespace eShopOnContainers;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;

    public AppShell (INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeRouting ();
        InitializeComponent ();
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if(this.Handler != null)
        {
            await _navigationService.InitializeAsync();
        }
    }

    private void InitializeRouting()
    {
        Routing.RegisterRoute("Filter", typeof(FiltersView));
        Routing.RegisterRoute("Basket", typeof(BasketView));
        Routing.RegisterRoute("Basket", typeof (BasketView));
        Routing.RegisterRoute("Settings", typeof (SettingsView));
        Routing.RegisterRoute("OrderDetail", typeof (OrderDetailView));
        Routing.RegisterRoute("CampaignDetails", typeof(CampaignDetailsView));
        Routing.RegisterRoute("Checkout", typeof (CheckoutView));
    }
}