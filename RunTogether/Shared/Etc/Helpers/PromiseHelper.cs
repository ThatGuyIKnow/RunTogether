using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.QR.QRScanner
{
    public class PromiseHelper
    {
        private DotNetObjectReference<PromiseHelper> objRef;

        // Generic Types isn't available via JS Interop as of current, so...
        private Action<string> _resolve;
        private Action<string> _reject;

        public void SetResolve(Action<string> resolve)
        {
            _resolve = resolve;
        }
        public void SetReject(Action<string> reject)
        {
            _reject = reject;
        }

        [JSInvokable]
        public void ResolvePromiseString(string data)
        {
            objRef?.Dispose();
            _resolve(data);
        }

        [JSInvokable]
        public void RejectPromiseString(string data)
        {
            objRef?.Dispose();
            _reject(data);
        }
    }
}
