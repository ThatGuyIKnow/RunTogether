import { StageFactory } from './StageFactory';
import { RunRoute } from './RunRoute';

export class RunRouteFactory {
    constructor() { }

    CreateRunRoute(runRouteData, editor = false, map = null, objRef = null) {
        const { Name, Stages } = runRouteData;

        const stageFactory = new StageFactory();
        let stages = [];
        let currStage;
        let flipped = false;
        if (Array.isArray(Stages)) {
            Stages.forEach(stage => {
                currStage = stageFactory.CreateStage(stage, flipped, editor, map, objRef);
                stages.push(currStage);
                flipped = currStage.EvenNumberOfCurves() === flipped;
            });
        }

        return new RunRoute(Name, stages);

    }
}