using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunTogether.Data;
using Microsoft.AspNetCore.Components.Forms;

namespace RunTogether.Shared.Forms
{
    public partial class CreateNewSponsor
    {

        Sponsor NewSponsor = new Sponsor();

        private IList<string> imageDataUrls = new List<string>();

        async public void OnSubmit(Sponsor sponsor)
        {

            dbContext.Sponsors.Add(sponsor);
            dbContext.SaveChanges();
            this.dialogService.Close(true);
        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("Sponsor Submission was invalid.");
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var format = "image/png";

            var file = await e.File.RequestImageFileAsync(format, 400, 400);
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            NewSponsor.Image = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        }

    }
}
