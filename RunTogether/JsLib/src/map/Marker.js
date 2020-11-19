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
    constructor(stage, path) {
        super(stage._layer, stage.startPoint);

        if (!isClassOrSubclass(stage, AbstractStage))
            throw new TypeError("Class 'Popup' requires 'stage' Stage parameter");

        this._stage = stage;
        this._path = path;
        this.content = '<div class="popup">' +
                            `<h3>${stage.sponsor.name}</h3><br>` +
                            `<p>${stage.sponsor.message}</p>` +
                            "<hr>" + "<h4>Løberne</h4>" +
                            `${this.ConstructContent(stage.runners)}` +
                       "</div>";
    }

    ConstructContent(runners) {
        let content = "";
        runners.forEach(runner => {
            content += `<p>${runner.name}</p>`;
        });
        return content;
    }

    AddToLayer(layer) {

        let customMarker = L.icon({
            iconUrl: '/markers/RU_CUSTOM_MARKER.png',
            iconSize: [24, 41],
            iconAnchor: [12, 41],
            popupAnchor: [1, -34],
            tooltipAnchor: [16, -28],
        }); 

        this._popup = Leaflet.popup();
        this._popup.setContent(this.content);

        this._marker = Leaflet.marker(this._stage.startPoint.toArray(), { icon: customMarker} );
        this._marker2 = Leaflet.marker(this._stage.endPoint.toArray(), { icon: customMarker } ); 

        this._marker.addTo(this._stage._layer);
        this._marker2.addTo(this._stage._layer);

        this._path.bindPopup(this._popup);
        this._marker.bindPopup(this._popup);

    }

    //OpenPopup() {
    //    this._mapMarker.openPopup();
    //}
}

/*export class ViewerMarker extends AbstractMarker {
    _stage;

    constructor(stage) {
        super(stage.startPoint);

        if (!isClassOrSubclass(stage, AbstractStage))
            throw new TypeError("Class 'ViewerMarker' requires 'stage' Stage parameter");
        this._stage = stage;
    }

    AddToLayer(layer) {
        let customMarker = L.icon({
            iconUrl: '/markers/RU_CUSTOM_MARKER.png', 
            iconSize: [24, 41], 
            iconAnchor: [12, 41],
            popupAnchor: [1, -34],
            tooltipAnchor: [16, -28],
        }); 

        this._marker = Leaflet.marker(this._stage.startPoint.toArray(), { icon: customMarker });
        this._marker2 = Leaflet.marker(this._stage.endPoint.toArray(), { icon: customMarker });
        this._marker.addTo(this._stage._layer);
        this._marker2.addTo(this._stage._layer);
        this._marker.bindPopup(this._popup);
    }

}*/

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
        //this.point.x = marker.getLatLng().lat;
        //this.point.y = marker.getLatLng().lng;

        //this._path.startPoint.x = marker.getLatLng().lat;
        //this._path.startPoint.y = marker.getLatLng().lng;

        //this._prevPath.endPoint.x = marker.getLatLng().lat;
        //this._prevpath.endPoint.y = marker.getLatLng().lng;

        //send a stages back to blazor, for saving to DB
        console.log("stages touched by point");
        console.log(this._stage);
        console.log(this._prevStage);

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


/*
 * MouseDown / MouseUp
 * MapDragging Off / On
 * /
 
/*export class RunnersPopup extends AbstractMarker {
    sponsor;
    content = "";
    _path;
    constructor(map, path, runners) {
        super(map, new Point(0, 0));

        if (!Array.isArray(runners) ||
            !runners.every(r => isClassOrSubclass(r, Runner)))
            throw new TypeError("Class 'RunnerPopup' requires 'Runners' Runner[] parameter");

        this._map = map;
        this._path = path;
        this.point = point;
        this.sponsor = sponsor;
        this.content = ConstructContent(runners);
    }

    ConstructContent(runners) {
        let content = '<div class="runnerPopupContent">';
        runners.forEach(runner => {
            content += `<p>${runner.name}</p>`;
        });
        content += "</div>";
        return content;
    }*/

//    AddToMap(map) {
//        this._map = map;
//        this._mapMarker = Leaflet.popup();
//        this._mapMarker.setContent(this.content);
//        this._path.bindPopup(this._mapMarker);
//        //Leaflet.marker(point.toArray()).addTo(this._map)
//    }

///*    L.curve(lineArray).bindPopup('Dette er noget tekst').openPopup().addTo(mymap); */
//    //OpenPopup() {
//    //    this._mapMarker.openPopup();
//    //}
//}
