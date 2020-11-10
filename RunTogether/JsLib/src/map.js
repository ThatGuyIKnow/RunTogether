
/*Global variable for the map class*/
let mymap, layerGroup, polyline;
/*let maxBounds1 = [51.649, 0.49]; 
let maxBounds2 = [59.799, 18.68];  
let bounds = L.latLngBounds(maxBounds1, maxBounds2);
*/
/*Class for the map*/
export class mapClass {

    constructor(obj) {

        /*console.log("from constructor!");*/

        this.obj = obj; 
        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        this.createCurvedLine = this.createCurvedLine.bind(this); 
        this.removeMarkersAndLines = this.removeMarkersAndLines.bind(this);
    }

    /* A Method that initializes the map */
    initializeMap() {

        /*Pointing mymap to leaflet map and setting the viewpoint and start zoom point*/
        mymap = L.map('mapid').setView([55.964, 9.992], 6.5);

/*        mymap.setMaxBounds(bounds);
*/
        /* Appling tile layer to the map*/
        L.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
/*            maxBounds: bounds,
            maxBoundsViscosity: 1, */
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

        let stagenumber = 0, linjearray = [], j, activeRoute = [];

        for (i = 0; i < this.obj.Stages.length; i++) {
            let startpoint = this.obj.Stages[i].StartPoint;
            let endpoint = this.obj.Stages[i].EndPoint
            let SponsorName = this.obj.Stages[i].Sponsor.Name;
            let SponserMessage = this.obj.Stages[i].Sponsor.Message;
            let RunnerName = this.obj.Stages[i].Runner.Name;
            let SponsorPictureURL = this.obj.Stages[i].Sponsor.PictureURL;

            stagenumber++;

            marker = L.marker(startpoint).bindPopup('Start på segment: ' + stagenumber + '<br\>Bliver løbet af: ' + RunnerName).openPopup();
            layerGroup.addLayer(marker);

            linjearray.push(this.obj.Stages[i].StartPoint);

            for (j = 0; j < this.obj.Stages[i].Throughpoint.length; j++) {
                linjearray.push(this.obj.Stages[i].Throughpoint[j]);
            }

            linjearray.push(this.obj.Stages[i].EndPoint);

            polyline = L.polyline(linjearray).bindPopup('Dette er segment: ' + stagenumber +
                '<br\>Sponsernavn: ' + SponsorName +
                '<br\><p>Deres sponser besked: ' + SponserMessage +
                '<br\><img src=' + SponsorPictureURL + ' asp-append-version="true" width="300px" />')
                .openPopup();

            /*polyline.addTo(mymap);*/

            polyline.setStyle({ color: '#db5d57' });

            linjearray = [];

            if (i == (this.obj.Stages.length - 1)) {
                marker = L.marker(endpoint).bindPopup('Dette er slutningen på løbet').openPopup();
                layerGroup.addLayer(marker);
            }


            if (this.obj.Stages[i].StageNotStarted == true) {
                console.log("Denne rute er ikke begyndt");
            } else if (this.obj.Stages[i].Stageactive == true && this.obj.Stages[i].StageNotStarted == false || this.obj.Stages[i].Stagecompleted == true && this.obj.Stages[i].StageNotStarted == false) {
                activeRoute.push(startpoint);
                for (j = 0; j < this.obj.Stages[i].Throughpoint.length; j++) {
                    activeRoute.push(this.obj.Stages[i].Throughpoint[j]);
                }
                activeRoute.push(endpoint);
            }

            const options = {
                use: L.polyline, delay: 800, dashArray: [49, 44], weight: 5, color: "#FF0023", pulseColor: "#5C0202", "paused": false, reverse: false, hardwareAccelerated: false
            };

            const path = new L.Polyline.AntPath(activeRoute, options);
            /*            path.addTo(mymap)           */
            activeRoute = [];
        }

        /*        this.createCurvedLine([57.6802, 10.4877], [57.5953, 10.2688]);
                this.createCurvedLine([57.5953, 10.2688], [57.3584, 10.3539]); 
                this.createCurvedLine([57.3584, 10.3539], [57.311, 9.8076]); 
                
                StartPoint: [57.6802, 10.4877],
			EndPoint: [57.0838, 9.9449],
			Throughpoint: [[57.5953, 10.2688], [57.3248, 10.2641]],

        */

        let pathtwo = L.curve(['M', [57.6802, 10.4877],
            'C', [57.5953, 10.2688], [57.334, 10.1976], [57.3057, 9.9371], 
            'T', [57.0838, 9.9449]]).addTo(mymap);

        L.marker([57.6802, 10.4877]).bindPopup('M er her').openPopup().addTo(mymap);
        L.marker([57.5953, 10.2688]).bindPopup('Q1 er her').openPopup().addTo(mymap);
        L.marker([57.3248, 10.2641]).bindPopup('Q2 er her').openPopup().addTo(mymap);
        L.marker([57.3248, 10.2641]).bindPopup('T er her').openPopup().addTo(mymap);

        let paththree = L.curve(['M', [50.54136296522163, 28.520507812500004],
            'C', [52.214338608258224, 28.564453125000004],
            [48.45835188280866, 33.57421875000001],
            [50.680797145321655, 33.83789062500001],
            'V', [48.40003249610685],
            'L', [47.45839225859763, 31.201171875],
            [48.40003249610685, 28.564453125000004], 'Z'],
            { color: 'red', fill: true }).addTo(mymap);

        L.marker([50.54136296522163, 28.520507812500004]).bindPopup('M er her').openPopup().addTo(mymap);
        L.marker([52.214338608258224, 28.564453125000004]).bindPopup('C1 er her').openPopup().addTo(mymap);
        L.marker([48.45835188280866, 33.57421875000001]).bindPopup('C2 er her').openPopup().addTo(mymap);
        L.marker([50.680797145321655, 33.83789062500001]).bindPopup('C3 er her').openPopup().addTo(mymap);


        L.marker([47.45839225859763, 31.201171875]).bindPopup('L1 er her').openPopup().addTo(mymap);
        L.marker([48.40003249610685, 28.564453125000004]).bindPopup('L2 er her').openPopup().addTo(mymap);
        



        let pathOne = L.curve(['M', [50.14874640066278, 14.106445312500002],
            'Q', [51.67255514839676, 16.303710937500004],[50.14874640066278, 18.676757812500004],
            'T', [49.866316729538674, 25.0927734375]]);


        L.marker([50.14874640066278, 14.106445312500002]).bindPopup('M er her').openPopup().addTo(mymap); 

        L.marker([51.67255514839676, 16.303710937500004]).bindPopup('Q1 er her').openPopup().addTo(mymap); 

        L.marker([50.14874640066278, 18.676757812500004]).bindPopup('Q2 er her').openPopup().addTo(mymap); 

        L.marker([49.866316729538674, 25.0927734375]).bindPopup('T er her').openPopup().addTo(mymap); 

        pathOne.addTo(mymap); 

        mymap.fitBounds(polyline.getBounds());
        
        /*
         * 			StartPoint: [57.6802, 10.4877],
			EndPoint: [57.0838, 9.9449],
			Throughpoint: [[57.5953, 10.2688], [57.3248, 10.2641]],
        

        const antCurve = antPath(
            [
                'M', latlng1,
                'Q', midpointLatLng,
                latlng2
            ], { use: L.curve, color: "red", fill: true });

        */
    }


    createCurvedLine(latlng1, latlng2) {
        
    }

    /* A Method to remove markers and lines*/
    removeMarkersAndLines() {
        mymap.removeLayer(layerGroup);
    }

    onMapClick() {

        let pointArray = [];  

        var popup = L.popup();

        function onMapClick(e) {
            popup
                .setLatLng(e.latlng)
                .setContent("You clicked the map at " + e.latlng.toString())
                .openOn(mymap);

            pointArray.push(e.latlng.toString());
            console.log(pointArry);
        }

        mymap.on('click', onMapClick);
    }
}


   //If the layer group has to be spilt up
    // addPolyline(latlngs) {
    //    polyline = L.polyline(this.latlngs, { color: 'red' }).addTo(mymap);
    //    mymap.fitBounds(polyline.getBounds());
    //}


//export function onMapClick() {

//    var popup = L.popup();

//    function onMapClick(e) {
//        popup
//            .setLatLng(e.latlng)
//            .setContent("You clicked the map at " + e.latlng.toString())
//            .openOn(mymap);
//    }

//    mymap.on('click', onMapClick);
//}



