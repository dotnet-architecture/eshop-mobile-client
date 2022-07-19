namespace eShopOnContainers.UnitTests;

public class MainViewModelTests
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;

    public MainViewModelTests()
    {
        _dialogService = new MockDialogService();
        _navigationService = new MockNavigationService();
        _settingsService = new MockSettingsService();
    }

    [Fact]
    public void SettingsCommandIsNotNullWhenViewModelInstantiatedTest()
    {
        var mainViewModel = new MainViewModel(_dialogService, _navigationService, _settingsService);
        Assert.NotNull(mainViewModel.SettingsCommand);
    }

    [Fact]
    public void IsBusyPropertyIsFalseWhenViewModelInstantiatedTest()
    {
        var mainViewModel = new MainViewModel(_dialogService, _navigationService, _settingsService);
        Assert.False(mainViewModel.IsBusy);
    }
}
