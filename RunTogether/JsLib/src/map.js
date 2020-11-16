
import L from 'leaflet';
import '@elfalem/leaflet-curve';
import { settings } from './mapSettings.json';
import {StageFactory, RunRouteFactory} from "./map/index";

/*Global variable for the map class*/
let mymap, layerGroup;
let maxBounds1 = [51.649, 0.49]; 
let maxBounds2 = [59.799, 18.68]; 
let bounds = L.latLngBounds(maxBounds1, maxBounds2);

/*Class for the map*/
export class mapClass {

    constructor() {
        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        //this.calulateControlPoints = this.calulateControlPoints.bind(this); 
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
    }

    /* A Method that initializes the map */
    initializeMap() {

        /*Pointing mymap to leaflet map and setting the viewpoint and start zoom point*/
        mymap = L.map('mapid').setView([55.964, 9.992], 6.5);

        mymap.setMaxBounds(bounds);

        /* Appling tile layer to the map*/
        L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
            maxBounds: bounds,
            maxBoundsViscosity: 1, 
            ext: 'jpg'
        }).addTo(mymap);

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

    }

    /* A Method to add markers and lines*/
    addMarkersAndLines(object) {

        this.removeMarkersAndLines();

        let obj = JSON.parse(object);

        console.log(object);
        console.log(obj);

        let i = 0, j = 0, n = 0, curvedLine, lineArray = [], runnerName = [];

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

        /*Creates curvedlies from object with stages*/
        /* for (i = 0; i < this.obj.Stages.length; i++) {

            let startpoint = this.obj.Stages[i].StartPoint;
            let endpoint = this.obj.Stages[i].EndPoint
            let throughpoint, guidepoint;

            lineArray.push('M', startpoint,);

             for (j = 0; j < this.obj.Stages[i].Throughpoint.length; j++) {

                if (j == this.obj.Stages[i].Throughpoint.length) {
                    throughpoint = this.obj.Stages[i].Throughpoint[j];
                    guidepoint = this.obj.Stages[i].GuidePoint[j];
                    lineArray.push('Q', guidepoint, throughpoint,);
                } else {
                    guidepoint = this.obj.Stages[i].GuidePoint[j];
                    lineArray.push('Q', guidepoint, endpoint);
                }

                console.log(lineArray);
             }

            for (n = 0; n > this.obj.Stages[i].GuidePoint[j]) {

            }

            L.marker(startpoint).addTo(mymap);
            L.marker(endpoint).addTo(mymap);

            curvedLine = L.curve(lineArray, { color: 'red' }).bindPopup('Dette er noget tekst').openPopup();

            layerGroup.addLayer(curvedLine).addTo(mymap); 

            mymap.fitBounds(curvedLine.getBounds());


            calulateControlPoints(latlng1, latlng2); 

        }
*/
        //let latlng1 = [57.0405, 9.9101];
        //let latlng2 = [57.0257, 9.9062];
        //let latlng3 = [57.0039, 9.9027];

        //let midtpunkt1 = this.calulateControlPoints(latlng1, latlng2, "right"); 
        //let midtpunkt2 = this.calulateControlPoints(latlng3, latlng4, "left"); 

        //let curvedPath = L.curve(
        //    [
        //        'M', latlng1,
        //        'Q', midtpunkt1, latlng2,
        //        'Q', midtpunkt2, latlng3
        //    ], { color: 'red' }).addTo(mymap);
        //// curvedPath._path = DOMElement


        //curvedPath._path.classList.add("activeStage");
        //curvedPath._renderer.on('update',
        //    (e) => {
        //        let length = curvedPath._path.getTotalLength();
        //        curvedPath._path.style.strokeDasharray = length;
        //        curvedPath._path.style.strokeDashoffset = (length) / 4;
        //        console.log(curvedPath);
        //        console.log(e);
        //        console.log(length);
        //    });
        //console.log(Point);
        //console.log(ActiveStage);
        let routeFactory = new RunRouteFactory();
        let stageTemp1 = {
            StartPoint: [57.0405, 9.9101],
            EndPoint: [57.0039, 9.9000],
            ThroughPoints: [[57.0257, 9.9062], [57.0107, 9.9027]],
            Status: 'Completed',
            Sponsor: { Name: "Hoho", Message: "Haha", PictureUrl: "eee.com" },
            Runners: [
                { Name: "Julemand", Order: 0, Status: "Completed" },
                { Name: "Hulemand", Order: 1, Status: "Completed" },
                { Name: "Kuglemand", Order: 2, Status: "Completed" }
            ]
        };
        let stageTemp2 = {
            StartPoint: [57.0039, 9.9000],
            EndPoint: [56.9639, 9.8890],
            ThroughPoints: [[56.9857, 9.8960], [56.9757, 9.8920]],
            Status: 'Active',
            Sponsor: { Name: "Hoho", Message: "Haha", PictureUrl: "eee.com" },
            Runners: [
                { Name: "Julemand", Order: 0, Status: "Completed" },
                { Name: "Hulemand", Order: 1, Status: "Active" },
                { Name: "Kuglemand", Order: 2, Status: "NotStarted" }
            ]
        };
        let stageTemp3 = {
            StartPoint: [56.9639, 9.8890],
            EndPoint: [56.9239, 9.8700],
            ThroughPoints: [[56.9507, 9.8830], [56.9307, 9.8770]],
            Status: 'NotFinished',
            Sponsor: { Name: "Hoho", Message: "Haha", PictureUrl: "eee.com" },
            Runners: [
                { Name: "Julemand", Order: 0, Status: "NotStarted" },
                { Name: "Hulemand", Order: 1, Status: "NotStarted" },
                { Name: "Kuglemand", Order: 2, Status: "NotStarted" }
            ]
        };
        let data = { Name: "RunTogetherRunTest", Stages: [stageTemp1, stageTemp2, stageTemp3] };

        let routeRun = routeFactory.CreateRunRoute(data);
        routeRun.AddToMap(mymap);
    }

    calculateControlPoints(latlng1, latlng2, direction) {
        let offsetX = latlng2[1] - latlng1[1],
            offsetY = latlng2[0] - latlng1[0];

        let r = Math.sqrt(Math.pow(offsetX, 2) + Math.pow(offsetY, 2)),
            theta = Math.atan2(offsetY, offsetX);

        let thetaOffset = (11 / 10);

        let r2 = (r / 2) / (Math.cos(thetaOffset)), theta2; 

        if (direction == "right") {
            theta2 = theta + thetaOffset;
        } else if (direction == "left") {
            theta2 = theta - thetaOffset;
        }

        let  midpointX = (r2 * Math.cos(theta2)) + latlng1[1],
             midpointY = (r2 * Math.sin(theta2)) + latlng1[0];

        let midpointLatLng = [midpointY, midpointX];

        return midpointLatLng; 

    }

    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        mymap.removeLayer(layerGroup);
    }

    onMapClick() {

        let pointArray = [];  

        var popup = L.popup();

        function onMapClick(e) {
            popup
                .setLatLng(e.latlng)
                .setContent("You clicked the map at " + e.latlng.toString())
                .openOn(mymap);

            pointArray.push(e.latlng.toString());
            console.log(pointArry);
        }

        mymap.on('click', onMapClick);
    }
}


   //If the layer group has to be spilt up
    // addPolyline(latlngs) {
    //    polyline = L.polyline(this.latlngs, { color: 'red' }).addTo(mymap);
    //    mymap.fitBounds(polyline.getBounds());
    //}


//export function onMapClick() {

//    var popup = L.popup();

//    function onMapClick(e) {
//        popup
//            .setLatLng(e.latlng)
//            .setContent("You clicked the map at " + e.latlng.toString())
//            .openOn(mymap);
//    }

//    mymap.on('click', onMapClick);
//}
