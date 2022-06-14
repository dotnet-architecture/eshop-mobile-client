using eShopOnContainers.Extensions;
using eShopOnContainers.Models.User;
using eShopOnContainers.Services.Identity;
using eShopOnContainers.Services.OpenUrl;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Validations;
using eShopOnContainers.ViewModels.Base;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace eShopOnContainers.ViewModels
{
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
        private string _authUrl;

        public ValidatableObject<string> UserName
        {
            get => _userName;
        }

        public ValidatableObject<string> Password
        {
            get => _password;
        }

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
            get => _authUrl;
            set => SetProperty(ref _authUrl, value);
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

            if(query.ValueAsBool("Logout", out var logout) && logout == true)
            {
                PerformLogout ();
            }
        }

        public override Task InitializeAsync ()
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

                        await NavigationService.NavigateToAsync ("//Main/Catalog");
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

            if (unescapedUrl.Equals(GlobalSetting.Instance.LogoutCallback))
            {
                _settingsService.AuthAccessToken = string.Empty;
                _settingsService.AuthIdToken = string.Empty;
                IsLogin = false;
                LoginUrl = _identityService.CreateAuthorizationRequest();
            }
            else if (unescapedUrl.Contains(GlobalSetting.Instance.Callback))
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
                        await NavigationService.NavigateToAsync ("//Main/Catalog");
                    }
                }
            }
        }

        private Task SettingsAsync()
        {
            return NavigationService.NavigateToAsync(
                "Settings",
                new Dictionary<string, object>{ { "reset", true } });
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
}