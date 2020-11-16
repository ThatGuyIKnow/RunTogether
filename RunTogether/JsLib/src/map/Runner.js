
export class Runner {
    name;
    order;
    status;

    constructor(name, order, status) {
        if (typeof name !== "string" ||
            typeof order !== "number" ||
            typeof status !== "string")
            throw new TypeError("Class 'Runner' constructor has parameters '<name : string, order : number, status : string>'");

        this.name = name;
        this.order = order;
        this.status = status;
    }
}
