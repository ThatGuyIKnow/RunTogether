import { SidebarCollapseFunctionality } from './SidebarHide';
import { QrScannerClass } from './qrScanner';
import "../styles/qrScanner.css";

import { mapClass } from './map';
import "../styles/map_style.css";

import { CommonJS } from "./common.js";

export const Common = new CommonJS();
export const QrScanner = new QrScannerClass();
export const Sidebar = new SidebarCollapseFunctionality();

//export async function tester(object) {
//    console.log(object);
    
//    latlngs = await JSON.parse(object).Coordinates;

//    console.log(latlngs);
//}

let Route = {
	Stages: [
		{
			StartPoint: [57.0405, 9.9101],
			/*			GuidePoint: [[57.0383, 9.9057], [57.0289, 9.9122]], */
			Throughpoint: [[57.0342, 9.9089]],
			EndPoint: [57.0257, 9.9062],
			Status: "Active",
			Runner: [{ Status: "completed", Name: "Casper", Order: 0 },
				     { Status: "active", Name: "Puma", Order: 1 }],
			Sponsor: {
				Name: "Lejenregnskabschef.dk",
				/*PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",*/
				Message: "Vi er stolte sponsorer"
			}
		}
	]
}	
/*let i, stagenumber = 0;
for (i = 0; i < Route.Stages.length; i++) {
	stagenumber++;
	console.log("Stage: " + stagenumber +
		", Runner: " + Route.Stages[i].Runner.Name +
		", Sponsor: " + Route.Stages[i].Sponsor.Name +
		", Sponsor besked: " + Route.Stages[i].Sponsor.Message); 
}
*/
export const Map = new mapClass(Route);
