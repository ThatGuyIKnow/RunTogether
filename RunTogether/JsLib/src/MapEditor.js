
import L from 'leaflet';
import '@elfalem/leaflet-curve';
import { StageFactory, RunRouteFactory } from "./map/index";

/*Global variable for the map class*/
let myeditmap, layerGroup, polyline;
let maxBounds1 = [51.649, 0.49];
let maxBounds2 = [59.799, 18.68];
let bounds = L.latLngBounds(maxBounds1, maxBounds2);
let lineArray = [];
let stages = [];
let pointIds = {};
let lineIds = {};
let run




/*Class for the map*/
export class mapEditorClass {

    constructor() {
        this.initializeMap = this.initializeMap.bind(this);
        this.loadRoute = this.loadRoute.bind(this);


        this.dotnetHelper = null;
    }

    /* A Method that initializes the map */
    initializeMap(objRef) {

        //Bind to event trigger from C#
        this.dotnetHelper = objRef;

        /*Pointing myeditmap to leaflet map and setting the viewpoint and start zoom point*/
        myeditmap = L.map('mapid', { doubleClickZoom: false}).setView([55.964, 9.992], 6.5);
        myeditmap.setMaxBounds(bounds);

        myeditmap.on('dblclick', e => this.addNewMarker(e));

        /* Appling tile layer to the map*/
        L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
            maxBounds: bounds,
            maxBoundsViscosity: 1,
            ext: 'jpg'
        }).addTo(myeditmap);

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(myeditmap);

    }

    loadRoute(serialData) {

        let routeFactory = new RunRouteFactory();

        run = routeFactory.CreateRunRoute(serialData, true);

        run.AddToMap(myeditmap);
    }

    addNewMarker(e) {
        if (run.stages.length < 1) {

            let stageFactory = new StageFactory();
            let currStage = stageFactory.CreateStage({ StartPoint: [e.latlng.lat, e.latlng.lng], ThroughPoints: [], EndPoint: [e.latlng.lat, e.latlng.lng] }, true, true);
            run.stages.push(currStage);
        }
        else {
            let lastX = run.stages[run.stages.length - 1].endPoint.x; 
            let lastY = run.stages[run.stages.length - 1].endPoint.y;

            //send a segment back to blazor, for saving to DB
            this.dotnetHelper.invokeMethodAsync('Trigger', 'AddSegment',
                JSON.stringify(
                    {
                        StartPoint: { X: lastX, Y: lastY },
                        EndPoint: { X: e.latlng.lat, Y: e.latlng.lng }
                    }));
        }
    }

 }
