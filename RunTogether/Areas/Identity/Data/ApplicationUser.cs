﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RunTogether.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] 
        public int RunnerId { get; set; } = 0;

        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }

        public int? RunId { get; set; }
        public Run? Run { get; set; }

        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }
    }
}