import Leaflet from 'leaflet';
import { Point } from './Point';
import { Runner } from './Runner';
import { Sponsor } from './Sponsor';
import { isClassOrSubclass } from '../utils/ClassTypeUtils';

class AbstractMarker {
    _mapMarker;
    _map;
    point;

    constructor(map, point) {
        if (this.constructor === AbstractMarker)
            throw new TypeError("Cannot construct abstract class 'AbstractMarker'");

        if (!isClassOrSubclass(point, Point))
            throw new TypeError("Class extending abstract class 'Marker' requires 'Point' Point parameter");

        if (!isClassOrSubclass(map, Leaflet.Map))
            throw new TypeError("Class extending abstract class 'Marker' requires 'Map' Map parameter");
    }
}

export class SponsorPopup extends AbstractMarker {
    sponsor;
    content = "";
    constructor(map, point, sponsor) {
        super(map, point);

        if (!isClassOrSubclass(sponsor, Sponsor))
            throw new TypeError("Class 'SponsorPopup' requires 'Sponsor' Sponsor parameter");

        this._map = map;
        this.point = point;
        this.sponsor = sponsor;
        this.content = '<div class="sponsorPopupContent">' +
                         `<h3>${sponsor.name}</h3><br>` +
                         `<p>${sponsor.message}</p>` +
                       "</div>";
    }
    

    AddToMap(map) {
        this._map = map;
        this._mapMarker = Leaflet.popup();
        this._mapMarker.setLanLng(this.point.toArray())
            .setContent(this.content)
            .addTo(this._map);
        //Leaflet.marker(point.toArray()).addTo(this._map)
    }

    //OpenPopup() {
    //    this._mapMarker.openPopup();
    //}
}

export class RunnersPopup extends AbstractMarker {
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
    }

    AddToMap(map) {
        this._map = map;
        this._mapMarker = Leaflet.popup();
        this._mapMarker.setContent(this.content);
        this._path.bindPopup(this._mapMarker);
        //Leaflet.marker(point.toArray()).addTo(this._map)
    }

/*    L.curve(lineArray).bindPopup('Dette er noget tekst').openPopup().addTo(mymap); */
    //OpenPopup() {
    //    this._mapMarker.openPopup();
    //}
}

/* 
 * MouseDown / MouseUp
 * MapDragging Off / On
 * /