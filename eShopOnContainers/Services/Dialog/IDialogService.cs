namespace eShopOnContainers.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string message, string title, string buttonLabel);
}