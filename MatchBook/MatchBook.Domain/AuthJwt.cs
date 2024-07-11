using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchBook.Domain
{
    public class AuthJwt
    {
        public AuthJwt()
        {
            
        }

        public AuthJwt(string token, string refreshToken, int userId)
        {
            Token = token;
            RefreshToken = refreshToken;
            UserId = userId;
        }
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public int UserId { get; set; }
    }
}
