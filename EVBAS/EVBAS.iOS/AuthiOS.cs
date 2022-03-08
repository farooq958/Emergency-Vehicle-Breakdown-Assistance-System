using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVBAS.iOS;
using EVBAS.Registration;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Firebase.Auth;
[assembly: Dependency(typeof(AuthiOS))]
namespace EVBAS.iOS
{

    class AuthiOS : IAuth
    {
        public bool IsSignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string Password)
        {
            try
            {
                var newuser = await Auth.DefaultInstance.SignInWithPasswordAsync(email, Password);
                var token = newuser.User.GetIdTokenAsync();
                return await token;
            }
            catch (Exception e)
            {

                return string.Empty;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string Password)
        {
            try
            {
                var newuser = await Auth.DefaultInstance.CreateUserAsync(email, Password);
                var token = newuser.User.GetIdTokenAsync();
                return await token;
            }
            catch (Exception e)
            {

                return string.Empty;
            }
        }
    }
}