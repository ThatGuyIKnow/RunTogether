using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RunTogether.Areas.Identity
{

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = IdentityRoleTypes.Runner,
                    NormalizedName = "RUNNER",
                    Id = "runner"
                },
                new IdentityRole
                {
                    Name = IdentityRoleTypes.Organiser,
                    NormalizedName = "ORGANISER",
                    Id = "organiser"
                }
            );
        }
    }
}
