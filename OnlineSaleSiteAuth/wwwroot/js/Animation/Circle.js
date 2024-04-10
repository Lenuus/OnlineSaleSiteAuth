import { ctx, randomPoint } from "./canvas.js";
import { distance, randomColor } from "./helper.js";

export class Circle {
    static list = [];
    static GROW_SPEED = 0.1;
    static MAX_NUMBER = 100000;

    static updateCircles() {
        Circle.list.forEach((c) => c.update());
    }

    static drawCircles() {
        Circle.list.forEach((c) => c.draw());
    }

    static createCircle() {
        if (Circle.list.length < Circle.MAX_NUMBER) {
            let point = randomPoint();
            let tries = 0;
            while (
                Circle.list.some((circle) => circle.contains(point))
            ) {
                tries++;
                if (tries == 1000) {
                    return;
                }
                point = randomPoint();
            }
            new Circle(point, 1);
        }
    }

    static shrinkAfterTouch() {
        for (let i = 0; i < Circle.list.length; i++) {
            for (let j = i + 1; j < Circle.list.length; j++) {
                const circle1 = Circle.list[i];
                const circle2 = Circle.list[j];
                if (circle1.intersects(circle2)) {
                    circle1.growingSpeed = -Circle.GROW_SPEED;
                    circle2.growingSpeed = -Circle.GROW_SPEED;
                }
            }
        }
    }

    constructor(center, radius) {
        this.center = center;
        this.radius = radius;
        this.color = randomColor();
        this.growingSpeed = Circle.GROW_SPEED;
        Circle.list.push(this);
    }

    update() {
        this.radius += this.growingSpeed;
        if (this.radius <= 1) {
            this.radius = 1;
            this.growingSpeed = Circle.GROW_SPEED;
        }
    }

    draw() {
        ctx.globalAlpha = Math.min(1, this.radius / 10);
        ctx.fillStyle = this.color;
        ctx.beginPath();
        ctx.arc(...this.center, this.radius, 0, 2 * Math.PI, false);
        ctx.fill();
    }

    contains(point) {
        return distance(this.center, point) <= this.radius;
    }

    intersects(otherCircle) {
        return (
            distance(this.center, otherCircle.center) <=
            this.radius + otherCircle.radius
        );
    }
}