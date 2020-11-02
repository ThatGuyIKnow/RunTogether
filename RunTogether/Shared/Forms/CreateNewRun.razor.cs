using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.Forms
{
    public partial class CreateNewRun
    {
        Run run = new Run();

        public void OnSubmit(String RunName, DateTime Start, DateTime End, String QR)
        {
            Run RunObj = new Run() { Name = RunName, StartDate = Start, EndDate = End, QRString = QR };
            Console.WriteLine("YEY!");
            this.dialogService.Close(true);
            dbContext.Runs.Add(RunObj);
            dbContext.SaveChanges();
        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("AW");
        }
    }
}
