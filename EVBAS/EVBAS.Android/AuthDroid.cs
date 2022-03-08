using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EVBAS.Droid;
using EVBAS.Registration;
using Firebase.Auth;
using Xamarin.Forms;
[assembly: Dependency (typeof(AuthDroid))]
namespace EVBAS.Droid
{
    class AuthDroid : IAuth
    {
        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string Password)
        {
            try
            {
                var newuser = await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, Password);
                var token = newuser.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {

                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {

                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string Password)
        {
            try
            {
                var newuser = await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email,Password);
                var token =  newuser.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {

                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {

                e.PrintStackTrace();
                return string.Empty;
            }
        }
    }
}