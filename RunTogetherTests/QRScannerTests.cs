using System;
using System.Linq;
using Xunit;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Internal;
using Radzen.Blazor;
using RunTogether.Shared.QR.QRScanner;

namespace RunTogetherTests
{
    public class QRScannerTests : TestContext
    {
        [Fact]
        public void CreateQRScanner_InvokeJS_Exists()
        {
            using var ctx = new TestContext();
            var mockJS = ctx.Services.AddMockJSRuntime();
            mockJS.SetupVoid("Main.QrScanner.CreateQrScanner");
            mockJS.Setup<bool>("Main.QrScanner.HasCamera").SetResult(true);
            var cut = ctx.RenderComponent<QRScanner>();

            cut.WaitForState(() =>
                mockJS.Invocations.Values.SelectMany(x => x).Select(x => x.Identifier).Contains("Main.QrScanner.HasCamera")
                );
            var button = cut.FindComponent<RadzenButton>();
            button.Instance.OnClick(new MouseEventArgs());


            var invocations = mockJS.Invocations.Values.SelectMany(x => x).Select(x => x.Identifier);
            Assert.Contains("Main.QrScanner.CreateQrScanner", invocations);
        }

        [Fact]
        public void StartQRScanner_InvokeJS_UpdatesVideoElem()
        {
            using var ctx = new TestContext();
            var mockJS = ctx.Services.AddMockJSRuntime();
            mockJS.SetupVoid("Main.QrScanner.StartQrScanner");
            mockJS.Setup<bool>("Main.QrScanner.HasCamera").SetResult(true);
            var cut = ctx.RenderComponent<QRScanner>();


            cut.WaitForState(() =>
                mockJS.Invocations.Values.SelectMany(x => x).Select(x => x.Identifier).Contains("Main.QrScanner.HasCamera")
            );
            var button = cut.FindComponent<RadzenButton>();
            button.Instance.OnClick(new MouseEventArgs());

            var invocations = mockJS.Invocations.Values.SelectMany(x => x).Select(x => x.Identifier);
            Assert.Contains("Main.QrScanner.StartQrScanner", invocations);
        }

    }
}