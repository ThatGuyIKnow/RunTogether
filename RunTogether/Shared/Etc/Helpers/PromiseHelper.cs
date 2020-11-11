using System;
using Microsoft.JSInterop;

namespace RunTogether.Shared.Etc.Helpers
{
    public class PromiseHelper<T> : IDisposable
    {
        public readonly DotNetObjectReference<PromiseHelper<T>> objRef;

        // Generic Types isn't available via JS Interop as of current, so the class needs the 
        // expected return type defined at creation
        private Action<T>? _resolve;
        private Action<T>? _reject;

        public PromiseHelper()
        {
            this.objRef = DotNetObjectReference.Create<PromiseHelper<T>>(this);
        }

        public void SetResolve(Action<T> resolve)
        {
            _resolve = resolve;
        }
        public void SetReject(Action<T> reject)
        {
            _reject = reject;
        }

        [JSInvokable]
        public void ResolvePromise(T data)
        {
            _resolve?.Invoke(data);
            Dispose();
        }

        [JSInvokable]
        public void RejectPromise(T data)
        {
            _reject?.Invoke(data);
            Dispose();
        }

        public void Dispose()
        {
            this.objRef?.Dispose();
        }
    }
}
