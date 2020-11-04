import QrScanner from 'qr-scanner';
import qrScannerWorkerSource from '!!raw-loader!../node_modules/qr-scanner/qr-scanner-worker.min.js';
QrScanner.WORKER_PATH = URL.createObjectURL(new Blob([qrScannerWorkerSource]));

export class QrScannerClass
{
    constructor() {
        this.qrScanner = null;
        this.dotnetHelper = null;

        this.CreateQrScanner   = this.CreateQrScanner.bind(this);
        this.StartQrScanner    = this.StartQrScanner.bind(this);
        this.StopQrScanner     = this.StopQrScanner.bind(this);
        this.DestroyQrScanner  = this.DestroyQrScanner.bind(this);
        this.ChangeFlashStatus = this.ChangeFlashStatus.bind(this);
        this.HasFlash          = this.HasFlash.bind(this);
        this.HasCamera         = this.HasCamera.bind(this);
    }

    CreateQrScanner(videoElemId, objRef) {
        this.dotnetHelper = objRef;
        const videoElem = document.getElementById(videoElemId);
        this.qrScanner = new QrScanner(videoElem,
            result => this.dotnetHelper.invokeMethodAsync('ResolvePromise', result)
        );
    }

    ChangeFlashStatus(status) {
        if (status) {
            this.qrScanner.turnFlashOn();
        } else {
            this.qrScanner.turnFlashOff();
        }
    }

    StartQrScanner()    { this.qrScanner.start();           }
    StopQrScanner()     { this.qrScanner.stop();            }
    DestroyQrScanner()  { this.qrScanner.destroy();         }
    HasFlash ()         { return this.qrScanner.hasFlash(); }
    HasCamera()         { return QrScanner.hasCamera();     }
}
