var apiurl = "http://localhost:50716/api/intranet/";
var order;
//var activeBuqueteIndex = 0;
//var currentproducts;

var cname = "order";
var cvalue = "";
var exdays = 1;

$(document).ready(function () {
    getAllOrders();
})

function getAllOrders() {
    $.ajax({
        url: apiurl + "get-orders",
        type: "GET",
        success: function (result) {
            console.log(result);
            $("#body").empty();
            $("#buybag").empty();
            $("#body").append("<table id='myTable' class='table'>" +
                                "<tr style='text-align:center;'>" +
                                    "<th>" +
                                        "<label>Order Id</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Kommentar</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Totala priset</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Betalnings metod</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Leverans metod</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Leverans Adress</label>" +
                                    "</th>" +
                                    "<th>" +
                                        "<label>Skickad?</label>" +
                                    "</th>" +
                                "</tr>");
            $.each(result, function (i, order) {
                $("#myTable tbody").append("<tr>" +
                                    "<td> " +
                                        order.Id +
                                    "</td>" +
                                    "<td>" +
                                        order.Comment +
                                    "</td>" +
                                    "<td> " +
                                        order.TotalPrice +
                                    "</td> " +
                                    "<td> " +
                                        order.Payment.Method +
                                    "</td> " +
                                    "<td> " +
                                        order.Delivery.Method +
                                    "</td> " +
                                    "<td> " +
                                        order.Address.Street +
                                        "<br/>" +
                                        order.Address.PostalCode +
                                    "</td> " +
                                    "<td> " +
                                        order.IsDelivered +
                                    "</td> " +
                                    "<td> " +
                                        "<input type='button' value='Edit' onclick='getOrderToEdit(" + order.Id + ")' class='btn btn-info'/> " +
                                    "</td> " +
                                    "<td> " +
                                        "<input type='button' value='Deliver' onclick='deliverOrder(" + order.Id + ")' class='btn btn-primary' /> " +
                                    "</td> " +
                                    "<td> " +
                                        "<input type='button' value='Delete' onclick='deleteOrder(" + order.Id + ")' class='btn btn-danger' /> " +
                                    "</td>" +
                                "</tr>");
            })

            $("#body").append("</table>");
        }
    })
}

function deleteOrder(id) {
    $.ajax({
        url: apiurl + "delete-order/" + id,
        type: "DELETE",
        success: function () {
            getAllOrders();
        }
    })
}

function deliverOrder(id) {
    $.ajax({
        url: apiurl + "deliver-order/" + id,
        type: "PUT",
        success: function () {
            getAllOrders();
        }
    })
}

function getOrderToEdit(id) {
    $.ajax({
        url: apiurl + "get-order/" + id,
        type: "GET",
        success: function (result) {
            order = result;
            renderCart();
            $("#body").empty();
            $("#body").append("<div><label>OrderId: " + result.Id + "</label></div>" +
                  "<div>Comment: <input type='text' id='commentinput' value='" + result.Comment + "'/></div>" +
                  "<div id='totalPriceDiv'>TotalPrice: " + result.TotalPrice + " kr</div> " +
                  "<br/><label>AddressId: " + result.Address.Id + "</label>" +
                  "<div>Gata: <input type='text' id='streetinput' value='" + result.Address.Street + "' /><br/>PostNr: <input type='text' id='postalcodeinput' value='" + result.Address.PostalCode + "'/></div>" +
                  "<br/><div>Email: " + result.Email.Email + "</div>" +
                  "<input type='button' onclick='updateOrderToDb("+order.Id+")' class='btn btn-primary' value='Uppdatera ordern' />");
        }
    })
}

function renderCart() {
    $("#buybag").empty();

    var index = 1;
    for (var b = 0; b < order.Buquettes.length; b++) {
        $("#buybag").append("<div class='panel panel-default' id='cart-panel" + b + "'>");

        $("#cart-panel" + b + "").append("<div class='panel-heading selectable' id='header-buquette" + b + "'>");

        $("#header-buquette" + b + "").append(
                "<span>&nbsp;</span>" +
                "<h3 class='panel-title' style='float: left;'><span class='glyphicon glyphicon-tree-deciduous'></span>" + 'Bukett ' + index++ + "</h3>" +
                "<div style='float: right;'>" +
                    "<button class='btn btn-default btn-xs' type='button' onclick='decreaseBuquetteQuantity(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-minus-sign'></span>" +
                    "</button>" +
                    "<span class='badge'>" + order.Buquettes[b].Quantity + "</span>" +
                    "<button class='btn btn-default btn-xs' type='button' onclick='increaseBuquetteQuantity(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-plus-sign'></span>" +
                    "</button>" +
                    "<button value='" + b + "' class='btn btn-default btn-xs' type='button' onclick='deleteBuquetteFromOrder(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-trash'></span>" +
                    "</button>");

        $("#cart-panel" + b + "").append("<div class='panel-body panel-body-menus' id='body-buquette" + b + "'>");

        $("#body-buquette" + b + "").append("<ul class='cart-list' id='productlist-buquette" + b + "'>");

        for (var p = 0; p < order.Buquettes[b].Products.length; p++) {
            $("#productlist-buquette" + b + "").append(
                "<li>" +
                    "<span>&nbsp;</span>" +
                    "<h3 class='panel-title' style='float: left;'>" + order.Buquettes[b].Products[p].Name + "</h3>" +
                    "<div style='float: right;'>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='loweringQuantity(" + order.Buquettes[b].Products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-minus-sign'></span>" +
                        "</button>" +
                        "<span class='badge'>" + order.Buquettes[b].Products[p].Quantity + "</span>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='addQuantity(" + order.Buquettes[b].Products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-plus-sign'></span>" +
                        "</button>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='deleteProductFromBuquette(" + order.Buquettes[b].Products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-trash' ></span>" +
                        "</button>");
        }
        $("#body-buquette" + b + "").append('<h4><span class="label label-default label-left" id="buquette-price">' + order.Buquettes[b].Sum + ' kr</span></h4>');
    }
}

function addQuantity(id, b) {
    for (var i = 0; i < order.Buquettes[b].Products.length; i++) {
        if (order.Buquettes[b].Products[i].Id == id) {
            order.Buquettes[b].Products[i].Quantity++;
            order.Buquettes[b].Sum += order.Buquettes[b].Products[i].Price;
        }
    }

    calculateOrderTotalPrice();
    renderCart();
}

function loweringQuantity(id, b) {
    for (var i = 0; i < order.Buquettes[b].Products.length; i++) {
        if (order.Buquettes[b].Products[i].Id == id) {
            order.Buquettes[b].Products[i].Quantity--;
            order.Buquettes[b].Sum -= order.Buquettes[b].Products[i].Price;
        }
    }
    calculateOrderTotalPrice();
    renderCart();
}

function deleteProductFromBuquette(id, b) {
    $.each(currentproducts, function (i, product) {
        if (product.Id == id) {
            currentProduct = product;
        }

    });

    for (var i = 0; i < order.Buquettes[b].Products.length; i++) {
        if (order.Buquettes[b].Products[i].Id == id) {
            order.Buquettes[b].PriceSum -= order.Buquettes[b].Products[i].Price * order.Buquettes[b].Products[i].Quantity;
            order.Buquettes[b].Products.splice(order.Buquettes[b].Products.indexOf(order.Buquettes[b].Products[i]), 1);
        }
    }
    renderCart();
}

function decreaseBuquetteQuantity(buquetteIndex) {
    order.Buquettes[buquetteIndex].Quantity--;

    if (order.Buquettes[buquetteIndex].Quantity == 0) {
        deleteBuquetteFromOrder(buquetteIndex);
    }

    calculateOrderTotalPrice();
    renderCart();
}

function increaseBuquetteQuantity(buquetteIndex) {
    order.Buquettes[buquetteIndex].Quantity++;

    renderCart();
    calculateOrderTotalPrice();
}

function deleteBuquetteFromOrder(buquetteIndex) {
    order.Buquettes.splice(order.Buquettes.indexOf(order.Buquettes[buquetteIndex]), 1);

    renderCart();
    calculateOrderTotalPrice();
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));

    var expires = "expires=" + d.toUTCString();

    $.cookie(cname, cvalue, { path: "/" });
}

function getCookie() {
    if ($.cookie(cname) == null) {
        createNewBouqett();
    }
    else {
        order = JSON.parse($.cookie(cname));
    }
}

function calculateOrderTotalPrice() {
    var totalPrice = 0;

    for (var b = 0; b < order.Buquettes.length; b++) {
        totalPrice += order.Buquettes[b].Sum * order.Buquettes[b].Quantity;
    }

    order.TotalPrice = totalPrice;
    $("#totalPriceDiv").html("TotalPrice: " + totalPrice + " Kr");
}

function updateOrderToDb(id) {

    order.Comment = $("#commentinput").val();
    order.Address.Street = $("#streetinput").val();
    order.Address.PostalCode = $("#postalcodeinput").val();

    $.ajax({
        url: "http://localhost:50716/api/intranet/edit-order/"+id,
        data: order,
        method: "PUT",
        success: function (result) {
            getAllOrders();
        },
    });
}
