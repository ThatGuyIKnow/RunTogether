using Microsoft.AspNetCore.Components;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.QR
{
    public partial class QRGenerator
    {
        private string qrCodeImageAsBase64;

        [Parameter] public string code { get; set; } = null;
        [Parameter] public int size { get; set; } = 30;
        [Parameter] public string color { get; set; } = "#000000";

        protected override void OnParametersSet()
        {
            if (code != default && size != default && color != default)
            {
                GenQR();
            }
        }

        private void GenQR()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            qrCodeImageAsBase64 = qrCode.GetGraphic(size, color, "#ffffff", true);
        }
    }
}
