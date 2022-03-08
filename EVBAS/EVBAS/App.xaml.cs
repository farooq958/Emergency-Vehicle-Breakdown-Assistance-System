using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

          MainPage =new NavigationPage( new Registration.LoginPage());
           // MainPage = new NavigationPage(new Dashboard.ProfilePage("Check"));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
