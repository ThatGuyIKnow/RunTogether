import Leaflet from 'leaflet';
import { Point } from './Point';
import { AbstractStage } from './Stage';
import { Sponsor } from './Sponsor';
import { isClassOrSubclass } from '../utils/ClassTypeUtils';

class AbstractMarker {
    _marker;
    _layer;
    point;

    constructor(layer, point) {
        if (this.constructor === AbstractMarker)
            throw new TypeError("Cannot construct abstract class 'AbstractMarker'");

        if (!isClassOrSubclass(point, Point))
            throw new TypeError("Class extending abstract class 'Marker' requires 'Point' Point parameter");

        if (!isClassOrSubclass(layer, Leaflet.Layer))
            throw new TypeError("Class extending abstract class 'Marker' requires 'Layer' Layer parameter");
    }
}

export class Popup extends AbstractMarker {
    _stage;

    _path;

    content = "";

    _popup;

    customMarkerStandard = L.icon({
        iconUrl: '/markers/RU_FLAG_WAYPOINT.png',
        iconSize: [44, 90],
        iconAnchor: [12, 90],
        popupAnchor: [1, -34],
    });

    customMarkerStart = L.icon({
        iconUrl: '/markers/RU_FLAG_START.png',
        iconSize: [44, 105],
        iconAnchor: [0, 105],
        popupAnchor: [1, -34],
        tooltipAnchor: [16, -28],
    });

    customMarkerEnd = L.icon({
        iconUrl: '/markers/RU_FLAG_END.png',
        iconSize: [44, 105],
        iconAnchor: [0, 105],
        popupAnchor: [1, -34],
        tooltipAnchor: [16, -28],
    });

    constructor(stage, path, stageIndex, lastStage) {
        super(stage._layer, stage.startPoint);

        if (!isClassOrSubclass(stage, AbstractStage))
            throw new TypeError("Class 'Popup' requires 'stage' Stage parameter");

        this._stage = stage;
        this._path = path;
        this.stageIndex = stageIndex;
        this.lastStage = lastStage;
        this.content = '<div class="popup">' +
            this.ConstructSponsor(stage.sponsor) +
            "<hr>" +
            this.ConstructRunners(stage.runners) +
            "</div>";
    }

    ConstructSponsor(sponsor) {

        if (sponsor == undefined && sponsor == null) return "";

        return `<img src="${sponsor.pictureUrl}" asp-append-version="true" height="200px" weight="auto">` +
            `<h3>${sponsor.name}</h3><br>` +
            `<p>${sponsor.message}</p>`;
    }

    ConstructRunners(runners) {
        if (runners == undefined || runners === null || runners.length === 0) return "";
        let content = "";
        runners.forEach(runner => {
            content += `<p>${runner.name}</p>`;
        });
        return "<h4>Løberne</h4>" + content;
    }

    AddToLayer(layer) {


        if (this.stageIndex === 0)
            this._marker = Leaflet.marker(this._stage.startPoint.toArray(), { icon: this.customMarkerStart });
        else
            this._marker = Leaflet.marker(this._stage.startPoint.toArray(), { icon: this.customMarkerStandard });


        if (this.lastStage)
            this._marker2 = Leaflet.marker(this._stage.endPoint.toArray(), { icon: this.customMarkerEnd });
        else
            this._marker2 = Leaflet.marker(this._stage.endPoint.toArray(), { icon: this.customMarkerStandard });

        this._marker.addTo(this._stage._layer);
        this._marker2.addTo(this._stage._layer);

        if ((this._stage.sponsor !== undefined && this._stage.sponsor !== null) ||
            (Array.isArray(this._stage.runners) && this._stage.runners.length > 0)) {
            this._popup = Leaflet.popup();
            this._popup.setContent(this.content);
            this._path.bindPopup(this._popup);
            this._marker.bindPopup(this._popup);
        }
    }
}

export class EditorMarker extends AbstractMarker {
    constructor(layer, point, map, stage, prevStage, dotnetHelper, lastMarker) {
        super(layer, point);

        this._layer = layer;
        this.point = point;
        this._map = map;

        this._dotnetHelper = dotnetHelper; 
        this._stage = stage;
        this._prevStage = prevStage;
        this._lastMarker = lastMarker; 
    }

    AddToLayer(layer) {
        this._layer = layer;
        this._mapMarker = Leaflet.circleMarker(this.point.toArray(), { bubblingMouseEvents: false, fillOpacity: 1 });
        this._mapMarker.addTo(this._layer);

        this.InteractableMarker(this._mapMarker); 

    }

    InteractableMarker(marker) {
        //constantly sets the markers pos to the curser pos
        function trackCursor(evt) {
            marker.setLatLng(evt.latlng);
        }

        //Drag marker 
            marker.on("mousedown", () => {
            this._map.dragging.disable();
            this._map.on("mousemove", trackCursor);
        })

        //Stop dragging marker 
            marker.on("mouseup", () => {
                this._map.dragging.enable();
                this._map.off("mousemove", trackCursor);
                this.markerDragEnd(marker);
         })

        return marker;
    }

    markerDragEnd(marker) {
        //send a stages back to blazor, for saving to DB

        if (this._lastMarker == true) {
            this._dotnetHelper.invokeMethodAsync('Trigger', 'EditStage',
                JSON.stringify(
                    {
                        StageId: this._stage.stageId,
                        StartPoint: { X: this._stage.startPoint.x, Y: this._stage.startPoint.y },
                        EndPoint: { X: marker.getLatLng().lat, Y: marker.getLatLng().lng }
                    }));
        }
        else {
            this._dotnetHelper.invokeMethodAsync('Trigger', 'EditStage',
                JSON.stringify(
                    {
                        StageId: this._stage.stageId,
                        StartPoint: { X: marker.getLatLng().lat, Y: marker.getLatLng().lng },
                        EndPoint: { X: this._stage.endPoint.x, Y: this._stage.endPoint.y }
                    }));

            this._dotnetHelper.invokeMethodAsync('Trigger', 'EditStage',
                JSON.stringify(
                    {
                        StageId: this._prevStage.stageId,
                        StartPoint: { X: this._prevStage.startPoint.x, Y: this._prevStage.startPoint.y },
                        EndPoint: { X: marker.getLatLng().lat, Y: marker.getLatLng().lng }
                    }));
        }
    }
}

