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
        public TimeSpan? startTime { get; set; }
        public async Task StartStopWatch(Action state, TimeSpan? runnerStartTime)
        {
            stopWatchActive = true;

            if (runnerStartTime == null) startTime = DateTime.Now.TimeOfDay;
            else startTime = runnerStartTime;


            while (stopWatchActive)
            {
                await Task.Delay(1000);

                if (stopWatchActive)
                {
                    //Because of the Task.Delay, chances are that when the “Stop” button is clicked, the cycle has already started.
                    //This means that if we do not check for the Boolean value it will add another second to the already reset variable.
                    stopWatchValue = (TimeSpan)(DateTime.Now.TimeOfDay - startTime);
                    state();
                }
            }
        }

        public void StopStopWatch()
        {
            stopWatchActive = false;
        }
    }
}
