import { SidebarCollapseFunctionality } from './SidebarHide';

import { QrScannerClass } from './qrScanner'
import "../styles/qrScanner.css";

import { mapClass } from './map'
import "../styles/map_style.css";

import { CommonJS } from "./common.js"

export const Common = new CommonJS();
export const QrScanner = new QrScannerClass();
export const sidebar = new SidebarCollapseFunctionality();

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
			EndPoint: [56.1413, 9.3942],
			Throughpoint: [[56.8062, 9.3465], [56.7239, 9.9995], [56.3757, 9.2638]],
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
			StartPoint: [56.1413, 9.3942],
			EndPoint: [54.8856, 9.1176],
			Throughpoint: [[55.815, 9.593], [55.744, 8.5993], [55.6216, 9.3512]],
			StageNotStarted: true,
			Stageactive: false,
			Stagecompleted: false,
			Runner: { Name: "Samsung", RunnerID: 15 },
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
