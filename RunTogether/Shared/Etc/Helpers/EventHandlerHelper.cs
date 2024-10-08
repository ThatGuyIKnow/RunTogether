﻿using System;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace RunTogether.Shared.Etc.Helpers
{
    public class EventHandlerHelper : IDisposable
    {
        public readonly DotNetObjectReference<EventHandlerHelper> ObjRef;

        private readonly Dictionary<string, Action<string>> _eventHandlers = new Dictionary<string, Action<string>>();
        public Dictionary<string, Action<string>> EventHandlers => _eventHandlers;

        public EventHandlerHelper()
        {
            ObjRef = DotNetObjectReference.Create(this);
        }

        public void AddHandler(string eventTrigger, Action<string> handler)
        {
            string eventTriggerNorm = eventTrigger.ToUpper();
            _eventHandlers.Add(eventTriggerNorm, handler);
        }

        public bool RemoveHandler(string eventTrigger)
        {
            string eventTriggerNorm = eventTrigger.ToUpper();
            return _eventHandlers.Remove(eventTriggerNorm);
        }

        [JSInvokable]
        public void Trigger(string eventTrigger, string? data = null)
        {
            string eventTriggerNorm = eventTrigger.ToUpper();
            try
            {
                _eventHandlers[eventTriggerNorm].Invoke(data);
            }
            catch (Exception e) when (e is ArgumentNullException || e is KeyNotFoundException)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            ObjRef.Dispose();
        }
    }
}
