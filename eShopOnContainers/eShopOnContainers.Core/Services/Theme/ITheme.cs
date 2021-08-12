using System.Drawing;

namespace eShopOnContainers.Core.Services.Theme
{
    public interface ITheme
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
