using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace RunTogether.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int RunnerId { get; set; } = 0;

        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }

        public int? RunId { get; set; }
        [JsonIgnore]
        public Run? Run { get; set; }
        [JsonIgnore]
        public List<StageAssignment> StageAssignments { get; set; } = new List<StageAssignment>();

        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }

        public override string ToString()
        {
            return this.FirstName;
        }
    }
}