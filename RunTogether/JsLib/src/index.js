import { SidebarCollapseFunctionality } from './SidebarHide';
import { QrScannerClass } from './qrScanner';
import "../styles/qrScanner.css";

import { mapEditorClass } from './MapEditor';
import { mapClass } from './map';
import "../styles/map_style.css";

import { CommonJS } from "./common.js";

export const Common = new CommonJS();
export const QrScanner = new QrScannerClass();
export const Sidebar = new SidebarCollapseFunctionality();

export const Map = new mapClass();
export const MapEditor = new mapEditorClass();



