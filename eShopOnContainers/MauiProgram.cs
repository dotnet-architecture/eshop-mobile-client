using CommunityToolkit.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
using eShopOnContainers.Services.FixUri;
using eShopOnContainers.Services.Identity;
using eShopOnContainers.Services.Location;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.OpenUrl;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.RequestProvider;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.Theme;
using eShopOnContainers.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace eShopOnContainers;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
        => MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureEffects(
                effects =>
                {
                })
            .UseMauiCommunityToolkit()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Font_Awesome_5_Free-Regular-400.otf", "FontAwesome-Regular");
                    fonts.AddFont("Font_Awesome_5_Free-Solid-900.otf", "FontAwesome-Solid");
                    fonts.AddFont("Montserrat-Bold.ttf", "Montserrat-Bold");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat-Regular");
                    fonts.AddFont("SourceSansPro-Regular.ttf", "SourceSansPro-Regular");
                    fonts.AddFont("SourceSansPro-Solid.ttf", "SourceSansPro-Solid");
                })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ISettingsService, SettingsService>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        mauiAppBuilder.Services.AddSingleton<IDialogService, DialogService>();
        mauiAppBuilder.Services.AddSingleton<IOpenUrlService, OpenUrlService>();
        mauiAppBuilder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        mauiAppBuilder.Services.AddSingleton<IIdentityService, IdentityService>();
        mauiAppBuilder.Services.AddSingleton<IFixUriService, FixUriService>();
        mauiAppBuilder.Services.AddSingleton<ILocationService, LocationService>();

        mauiAppBuilder.Services.AddSingleton<ITheme, Theme>();

        mauiAppBuilder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>(
            serviceProvider =>
            {
                var requestProvider = serviceProvider.GetService<IRequestProvider>();
                var fixUriService = serviceProvider.GetService<IFixUriService>();
                var settingsService = serviceProvider.GetService<ISettingsService>();

                var aes =
                    new AppEnvironmentService(
                        new BasketMockService(), new BasketService(requestProvider, fixUriService),
                        new CampaignMockService(), new CampaignService(requestProvider, fixUriService),
                        new CatalogMockService(), new CatalogService(requestProvider, fixUriService),
                        new OrderMockService(), new OrderService(requestProvider),
                        new UserMockService(), new UserService(requestProvider));

                aes.UpdateDependencies(settingsService.UseMocks);
                return aes;
            });

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ViewModels.LoginViewModel>();
        mauiAppBuilder.Services.AddSingleton<ViewModels.BasketViewModel>();
        mauiAppBuilder.Services.AddSingleton<ViewModels.CatalogViewModel>();
        mauiAppBuilder.Services.AddSingleton<ViewModels.ProfileViewModel>();

        mauiAppBuilder.Services.AddTransient<ViewModels.CheckoutViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.LoginViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.MainViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.OrderDetailViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.SettingsViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CampaignViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CampaignDetailsViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<Views.BasketView>();
        mauiAppBuilder.Services.AddTransient<Views.CampaignDetailsView>();
        mauiAppBuilder.Services.AddTransient<Views.CampaignView>();
        mauiAppBuilder.Services.AddTransient<Views.CatalogView>();
        mauiAppBuilder.Services.AddTransient<Views.CheckoutView>();
        mauiAppBuilder.Services.AddTransient<Views.FiltersView>();
        mauiAppBuilder.Services.AddTransient<Views.LoginView>();
        mauiAppBuilder.Services.AddTransient<Views.OrderDetailView>();
        mauiAppBuilder.Services.AddTransient<Views.ProfileView>();
        mauiAppBuilder.Services.AddTransient<Views.SettingsView>();

        return mauiAppBuilder;
    }
}