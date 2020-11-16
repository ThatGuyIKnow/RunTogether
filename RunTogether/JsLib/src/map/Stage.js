import Leaflet from 'leaflet';
import '@elfalem/leaflet-curve';
import { Point } from './Point';
import {isClassOrSubclass} from '../utils/ClassTypeUtils';

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

    AddToMap(map) {
        if (!isClassOrSubclass(map, Leaflet.Map))
            throw new TypeError("Parameter 'map' should be or extend from Map class");

        this._map = map;
        this.path = this.CreatePath();
        this.path.addTo(this._map);
    }

    RemoveFromMap() {
        this.path.removeFrom(this._map);
        this._map = null;
    }

    CreatePath() {
        const path = [];
        const points = [this.startPoint, ...this.throughPoints, this.endPoint];

        path.push('M', this.startPoint.toArray());

        let direction = this.flipped ? -1 : 1;
        for (let i = 0; i < points.length - 1; i++) {
            let controlPoint = this.CalculateControlPoint(points[i], points[i + 1], direction);
            path.push('Q', controlPoint.toArray(), points[i + 1].toArray());
            direction *= -1;
        }
        return Leaflet.curve(path);
    }

    CalculateControlPoint(point1, point2, amplitude) {
        if (!isClassOrSubclass(point1, Point) ||
            !isClassOrSubclass(point2, Point) ||
            typeof amplitude !== "number")
            throw new TypeError("Method 'CalculateControlPoints' use parameters '<latlng1 : Point, latlng2 : Point, amplitude : Number>'");

        const latlng1 = point1.toArray(),
            latlng2 = point2.toArray();

        let offsetX = latlng2[1] - latlng1[1],
            offsetY = latlng2[0] - latlng1[0];

        let r = Math.sqrt(Math.pow(offsetX, 2) + Math.pow(offsetY, 2)),
            theta = Math.atan2(offsetY, offsetX);

        let thetaOffset = (11 / 10);

        let r2 = (r / 2) / (Math.cos(thetaOffset));
        let theta2 = theta + (amplitude * thetaOffset);

        let midpointX = (r2 * Math.cos(theta2)) + latlng1[1],
            midpointY = (r2 * Math.sin(theta2)) + latlng1[0];

        return new Point(midpointY, midpointX);
    }

    EvenNumberOfCurves() {
        console.log(this.throughPoints);
        return this.throughPoints.length % 2 === 1;
    }
}

export class InactiveStage extends AbstractStage {

    _map = null;
    _className = "";
    notCompletedClass = "notStartedStage";
    completedClass = "completedStage";
    runners = [];
    path = null;
    flipped = false;
    completed = false;

    constructor(startPoint, endPoint, throughPoints = [], flipped = false, completed = false) {
        super(startPoint, endPoint, throughPoints, flipped);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.flipped = flipped;
        this.completed = completed;
        this._className = this.completed ? this.completedClass : this.notCompletedClass;
    }

    AddToMap(map) {
        super.AddToMap(map);
        this.path._path.classList.add(this._className);
    }
}

export class ActiveStage extends AbstractStage {

    _map = null;
    _overlayPercentage = 0.0;
    runners = [];
    className = "activeStage";
    overlayClassName = "activeStageOverlay";
    path = null;
    overlayPath = null;
    flipped = false;

    constructor(startPoint, endPoint, throughPoints = [], flipped = false) {
        super(startPoint, endPoint, throughPoints, flipped);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        this.flipped = flipped;
    }

    AddToMap(map) {
        super.AddToMap(map);
        this.path._path.classList.add(this.className);

        this.overlayPath = this.CreatePath();
        this.overlayPath.addTo(this._map);
        this.overlayPath._path.classList.add(this.overlayClassName);

        this.SetOverlayPercentage(this.CalculateOverlayPercentage());
        this.UpdateOverlay();
        this.overlayPath._renderer.on('update', () => {
            this.UpdateOverlay();
        });
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

        if (this._map !== null) {
            this.UpdateOverlay();
        }
    }

    UpdateOverlay() {
        let length = this.overlayPath._path.getTotalLength();
        this.overlayPath._path.style.strokeDashoffset = (1 - this._overlayPercentage) * length;
        this.overlayPath._path.style.strokeDasharray = length;
    }
}

export class EditStage extends AbstractStage {

    _map = null;
    _overlayPercentage = 0.0;
    runners = [];
    className = "editStage";
    //overlayClassName = "activeStageOverlay";
    path = null;
    overlayPath = null;
    flipped = false;

    constructor(startPoint, endPoint, throughPoints = [], flipped = false) {
        super(startPoint, endPoint, throughPoints, flipped);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throughPoints = throughPoints;
        //this.flipped = flipped;
    }

    AddToMap(map) {
        super.AddToMap(map);
        //this.path._path.classList.add(this.className); ?? 
        this.InteractablePath(this.path);
    }

    InteractablePath(path) {
        path.on('mouseover', () => {
            path.setStyle({ color: '#ff5c26', weight: 12 });
        })

        path.on('mouseout', () => {
            path.setStyle({ color: '#db5d57', weight: 6 });
        })

        path.on('click', () => {;
            this.dotnetHelper.invokeMethodAsync('Trigger', 'SendStageId',
                JSON.stringify(
                    {
                        StageId: this.stageId
                    }));
        })
    }

}
