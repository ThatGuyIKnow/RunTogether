
/*Global variable for the map class*/
let mymap, layerGroup, polyline;
let maxBounds1 = [51.649, 0.49]; 
let maxBounds2 = [59.799, 18.68];  
let bounds = L.latLngBounds(maxBounds1, maxBounds2);

/*Class for the map*/
export class mapClass {

    constructor(obj) {

        /*console.log("from constructor!");*/

        this.obj = obj; 
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

        /*      let latlngs = JSON.parse(obj).Coordinates; */
        /*      console.log(latlngs);                       */

        let i = 0, marker;

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

        /*Creating startpoint and endpoint*//*
        for (i = 0; i < latlngs.length; i++) {
            segNum = i + 1;
            marker = L.marker(latlngs[i]).bindPopup('Start for segment ' + segNum +
                '<br />Dette segment er sponseret af [SPONSOR].</p>').openPopup();
            layerGroup.addLayer(marker);
        }
*/

        let stagenumber = 0, linjearray =[], j;

        for (i = 0; i < this.obj.Stages.length; i++) {
            let startpoint = this.obj.Stages[i].StartPoint;
            let endpoint = this.obj.Stages[i].EndPoint
            let SponsorName = this.obj.Stages[i].Sponsor.Name;
            let SponserMessage = this.obj.Stages[i].Sponsor.Message;
            let RunnerName = this.obj.Stages[i].Runner.Name;
            let SponsorPictureURL = this.obj.Stages[i].Sponsor.PictureURL; 

            stagenumber++;
/*            console.log("Stage: " + stagenumber +
                ", Runner: " + this.obj.Stages[i].Runner.Name +
                ", Sponsor: " + this.obj.Stages[i].Sponsor.Name);
*/


            marker = L.marker(startpoint).bindPopup('Start på segment: ' + stagenumber + '<br\>Bliver løbet af: ' + RunnerName).openPopup();
            layerGroup.addLayer(marker);

            linjearray.push(startpoint); 

            for (j = 0; j < this.obj.Stages[i].Throughpoint.length; j++) {
                linjearray.push(this.obj.Stages[i].Throughpoint[j]); 
            }

            linjearray.push(endpoint); 

            if (i == (this.obj.Stages.length-1)) {
                marker = L.marker(endpoint).bindPopup('Dette er endpoint for hele løbet').openPopup();
                layerGroup.addLayer(marker);
            }

            polyline = L.polyline(linjearray).bindPopup('Dette er segment: ' + stagenumber +
                '<br\>Sponsernavn: ' + SponsorName +
                '<br\><p>Deres sponser besked: <p\>' + SponserMessage +
                '<br\><img src=' + SponsorPictureURL + ' asp-append-version="true" width="300px" />')
                .openPopup().addTo(mymap);

                if (this.obj.Stages[i].StageNotStarted == true) {
                    polyline.setStyle({ color: '#db5d57' });
                } else if (this.obj.Stages[i].Stageactive == true) {
                    polyline.setStyle({ color: 'yellow' });
                } else if (this.obj.Stages[i].Stagecompleted == true) {
                    polyline.setStyle({ color: 'green' });
                }

            linjearray = []; 
        }

/*         mymap.fitBounds(polyline.getBounds());*/
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
