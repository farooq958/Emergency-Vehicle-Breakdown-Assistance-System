using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVBAS.Registration
{
    public interface IAuth
    {

        Task<string> LoginWithEmailAndPassword(string email, string Password);
        Task<string> SignUpWithEmailAndPassword(string email, string Password);
        bool IsSignIn();
    }
}
