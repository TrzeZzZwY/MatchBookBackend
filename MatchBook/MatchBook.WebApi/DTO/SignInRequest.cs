using MatchBook.Domain;
using MediatR;

namespace MatchBook.WebApi.DTO
{
    public class SignInRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
