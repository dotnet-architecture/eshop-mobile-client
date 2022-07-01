namespace eShopOnContainers.Views;

public partial class FiltersView : ContentPage
{
    public FiltersView(CatalogViewModel viewModel)
    {
        this.BindingContext = viewModel;

        InitializeComponent();
    }
}
