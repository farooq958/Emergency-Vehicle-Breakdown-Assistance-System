using EVBAS.Dashboard;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EVBAS.Model
{
 public  class DBFirebaseServices
    {
        FirebaseClient client;

        public ObservableCollection<Resultdata> Emplst { get; private set; }

        public DBFirebaseServices ()
        {
            client = new FirebaseClient("https://emergency-system-eb0f8-default-rtdb.firebaseio.com/");
        }
        public  async Task Addulocation(double lattitude,double longitude)
        {
            uLocation U = new uLocation() {ulattitude=lattitude , ulongitude = longitude  };
            await client.Child("Ulocation").PostAsync(U);

        }
        public async Task Addmlocation(double lattitude, double longitude)
        {
           Mlocation M = new Mlocation () { mlattitude = lattitude, mlongitude = longitude };
            await client.Child("Mlocation").PostAsync(M);

        }
        public async Task AddmDetails(int id, string name, string details,double lattitude,double longittude,string email)
        {
            Mlocation M = new Mlocation() { Id = id, Name = name, Details = details ,mlattitude = lattitude ,mlongitude = longittude , Email= email};
            await client.Child("MDetails").PostAsync(M);

        }
        public async Task AddLoginDetails(Guid guid, int id, string name, string details, double lattitude, double longittude, string email,string description, string type)
        {
            Users M = new Users() {guidd = guid.ToString() , Id = id, Name = name, Details = details, mlattitude = lattitude, mlongitude = longittude, Email = email,Description = description ,Type = type  };
            await client.Child("LoginDetailsM").PostAsync(M);

        }
        public async Task AdduDetails(int id, string name, string details, string email)
        {
            uLocation M = new uLocation() { Id = id, Name = name, Details = details, Email = email };
            await client.Child("UDetails").PostAsync(M);

        }
        public ObservableCollection<uLocation> GetULocations()
        {
            var udata = client.Child("Ulocation").AsObservable<uLocation>().AsObservableCollection();
            return udata;
        }

        public async Task<bool> GetTypeAsync(string Emaill)
        {
            var data = await GetMlocationsAsync();
            bool flag = false;
            foreach (var item in data)
            {
                if (Emaill == item.Email)
                {

                     flag=true;
                }
            }
            return flag;

        }
        public async Task deleteasync( string email)

        {
         //   var datta = await GetPerson(email);

            /*var todelete = (await client.Child("Mdetails").OnceAsync<Mlocation>()).FirstOrDefault(a => a.Object.Name == datta.Name || a.Object.Id == datta.Id || a.Object.Email == datta.Email ||
             a.Object.mlattitude == datta.mlattitude || a.Object.mlongitude == datta.mlongitude || a.Object.Details == datta.Details);*/

            var todelete = (await client.Child("MDetails").OnceAsync<Mlocation>()).Where(a => a.Object.Email == email).LastOrDefault();
            await client.Child("MDetails").Child(todelete.Key).DeleteAsync();

           /* await client.Child("Mdetail").Child(todelete.Email).DeleteAsync();
            await client.Child("Mdetail").Child(todelete.Id.ToString()).DeleteAsync();
            await client.Child("Mdetail").Child(todelete.Details).DeleteAsync();
            await client.Child("Mdetail").Child(todelete.mlattitude.ToString()).DeleteAsync();
            await client.Child("Mdetail").Child(todelete.mlongitude.ToString()).DeleteAsync();
            await client.Child("Mdetail").Child(todelete.Name).DeleteAsync();*/


        }
        public async Task<Users> GetPerson(string email)
        {
            List<Users> allPersons = await GetLoginDetails();
            
            var returnn = allPersons.Find(a => a.Email == email);
          
            return returnn;
           
        }
        public async Task<bool> gettype (string email)
        {
            List<Users> allPersons = await GetLoginDetails();

            var returnn = allPersons.Find(a => a.Email == email);
            if (returnn == null)
            {
                return true;
            }

            return false;
        }
        public async Task<uLocation> GetuPerson(string email)
        {
            List<uLocation> allPersons = await GetUlocationsAsync();

            var returnn = allPersons.Where(a => a.Email == email).FirstOrDefault();

            return returnn;

        }


        public async Task<List<Mlocation>> GetMlocationsAsync()
        {

            return (await client.Child("MDetails")
              .OnceAsync<Mlocation>()).Select(item => new Mlocation
              {

                  mlattitude = item.Object.mlattitude,
                  mlongitude = item.Object.mlongitude,
                  Details = item.Object.Details,
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                 Email = item.Object.Email

              }).ToList();
        }
        public async Task<List<Users>> GetLoginDetails()
        {

            return (await client.Child("LoginDetailsM")
              .OnceAsync<Users>()).Select(item => new Users
              {

                  mlattitude = item.Object.mlattitude,
                  mlongitude = item.Object.mlongitude,
                  Details = item.Object.Details,
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                  Email = item.Object.Email,
                  Type = item.Object.Type,
                  Description = item.Object.Description,
                  guidd = item.Object.guidd
                  
                  
              }).ToList();
        }
        public async Task<List<uLocation>> GetUlocationsAsync()
        {

            return (await client.Child("UDetails")
              .OnceAsync<uLocation>()).Select(item => new uLocation
              {

                  ulattitude = item.Object.ulattitude,
                  ulongitude = item.Object.ulongitude,
                  Details = item.Object.Details,
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                  Email = item.Object.Email

              }).ToList();
        }
        
       public async Task notifyAsync( string umail,string memail,int id)

        {
            

          Responses M = new Responses() { UEmail = umail, MEmail = memail,MId =id };
            await client.Child("ResponseToMechanic").PostAsync(M);


        }
        public async Task<List<Responses>> GetNotify()
        {

            return (await client.Child("ResponseToMechanic")
              .OnceAsync<Responses>()).Select(item => new Responses
              {
                  MEmail = item.Object.MEmail,
                  UEmail = item.Object.UEmail,
                  MId = item.Object.MId


              }).ToList();
        }
        public async Task updateid(string Memail)
        {
            Random rd = new Random();
            var toUpdateid = (await client
              .Child("LoginDetailsM")
              .OnceAsync<Users>()).Where(a => a.Object.Email == Memail).LastOrDefault();
            await client
              .Child("LoginDetailsM")
              .Child(toUpdateid.Key)
              .PutAsync(new Users() {Email = Memail, Id = rd.Next(100000),Name = toUpdateid.Object.Name,Type = toUpdateid.Object.Type,Description = toUpdateid.Object.Description,Details = toUpdateid.Object.Details });

        }
   public async Task<string> notifyme(int mid)
{
            var data = await GetNotify();
            var returnn = data.Find(a => a.MId == mid);
            if (returnn == null)
            {
                return "notfound"  ;
            }
            else

            return returnn.UEmail;


        }
    /*    public async Task<List<chat>> getkeys ()
        {
            var keys = (await client.Child("ChatApp").OnceAsync<chat>()).Select(
                item => new chat

                {
                    Idd = item.Key,
                   

                }).ToList();

            return keys;

        }*/
     /*   public async Task<string> getthekeyss(string keyc)
        {
            var allm = await getkeys();
            var keyee = allm.Find(a => a.Idd == keyc);
            // var key=  await (client.Child("ChatApp").OnceAsync<chat>()).Where(A=)
            return keyee.ToString();

        }*/

        public async Task saveMessage(chat _ch, string _room)
        {

            await client.Child("ChatApp/" + _room + "/Messages")
                    .PostAsync(_ch);
        }
        public async Task savefeedback(string mail,string userf,string usern,string guidd)
        {
            Feedback fk = new Feedback { Memail = mail, Userfeedback = userf, Username = usern };
            await client.Child("Feedbacks/"+ guidd+"/feedback")
                    .PostAsync(fk);
        }
        public async Task <List<chat>> subChat(string _roomKEY)
        {
         //var key= getthekeyss(keyy);

            var toreturn = (await client.Child("ChatApp/"+_roomKEY+"/Messages").OnceAsync<chat>()).Select(
                  item => new chat

                  {
                     UserMessage = item.Object.UserMessage,
                     UserName = item.Object.UserName,
                     dateTimee = item.Object.dateTimee
                     

                  }).ToList();

            return toreturn.OrderBy(x=>x.dateTimee).ToList();
        }
        public bool chek = true;
        public DateTime dt;
        public async Task<List<chat>> filterChat(string _roomKEY)
        {
            //var key= getthekeyss(keyy);

            var toreturn = await subChat(_roomKEY);
            if (chek == true)
            {
var lasttime = toreturn.FirstOrDefault().dateTimee;
                dt = lasttime;
                chek = false;
            }
            
     // toreturn.Sort((x,y) => x.dateTimee.CompareTo(dt));
            // Console.WriteLine(lasttime.dateTimee);
           var sortt = toreturn.OrderBy(x => x.dateTimee.ToString().Replace(x.dateTimee.ToString(),dt.ToString()));

            return sortt.ToList();
        }


        public  void Loadmeassages (string id)
        {

           // DBFirebaseServices db = new DBFirebaseServices();
         //   chat ch = new chat();
           /* var data = await db.subChat(id);
            foreach (var item in data)
            {
                ch.UserMessage = item.UserMessage;
                ch.UserName = item.UserName;
            }*/
            //MessagingCenter.Send<chat>(ch, id);

        }

        public async Task<List<Feedback>> getALLFeedbacks(string _guidd)
        {

            return (await client.Child("Feedbacks/"+_guidd+"/feedback")
              .OnceAsync<Feedback>()).Select(item => new Feedback
              {
                  Userfeedback = item.Object.Userfeedback,
                  Username = item.Object.Username,
                  Memail = item.Object.Memail
                 

              }).ToList();
        }
      

    }
}
