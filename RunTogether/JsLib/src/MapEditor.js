
/*Global variable for the map class*/
let myeditmap, layerGroup, polyline;
let maxBounds1 = [51.649, 0.49];
let maxBounds2 = [59.799, 18.68];
let bounds = L.latLngBounds(maxBounds1, maxBounds2);
let pointArray = [];




/*Class for the map*/
export class mapEditorClass {

    constructor() {

        console.log("from constructor!");

        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
    }

    /* A Method that initializes the map */
    initializeMap() {

        /*Pointing myeditmap to leaflet map and setting the viewpoint and start zoom point*/
        myeditmap = L.map('mapid').setView([55.964, 9.992], 6.5);

        myeditmap.setMaxBounds(bounds);

        myeditmap.on('click', this.onMapClick);

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

    /* A Method to add markers and lines*/
    addMarkersAndLines(obj) {

        this.removeMarkersAndLines();
        /*        myeditmap.removeLayer(layerGroup);
        */
        let latlngs = JSON.parse(obj).Coordinates

        console.log(latlngs)

        let i = 0;
        let segNum = 0;
        let marker;

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(myeditmap);

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
        myeditmap.fitBounds(polyline.getBounds());

    }


    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        myeditmap.removeLayer(layerGroup);
    }



        

    onMapClick(e) {
        //pointArray.push(e.latlng.toString());
        console.log("test");
    }

        


    
}
