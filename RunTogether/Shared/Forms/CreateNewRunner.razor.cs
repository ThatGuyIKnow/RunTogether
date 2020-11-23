using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RunTogether.Shared.Forms
{
    public partial class CreateNewRunner
    {
        [Parameter] public Run selectedRun { get; set; }

        ApplicationUser runner = new ApplicationUser();
        RadzenCompareValidator compareValidator;

        public string targetValue { get; set; }

        async public void OnSubmit(string firstName, string lastName, string email, Run selectedRun)
        {
            targetValue = default;
            if (selectedRun.Runners.Any(r => r.Email == runner.Email) == false)
            {
                Console.WriteLine(selectedRun.Name + " " + selectedRun.ID);
                await userCreation.CreateRunner(firstName, lastName, email, selectedRun);
                Console.WriteLine("YEY!");
                this.dialogService.Close(true);
            }
            else
            {
                targetValue = runner.Email;
                compareValidator.EditContext.Validate();
            }
        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("AW");
        }

    }
}
