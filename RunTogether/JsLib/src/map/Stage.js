import Leaflet from 'leaflet';
import '@elfalem/leaflet-curve';
import { Point } from './Point';
import { Popup, EditorMarker } from './Marker';
import { Sponsor } from './Sponsor';
import { Runner } from './Runner';  
import { isClassOrSubclass } from '../utils/ClassTypeUtils';

/*
 * Abstract Stage class
 */
export class AbstractStage {
    constructor(startPoint, endPoint, throughPoints, flipped) {
        if (this.constructor === AbstractStage)
            throw new TypeError("Cannot construct abstract class 'Stage'");

        // Point StartPoint;
        if (!isClassOrSubclass(startPoint, Point))
            throw new TypeError("Class extending abstract class 'Stage' requires 'StartPoint' Point parameter");

        // Point EndPoint;
        if (!isClassOrSubclass(endPoint, Point))
            throw new TypeError("Class extending abstract class 'Stage' requires 'EndPoint' Point parameter");

        // Point[] ThroughPoints;
        if (!Array.isArray(throughPoints) ||
            !throughPoints.every(p => isClassOrSubclass(p, Point)))
            throw new TypeError("Class extending abstract class 'Stage' requires 'ThroughPoints' Point Array parameter");

        // Boolean flipped
        if (typeof flipped !== "boolean")
            throw new TypeError("Class extending abstract class 'Stage' requires 'Flipped' boolean parameter");
    }

    AddToLayer(layer) {
        if (!isClassOrSubclass(layer, Leaflet.Layer))
            throw new TypeError("Parameter 'layer' should be or extend from Layer class");
        
        this._layer = layer;
        this.path = this.CreatePath().setStyle({ 'weight': 10 });
        this.path.addTo(this._layer);
        this.AddHoverFocus(this.path);
        this.AddPopup(this, this.path);
        this.path._path.setAttribute('filter', 'url(#pathFilter)');
    }

    AddHoverFocus(target, trigger = null) {
        if (trigger === null) trigger = target;
        trigger.on('mouseover', () => target.setStyle({ weight: 15 }));
        trigger.on('mouseout', () => target.setStyle({ weight: 10 }));
    }


    RemoveFromLayer() {
        this.path.removeFrom(this._layer);
        this._layer = null;
    }

    CreatePath() {
        const path = [];
        const points = [this.startPoint, ...this.throughPoints, this.endPoint];

        points.forEach(p => {
            path.push(p.toArray());
        });
        return Leaflet.polyline(path);
    }


    //CreatePath() {
    //    const path = [];
    //    const points = [this.startPoint, ...this.throughPoints, this.endPoint];

    //    path.push('M', this.startPoint.toArray());

    //    let direction = this.flipped ? -1 : 1;
    //    for (let i = 0; i < points.length - 1; i++) {
    //        let controlPoint = this.CalculateControlPoint(points[i], points[i + 1], direction);
    //        path.push('Q', controlPoint.toArray(), points[i + 1].toArray());
    //        direction *= -1;
    //    }
    //    return Leaflet.curve(path);
    //}

    CalculateControlPoint(point1, point2, amplitude) {
        if (!isClassOrSubclass(point1, Point) ||
            !isClassOrSubclass(point2, Point) ||
            typeof amplitude !== "number")
            throw new TypeError("Method 'CalculateControlPoints' use parameters '<latlngFrom : Point, latlngTo : Point, amplitude : Number>'");

        const latlngFrom = point1.toArray(),
              latlngTo = point2.toArray();

        let offsetX = latlngTo[1] - latlngFrom[1],
            offsetY = latlngTo[0] - latlngFrom[0];

        let r = Math.sqrt(Math.pow(offsetX, 2) + Math.pow(offsetY, 2));
        let theta = Math.atan2(offsetY, offsetX);

        let thetaOffset = (11 / 10);

        let r2 = (r / 2) / (Math.cos(thetaOffset));
        let theta2 = theta + (amplitude * thetaOffset);

        let controlPointX = (r2 * Math.cos(theta2)) + latlngFrom[1];
        let controlPointY = (r2 * Math.sin(theta2)) + latlngFrom[0];

        return new Point(controlPointY, controlPointX);
    }

    AddPopup(stage, path) {
        let popup = new Popup(stage, path, this.stageIndex, this.lastStage);
        popup.AddToLayer();
        this._popup = popup;
        return popup;
    } 

    EvenNumberOfCurves() {
        return this.throughPoints.length % 2 === 1;
    }

    PassBind(bind) {
        this.GetPrevLine = bind; 
    }
}

export class InactiveStage extends AbstractStage {

    _layer = null;
    _className = "";
    notCompletedClass = "notStartedStage";
    completedClass = "completedStage";
    runners = [];
    path = null;
    flipped = false;
    completed = false;

    constructor(stageIndex, lastStage, startPoint, endPoint, throughPoints = [], flipped = false, completed = false) {
        super(startPoint, endPoint, throughPoints, flipped);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.flipped = flipped;
        this.completed = completed;
        this._className = this.completed ? this.completedClass : this.notCompletedClass;
        this.stageIndex = stageIndex;
        this.lastStage = lastStage;
    }

    AddToLayer(layer) {
        super.AddToLayer(layer);
        this.path._path.classList.add(this._className);
    }
}

export class ActiveStage extends AbstractStage {

    _layer = null;
    _overlayPercentage = 0.0;
    _activePopup;
    runners = [];
    className = "activeStage";
    overlayClassName = "activeStageOverlay";
    path = null;
    overlayPath = null;
    flipped = false;

    constructor(stageIndex, lastStage, startPoint, endPoint, throughPoints = [], flipped = false) {
        super(startPoint, endPoint, throughPoints, flipped);
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.flipped = flipped;
        this.stageIndex = stageIndex;
        this.lastStage = lastStage;
    }

    AddToLayer(layer) {
        super.AddToLayer(layer);
        this.path._path.classList.add(this.className);

        this.overlayPath = this.CreatePath();
        this.overlayPath.addTo(this._layer);
        this.overlayPath.setStyle({ 'weight': 10 });
        this.overlayPath._path.classList.add(this.overlayClassName);
        this.overlayPath._path.setAttribute('filter', 'url(#pathFilter)');

        this.overlayPath._renderer.on('update', () => {
            this.UpdateOverlay();
        });
        this.SetOverlayPercentage(this.CalculateOverlayPercentage());
        this.AddHoverFocus(this.overlayPath, this.path);
        this.AddHoverFocus(this.path);
        this.AddHoverFocus(this.overlayPath);
        this._activePopup = this.AddPopup(this, this.overlayPath);
    }

    CalculateOverlayPercentage() {
        let runnerCount = this.runners.length;
        if (runnerCount === 0) {
            SetOverlayPercentage(0);
            return;
        }
        let completedCount = this.runners.reduce((sum, runner) => {
            return sum + (runner.status === 'Completed' ? 1 : 0);
        }, 0);
        
        return completedCount / runnerCount;
    }

    SetOverlayPercentage(pct) {
        if (typeof pct !== "number")
            throw new TypeError("Method 'SetOverlayPercentage' use parameters '<pct : Number>'");
        this._overlayPercentage = pct;

        if (this._layer !== null) {
            this.UpdateOverlay();
        }
    }

    UpdateOverlay() {
        if (this.overlayPercentage >= 1) {
            this.overlayPath._lanlngs = this.path._lanlngs;
        }
        let points = [this.startPoint, ...this.throughPoints, this.endPoint];
        // Measure the lengths between each consecutive point
        let lengths = [];
        for (let i = 0; i < points.length - 1; i++) 
            lengths.push(this.GetLength(points[i], points[i + 1]));
        // Get the length the runner has run along that strech
        let overlayLength = lengths.reduce((accum, curr) => accum + curr) * this._overlayPercentage;

        // Get the streches the runners have completed
        let lanlngs = [this.startPoint.toArray(),];
        let accumOverlayLength = overlayLength;
        let i = 0;
        for(; i < lengths.length; i++){
            if (lengths[i] > accumOverlayLength) break;
            lanlngs.push(points[i + 1].toArray());
            accumOverlayLength -= lengths[i];
        }

        //Set the final partial strech
        let finalStrech1 = points[i - 1].toArray(),
            finalStrech2 = points[i].toArray();
        let delta = [finalStrech2[0] - finalStrech1[0], finalStrech2[1] - finalStrech1[1]];
        let finalStrechPct = accumOverlayLength / lengths[i];
        lanlngs.push([finalStrech1[0] + delta[0] * finalStrechPct, finalStrech1[1] + delta[1] * finalStrechPct]);


        this.overlayPath.setLatLngs(lanlngs);
    }

    GetLength(point1, point2) {
        const p1 = point1.toArray(),
            p2 = point2.toArray();
        return Math.sqrt(Math.pow((p2[0] - p1[0]), 2) + Math.pow((p2[1] - p1[1]), 2));
    }
}

export class EditStage extends AbstractStage {

    _layer = null;
    _overlayPercentage = 0.0;
    runners = [];
    className = "notStartedStage";
    path = null;
    overlayPath = null;
    flipped = false;


    constructor(startPoint, endPoint, throughPoints = [], flipped = false, stageId, map, objRef, stageIndex, lastStage) {
        super(startPoint, endPoint, throughPoints, flipped);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.flipped = flipped;
        this.stageId = stageId;

        this._map = map; 
        this._dotnetHelper = objRef;
        this._stageIndex = stageIndex;
        this._lastStage = lastStage;
    }

    AddToLayer(layer) {
        super.AddToLayer(layer, this.startPoint);

        this._prevStage = this.GetPrevLine(this._stageIndex);

        this.path._path.classList.add(this.className);
        this.InteractablePath(this.path);

        this._marker = new EditorMarker(this._layer, this.startPoint, this._map, this, this._prevStage, this._dotnetHelper, false);
        this._marker.AddToLayer(this._layer);

        if (this._lastStage == true) {
            this._marker = new EditorMarker(this._layer, this.endPoint, this._map, this, this._prevStage, this._dotnetHelper, true);
            this._marker.AddToLayer(this._layer);
        } 
    }

    InteractablePath(path) {
        path.on('mouseover', () => {
            path.setStyle({ color: '#ff5c26', weight: 12 });
        })

        path.on('mouseout', () => {
            path.setStyle({ color: '#db5d57', weight: 6 });
        })

        path.on('click', () => {
            this._dotnetHelper.invokeMethodAsync('Trigger', 'SendStageId',
                JSON.stringify(
                    {
                        StageId: this.stageId
                    }));
        })
    }

    AddPopup() {
        //empty for now
    } 

}
