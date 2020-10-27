
export function test() {
    console.log("Funktion som tester om der er forbindelse");
}

let mymap;

 
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


        /*var OpenStreetMap_Mapnik = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(mymap);
       */
    /*    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox/satellite-v9',
            tileSize: 512,
            zoomOffset: -1,
            accessToken: 'pk.eyJ1IjoiZWF3b29wIiwiYSI6ImNrZ2piaXQ0ODB3b2YyenRldDh6dXV1YXAifQ.RkCts-bAJZYnuCAL_fBf0w'
        }).addTo(mymap);*/

    //Test for sponser images and test to markers 

    addMarkers(); 
    addPolyline(latlngs);   

    // Text to check that the function is done
    console.log("leaflet_start() function is done ");

}

function addMarkers() {
    L.marker([57.0117789, 9.9907118]).bindPopup('Start for segment 1<br/>Dette segment er sponseret af State.</p><br/><img src="/logos/State_Logo_v1.jpg" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
    L.marker([57.00967, 10.00404]).bindPopup('Start for segment 2<br />Dette segment er sponseret af FrugtKurven.</p><br/><img src="/logos/Frugtkurven_Logo.png" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
    L.marker([58.0123239, 10.9940051]).bindPopup('Start for segment 3<br />Dette segment er sponseret af FrugtKurven.</p><br/><img src="/logos/Frugtkurven_Logo.png" asp-append-version="true" width="300px" />').openPopup().addTo(mymap);
}

let latlngs = [
    [57.0117789, 9.9907118],
    [57.00967, 10.00404],
    [58.0123239, 10.9940051]
];


function addPolyline(latlngs) {      
        
    let polyline = L.polyline(latlngs, { color: 'red' }).addTo(mymap);
    mymap.fitBounds(polyline.getBounds());

    let latlngs = [
        [57.0117789, 9.9907118],
        [57.0123239, 9.9939051],
        [57.0123239, 9.9939051]
    ];

    let polyline = L.polyline(latlngs, { color: 'red' }).addTo(mymap);

    console.log("To marker? tak");
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
