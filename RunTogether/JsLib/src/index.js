import { SidebarCollapseFunctionality } from './SidebarHide';
import { QrScannerClass } from './qrScanner';
import "../styles/qrScanner.css";

import { mapClass } from './map';
import "../styles/map_style.css";

import { CommonJS } from "./common.js";

export const Common = new CommonJS();
export const QrScanner = new QrScannerClass();
export const sidebar = new SidebarCollapseFunctionality();

/*Test array with startpoint and endpoint coordinates*/
let latlngs = [
    [57.0117789, 9.9907118],
    [56.7499, 9.9921],
    [56.467, 9.2708],
    [56.0221, 9.2288],
    [55.6123, 9.1428]
];

export const Map = new mapClass(latlngs);
