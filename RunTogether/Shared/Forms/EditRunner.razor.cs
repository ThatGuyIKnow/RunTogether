using System;
using System.Collections.Generic;

namespace RunTogether.Shared.Forms
{
    public partial class EditRunner
    {
        string value;

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        void Change(string value, string name)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value}");
            StateHasChanged();
        }
    }
}
