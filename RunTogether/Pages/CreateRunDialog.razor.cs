using Radzen;
using System;

namespace RunTogether.Pages
{
    public partial class CreateRunDialog
    {
        Run run = new Run();

        void OnSubmit(Run run)
        {
            Console.WriteLine("YEY!");
            this.dialogService.Close(true);
        }

        void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
            Console.WriteLine("AW");
        }
    }
}
