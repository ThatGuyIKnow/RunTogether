import { isClassOrSubclass } from '../utils/ClassTypeUtils';
import { ActiveStage, InactiveStage, EditStage } from './Stage';
import {Point} from './Point';
import {Runner} from './Runner';
import {Sponsor} from './Sponsor';

export class StageFactory {
    constructor() { }

    CreateStage(stageData, flipped = false, editStage = false, map = null, objRef = null, stageIndex, lastStage) {
        const startPoint = new Point(...stageData.StartPoint);
        const endPoint = new Point(...stageData.EndPoint);
        const throughPoints = this.ConstructThroughPoints(stageData.ThroughPoints);
        let runners = null;
        if(Array.isArray(stageData.Runners) && stageData.Runners.length > 0) 
            runners = this.ConstructRunners(stageData.Runners);

    let sponsor = null;
    if (stageData.Sponsor !== undefined)
        sponsor = new Sponsor(stageData.Sponsor.Name,
            stageData.Sponsor.Message,
            stageData.Sponsor.PictureURL);

        console.log(stageData.Sponsor);

        let stage;

        if (editStage == true) {
            stage = new EditStage(startPoint, endPoint, throughPoints, flipped, stageData.StageId, map, objRef, stageIndex, lastStage); 
        }
        else {
            if (stageData.Status === 'Active')
                stage = new ActiveStage(stageIndex, lastStage, startPoint, endPoint, throughPoints, flipped);
            else
                stage = new InactiveStage(stageIndex, lastStage, startPoint, endPoint, throughPoints, flipped, stageData.Status === 'Completed');
        }


        stage.runners = runners;
        stage.sponsor = sponsor;
        stage.status = stageData.Status;
       
        return stage;
    }

    ConstructThroughPoints(points) {
        const result = [];
        points.forEach(point => {
            result.push(new Point(...point));
        });
        return result;
    }

    ConstructRunners(runners) {
        const result = [];
        runners.forEach(runner => {
            const { Name, Order, Status } = runner;
            result.push(new Runner(Name, Order, Status));
        });
        return result;
    }
}