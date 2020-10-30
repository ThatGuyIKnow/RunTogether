﻿
﻿import { QrScannerClass } from './qrScanner'
import "../styles/qrScanner.css";

import { mapClass } from './map'
import "../styles/map_style.css";


export const QrScanner = new QrScannerClass();


/*Test array for the map*/
let latlngs = [
    [57.0117789, 9.9907118],
    [56.7499, 9.9921],
    [56.467, 9.2708],
    [56.0221, 9.2288],
    [55.6123, 9.1428]
];

export const Map = new mapClass(latlngs);

