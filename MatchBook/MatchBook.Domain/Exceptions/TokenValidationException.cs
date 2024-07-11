using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Domain.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message = "Invalid token") : base(message)
        {

        }
    }
}
