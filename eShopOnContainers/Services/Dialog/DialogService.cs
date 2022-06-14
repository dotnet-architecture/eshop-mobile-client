using System.Threading.Tasks;
using Microsoft.Maui;

namespace eShopOnContainers.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return App.Current.MainPage.DisplayAlert (title, message, buttonLabel);
        }
    }
}
