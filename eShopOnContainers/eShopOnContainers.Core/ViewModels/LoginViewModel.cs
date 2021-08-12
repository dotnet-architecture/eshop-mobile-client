using eShopOnContainers.Core.Extensions;
using eShopOnContainers.Core.Models.User;
using eShopOnContainers.Core.Services.Identity;
using eShopOnContainers.Core.Services.OpenUrl;
using eShopOnContainers.Core.Services.Settings;
using eShopOnContainers.Core.Validations;
using eShopOnContainers.Core.ViewModels.Base;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eShopOnContainers.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;
        private bool _isMock;
        private bool _isValid;
        private bool _isLogin;
        private string _authUrl;

        private ISettingsService _settingsService;
        private IOpenUrlService _openUrlService;
        private IIdentityService _identityService;

        public LoginViewModel()
        {
            _settingsService = DependencyService.Get<ISettingsService> ();
            _openUrlService = DependencyService.Get<IOpenUrlService> ();
            _identityService = DependencyService.Get<IIdentityService> ();

            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            InvalidateMock();
            AddValidations();
        }

        public ValidatableObject<string> UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsMock
        {
            get => _isMock;
            set
            {
                _isMock = value;
                RaisePropertyChanged(() => IsMock);
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                _isLogin = value;
                RaisePropertyChanged(() => IsLogin);
            }
        }

        public string LoginUrl
        {
            get => _authUrl;
            set
            {
                _authUrl = value;
                RaisePropertyChanged(() => LoginUrl);
            }
        }

        public ICommand MockSignInCommand => new Command(async () => await MockSignInAsync());

        public ICommand SignInCommand => new Command(async () => await SignInAsync());

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());

        public ICommand NavigateCommand => new Command<string>(async (url) => await NavigateAsync(url));

        public ICommand SettingsCommand => new Command(async () => await SettingsAsync());

        public ICommand ValidateUserNameCommand => new Command(() => ValidateUserName());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        public override Task InitializeAsync (IDictionary<string, string> query)
        {
            var logout = query.GetValueAsBool ("Logout");

            if(logout.ContainsKeyAndValue && logout.Value == true)
            {
                Logout ();
            }

            return Task.CompletedTask;
        }

        private async Task MockSignInAsync()
        {
            IsBusy = true;
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

            IsBusy = false;
        }

        private async Task SignInAsync()
        {
            IsBusy = true;

            await Task.Delay(10);

            LoginUrl = _identityService.CreateAuthorizationRequest();

            IsValid = true;
            IsLogin = true;
            IsBusy = false;
        }

        private Task RegisterAsync()
        {
            return _openUrlService.OpenUrl(GlobalSetting.Instance.RegisterWebsite);
        }

        private void Logout()
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
                new Dictionary<string, string>
                {
                    { "reset", "true" },
                });
        }

        private bool Validate()
        {
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();

            return isValidUser && isValidPassword;
        }

        private bool ValidateUserName()
        {
            return _userName.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
        }

        public void InvalidateMock()
        {
            IsMock = _settingsService.UseMocks;
        }
    }
}