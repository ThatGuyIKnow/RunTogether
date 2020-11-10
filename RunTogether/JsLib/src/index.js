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
	Stages:[
		{
			StartPoint: [57.6802, 10.4877],
			EndPoint: [57.0838, 9.9449],
			Throughpoint: [[57.5953, 10.2688], [57.3248, 10.2641]],
			KeyStartpoint: true, 
			KeyEndpoint: false, 
			StageNotStarted: false,	
			Stageactive: false,
			Stagecompleted: true,
			Runner: { Name: "Casper", RunnerID: 13 },
			Sponsor: {
				Name: "State",
				PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/icon5.png",
				Message: "Dette er vores bedste sponsoret"
			}
		},	
		{
			StartPoint: [57.0838, 9.9449],
			EndPoint: [56.9750, 9.7498],
			Throughpoint: [[57.0353, 9.9074], [57.0058, 9.9127], [56.9751, 9.8571]],
			KeyStartpoint: true,
			KeyEndpoint: false, 
			StageNotStarted: false,
			Stageactive: true,
			Stagecompleted: false,
			Runner: { Name: "Puma", RunnerID: 14 },
			Sponsor: {
				Name: "Lejenregnskabschef.dk",
				PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
				Message: "Dette er vores bedste sponsoret"
			}
		}, 
		{
			StartPoint: [56.9750, 9.7498],
			EndPoint: [56.8027, 9.515],
			Throughpoint: [[56.9821, 9.6421], [56.9443, 9.6106], [56.9147, 9.6132]],
			KeyStartpoint: false,
			KeyEndpoint: true, 
			StageNotStarted: true,
			Stageactive: false,
			Stagecompleted: false,
			Runner: { Name: "Samsung", RunnerID: 15 },
			Sponsor: {
				Name: "Frugtkurven",
				PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/icon1.png",
				Message: "Dette er vores bedste sponsoret"
			}
		},
		{
			StartPoint: [56.8027, 9.515],
			EndPoint: [56.6358, 9.7982],
			Throughpoint: [[56.7681, 9.5772], [56.6811, 9.6597], [56.6732, 9.6788]],
			KeyStartpoint: false,
			KeyEndpoint: true,
			StageNotStarted: true,
			Stageactive: false,
			Stagecompleted: false,
			Runner: { Name: "Denver", RunnerID: 16 },
			Sponsor: {
				Name: "Frugtkurven",
				PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/icon1.png",
				Message: "Dette er vores bedste sponsoret"
			}
		}


	]
}
let i, stagenumber = 0;
for (i = 0; i < Route.Stages.length; i++) {
	stagenumber++;
	console.log("Stage: " + stagenumber +
		", Runner: " + Route.Stages[i].Runner.Name +
		", Sponsor: " + Route.Stages[i].Sponsor.Name +
		", Sponsor besked: " + Route.Stages[i].Sponsor.Message); 
}

export const Map = new mapClass(Route);
