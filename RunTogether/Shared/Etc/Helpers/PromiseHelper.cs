using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.QR.QRScanner
{
    public class PromiseHelper<T>
    {
        private readonly DotNetObjectReference<PromiseHelper<T>> objRef;

        // Generic Types isn't available via JS Interop as of current, so...
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
            objRef?.Dispose();
            _resolve?.Invoke(data);
        }

        [JSInvokable]
        public void RejectPromiseString(T data)
        {
            objRef?.Dispose();
            _reject?.Invoke(data);
        }
    }
}
