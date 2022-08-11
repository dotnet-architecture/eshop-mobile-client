namespace eShopOnContainers.Views
{
    public partial class BasketView : ContentPageBase
    {
        public BasketView(BasketViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}