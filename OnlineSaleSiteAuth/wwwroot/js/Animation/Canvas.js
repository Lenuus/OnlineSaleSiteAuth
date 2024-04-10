import { randomInteger } from "./helper.js";

export const canvas = document.getElementById("canvas");
export const ctx = canvas.getContext("2d");

function makeCanvasFullscreen() {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
}

makeCanvasFullscreen();

window.addEventListener("resize", makeCanvasFullscreen);

export function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

export function randomPoint() {
    return [
        randomInteger(0, canvas.width),
        randomInteger(0, canvas.height),
    ];
}