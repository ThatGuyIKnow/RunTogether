using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Pages
{
    public class CustomStopWatch
    {
        public bool stopWatchActive;
        public TimeSpan stopWatchValue;
        public async Task StartStopWatch()
        {
            //stopWatchActive = true;
            while (stopWatchActive)
            {
                await Task.Delay(1000);

                if (stopWatchActive)
                {
                    //Because of the Task.Delay, chances are that when the “Stop” button is clicked, the cycle has already started.
                    //This means that if we do not check for the Boolean value it will add another second to the already reset variable.
                    stopWatchValue = stopWatchValue.Add(new TimeSpan(0, 0, 1));
                    //StateHasChanged();
                }
            }
        }

        public bool StopStopWatch()
        {
            stopWatchActive = false;
            return stopWatchActive;
        }
    }
}
