using EVBAS.Dashboard;
using EVBAS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IAuth auth;
      

        public LoginPage()
        {
            InitializeComponent();
            auth =DependencyService.Get<IAuth>();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync( new SignUpPage());
        }
        public bool check;
        private async void Login_Clicked_1(object sender, EventArgs e)
        {
            string emailLogin = LoginEmail.Text;
            check = verification();

            if (check == true)
            {


                string token = await auth.LoginWithEmailAndPassword(LoginEmail.Text, LoginPassword.Text);
                if (token != string.Empty)
                {
                    DBFirebaseServices db = new DBFirebaseServices();

                    var bl = await db.gettype(emailLogin);
                    // var ul = await db.GetuPerson(emailLogin);
                    if (bl == false)
                    {
                        DBFirebaseServices db1 = new DBFirebaseServices();
                        await db1.updateid(LoginEmail.Text);
                        await Navigation.PushAsync(new MechanicPage(LoginEmail.Text));

                    }
                    else
                    {

                        await Navigation.PushAsync(new UserPage(LoginEmail.Text));

                    }


                }
                else
                {
                  await  DisplayAlert("Error,Not found", "Something Went Wrong Email Not found  ,Make sure password and email is not empty", "Ok");
                }
            }
            else
            {
               await DisplayAlert("Error", "Something Went Wrong ,Make sure password and email is not empty", "Ok");
                return;
            }
        }
    
    
    private bool verification()
        {
            if (LoginEmail.Text==null || LoginPassword.Text==null)
            {
                
                return false;
            }
            return true;
        }
    }
}