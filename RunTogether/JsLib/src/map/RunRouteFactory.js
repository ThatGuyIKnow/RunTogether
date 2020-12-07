import { StageFactory } from './StageFactory';
import { RunRoute } from './RunRoute';

export class RunRouteFactory {
    constructor() { }

    CreateRunRoute(runRouteData, editor = false, map = null, objRef = null) {
        const { Name, Stages } = runRouteData;

        const stageFactory = new StageFactory();
        let stages = [];

        let currStage;
        let lastStage = false;
        if (Array.isArray(Stages)) {
            Stages.forEach((stage, index) => {
                if (index === (Stages.length - 1)) {
                    lastStage = true; 
                }
                currStage = stageFactory.CreateStage(stage, editor, map, objRef, index, lastStage);
                stages.push(currStage);
            });
        }

        return new RunRoute(Name, stages);

    }
}