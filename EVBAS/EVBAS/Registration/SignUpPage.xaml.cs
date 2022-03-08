using EVBAS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        IAuth Auth;
        public Random Random;
        public SignUpPage()
        {
            InitializeComponent();
            Auth = DependencyService.Get<IAuth>();
            Random = new Random();
        }
        public SignUpPage(string user)
        {
            InitializeComponent();

        }
        public bool check;

        private async void Signup_Clicked(object sender, EventArgs e)
        {
             //verification();
            check = verifications();
            if (check == true)
            {
                var user = await Auth.SignUpWithEmailAndPassword(SignupEmail.Text, Password.Text);




                if (user != null)
                {

                    DBFirebaseServices dbb = new DBFirebaseServices();
                    if (UserPicker.SelectedItem.ToString() == "Mechanic")
                    {
                        await dbb.AddLoginDetails(Guid.NewGuid(),Random.Next(1000), Username.Text, Details.Text, 0, 0, SignupEmail.Text, MDescription.Text, MechanicType.SelectedItem.ToString());
                    }
                    else
                    {
                        await dbb.AdduDetails(Random.Next(1000), Username.Text, Details.Text, SignupEmail.Text);
                    }

                    await DisplayAlert("Success", "User has been Created, Please Login", "Ok");
                    _ = Navigation.PushAsync(new LoginPage());

                }
                else
                {

                    await DisplayAlert("Error", "Something Went Wrong", "Ok");
                    return;
                }
            }
            else
            {
           //  await   DisplayAlert("Error", "Something Went Wrong Please check your Name, password, Email,Contact,Make sure its not empty ", "Ok");
                return;

            }
        }

        private  void verification()
        {
           
            if (SignupEmail.Text == null || Password.Text == null || Details.Text == null || Username.Text == null )
            {

                 DisplayAlert("Error", "Something Went Wrong", "Ok");
                return;
            }
            if (UserPicker.SelectedIndex == -1)
            {
                 DisplayAlert("Error", "Please select The type of user", "OK");
                return;
            }
        }
        private bool verifications()
        {
            var invalidname = @"[0-9]";
            var validemail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";



            if (SignupEmail.Text == null || Password.Text == null || Details.Text == null || Username.Text == null)
            {

                DisplayAlert("ErrorEmpty", "Fields can not be empty ", "Ok");
                return false;
            }
            if ( Password.Text.Length <= 6)
            {
                DisplayAlert("Password error ", "Length Of password is not accepted ", "Ok");
                return false;
            }
            if (Regex.IsMatch(Username.Text,invalidname)) 
            {

                DisplayAlert("Invalid Username", "Username can not Contain  Numbers ", "Ok");
                return false;
            }
            if (!Regex.IsMatch(SignupEmail.Text,validemail))
            {

                DisplayAlert("Invalid Email", "Invalid Email not accepted ", "Ok");
                return false;
            }
            if (!Regex.IsMatch(Details.Text,invalidname))
            {

                DisplayAlert("Contact error", "Cannot have literal other than numbers ", "Ok");
                return false;
            }
            if (UserPicker.SelectedIndex == -1)
            {
               // DisplayAlert("Error", "Please select The type of user", "OK");
                return false;
            }
            return true;
        }

        private void Register_Clicked(object sender, EventArgs e)
        {

        }

        private void UserPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserPicker.SelectedIndex==1)
            {
                MechanicType.IsVisible = true;
                Descriptionframe.IsVisible = true;
                labelP.IsVisible = true;
            }
            else
            {
                MechanicType.IsVisible = false;
                Descriptionframe.IsVisible = false;
                labelP.IsVisible = false;
            }   

        }
    }
}