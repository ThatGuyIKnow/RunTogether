using Microsoft.AspNetCore.Identity;
using RunTogether.Shared.Forms;
using RunTogether.Areas.Identity;
using System.Collections.Generic;
using RunTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Linq;

namespace RunTogether.Pages.AdminPages
{
    public partial class Sponsors
    {

        [Parameter] public int id { get; set; }

        RadzenGrid<ApplicationUser> sponsorTable;

        Sponsors run = new Sponsors();

        protected override async Task OnInitializedAsync()
        {

           /* sponsor = dbContext.Sponsors
                .Where(r => r.ID == id)
                .Include(r => r.Runners).FirstOrDefault();*/

        }

    }
}
