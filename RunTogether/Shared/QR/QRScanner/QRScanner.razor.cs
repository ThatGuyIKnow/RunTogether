﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RunTogether.Shared.QR.QRScanner
{
    public partial class QRScanner : IDisposable
    {
        private const string HideCss = "display-none";
        private const string EnterCss = "enterUp";
        private const string ExitCss = "exitDown";

        private string _currentClass = HideCss;

        private string? _qrCode = null;
        private bool hasCamera = false;
        private bool hasFlash = false;
        private bool flashOn = false;

        [Parameter]
        public int FadeTime { get; set; } = 500;

        [Parameter]
        public EventCallback<string> OnCodeScanned { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                hasCamera = await jsRuntime.InvokeAsync<bool>("Main.QrScanner.HasCamera");
                StateHasChanged();
            }
        }


        private async void InitializeQrScanner()
        {
            // Create QR Scanner and start it up
            CreateQrScanner();
            StartQrScanner();
        }


        private async Task CreateQrScanner()
        {
            // When the QR Scanner returns a code, execute the function
            PromiseHelper.SetResolve(CodeReceivedHandler);

            // (Creation function, video HTML id, objRef from PromiseHelper)
            jsRuntime.InvokeVoidAsync(
                "Main.QrScanner.CreateQrScanner",
                "qrVideo",
                DotNetObjectReference.Create(PromiseHelper));

            // Check if camera has flash available
            hasFlash = await jsRuntime.InvokeAsync<bool>("Main.QrScanner.HasFlash");
            StateHasChanged();
        }


        private void StartQrScanner()
        {
            // Animates entrance and starts the scanner
            _currentClass = EnterCss;
            jsRuntime.InvokeAsync<string>("Main.QrScanner.StartQrScanner");
        }

        private void DestroyQrScanner()
        {
            // Animate exit and wait for animation to finish
            _currentClass = ExitCss;

            Task.Run(async () =>
            {
                await Task.Delay(FadeTime);
                await jsRuntime.InvokeVoidAsync(
                    "Main.QrScanner.DestroyQrScanner",
                    "qrVideo");
                _currentClass = HideCss;
            });
        }

        private void CodeReceivedHandler(string data)
        {
            // After the code has been scanned and sent out to the
            // event handlers, the QrScanner closes.
            _qrCode = data;

            OnCodeScanned.InvokeAsync(_qrCode);

            DestroyQrScanner();
        }

        private void ToggleFlash()
        {
            flashOn = !flashOn;
            jsRuntime.InvokeVoidAsync(
                "Main.QrScanner.ChangeFlashStatus",
                flashOn);
            StateHasChanged();
        }

        public void Dispose()
        {
            jsRuntime.InvokeVoidAsync(
                "Main.QrScanner.DestroyQrScanner",
                "qrVideo");
        }

}
}
