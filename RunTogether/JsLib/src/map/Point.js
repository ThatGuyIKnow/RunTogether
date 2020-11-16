export class Point {
    constructor(x, y) {
        if (typeof x !== "number" || typeof y !== "number")
            throw new TypeError("Class 'Point' constructor has parameters '<x : number, y : number>'");

        this.x = x;
        this.y = y;
    }

    toArray() { return [this.x, this.y] }
}