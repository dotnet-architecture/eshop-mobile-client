using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace eShopOnContainers.Core
{
    public partial class AppShell : Shell
    {
        public AppShell ()
        {
            InitializeRouting ();
            InitializeComponent ();
        }

        private void InitializeRouting()
        {
            Routing.RegisterRoute (Views.MainView.RouteName, typeof (Views.MainView));
        }
    }
}
