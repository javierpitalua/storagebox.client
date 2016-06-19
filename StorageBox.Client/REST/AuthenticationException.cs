using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client.REST
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string errorMessage, AuthenticationResponse response) : base(errorMessage)
        {
            this.Response = response;
        }

        public AuthenticationResponse Response { get; set; }
    }
}
