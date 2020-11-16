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
        this._popup = Leaflet.popup();
        this._popup.setContent(this.content);
        
        this._marker = Leaflet.marker(this._stage.startPoint.toArray());
        this._marker2 = Leaflet.marker(this._stage.endPoint.toArray()); 

        this._marker.addTo(this._stage._layer);
        this._marker2.addTo(this._stage._layer);

        this._path.bindPopup(this._popup);
        this._marker.bindPopup(this._popup);

        //Leaflet.marker(point.toArray()).addTo(this._map)

        // layerGroup.addLayer(_marker);
    }

    //OpenPopup() {
    //    this._mapMarker.openPopup();
    //}
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
