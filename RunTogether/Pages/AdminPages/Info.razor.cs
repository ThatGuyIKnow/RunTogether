using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace RunTogether.Pages.AdminPages
{
    public partial class Info
    {
        [Parameter] public int id { get; set; }

        Run run = new Run();

        protected override async Task OnInitializedAsync()
        {

            run = dbContext.Runs
                .Find(id);

        }
    }
}
