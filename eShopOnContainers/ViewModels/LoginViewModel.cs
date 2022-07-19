using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Identity;
using eShopOnContainers.Services.OpenUrl;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Validations;
using eShopOnContainers.ViewModels.Base;
using IdentityModel.Client;

namespace eShopOnContainers.ViewModels;

public class LoginViewModel : ViewModelBase
{

    private readonly ISettingsService _settingsService;
    private readonly IOpenUrlService _openUrlService;
    private readonly IIdentityService _identityService;

    private readonly ValidatableObject<string> _userName;
    private readonly ValidatableObject<string> _password;

    private bool _isMock;
    private bool _isValid;
    private bool _isLogin;
    private string _loginUrl;

    public ValidatableObject<string> UserName => _userName;

    public ValidatableObject<string> Password => _password;

    public bool IsMock
    {
        get => _isMock;
        set => SetProperty(ref _isMock, value);
    }

    public bool IsValid
    {
        get => _isValid;
        set => SetProperty(ref _isValid, value);
    }

    public bool IsLogin
    {
        get => _isLogin;
        set => SetProperty(ref _isLogin, value);
    }

    public string LoginUrl
    {
        get => _loginUrl;
        set => SetProperty(ref _loginUrl, value);
    }

    public ICommand MockSignInCommand { get; }

    public ICommand SignInCommand { get; }

    public ICommand RegisterCommand { get; }

    public ICommand NavigateCommand { get; }

    public ICommand SettingsCommand { get; }

    public ICommand ValidateCommand { get; }

    public LoginViewModel(
        IOpenUrlService openUrlService, IIdentityService identityService,
        IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        _settingsService = settingsService;
        _openUrlService = openUrlService;
        _identityService = identityService;

        _userName = new ValidatableObject<string>();
        _password = new ValidatableObject<string>();

        MockSignInCommand = new AsyncRelayCommand(MockSignInAsync);
        SignInCommand = new AsyncRelayCommand(SignInAsync);
        RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        NavigateCommand = new AsyncRelayCommand<string>(NavigateAsync);
        SettingsCommand = new AsyncRelayCommand(SettingsAsync);
        ValidateCommand = new RelayCommand(() => Validate());

        InvalidateMock();
        AddValidations();
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);

        if (query.ValueAsBool("Logout") == true)
        {
            PerformLogout();
        }
    }

    public override Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    private async Task MockSignInAsync()
    {
        await IsBusyFor(
            async () =>
            {
                IsValid = true;
                bool isValid = Validate();
                bool isAuthenticated = false;

                if (isValid)
                {
                    try
                    {
                        await Task.Delay(10);

                        isAuthenticated = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                    }
                }
                else
                {
                    IsValid = false;
                }

                if (isAuthenticated)
                {
                    _settingsService.AuthAccessToken = GlobalSetting.Instance.AuthToken;

                    await NavigationService.NavigateToAsync("//Main/Catalog");
                }
            });
    }

    private async Task SignInAsync()
    {
        await IsBusyFor(
            async () =>
            {
                await Task.Delay(10);

                LoginUrl = _identityService.CreateAuthorizationRequest();

                IsValid = true;
                IsLogin = true;
            });
    }

    private Task RegisterAsync()
    {
        return _openUrlService.OpenUrl(GlobalSetting.Instance.RegisterWebsite);
    }

    private void PerformLogout()
    {
        var authIdToken = _settingsService.AuthIdToken;
        var logoutRequest = _identityService.CreateLogoutRequest(authIdToken);

        if (!string.IsNullOrEmpty(logoutRequest))
        {
            // Logout
            LoginUrl = logoutRequest;
        }

        if (_settingsService.UseMocks)
        {
            _settingsService.AuthAccessToken = string.Empty;
            _settingsService.AuthIdToken = string.Empty;
        }

        _settingsService.UseFakeLocation = false;

        UserName.Value = string.Empty;
        Password.Value = string.Empty;
    }

    private async Task NavigateAsync(string url)
    {
        var unescapedUrl = System.Net.WebUtility.UrlDecode(url);

        if (unescapedUrl.Equals(GlobalSetting.Instance.LogoutCallback, StringComparison.OrdinalIgnoreCase))
        {
            _settingsService.AuthAccessToken = string.Empty;
            _settingsService.AuthIdToken = string.Empty;
            IsLogin = false;
            LoginUrl = _identityService.CreateAuthorizationRequest();
        }
        else if (unescapedUrl.Contains(GlobalSetting.Instance.Callback, StringComparison.OrdinalIgnoreCase))
        {
            var authResponse = new AuthorizeResponse(url);
            if (!string.IsNullOrWhiteSpace(authResponse.Code))
            {
                var userToken = await _identityService.GetTokenAsync(authResponse.Code);
                string accessToken = userToken.AccessToken;

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    _settingsService.AuthAccessToken = accessToken;
                    _settingsService.AuthIdToken = authResponse.IdentityToken;
                    await NavigationService.NavigateToAsync("//Main/Catalog");
                }
            }
        }
    }

    private Task SettingsAsync()
    {
        return NavigationService.NavigateToAsync("Settings");
    }

    private bool Validate()
    {
        return UserName.Validate() && Password.Validate();
    }

    private void AddValidations()
    {
        UserName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
        Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
    }

    public void InvalidateMock()
    {
        IsMock = _settingsService.UseMocks;
    }
}
