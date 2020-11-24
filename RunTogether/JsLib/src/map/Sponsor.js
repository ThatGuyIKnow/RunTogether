
export class Sponsor {
    name;
    message;
    pictureUrl;

    constructor(name, message, pictureUrl = null) {
        if (typeof name !== "string" ||
            typeof message !== "string" ||
            (pictureUrl !== null && typeof pictureUrl !== "string"))
            throw new TypeError("Class 'Sponsor' constructor has parameters '<name : string, message : string, pictureUrl : ?string>'");

        this.name = name;
        this.message = message;
        this.pictureUrl = pictureUrl;
    }

}