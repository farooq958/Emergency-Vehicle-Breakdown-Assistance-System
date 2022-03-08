using EVBAS.ChatServices;
using EVBAS.Model;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public string Memail;  
    public    DBFirebaseServices dBFirebase = new DBFirebaseServices();
        public ProfilePage(string email)
        {
            //chatframe.IsVisible = false;
           // chatframe.IsEnabled = false;
            Memail = email;
            feedbackemail = email;
            InitializeComponent();
           // DisplayAlert("check", email, "ok");     
            nameee(email);
             //updateUIAsync();
        }
        public string mailu;
        public int idu;
        public string feedbackemail;
        public ProfilePage(string email,string user,string umail,int id)
        {
            feedbackemail = email;
            InitializeComponent();
            
            mailu = umail;
            idu = id;
            chatframe.IsVisible = true;
            
            nameee(email);
            if (user == "user")
            {
               // DisplayAlert("check", "from usertapped", "ok");            
            }
            // updateUIAsync();
        }

        public ProfilePage()
        {
        }
       protected override  void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(TimeSpan.FromMilliseconds(2000), loop2);
            bool loop2()
            {
                Device.BeginInvokeOnMainThread(() => {
                    _ = updateUIAsync();
                    //more stuff;
                });
                return true;
            }
          

        }
        public ObservableCollection<Feedback> Ulist = new ObservableCollection<Feedback>();
        private async Task updateUIAsync()
        {
            DBFirebaseServices dB = new DBFirebaseServices();
            // var data2 = await dB.getALLFeedbacks();
            var dataa = await dB.GetPerson(feedbackemail);

            var data = await dB.getALLFeedbacks(dataa.guidd);
         
            //Ulist = new ObservableCollection<Feedback>()
              //  {
                // new Feedback {Username= data.Username,Userfeedback=data.Userfeedback,Memail=data.Memail }
                //};
           // await DisplayAlert(Ulist.Count.ToString(), " ", " ");
            FeedView.BindingContext = data;
            

            /*    await DisplayAlert(feedbackemail, data.Count.ToString(), data2.Count.ToString());
                foreach (var item in data2)
                {

                        if (item.Memail == feedbackemail)
                        {


                        Ulist = new ObservableCollection<Feedback>()
                    {
                     new Feedback {Username= item.Username,Userfeedback=item.Userfeedback,Memail=item.Memail }
                    };

                    }

                }
                Ulist = new ObservableCollection<Feedback>()
                    {
                     new Feedback {Username= data.Username,Userfeedback=data.Userfeedback,Memail=data.Memail }
                    };
                 await DisplayAlert(Ulist.Count.ToString()," "," ");
                FeedView.BindingContext = data2; */
        }
        
        public async void nameee(string mail)
        {
          
            var getname = await dBFirebase.GetPerson(mail);
            var getcontact = await dBFirebase.GetPerson(mail);
            var gettypee = await dBFirebase.GetPerson(mail);
            var getdescription = await dBFirebase.GetPerson(mail);
            Titlename.Text = "Welcome  " + getname.Name;
            MechanicName.Text = getname.Name;
            McType.Text = gettypee.Type;
            MechanicContact.Text = getcontact.Details;
            Description.Text = getdescription.Description;
            //var getcontact = await db.GetPerson(mail);

        }

        private async void ChatButton_Clicked(object sender, EventArgs e)
        {
            DBFirebaseServices db = new DBFirebaseServices();
        var username = await db.GetuPerson(mailu);

            //MessagingCenter.Send<ProfilePage>(this, idu.ToString());
           db.Loadmeassages(idu.ToString());
          await  Navigation.PushAsync(new ChatServices.ChatPage(mailu, username.Name, idu));

            
        }

        

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Feedbackpopup();
        }

        private async Task Feedbackpopup()
        {
            DBFirebaseServices db = new DBFirebaseServices();
            var username = await db.GetuPerson(mailu);

            await Navigation.PushPopupAsync(new Feedbackpopup(feedbackemail, username.Name));
        }
    }
}