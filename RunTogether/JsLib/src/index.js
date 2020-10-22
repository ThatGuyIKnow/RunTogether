
export function test() {
    console.log("Det virker måske?");
}

let mymap;


export function leaflet_start() {

    mymap = L.map('mapid').setView([51.505, -0.09], 13);

    var OpenStreetMap_Mapnik = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(mymap);

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

    L.marker([51.5, -0.09]).addTo(mymap);

    console.log("EN marker? tak");
}




