using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feedbackpopup : PopupPage
    {
        public string memail;
        public Feedbackpopup(string Memail,string Uname)
        {
            InitializeComponent();
            memail = Memail;
        
           Userr.Text= Uname;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private async void SaveIt_Clicked(object sender, EventArgs e)
        {
            Model.DBFirebaseServices db = new Model.DBFirebaseServices();
            var feedOBJ = new Model.Feedback { Userfeedback = Userfeedback.Text,Username = Userr.Text, Memail = memail  };
          var g= await db.GetPerson(memail);

         await db.savefeedback(memail,Userfeedback.Text,Userr.Text,g.guidd);
            await DisplayAlert("success", "saved", "ok");
          await Navigation.PopPopupAsync();
        }
    }
}