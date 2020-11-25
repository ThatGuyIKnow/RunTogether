using System;
using System.Collections.Generic;
using System.Text;
using Bunit;
using RunTogether.Shared.Etc.Helpers;
using Xunit;

namespace RunTogetherTests
{
    public class EventHandlerHelperTests : TestContext
    {
        [Fact]
        public void EventHandleHelper_AddHandler_HandlerContains()
        {
            EventHandlerHelper handler = new EventHandlerHelper();

            handler.AddHandler("click", Console.WriteLine);

            Assert.Contains("CLICK", handler.EventHandlers.Keys);
        }

        [Fact]
        public void EventHandleHelper_TriggerNoObject_EventTriggers()
        {
            bool result = false;
            EventHandlerHelper handler = new EventHandlerHelper();
            handler.AddHandler("click", o => result = true);

            handler.Trigger("click");

            Assert.True(result);
        }
        [Fact]
        public void EventHandleHelper_TriggerWithObject_EventTriggers()
        {
            bool result = false;
            EventHandlerHelper handler = new EventHandlerHelper();
            handler.AddHandler("click", o => result = o == "true");

            handler.Trigger("click", "true");

            Assert.True(result);
        }
    }
}
