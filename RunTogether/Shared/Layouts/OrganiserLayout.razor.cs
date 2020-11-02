using Radzen;
using RunTogether.Shared.Forms;
using System.Collections.Generic;

namespace RunTogether.Shared.Layouts
{
    public partial class OrganiserLayout
    {
        void ShowDialog(MenuItemEventArgs e)
        {
            if (e.Text == "Skab ny løber")
            {
                dialogService.Open<CreateNewRunner>("Skab ny løber",
                    new Dictionary<string, object>(),
                    new DialogOptions() { Width = "700px", Height = "530px", Left = "calc(50% - 350px)", Top = "calc(50% - 265px)" });
            }

            if (e.Text == "Rediger løber")
            {
                dialogService.Open<EditRunner>("Rediger løber",
                    new Dictionary<string, object>(),
                    new DialogOptions() { Width = "700px", Height = "530px", Left = "calc(50% - 350px)", Top = "calc(50% - 265px)" });
            }

            if (e.Text == "Skab ny rute")
            {
                dialogService.Open<CreateNewRoute>("Skab ny rute",
                    new Dictionary<string, object>(),
                    new DialogOptions() { Width = "700px", Height = "530px", Left = "calc(50% - 350px)", Top = "calc(50% - 265px)" });
            }

            if (e.Text == "Rediger rute")
            {
                dialogService.Open<EditRoute>("Rediger rute",
                    new Dictionary<string, object>(),
                    new DialogOptions() { Width = "700px", Height = "530px", Left = "calc(50% - 350px)", Top = "calc(50% - 265px)" });
            }
        }
    }
}
