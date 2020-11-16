
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
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
        this.loadRoute = this.loadRoute.bind(this);
        this.drawRoute = this.drawRoute.bind(this);

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

    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        myeditmap.removeLayer(layerGroup);
    }


    drawRoute() {
        console.log("draw function was called");
        console.log(lineArray);
        this.removeMarkersAndLines();
        pointIds = {};
        lineIds = {};

        //Creating layer group and adding to map
        layerGroup = L.layerGroup().addTo(myeditmap);

        let i;
        let marker;

        //Create polyline
        for (i = 0; i < lineArray.length; i++) {

            polyline = L.curve(lineArray[i], { color: '#db5d57', weight: 6 });
            layerGroup.addLayer(polyline);

            //Assigning lines an ID by "exploiting" layergroups 
            lineIds[layerGroup.getLayerId(polyline)] = i;

        }

        ////Create markers
        for (i = 0; i < lineArray.length; i++) {
            //console.log(i);  
            //console.log(lineArray[i]);
            if (i == 0) {
                marker = L.circleMarker(lineArray[i][1], { bubblingMouseEvents: false, fillOpacity: 1 });
                layerGroup.addLayer(marker);
                //Assigning markers an ID by "exploiting" layergroups 
                pointIds[layerGroup.getLayerId(marker)] = i;

                //Add functionallity to markers
                this.moveableMarker(myeditmap, marker);
            }
            marker = L.circleMarker(lineArray[i][3], { bubblingMouseEvents: false, fillOpacity: 1 });
            layerGroup.addLayer(marker);
            //Assigning markers an ID by "exploiting" layergroups 
            pointIds[layerGroup.getLayerId(marker)] = i;

            //Add functionallity to markers
            this.moveableMarker(myeditmap, marker);
        }
    }
        

    addNewMarker(e) {

        let lastLine = lineArray.length - 1;

        if (lineArray.length < 1) {
            //nasty hack for making first point work.... 
            let stageFactory = new StageFactory();
            let currStage = stageFactory.CreateStage({ StartPoint: [e.latlng.lat, e.latlng.lng], ThroughPoints: [], EndPoint: [e.latlng.lat, e.latlng.lng] }, true, true);
            run.stages.push(currStage);

            //run.stages.push(['M', [e.latlng.lat, e.latlng.lng], 'L', [e.latlng.lat, e.latlng.lng]]);
        }
        else {
            lineArray.push(['M', [lineArray[lastLine][3][0], lineArray[lastLine][3][1]], 'L', [e.latlng.lat, e.latlng.lng]]);

            console.log(run.stages[run.stages.length-1].endPoint.x)

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

        run.AddToMap(myeditmap); 
    }

        
    moveableMarker(map, marker) {
        //constantly sets the markers pos to the curser pos
        function trackCursor(evt) {
            marker.setLatLng(evt.latlng);
        }

        //Drag marker 
        marker.on("mousedown", () => {
            map.dragging.disable();
            map.on("mousemove", trackCursor);
        })

        //Stop dragging marker 
        marker.on("mouseup", () => {
            map.dragging.enable();
            map.off("mousemove", trackCursor);
            this.markerDragEnd(marker);
        }) 

        return marker
    }

    markerDragEnd(marker) {
        //push new coordinates to array and redraw route. 
        lineArray[pointIds[layerGroup.getLayerId(marker)]] = marker.getLatLng();
        this.drawRoute();
        }
 }
