
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

        console.log("from constructor!");

        this.initializeMap = this.initializeMap.bind(this);
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
        this.loadRoute = this.loadRoute.bind(this); 
    }

    /* A Method that initializes the map */
    initializeMap() {

        /*Pointing myeditmap to leaflet map and setting the viewpoint and start zoom point*/
        myeditmap = L.map('mapid', { doubleClickZoom: false}).setView([55.964, 9.992], 6.5);

        myeditmap.setMaxBounds(bounds);

        myeditmap.addEventListener('dblclick', e => this.onMapClick(e));

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
        let json = JSON.parse(serialData);
        //console.log(json);
        let stages = [];
        stages = json.Stages; 

        stages.forEach(element => {
            console.log(element);
            pointArray.push({ lat: element.StartPoint.X, lng: element.StartPoint.Y });
            pointArray.push({ lat: element.EndPoint.X, lng: element.EndPoint.Y });
        });

        this.drawRoute();
    }

    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        myeditmap.removeLayer(layerGroup);
    }


    drawRoute() {
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
            lineIds[layerGroup.getLayerId(polyline)] = i;

            this.interactableLine(polyline);
        }

        
        

        //Create markers
        for (i = 0; i < pointArray.length; i++) {
            marker = L.circleMarker(pointArray[i], { bubblingMouseEvents: false, fillOpacity: 1 });
            layerGroup.addLayer(marker);
            pointIds[layerGroup.getLayerId(marker)] = i;

            this.moveableMarker(myeditmap, marker);
        }
    }
        

    onMapClick(e) {
        pointArray.push(e.latlng);
        this.drawRoute();
        console.log(pointArray);
    }

        
    moveableMarker(map, marker) {
        function trackCursor(evt) {
            marker.setLatLng(evt.latlng);
        }

        marker.on("mousedown", () => {
            map.dragging.disable();
            map.on("mousemove", trackCursor);
        })

        marker.on("mouseup", () => {
            map.dragging.enable();
            map.off("mousemove", trackCursor);
            this.markerDragEnd(marker);
        }) 

        return marker
    }

    markerDragEnd(marker) {
        console.log("dragEnd")
        pointArray[pointIds[layerGroup.getLayerId(marker)]] = marker.getLatLng();
        this.drawRoute();
        }


    interactableLine(polyline) {
        polyline.on('mouseover', () => {
            polyline.setStyle({ color: '#ff5c26', weight: 12 });
        })

        polyline.on('mouseout', () => {
            polyline.setStyle({ color: '#db5d57', weight: 6  });
        })

        polyline.on('click', () => console.log("from line"))
    }

 }
