
/*Global variable for the map class*/
let mymap, layerGroup, polyline;
let maxBounds1 = [51.649, 0.49]; 
let maxBounds2 = [59.799, 18.68];  
let bounds = L.latLngBounds(maxBounds1, maxBounds2);




/*Class for the map*/
export class mapClass {

    constructor() {

        console.log("from constructor!");

        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
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
    addMarkersAndLines(obj) {

        this.removeMarkersAndLines(); 
/*        mymap.removeLayer(layerGroup); 
*/
        let latlngs = JSON.parse(obj).Coordinates

        console.log(latlngs)

        let i = 0;
        let segNum = 0;
        let marker;

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

        /*Creating markers*/
        for (i = 0; i < latlngs.length; i++) {
            segNum = i + 1;
            marker = L.marker(latlngs[i]).bindPopup('Start for segment ' + segNum +
                '<br />Dette segment er sponseret af [SPONSOR].</p>').openPopup();
            layerGroup.addLayer(marker);
        }

        /*Creating polyline and fiting the polyline and markers to the map view*/
        polyline = L.polyline(latlngs, { color: '#db5d57' });
        layerGroup.addLayer(polyline);
        mymap.fitBounds(polyline.getBounds());

    }   


    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        mymap.removeLayer(layerGroup);
    }
}

/*
 *   //If the layer group has to be spilt up
     addPolyline(latlngs) {
        polyline = L.polyline(this.latlngs, { color: 'red' }).addTo(mymap);
        mymap.fitBounds(polyline.getBounds());
    }


export function onMapClick() {

    var popup = L.popup();

    function onMapClick(e) {
        popup
            .setLatLng(e.latlng)
            .setContent("You clicked the map at " + e.latlng.toString())
            .openOn(mymap);
    }

    mymap.on('click', onMapClick);
}


*/
