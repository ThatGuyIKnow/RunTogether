
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
                    StartPoint: [57.0257, 9.9062],
                    ThroughPoints: [[56.9658, 9.7532]],
                    EndPoint: [56.8659, 10.0805],
                    Status: "Completed",
                    Runners: [
                        { Status: "Completed", Name: "Lasper", Order: 0 },
                        { Status: "Completed", Name: "Kuma", Order: 1 }
                    ],
                    Sponsor: {
                        Name: "State Energy Gunk",
                        PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
                        Message: "Eat our Gunkz"
                    }
                }, {
                    StartPoint: [56.8659, 10.0805],
                    ThroughPoints: [[56.7015, 9.511]],
                    EndPoint: [55.9738, 9.6262],
                    Status: "Active",
                    Runners: [{ Status: "Completed", Name: "Casper", Order: 0 },
                        { Status: "Active", Name: "Puma", Order: 1 }],
                    Sponsor: {
                        Name: "Lejenregnskabschef.dk",
                        PictureURL: "https://www.runtogether.dk/wp-content/uploads/2020/03/jhuih.png",
                        Message: "Vi er stolte sponsorer"
                    }
                }
            ]
        }	

        let routeFactory = new RunRouteFactory();   
/*      let obj = JSON.parse(object);*/
        let routeRun = routeFactory.CreateRunRoute(testRoute);
        routeRun.AddToLayer(layerGroup);

        this.AddFilter();
    }

    AddFilter() {
        var svg = mymap.getPanes().overlayPane.firstChild,
            svgFilter = document.createElementNS('http://www.w3.org/2000/svg', 'filter');

        svgFilter.setAttribute('id', 'pathFilter');
        svgFilter.setAttribute('x', '-100%');
        svgFilter.setAttribute('y', '-100%');
        svgFilter.setAttribute('width', '500%');
        svgFilter.setAttribute('height', '500%');


        let s1 = '<feGaussianBlur in="SourceAlpha" stdDeviation = "2" result = "BLUR" ></feGaussianBlur >' +
            '<feSpecularLighting in="BLUR" surfaceScale="6" specularExponent="30" result="SPECULAR" lighting-color="#white">' +
            '<fePointLight x="40" y="-30" z="200" result="b61119e7-b6a7-4e48-a1e1-1e36ab0c51be"></fePointLight>' +
            '</feSpecularLighting>' +
            '<feComposite in="SPECULAR" in2="SourceAlpha" operator="in" result="COMPOSITE"></feComposite>' +
            '<feMerge result="9ab789a6-a71b-42be-b5de-48a608b4a107">' +
            '<feMergeNode in="SourceGraphic"></feMergeNode><feMergeNode in="COMPOSITE"></feMergeNode>' +
            '</feMerge>';

        svgFilter.innerHTML = s1;
        svg.appendChild(svgFilter);
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