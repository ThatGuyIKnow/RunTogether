import { ActiveStage, InactiveStage, EditStage } from './Stage';
import {Point} from './Point';
import {Runner} from './Runner';
import {Sponsor} from './Sponsor';

export class StageFactory {
    constructor() { }

    CreateStage(stageData, editStage = false, map = null, objRef = null, stageIndex, lastStage) {
        const startPoint = new Point(...stageData.StartPoint);
        const endPoint = new Point(...stageData.EndPoint);
        const throughPoints = this.ConstructThroughPoints(stageData.ThroughPoints);
        let runners = null;
        if(Array.isArray(stageData.Runners) && stageData.Runners.length > 0) 
            runners = this.ConstructRunners(stageData.Runners);

    let sponsor = null;
    if (stageData.Sponsor !== undefined && stageData.Sponsor !== null)
        sponsor = new Sponsor(stageData.Sponsor.Name,
            stageData.Sponsor.Message,
            stageData.Sponsor.PictureURL);

    let stage;

        if (editStage == true) {
            stage = new EditStage(startPoint, endPoint, throughPoints, stageData.StageId, map, objRef, stageIndex, lastStage); 
        }
        else {
            if (stageData.Status === 'Active')
                stage = new ActiveStage(stageIndex, lastStage, startPoint, endPoint, throughPoints);
            else
                stage = new InactiveStage(stageIndex, lastStage, startPoint, endPoint, throughPoints, stageData.Status === 'Completed');
        }


        stage.runners = runners;
        stage.sponsor = sponsor;
        stage.status = stageData.Status;

        return stage;
    }

    ConstructThroughPoints(points) {
        return points.map(point => new Point(...point));
    }

    ConstructRunners(runners) {
        const result = runners.map(runner => {
            const { Name, Order, Status } = runner;
            return new Runner(Name, Order, Status);
        });
        return result;
    }
}