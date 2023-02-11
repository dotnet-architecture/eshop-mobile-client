using CommunityToolkit.Mvvm.ComponentModel;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableObject, IViewModelBase, IDisposable
{
    private readonly SemaphoreSlim _isBusyLock = new(1, 1);

    private bool _disposedValue;

    [ObservableProperty]
    private bool _isInitialized;

    [ObservableProperty]
    private bool _isBusy;

    public INavigationService NavigationService { get; private set; }

    public ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task IsBusyFor(Func<Task> unitOfWork)
    {
        await _isBusyLock.WaitAsync();

        try
        {
            IsBusy = true;

            await unitOfWork();
        }
        finally
        {
            IsBusy = false;
            _isBusyLock.Release();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _isBusyLock?.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

