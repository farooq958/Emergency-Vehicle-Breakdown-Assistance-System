using EVBAS.Dashboard;
using EVBAS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EVBAS.ChatServices
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public int idd;
        
        public string username;
        public ObservableCollection<chat> chats = new ObservableCollection<chat>();
        DBFirebaseServices db = new DBFirebaseServices();

        public ChatPage()
        {
            

          /*  MessagingCenter.Subscribe<DBFirebaseServices>(this, idd.ToString(), async (p) =>
            {



                var data = await db.subChat(idd.ToString());
                foreach (var item in data)
                {
chats = new ObservableCollection<chat>()
                {
                    new chat{ UserMessage = item.UserMessage , UserName = item.UserName }

                };
                }
                
                 Chat.ItemsSource = chats;
                MessagingCenter.Unsubscribe<DBFirebaseServices>(this, idd.ToString());
            });/*
            /*  MessagingCenter.Subscribe<chat>(this, idd.ToString(),(p) =>
              {


                  DisplayAlert("subscribed succesfully", "pk","cancel");

                 // Chat.ItemsSource = chats;
                  MessagingCenter.Unsubscribe<chat>(this, idd.ToString());
              });*/

        }
    /*    protected override void OnAppearing()
        {
            base.OnAppearing();

            DBFirebaseServices db = new DBFirebaseServices();
            //db.Loadmeassages(idd.ToString());
            Device.StartTimer(TimeSpan.FromMilliseconds(5000), loop2);
            bool loop2()
            {
                Device.BeginInvokeOnMainThread(() => {
                    _ = updateUIAsync();
                    //more stuff;
                });
                return true;
            }
            //Chat.BeginRefresh();
        }*/

        private async Task updateUIAsync()
        { Chat.BeginRefresh();
            var data = await db.subChat(idd.ToString());

            // await DisplayAlert("ok", data.Count.ToString(), "ok");
            // var dataa =  data.o();
            Chat.BindingContext = data;
            Chat.EndRefresh();
           /* foreach (var item in data)
            {
                chats = new ObservableCollection<chat>()
                {
                    new chat{ UserMessage = item.UserMessage , UserName = item.UserName }

                };
Chat.ItemsSource = chats;
            }*/
     // await  DisplayAlert("ok", chats.Count.ToString(),"cancel");
            
        }

        public ChatPage(string Email,string Username,int id)
        {
            InitializeComponent();
            // OnAppearing();
           
            username = Username;
            idd = id;
            Device.StartTimer(TimeSpan.FromMilliseconds(2000), loop2);
            bool loop2()
            {
                Device.BeginInvokeOnMainThread(() => {
                    _ = updateUIAsync();
                    //more stuff;
                });
                return true;
            }
            //   DisplayAlert(idd.ToString(),"ok","ok");
            /*  MessagingCenter.Subscribe<chat>(this, id.ToString(), async (p) =>
               {

                 var data = await  db.subChat(id.ToString());

                   foreach (var item in data)
                   {
    chats = new ObservableCollection<chat>()
                   {
                       new chat{ UserMessage = item.UserMessage , UserName = item.UserName }

                   };

                   }



                  // Chat.ItemsSource = chats;
                   Chat.BindingContext = db.subChat(id.ToString());
                   MessagingCenter.Unsubscribe<chat>(this, idd.ToString());
               });
               /*   MessagingCenter.Subscribe<ProfilePage>(this, id.ToString(), (p) =>
                  {


                      DisplayAlert("subscribed succesfully", "pk", "cancel");
                      Chat.BindingContext = db.subChat(id.ToString(),id.ToString());
                      // Chat.ItemsSource = chats;
                      MessagingCenter.Unsubscribe<ProfilePage>(this, id.ToString());
                  });*/

        }
        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var chatOBJ = new chat { UserMessage = entMessage.Text, UserName = username ,dateTimee = DateTime.Now};
            await db.saveMessage(chatOBJ, idd.ToString());
           
            entMessage.Text = "";

        //  await updateUIAsync();
            //  Chat.ItemsSource = await db.subChat(idd.ToString(), "d");
          //  Chat.ItemsSource = await db.subChat(idd.ToString());
        }
    }
}