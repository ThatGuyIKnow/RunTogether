
/*Global variable for the map class*/
let mymap, layerGroup;
let maxBounds1 = [51.649, 0.49]; 
let maxBounds2 = [59.799, 18.68];  
let bounds = L.latLngBounds(maxBounds1, maxBounds2);

/*Class for the map*/
export class mapClass {

    constructor() {

        /*console.log("from constructor!");*/

        /*        this.obj = obj; */
        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        this.calulateControlPoints = this.calulateControlPoints.bind(this); 
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
    addMarkersAndLines(object) {

        this.removeMarkersAndLines();

        let obj = JSON.parse(object);  

        console.log(object); 
        console.log(obj); 

        let i = 0, j = 0, n=0, curvedLine, lineArray = [], runnerName = [];

        /*Creating layer group and adding to map*/
        layerGroup = L.layerGroup().addTo(mymap);

       /*Creates curvedlies from object with stages*/

        /* for (i = 0; i < this.obj.Stages.length; i++) {

            let startpoint = this.obj.Stages[i].StartPoint;
            let endpoint = this.obj.Stages[i].EndPoint
            let throughpoint, guidepoint;

            lineArray.push('M', startpoint,);

            for (j = 0; j < this.obj.Stages[i].GuidePoint.length; j++) {

                if (j < this.obj.Stages[i].Throughpoint.length) {
                    throughpoint = this.obj.Stages[i].Throughpoint[j];
                    guidepoint = this.obj.Stages[i].GuidePoint[j];
                    lineArray.push('Q', guidepoint, throughpoint,);
                } else {
                    guidepoint = this.obj.Stages[i].GuidePoint[j];
                    lineArray.push('Q', guidepoint, endpoint);
                }

                console.log(lineArray);
            }

            for (n = 0; n > this.obj.Stages[i].GuidePoint[j]) {

            }

            L.marker(startpoint).addTo(mymap);
            L.marker(endpoint).addTo(mymap);

            curvedLine = L.curve(lineArray, { color: 'red' }).bindPopup('Dette er noget tekst').openPopup();

            layerGroup.addLayer(curvedLine).addTo(mymap); 

            mymap.fitBounds(curvedLine.getBounds());


            calulateControlPoints(latlng1, latlng2); 

        }*/

        let latlng1 = [57.0405, 9.9101];
        let latlng2 = [57.0257, 9.9062]; 

        let latlng3 = [57.0257, 9.9062]; 
        let latlng4 = [57.0039, 9.9027];

        let midtpunkt1 = this.calulateControlPoints(latlng1, latlng2); 
        let midtpunkt2 = this.calulateControlPoints(latlng3, latlng4); 

        let curvedPath = L.curve(
            [
                'M', latlng1,
                'Q', midtpunkt1, latlng2,
                'Q', midtpunkt2, latlng4
            ], {color: 'red'}).addTo(mymap);

    }

    calulateControlPoints(latlng1, latlng2) {
        let offsetX = latlng2[1] - latlng1[1],
            offsetY = latlng2[0] - latlng1[0];

        let r = Math.sqrt(Math.pow(offsetX, 2) + Math.pow(offsetY, 2)),
            theta = Math.atan2(offsetY, offsetX);

        let thetaOffset = (10 / 10);

        let r2 = (r / 2) / (Math.cos(thetaOffset)),
            theta2 = theta + thetaOffset;

        let  midpointX = (r2 * Math.cos(theta2)) + latlng1[1],
             midpointY = (r2 * Math.sin(theta2)) + latlng1[0];

        let midpointLatLng = [midpointY, midpointX];

        return midpointLatLng; 

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

