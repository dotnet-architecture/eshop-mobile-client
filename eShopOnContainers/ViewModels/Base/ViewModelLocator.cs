//using eShopOnContainers.Services.Basket;
//using eShopOnContainers.Services.Catalog;
//using eShopOnContainers.Services.FixUri;
//using eShopOnContainers.Services.Identity;
//using eShopOnContainers.Services.Location;
//using eShopOnContainers.Services.Marketing;
//using eShopOnContainers.Services.OpenUrl;
//using eShopOnContainers.Services.Order;
//using eShopOnContainers.Services.RequestProvider;
//using eShopOnContainers.Services.Settings;
//using eShopOnContainers.Services.User;
//using eShopOnContainers.Services;
//using System;
//using System.Globalization;
//using System.Reflection;
//using Microsoft.Maui;

//namespace eShopOnContainers.ViewModels.Base
//{
//    public static class ViewModelLocator
//    {
//        //public static readonly BindableProperty AutoWireViewModelProperty =
//        //    BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

//        //public static bool GetAutoWireViewModel(BindableObject bindable)
//        //{
//        //    return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
//        //}

//        //public static void SetAutoWireViewModel(BindableObject bindable, bool value)
//        //{
//        //    bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
//        //}

//        public static bool UseMockService { get; set; }

//        static ViewModelLocator()
//        {
//            // Services - by default, TinyIoC will register interface registrations as singletons.
//            var settingsService = new SettingsService();
//            var requestProvider = new RequestProvider();
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ISettingsService>(settingsService);
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<INavigationService>(new NavigationService(settingsService));
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IDialogService>(new DialogService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IOpenUrlService>(new OpenUrlService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IRequestProvider>(requestProvider);
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IIdentityService>(new IdentityService(requestProvider));
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IFixUriService>(new FixUriService(settingsService));
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ILocationService>(new LocationService(requestProvider));
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IBasketService>(new BasketMockService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IOrderService>(new OrderMockService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IUserService>(new UserMockService());
//            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());

//            // View models - by default, TinyIoC will register concrete classes as multi-instance.
//            Microsoft.Maui.Controls.DependencyService.Register<BasketViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<CatalogViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<CheckoutViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<LoginViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<MainViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<OrderDetailViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<ProfileViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<SettingsViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<CampaignViewModel>();
//            Microsoft.Maui.Controls.DependencyService.Register<CampaignDetailsViewModel>();
//        }

//        public static void UpdateDependencies(bool useMockServices)
//        {
//            // Change injected dependencies
//            if (useMockServices)
//            {
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICatalogService>(new CatalogMockService());
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IBasketService>(new BasketMockService());
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IOrderService>(new OrderMockService());
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IUserService>(new UserMockService());
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICampaignService>(new CampaignMockService());

//                UseMockService = true;
//            }
//            else
//            {
//                var requestProvider = Microsoft.Maui.Controls.DependencyService.Get<IRequestProvider>();
//                var fixUriService = Microsoft.Maui.Controls.DependencyService.Get<IFixUriService>();
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IBasketService>(new BasketService(requestProvider, fixUriService));
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICampaignService>(new CampaignService(requestProvider, fixUriService));
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<ICatalogService>(new CatalogService(requestProvider, fixUriService));
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IOrderService>(new OrderService(requestProvider));
//                Microsoft.Maui.Controls.DependencyService.RegisterSingleton<IUserService>(new UserService(requestProvider));

//                UseMockService = false;
//            }
//        }

//        //public static T Resolve<T>() where T : class
//        //{
//        //    return Microsoft.Maui.Controls.DependencyService.Get<T>();
//        //}

//        //private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
//        //{
//        //    var view = bindable as Element;
//        //    if (view == null)
//        //    {
//        //        return;
//        //    }

//        //    var viewType = view.GetType();
//        //    var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
//        //    var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
//        //    var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

//        //    var viewModelType = Type.GetType(viewModelName);
//        //    if (viewModelType == null)
//        //    {
//        //        return;
//        //    }

//        //    var viewModel = Activator.CreateInstance(viewModelType);

//        //    view.BindingContext = viewModel;
//        //}
//    }
//}