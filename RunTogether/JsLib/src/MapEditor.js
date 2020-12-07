
import Leaflet from 'leaflet';
import '@elfalem/leaflet-curve';
import { StageFactory, RunRouteFactory } from "./map/index";

/*Class for the map*/
export class mapEditorClass {
    
    editMap = null;
    layerGroup = null;
    maxBounds1 = [51.649, 0.49];
    maxBounds2 = [59.799, 18.68];
    bounds;
    run;


    constructor() {
        this.bounds = Leaflet.latLngBounds(this.maxBounds1, this.maxBounds2);

        this.initializeMap = this.initializeMap.bind(this);
        this.loadRoute = this.loadRoute.bind(this);


        this.dotnetHelper = null;
    }

    /* A Method that initializes the map */
    initializeMap(objRef) {

        //Bind to event trigger from C#
        this.dotnetHelper = objRef;

        /*Pointing myeditmap to leaflet map and setting the viewpoint and start zoom point*/
        this.editMap = Leaflet.map('mapid', { doubleClickZoom: false}).setView([55.964, 9.992], 6.5);
        this.editMap.setMaxBounds(this.bounds);

        this.editMap.on('dblclick', e => this.addNewStage(e));

        /* Appling tile layer to the map*/
        Leaflet.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
            maxBounds: this.bounds,
            maxBoundsViscosity: 1,
            ext: 'jpg'
        }).addTo(this.editMap);

        /*Creating layer group and adding to map*/
        this.layerGroup = Leaflet.layerGroup().addTo(this.editMap);

    }

    loadRoute(serialData) {

        this.layerGroup.clearLayers();

        let routeFactory = new RunRouteFactory();

        this.run = routeFactory.CreateRunRoute(serialData, true, this.editMap, this.dotnetHelper);
        this.run.AddToLayer(this.layerGroup);
    }

    addNewStage(e) {
        if (this.run.stages.length < 1) {
            let stageFactory = new StageFactory();
            let currStage = stageFactory.CreateStage({ StartPoint: [e.latlng.lat, e.latlng.lng], ThroughPoints: [], EndPoint: [e.latlng.lat, e.latlng.lng] }, true, true);
            this.run.stages.push(currStage);
            let tempMarker = Leaflet.circleMarker(e.latlng, { fillOpacity: 1 }).addTo(this.layerGroup);
            }
        else {
            let lastX = this.run.stages[this.run.stages.length - 1].endPoint.x; 
            let lastY = this.run.stages[this.run.stages.length - 1].endPoint.y;

            //send a segment back to blazor, for saving to DB
            this.dotnetHelper.invokeMethodAsync('Trigger', 'AddStage',
                JSON.stringify(
                    {
                        StartPoint: { X: lastX, Y: lastY },
                        EndPoint: { X: e.latlng.lat, Y: e.latlng.lng }
                    }));
        }
    }

 }
