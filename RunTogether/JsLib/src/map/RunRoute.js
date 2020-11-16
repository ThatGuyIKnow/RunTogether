import { isClassOrSubclass } from '../utils/ClassTypeUtils';
import { AbstractStage } from './Stage';

export class RunRoute {
    _map = null;
    stages = null;
    name = "";

    constructor(name, stages) {
        if (typeof name !== "string" ||
            !Array.isArray(stages) ||
            stages.forEach(s => isClassOrSubclass(s, AbstractStage)))
            throw new TypeError("Class 'RunRoute' constructor has parameters '<name : string, stages : Stage[]>'");

        this.name = name;
        this.stages = stages;
    }

    AddToMap(map) {
        this._map = map;
        if (this.stages !== null) {
            this.stages.forEach(stage => {
                stage.AddToMap(map);
            });
        }
    }

    RemoveFromMap() {
        if (this.stages !== null) {
            this.stages.forEach(stage => {
                stage.RemoveFromMap();
            });
        }
        this._map = null;
    }
}