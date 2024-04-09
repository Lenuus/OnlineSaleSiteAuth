"use strict";

//var init = function () {
//    $(".btnAdd").on("click", addProduct);
//    $(".btnDelete").on("click", deleteProduct);
//}

var sendNotify = function (message, callback) {
    Swal.fire({
        text: message,
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
            callback();
        }
    });
}

$(function () {
    $(".btnAdd").on("click", function () {
        var id = $(this).attr("data-id");
        var quantity = 1;
        console.log("Ürün ID'si:", id);
        sendNotify("You wanna add product to your cart?", function () {
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
                    updateCartContent();
                },
                error: function (xhr, status, error) {
                    console.error('Hata:', error);
                    alert(xhr.responseText);
                }
            });
        });
    });

    $(".btnDelete").on("click", function () {
        var id = $(this).attr("data-id");
        Swal.fire({
            text: "You wanna remove the procut from site?",
            icon: "question",
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
                $.ajax({
                    url: "/Product/Delete/" + id,
                    type: "DELETE",
                    success: function (response) {
                        $("#product_" + id).remove();
                        console.log("Başarılı", response);
                        updateOrderCounts();
                    },
                    error: function (response) {
                        console.log("Hata: ", response);
                    }
                });
            }
        });
    });

    $(document).ready(function () {
        console.log('this is run on page load');
        updateCartContent();
    });

    function updateOrderCounts() {
        $("tbody tr").each(function (index) {
            $(this).find("td:first-child").text(index + 1);
        });
    }
    $(document).ready(function () {
        $('.carousel').carousel("cycle")
        interval: 2000
    });

    function updateCartContent() {
        $.ajax({
            url: "/Basket/GetCount",
            type: "GET",
            success: function (response) {
                $("#cartCount").text(response);
            },
            error: function (response) {
                console.log("Hata: ", response);
            }
        });
    }

});

(function () {
    init();
})