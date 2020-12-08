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

        async public void OnSubmit(string firstName, string lastName, string email, Run selectedRun)
        {
                Console.WriteLine(selectedRun.Name + " " + selectedRun.ID);
                await userCreation.CreateRunner(firstName, lastName, email, selectedRun);
                this.dialogService.Close(true);
        }


        //retunere Email hvis den blev fundet ellers default
        public string FindRunner(string Email)
        {
            if (selectedRun.Runners.Any(r => r.Email == Email) == true)
            {
                return Email;
            }
            return default;
        }

        void OnInvalidSubmit()
        {
<<<<<<< Updated upstream
            Console.WriteLine("Runner Submit was invalid.");
=======
>>>>>>> Stashed changes
        }

    }
}
