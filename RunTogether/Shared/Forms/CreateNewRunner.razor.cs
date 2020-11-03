using Microsoft.AspNetCore.Components;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RunTogether.Shared.Forms
{
    public partial class CreateNewRunner
    {

        [Parameter] public Run selectedRun { get; set; }
        ApplicationUser runner = new ApplicationUser();


        async public void OnSubmit(string firstName,string lastName, string email, Run selectedRun)
        {

            await userCreation.CreateRunner(firstName, lastName, email, selectedRun);
            Console.WriteLine("YEY!");
            this.dialogService.Close(true);
        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("AW");
        }

    }
}
