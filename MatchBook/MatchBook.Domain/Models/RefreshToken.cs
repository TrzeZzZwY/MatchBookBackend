﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchBook.Domain.Models.Identity;

namespace MatchBook.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }

        public int UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
