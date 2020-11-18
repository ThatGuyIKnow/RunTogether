
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


        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

/*      mymap.fitBounds(curvedLine.getBounds());
*/      let testRoute = {
            Name: "RunTogetherTest",
            Stages: [
                {
                    StartPoint: [57.0405, 9.9101],
                    ThroughPoints: [],
                    EndPoint: [57.0257, 9.9062],
                    Status: "Completed",
                    Runners: [{ Status: "Completed", Name: "Casper", Order: 0 },
                        { Status: "Completed", Name: "Puma", Order: 1 }],
                    Sponsor: {
                        Name: "Lejenregnskabschef.dk",
                        PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
                        Message: "Vi er stolte sponsorer"
                    }
                },
                {
                    StartPoint: [57.0257, 9.9062],
                    ThroughPoints: [],
                    EndPoint: [57.0107, 9.9020],
                    Status: "Active",
                    Runners: [
                        { Status: "Completed", Name: "Lasper", Order: 0 },
                        { Status: "Active", Name: "Kuma", Order: 1 }
                    ],
                    Sponsor: {
                        Name: "State Energy Gunk",
                        PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
                        Message: "Eat our Gunkz"
                    }
                }
            ]
        }	

        let routeFactory = new RunRouteFactory();   
/*      let obj = JSON.parse(object);*/
        let routeRun = routeFactory.CreateRunRoute(testRoute);
        routeRun.AddToLayer(layerGroup);
        console.log(layerGroup);
    }


    //onMapClick() {
    //    let pointArray = [];  

    //    var popup = L.popup();

    //    function onMapClick(e) {
    //        popup
    //            .setLatLng(e.latlng)
    //            .setContent("You clicked the map at " + e.latlng.toString())
    //            .openOn(mymap);

    //        pointArray.push(e.latlng.toString());
    //        console.log(pointArray);
    //    }

    //    mymap.on('click', onMapClick);
    //}
}