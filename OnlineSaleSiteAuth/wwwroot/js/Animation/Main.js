import { clearCanvas } from "./canvas.js";
import { Circle } from "./Circle.js";

function loop() {
    clearCanvas();
    Circle.createCircle();
    Circle.updateCircles();
    Circle.shrinkAfterTouch();
    Circle.drawCircles();
    requestAnimationFrame(loop);

}

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM tamamlandı. Sayfa yüklendi.");

    // Ürün kartlarının bulunduğu bölümün referansını alın
    const productCardsContainer = document.getElementById("productCards");

    // Ürün kartlarının arka planında animasyon oluşturmak için her birine canvas ekleyin
    const productCards = productCardsContainer.querySelectorAll(".col-md-3");
    productCards.forEach((card) => {
        const canvas = document.createElement("canvas");
        canvas.classList.add("productCanvas"); // İsteğe bağlı olarak bir CSS sınıfı ekleyebilirsiniz
        card.appendChild(canvas);
    });

    loop(); // Animasyon döngüsünü başlatın
});




loop();