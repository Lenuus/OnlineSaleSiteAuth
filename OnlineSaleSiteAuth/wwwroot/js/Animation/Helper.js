export function randomInteger(a, b) {
    return a + Math.floor((b - a) * Math.random());
}

export function distance([x, y], [u, v]) {
    return Math.sqrt(Math.pow(x - u, 2) + Math.pow(y - v, 2));
}

export function randomColor() {
    const r = randomInteger(50, 255);
    const g = randomInteger(0, 50);
    const b = randomInteger(50, 255);
    return `rgb(${r},${g},${b})`;
}