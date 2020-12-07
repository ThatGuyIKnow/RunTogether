import Leaflet from 'leaflet';
import '@elfalem/leaflet-curve';
import { Point } from './Point';
import { Popup, EditorMarker } from './Marker';
import { isClassOrSubclass } from '../utils/ClassTypeUtils';

/*
 * Abstract Stage class
 */
export class AbstractStage {
    constructor(startPoint, endPoint, throughPoints) {
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
    }

    AddToLayer(layer) {
        if (!isClassOrSubclass(layer, Leaflet.Layer))
            throw new TypeError("Parameter 'layer' should be or extend from Layer class");
        
        this._layer = layer;
        this.path = this.CreatePath().setStyle({ 'weight': 10 });
        this.path.addTo(this._layer);
        this.AddHoverFocus(this.path);
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

    AddPopup(stage, path) {
        let popup = new Popup(stage, path, this.stageIndex, this.lastStage);
        popup.AddToLayer();
        this._popup = popup;
        return popup;
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
    completed = false;

    constructor(stageIndex, lastStage, startPoint, endPoint, throughPoints = [], completed = false) {
        super(startPoint, endPoint, throughPoints);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.completed = completed;
        this._className = this.completed ? this.completedClass : this.notCompletedClass;
        this.stageIndex = stageIndex;
        this.lastStage = lastStage;
    }

    AddToLayer(layer) {
        super.AddToLayer(layer);
        super.AddPopup(this, this.path);
        this.path._path.setAttribute('filter', 'url(#pathFilter)');
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

    constructor(stageIndex, lastStage, startPoint, endPoint, throughPoints = []) {
        super(startPoint, endPoint, throughPoints);
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.stageIndex = stageIndex;
        this.lastStage = lastStage;
    }

    AddToLayer(layer) {
        super.AddToLayer(layer);
        super.AddPopup(this, this.path);
        this.path._path.setAttribute('filter', 'url(#pathFilter)');
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
            this.overlayPath.setLatLngs(this.path._lanlngs);
        }
        let points = [this.startPoint, ...this.throughPoints, this.endPoint];
        // Measure the lengths between each consecutive point
        let lengths = [];
        for (let i = 0; i < points.length - 1; i++) 
            lengths.push(this.GetLength(points[i], points[i + 1]));
        // Get the length the runner has run along that strech
        let overlayLength = lengths.reduce((accum, curr) => accum + curr) * this._overlayPercentage;

        // Get the streches the runners have completed
        let latlngs = [this.startPoint.toArray(),];
        let accumOverlayLength = overlayLength;
        let i = 0;
        for(; i < lengths.length; i++){
            if (lengths[i] > accumOverlayLength) break;
            latlngs.push(points[i + 1].toArray());
            accumOverlayLength -= lengths[i];
        }

        //Set the final partial stretch
        let finalStretch1 = points[i].toArray(),
            finalStretch2 = points[i+1].toArray();

        //Calculate and assign the partial stretch 
        let delta = [finalStretch2[0] - finalStretch1[0], finalStretch2[1] - finalStretch1[1]];
        let finalStretchPct = accumOverlayLength / lengths[i];
        latlngs.push([finalStretch1[0] + delta[0] * finalStretchPct, finalStretch1[1] + delta[1] * finalStretchPct]);


        this.overlayPath.setLatLngs(latlngs);
    }

    GetLength(point1, point2) {
        const p1 = point1.toArray(),
            p2 = point2.toArray();
        return Math.sqrt(Math.pow((p2[0] - p1[0]), 2) + Math.pow((p2[1] - p1[1]), 2));
    }
}

export class EditStage extends AbstractStage {

    _layer = null;
    runners = [];
    className = "notStartedStage";
    path = null;


    constructor(startPoint, endPoint, throughPoints = [], stageId, map, objRef, stageIndex, lastStage) {
        super(startPoint, endPoint, throughPoints);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.stageId = stageId;

        this._map = map; 
        this._dotnetHelper = objRef;
        this._stageIndex = stageIndex;
        this._lastStage = lastStage;
    }

    AddToLayer(layer) {
        //super.AddToLayer(layer, this.startPoint);
        super.AddToLayer(layer);

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
}
