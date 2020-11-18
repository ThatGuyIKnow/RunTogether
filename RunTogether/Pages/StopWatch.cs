using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Pages
{
    public class CustomStopWatch
    {
        public bool stopWatchActive { get; set; }
        public TimeSpan stopWatchValue { get; set; }
        public async Task StartStopWatch(Action state)
        {
            this.stopWatchActive = true;
            Debug.WriteLine(this.stopWatchActive);
            while (this.stopWatchActive)
            {
                await Task.Delay(1000);

                if (this.stopWatchActive)
                {
                    //Because of the Task.Delay, chances are that when the “Stop” button is clicked, the cycle has already started.
                    //This means that if we do not check for the Boolean value it will add another second to the already reset variable.
                    this.stopWatchValue = stopWatchValue.Add(new TimeSpan(0, 0, 1));
                    //StateHasChanged();
                    state();
                    Debug.WriteLine(this.stopWatchValue);
                }
            }
        }

        public void StopStopWatch()
        {
            this.stopWatchActive = false;
        }
    }
}
