using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.Forms
{
    public partial class EditRoute
    {
        IEnumerable<Stage> stages;


        //protected override void OnInitialized()
        //{
        //    //customers = dbContext.Customers.ToList(); <- exempel fra https://blazor.radzen.com/dropdown

        //    stages = applicationDbContext.Stages.ToList();  //måske?
        //}

        string value;

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        void Change(string value, string name)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value}");
            StateHasChanged();
        }

        void ChangeBound(object value, string name)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value}");
            StateHasChanged();
        }
    }
}
