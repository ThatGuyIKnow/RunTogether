import { QrScannerClass } from './qrScanner'
import { SidebarCollapseFunctionality } from './SidebarHide';
import "../styles/qrScanner.css";
import "../styles/map_style.css";


export const QrScanner = new QrScannerClass();
export const sidebar = new SidebarCollapseFunctionality();

/*export let a =  new leaflet_start();*/



/*This is going into map.js later*/
let mymap;

//Coordinates for the  polyline
let latlngs = [
    [57.0117789, 9.9907118],
    [57.00967, 10.00404],
    [58.0123239, 9.9940051]
];

export function leaflet_start() {

    // Start view for map (zoomlevel and viewpoint)
    mymap = L.map('mapid').setView([57.0117789, 9.9907118], 6);

    L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
        attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        /*subdomains: 'abcd',*/
        minZoom: 6,
        maxZoom: 16,
        ext: 'jpg'
    }).addTo(mymap);

    addMarkers();
    addPolyline(latlngs);

    console.log("leaflet_start() function is done ");
}

function addMarkers() {
    L.marker([57.0117789, 9.9907118]).bindPopup('Start for segment 1<br/>Dette segment er sponseret af State.</p><br/><img src="/logos/State_Logo_v1.jpg" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
    L.marker([57.00967, 10.00404]).bindPopup('Start for segment 2<br />Dette segment er sponseret af FrugtKurven.</p><br/><img src="/logos/Frugtkurven_Logo.png" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
    L.marker([58.0123239, 9.9940051]).bindPopup('Start for segment 3<br />Dette segment er sponseret af FrugtKurven.</p><br/><img src="/logos/Frugtkurven_Logo.png" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
}

function addPolyline(latlngs) {

    let polyline = L.polyline(latlngs, {
        color: 'red'
    }).addTo(mymap);
    mymap.fitBounds(polyline.getBounds());

    console.log("Add polyline");
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
