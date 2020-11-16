import { isClassOrSubclass } from '../utils/ClassTypeUtils';
import { AbstractStage } from './Stage';

export class RunRoute {
    _layer = null;
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

    AddToLayer(layer, map = null) {
        this._layer = layer;
        if (this.stages !== null) {
            this.stages.forEach(stage => {
                stage.AddToLayer(layer);
            });
        }
    }

    RemoveFromLayer() {
        if (this.stages !== null) {
            this.stages.forEach(stage => {
                stage.RemoveFromLayer();
            });
        }
        this._layer = null;
    }
}