import { SidebarCollapseFunctionality } from './SidebarHide';
import { QrScannerClass } from './qrScanner';
import "../styles/qrScanner.css";

import { mapClass } from './map';
import "../styles/map_style.css";

import { CommonJS } from "./common.js";

export const Common = new CommonJS();
export const QrScanner = new QrScannerClass();
export const Sidebar = new SidebarCollapseFunctionality();

let Route = {
	Name: "RunTogetherTest",
	Stages: [
		{
			StartPoint: [57.0405, 9.9101],
			Throughpoint: [[57.0342, 9.9089]],
			EndPoint: [57.0257, 9.9062],
			Status: "Active",
			Runner: [{ Status: "completed", Name: "Casper", Order: 0 },
				     { Status: "active", Name: "Puma", Order: 1 }],
			Sponsor: {
				Name: "Lejenregnskabschef.dk",
				PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
				Message: "Vi er stolte sponsorer"
			}
		}
	]
}	

export const Map = new mapClass(Route);
