using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Domain.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException(string message = "An identity exception occured"): base(message)
        {
            
        }
    }
}
