
export function test() {
    console.log("Det virker måske?");
}

let mymap;


export function leaflet_start() {

    // Start view for map (zoomlevel and viewpoint)
    mymap = L.map('mapid').setView([57.0117789, 9.9907118], 6);

    var Stamen_Watercolor = L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
        attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        /*subdomains: 'abcd',*/
        minZoom: 6,
        maxZoom: 18,
        ext: 'jpg'
    }).addTo(mymap);

    //var OpenStreetMap_Mapnik = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    //    maxZoom: 19,
    //    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    //}).addTo(mymap);

    /*    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox/satellite-v9',
            tileSize: 512,
            zoomOffset: -1,
            accessToken: 'pk.eyJ1IjoiZWF3b29wIiwiYSI6ImNrZ2piaXQ0ODB3b2YyenRldDh6dXV1YXAifQ.RkCts-bAJZYnuCAL_fBf0w'
        }).addTo(mymap);*/



    console.log("Virker leaflet?");

}

export function addMarker() {

    L.marker([57.0117789, 9.9907118]).addTo(mymap);
    L.marker([57.0123239, 9.9939051]).addTo(mymap);

    var latlngs = [
        [57.0117789, 9.9907118],
        [57.0123239, 9.9939051]
    ];

    var polyline = L.polyline(latlngs, { color: 'red' }).addTo(mymap);


    console.log("EN marker? tak");
}




