
let mymap, layerGroup, polyline;


export class mapClass {

    constructor(latlngs) {
        this.latlngs = latlngs;

        this.createMap = this.createMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
    }

    createMap() {
        mymap = L.map('mapid').setView([57.0117789, 9.9907118], 6);

        L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
            ext: 'jpg'
        }).addTo(mymap);
    }

    addMarkersAndLines(latlngs) {
        let i = 0;
        let segNum = 0;
        let marker;

        layerGroup = L.layerGroup().addTo(mymap);

        for (i = 0; i < this.latlngs.length; i++) {
            segNum = i + 1;
            marker = L.marker(this.latlngs[i]).bindPopup('Start for segment ' + segNum +
                '<br />Dette segment er sponseret af [SPONSOR].</p>').openPopup();
            layerGroup.addLayer(marker);
        }

        polyline = L.polyline(this.latlngs, { color: 'red' });
        layerGroup.addLayer(polyline).addTo(mymap);
        mymap.fitBounds(polyline.getBounds());
    }

    removeMarkersAndLines() {
        mymap.removeLayer(layerGroup);
    }
}

/*
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





