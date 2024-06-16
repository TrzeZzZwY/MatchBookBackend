using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Domain.Exceptions
{
    public class LoginCredentialsException : Exception
    {
        public LoginCredentialsException(string message = "Invalid credentials") : base(message)
        {

        }
    }
}
