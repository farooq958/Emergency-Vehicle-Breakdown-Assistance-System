using EVBAS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicPage : ContentPage
    {
        public string Email;
       // public string Nmail;
        public  MechanicPage(string text)
        {
            InitializeComponent();
            Email = text;
            nameee();
            Refreshh.IsEnabled = false;
        
        }
      public  string mname;
        public async void nameee()
        {DBFirebaseServices dBFirebase = new DBFirebaseServices();
   var getname = await dBFirebase.GetPerson(Email);
            MechanicName.Text = "Welcome " + getname.Name;
            mname = getname.Name;
        }
        public bool check = false;
        public bool check1;
     
        
        private async void ServiceSwitch_Toggled(object sender, ToggledEventArgs e)
        {

            if (ServiceSwitch.IsToggled)
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));
                Refreshh.IsEnabled = false;
                DBFirebaseServices service = new DBFirebaseServices();
                stopit = true;
                var data = await service.GetPerson(Email);
              //  users = data;

                await service.AddmDetails(data.Id, data.Name, data.Details, result.Latitude, result.Longitude, Email);
                idc = data.Id;
                check = true;
                //   var udata = await service.GetuPerson(Nmail);

                //    var notifyme =  await    service.notify(Email);
                MessagingCenter.Subscribe<Resultdata>(this, "Hi", (value) =>
                {
                    DisplayAlert("Need Assistance", value.Name + " " + "With Email:  " + value.Emaill + " " + " Need Your Assistance Contact Number:  " + value.Details, "Ok");
                    // Do something whenever the "Hi" message is received

                });
                await NewMethod(service, data);


                Device.StartTimer(TimeSpan.FromMilliseconds(8000), loop2);
                bool loop2()
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _ = updateUIAsync();
                        //more stuff;
                    });
                    return stopit;
                }

                /*   if (Refreshh.IsRefreshing)
                   {

                   }*/
               

            }
            else if(!ServiceSwitch.IsToggled)
            {
                stopit = false;
                Refreshh.IsEnabled = false;
                DBFirebaseServices db = new DBFirebaseServices();
                
                if (check==true)
                {
 await db.deleteasync(Email);
                    check = false;
                }
               

            }

        }
        public ObservableCollection<ResultUdata> Ulist = new ObservableCollection<ResultUdata>();
        public  bool stopit;
        private async Task NewMethod(DBFirebaseServices service, Users data)
        {
            var data1 = await service.notifyme(data.Id);
          //  await DisplayAlert("uemail", data1, "ok");
            if (data1 == "notfound")
            {
                var masg = "searching..";
                //  DependencyService.Get<Messageinterface>().longmessageshow(masg);
               // ActivityIndicator activityIndicator = new ActivityIndicator { Color = Color.Orange  };

                await DisplayAlert("Not found", masg,"ok");

                stopit = true;
            }
            else
            {
                var Udata = await service.GetuPerson(data1);
                Userlist.BeginRefresh();
                await DisplayAlert("Need Assistance", Udata.Name + "With Email " + Udata.Email + " Contact Number " + Udata.Details, "ok");
                Ulist = new ObservableCollection<ResultUdata>()
                {
                 new ResultUdata {UName=Udata.Name,UDetails=Udata.Details,UEmail=Udata.Email }
                };
                Userlist.ItemsSource = Ulist;
                Userlist.EndRefresh();
                stopit = false;
                ServiceSwitch.IsToggled = false;
                
            }
        }
 /*       protected override void OnAppearing()
        {
            base.OnAppearing();
            
                

                Device.StartTimer(TimeSpan.FromMilliseconds(5000), loop2);
                bool loop2()
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _ = updateUIAsync();
                    //more stuff;
                });
                    return stopit;
                }
            
        }*/
        private  void  ToolbarItem_Clicked(object sender, EventArgs e)
        {
             Navigation.PushAsync( new Dashboard.ProfilePage(Email));

        }
private async Task  updateUIAsync()
        {
            //  Refreshh.IsRefreshing = true;
            // return Task.CompletedTask;
            DBFirebaseServices db = new DBFirebaseServices();
            try
            {
                var DATA = await db.GetPerson(Email);
                //  await DisplayAlert("id", DATA.Id.ToString(), "ok");
                idc = DATA.Id;
                await NewMethod(db, DATA);
               // await Task.Delay(1000);
                //Refreshh.IsRefreshing = false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }

        public int idc = 0;
      /*  private async void Refreshh_Refreshing(object sender, EventArgs e)
        {
            DBFirebaseServices db = new DBFirebaseServices();
            try
            {
 var DATA = await db.GetPerson(Email);
             //  await DisplayAlert("id", DATA.Id.ToString(), "ok");
                idc = DATA.Id;
           await NewMethod(db,DATA);
            await Task.Delay(1000);
            Refreshh.IsRefreshing = false;
            }
            catch (Exception ex)
            {

                throw new Exception (ex.ToString());
            }
           
            
            
        }*/

        private void Userlist_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tap = e.Item as ResultUdata;
            //  MessagingCenter.Send<ProfilePage>(new ProfilePage(),idc.ToString());
            DBFirebaseServices db = new DBFirebaseServices();
          //   db.Loadmeassages(idc.ToString());
            Navigation.PushAsync(new ChatServices.ChatPage(Email,mname,idc));
        }
    }
}