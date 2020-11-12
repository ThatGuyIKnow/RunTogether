
/*Global variable for the map class*/
let myeditmap, layerGroup, polyline;
let maxBounds1 = [51.649, 0.49];
let maxBounds2 = [59.799, 18.68];
let bounds = L.latLngBounds(maxBounds1, maxBounds2);
let pointArray = [];
let pointIds = {}; 
let lineIds = {};





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
        console.log("loadRoute was called");
        //clear point array. 
        pointArray = [];
        
        //Load stages into an array        
        let json = JSON.parse(serialData);
        let stages = [];
        stages = json.Stages;

        //Convert stages to point array. 
        //Only include start point in first stage, since start and endpoint overlap
        stages.forEach((element, index) => {
            if (index == 0) {
                pointArray.push({ lat: element.StartPoint.X, lng: element.StartPoint.Y });
                pointArray.push({ lat: element.EndPoint.X, lng: element.EndPoint.Y });
            }
            else {
                pointArray.push({ lat: element.EndPoint.X, lng: element.EndPoint.Y });
            }
        });

        this.drawRoute();
    }

    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        myeditmap.removeLayer(layerGroup);
    }


    drawRoute() {
        console.log("draw function was called");
        this.removeMarkersAndLines();
        pointIds = {};
        lineIds = {};

        //Creating layer group and adding to map
        layerGroup = L.layerGroup().addTo(myeditmap);

        let i;
        let marker;

        //Create polyline
        for (i = 0; i < pointArray.length - 1; i++) {
            polyline = L.polyline(pointArray.slice(i, i + 2), { color: '#db5d57', weight: 6 });
            layerGroup.addLayer(polyline);
            //Assigning markers an ID by "exploiting" layergroups. Not in use yet.
            //lineIds[layerGroup.getLayerId(polyline)] = i;

            //Add functionallity to lines
            this.interactableLine(polyline);
        }

        //Create markers
        for (i = 0; i < pointArray.length; i++) {
            marker = L.circleMarker(pointArray[i], { bubblingMouseEvents: false, fillOpacity: 1 });
            layerGroup.addLayer(marker);
            //Assigning markers an ID by "exploiting" layergroups 
            pointIds[layerGroup.getLayerId(marker)] = i;

            //Add functionallity to markers
            this.moveableMarker(myeditmap, marker);
        }
    }
        

    addNewMarker(e) {
        pointArray.push(e.latlng);
        this.drawRoute();

        //if there are more than 1 point (wich means at least one start- and endpoint), 
        //send a segment back to blazor, for saving to DB
        if (pointArray.length > 1) {
            this.dotnetHelper.invokeMethodAsync('Trigger', 'AddSegment',
                JSON.stringify(
                {
                    StartPoint: { X: pointArray[pointArray.length - 2].lat, Y: pointArray[pointArray.length - 2].lng },
                    EndPoint: { X: pointArray[pointArray.length - 1].lat, Y: pointArray[pointArray.length - 1].lng }
                }));
        }

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
        pointArray[pointIds[layerGroup.getLayerId(marker)]] = marker.getLatLng();
        this.drawRoute();
        }


    interactableLine(polyline) {
        //polyline.on("mousedown", () => {
        //    //disable create point? 
        //})

        //polyline.on("mouseup", () => {
        //    //enable create point? 
        //}) 

        polyline.on('mouseover', () => {
            polyline.setStyle({ color: '#ff5c26', weight: 12 });
        })

        polyline.on('mouseout', () => {
            polyline.setStyle({ color: '#db5d57', weight: 6  });
        })

        polyline.on('click', () => {
            console.log("from line");

            this.dotnetHelper.invokeMethodAsync('Trigger', 'SendStageId',
                JSON.stringify(
                    {
                        StageId: 4
                    }));

        })
    }

 }
