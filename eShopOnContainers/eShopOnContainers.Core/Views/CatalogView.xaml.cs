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

        private bool _filterShown;

        public CatalogView()
        {
            InitializeComponent();

            SlideMenu = _filterView;

            MessagingCenter.Subscribe<CatalogViewModel>(this, MessageKeys.Filter, (sender) =>
            {
                Filter();
            });
        }

        public Action HideMenuAction
        {
            get;
            set;
        }

        public Action ShowMenuAction
        {
            get;
            set;
        }

        public Popup SlideMenu
        {
            get;
            set;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _filterView.BindingContext = BindingContext;
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            if(!_filterShown)
            {
                _filterShown = true;
                Navigation.ShowPopup (SlideMenu);
                ShowMenuAction?.Invoke();
            }
            else
            {
                _filterShown = false;
                SlideMenu.Dismiss (null);
            }
        }
    }
}