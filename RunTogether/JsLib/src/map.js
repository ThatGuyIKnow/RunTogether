
import Leaflet from 'leaflet';
import {RunRouteFactory} from "./map/index";

/*Class for the map*/
export class mapClass {
    myMap = null;
    layerGroup = null;
    maxBounds1 = [51.649, 0.49];
    maxBounds2 = [59.799, 18.68];
    bounds;

    constructor() {
        this.initializeMap = this.initializeMap.bind(this);
        this.addMarkersAndLines = this.addMarkersAndLines.bind(this);
        this.bounds = Leaflet.latLngBounds(this.maxBounds1, this.maxBounds2);
    }

    /* A Method that initializes the map */
    initializeMap() {   

        /*Pointing myMap to leaflet map and setting the viewpoint and start zoom point*/
        this.myMap = Leaflet.map('mapid').setView([55.964, 9.992], 6.5);

        this.myMap.setMaxBounds(this.bounds);

        /* Appling tile layer to the map*/
        Leaflet.tileLayer('https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}', {
            attribution: 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            subdomains: 'abcd',
            minZoom: 6,
            maxZoom: 13,
            maxBounds: this.bounds,
            maxBoundsViscosity: 1, 
            ext: 'jpg'
        }).addTo(this.myMap);

        /*Creating layer group and adding to map*/
        this.layerGroup = Leaflet.layerGroup().addTo(this.myMap);
    }

    /* A Method to add markers and lines*/
    addMarkersAndLines(object) {

        /*Removes old lines*/
        this.RemoveLayer();

        /*Creating layer group and adding to map*/
        this.layerGroup = Leaflet.layerGroup().addTo(this.myMap);

        let routeFactory = new RunRouteFactory();   

        let parsedData = JSON.parse(object);
        let routeRun = routeFactory.CreateRunRoute(parsedData);
        routeRun.AddToLayer(this.layerGroup);

        this.AddFilter();
    }

    AddFilter() {
        var svg = this.myMap.getPanes().overlayPane.firstChild,
            svgFilter = document.createElementNS('http://www.w3.org/2000/svg', 'filter');

        svgFilter.setAttribute('id', 'pathFilter');
        svgFilter.setAttribute('x', '-100%');
        svgFilter.setAttribute('y', '-100%');
        svgFilter.setAttribute('width', '500%');
        svgFilter.setAttribute('height', '500%');


        let s1 = '<feGaussianBlur in="SourceAlpha" stdDeviation = "2" result = "BLUR" ></feGaussianBlur >' +
            '<feSpecularLighting in="BLUR" surfaceScale="6" specularExponent="30" result="SPECULAR" lighting-color="#white">' +
            '<fePointLight x="40" y="-30" z="200" result="b61119e7-b6a7-4e48-a1e1-1e36ab0c51be"></fePointLight>' +
            '</feSpecularLighting>' +
            '<feComposite in="SPECULAR" in2="SourceAlpha" operator="in" result="COMPOSITE"></feComposite>' +
            '<feMerge result="9ab789a6-a71b-42be-b5de-48a608b4a107">' +
            '<feMergeNode in="SourceGraphic"></feMergeNode><feMergeNode in="COMPOSITE"></feMergeNode>' +
            '</feMerge>';

        svgFilter.innerHTML = s1;
        svg.appendChild(svgFilter);
    }

    RemoveLayer() {
        if (this.layerGroup != null || this.layerGroup != undefined)
        {
            this.myMap.removeLayer(this.layerGroup);
            this.layerGroup = Leaflet.layerGroup().addTo(this.myMap);
        }
    }
}