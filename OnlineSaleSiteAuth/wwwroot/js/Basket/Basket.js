"use strict";

var init = function () {
    $(".btnDecrease").on("click", decreaseProduct);
    $(".btnAdd").on("click", addProduct);
    $("#completeShopping").on("click", completeShopping);
};

var decreaseProduct = function () {
    var id = $(this).data("id");
    var quantity = 1;
    console.log("Ürün ID'si:", id);

    Swal.fire({
        text: "Are you sure you want to decrease the quantity?",
        icon: "warning",
        buttonsStyling: false,
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger"
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var product = {
                ProductId: id,
                Quantity: quantity,
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(product),
                url: "/Basket/Delete",
                contentType: "application/json",
                success: function (response) {
                    console.log("Başarılı", response);
                    refreshBasketList();
                },
                error: function (response) {
                    console.log("Hata: ", response);
                }
            });
        }
    });
};

var addProduct = function () {
    var id = $(this).attr("data-id");
    var quantity = 1;
    Swal.fire({
        text: "Do you wanna ad another one?",
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Yes",
        showCancelButton: true,
        cancelButtonText: "No",
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger"
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var product = {
                ProductId: id,
                Quantity: quantity,
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(product),
                url: "/Basket/Create",
                contentType: "application/json",
                success: function (response) {
                    console.log("Başarılı", response);
                    refreshBasketList();
                },
                error: function (response) {
                    console.log("Hata: ", response);
                }
            });
        }

    });
}


function refreshBasketList() {
    $.ajax({
        url: "/Basket/Index",
        type: "GET",
        success: function (response) {
            $("body").html(response);
        },
        error: function (response) {
            console.log("Hata: ", response);
        }
    });
}

var completeShopping = function () {
    if (confirm("Alışverişi tamamlamak istediğinize emin misiniz?")) {
        $.ajax({
            url: "/Basket/Clear",
            type: "DELETE",
            success: function (response) {
                console.log("Alışveriş tamamlandı.", response);
                refreshBasketList();
                postActivatedCoupons();
            },
            error: function (response) {
                console.log("Hata: ", response);
            }
        });
    }
}

function activateCoupon() {
    var couponCode = document.getElementById("couponInput").value;
    console.log("couponCode:", couponCode);
    $.ajax({
        type: "POST",
        data: JSON.stringify(couponCode),
        url: "/Coupon/ApplyCoupon",
        contentType: "application/json",
        success: function (mappedCoupon) {
            console.log("Success:", mappedCoupon);
            var totalPriceElement = document.getElementById('totalPrice');
            totalPriceElement.textContent = mappedCoupon.discountedPrice;
            document.getElementById("couponInput").value = "";

            var activatedCoupons = JSON.parse(localStorage.getItem('activatedCoupons')) || [];
            activatedCoupons.push(couponCode);
            localStorage.setItem('activatedCoupons', JSON.stringify(activatedCoupons));
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            alert(xhr.responseText);
        }
    });
}

function postActivatedCoupons() {
    var activatedCoupons = JSON.parse(localStorage.getItem('activatedCoupons')) || [];
    $.ajax({
        type: "POST",
        data: JSON.stringify(activatedCoupons),
        url: "/Coupon/ActivateCoupons",
        contentType: "application/json",
        success: function (response) {
            console.log("Kupon kodları başarıyla Controller'a gönderildi.");
            localStorage.removeItem('activatedCoupons');
        },
        error: function (error) {
            console.error('Hata:', error);
        }
    });
}

$(function () {
    init();
});