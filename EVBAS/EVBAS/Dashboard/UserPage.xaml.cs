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
    public partial class UserPage : ContentPage
    {
        public string emaiil;
        public UserPage(string mail)
        {
            InitializeComponent();

            _ = getusername(mail);
                
            emaiil = mail;
        }

        private async Task getusername(string mail)
        {
             DBFirebaseServices db = new DBFirebaseServices();
            var name =await db.GetuPerson(mail);
            Username.Text = "Welcome " +name.Name; 
        }

        public ObservableCollection<Resultdata> Emplst = new ObservableCollection<Resultdata>();
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));
            Result.Text = $"latt :{result.Latitude},long:{result.Longitude}";
            //   DBFirebaseServices  service = new DBFirebaseServices();
            //await  service.Addulocation(result.Latitude, result.Longitude);

            DBFirebaseServices db = new DBFirebaseServices();
            var mdata = await db.GetMlocationsAsync();
            if (mdata == null)
            {
                await DisplayAlert("Error ", "No mechanic available Please Try again", "Ok");
                return;
            }
            else
            { 
            urloc.IsVisible = true;
            linee.IsVisible = true;
            Distanceinkm.IsVisible = true;
            vmech.IsVisible = true;
            var locationstart = new Location(result.Latitude, result.Longitude);
           // await DisplayAlert("ok", mdata.Count.ToString(), "check");
           // ResultMechanics.ItemsSource = await db.getfiltermechanics(locationstart);
         //   ResultMechanics.ItemsSource = mdata;
           foreach (var item in mdata)
            {
                //var locationstart = new Location(result.Latitude, result.Longitude);
                var locationend = new Location(item.mlattitude, item.mlongitude);
                // await  DisplayAlert("check", locationend.ToString(), "ok");
                var distance = CalculateDistances(locationstart, locationend);
                Distancecheck.Text = distance.ToString();
                   
                    if (mdata.Count!=0)
                {

               
                if (distance < 5)
                {
                    Emplst = new ObservableCollection<Resultdata>() {
            new Resultdata { Id = item.Id , Name = item.Name,Details = item.Details ,Emaill =item.Email
            }


        };

                }
                
                ResultMechanics.ItemsSource = Emplst;}
}
                if (Emplst.Count == 0)
                {
                    Distancecheck.Text = "No Mechanic Availabe";
                   
                }
            }

            
          
        }
            private static double DegreesToRadians(double degrees)
            {
                return degrees * Math.PI / 180.0;
            }
            public static double CalculateDistances(Location location1, Location location2)
            {
                double circumference = 40000.0; // Earth's circumference at the equator in km
                double distance = 0.0;

                //Calculate radians
                double latitude1Rad = DegreesToRadians(location1.Latitude);
                double longitude1Rad = DegreesToRadians(location1.Longitude);
                double latititude2Rad = DegreesToRadians(location2.Latitude);
                double longitude2Rad = DegreesToRadians(location2.Longitude);

                double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

                if (logitudeDiff > Math.PI)
                {
                    logitudeDiff = 2.0 * Math.PI - logitudeDiff;
                }

                double angleCalculation =
                    Math.Acos(
                        Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                        Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

                distance = circumference * angleCalculation / (2.0 * Math.PI);

                return distance;
            }

            private async void ResultMechanics_ItemTapped(object sender, ItemTappedEventArgs e)
            {
                var details = e.Item as Resultdata;
                DBFirebaseServices dB = new DBFirebaseServices();
                await dB.notifyAsync(emaiil, details.Emaill, details.Id);
                await Navigation.PushAsync(new Dashboard.ProfilePage(details.Emaill, "user", emaiil, details.Id));
            Emplst.Clear();
            Distancecheck.Text = "No Mechanic Available";

            }
        }
    }
