namespace eShopOnContainers.Views;

public partial class CatalogView : ContentPageBase
{
    public CatalogView(CatalogViewModel viewModel)
    {
        this.BindingContext = viewModel;

        InitializeComponent();
        //basket.Command = viewModel.ViewBasketCommand;
    }
}