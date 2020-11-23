using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen.Blazor;
using RunTogether.Data;
using Microsoft.AspNetCore.Identity;
using RunTogether.Shared.Forms;
using RunTogether.Areas.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Radzen;

namespace RunTogether.Pages.AdminPages
{
    public partial class SponsorPage
    {
        [Parameter] public int id { get; set; }
        public string imageChange { get; set; }

        IQueryable<Sponsor> SponsorList;
        RadzenGrid<Sponsor> sponsorTable;
        

        protected override async Task OnInitializedAsync()
        {

            SponsorList = dbContext.Sponsors;

            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }

        void Close(dynamic result)
        {
            sponsorTable.Reload();
            StateHasChanged();
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var format = "image/png";

            var file = await e.File.RequestImageFileAsync(format, 100, 100);
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            imageChange = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        }



        void OnUpdateRow(Sponsor selectedSponsor)
        {
            imageChange = selectedSponsor.Image;
            dbContext.Update(selectedSponsor);
            dbContext.SaveChanges();
        }
        void EditRow(Sponsor selectedSponsor)
        {
            sponsorTable.EditRow(selectedSponsor);
        }
        void SaveRow(Sponsor selectedSponsor)
        {
            selectedSponsor.Image = imageChange;
            sponsorTable.UpdateRow(selectedSponsor);
        }
        void CancelEdit(Sponsor selectedSponsor)
        {
            sponsorTable.CancelEditRow(selectedSponsor);

            var sponSorEntry = dbContext.Entry(selectedSponsor);
            if (sponSorEntry.State == EntityState.Modified)
            {
                sponSorEntry.CurrentValues.SetValues(sponSorEntry.OriginalValues);
                sponSorEntry.State = EntityState.Unchanged;
            }
        }
        void DeleteRow(Sponsor selectedSponsor)
        {
            if (SponsorList.Contains(selectedSponsor))
            {
                dbContext.Remove<Sponsor>(selectedSponsor);

                dbContext.SaveChanges();

                sponsorTable.Reload();
            }
            else
            {
                sponsorTable.CancelEditRow(selectedSponsor);
            }
        }

    }
}
