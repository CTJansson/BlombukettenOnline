var apiUrl = "http://localhost:50716/api/datebase/";


var cname = "order";
var cvalue = "";
var exdays = 1;

var order;
var currentproducts;
var newBuquete;

var activeBuqueteIndex = 0;

$(document).ready(function () {
    getAllCategories();
    getAllColors();
    getAllProducts();
    
    order = new Object();
    order.address = new Object();
    order.paymentMethod = 1;
    order.deliveryMethod = 1;
    order.storeId = 1;
    order.address.name = "-";
    order.address.street = "-";
    order.address.postalcode = "-";
    order.subscription = true;
    order.comment = "";
    order.TotalSum = 0;
    order.buquettes = [];
    
    getCookie();
    
    activeBuqueteIndex = order.buquettes.length - 1;
    
    calculateOrderTotalPrice();
    renderCart();
});


function getAllCategories() {

    $.ajax({
        url: apiUrl + "getcategories",
        type: "GET",
        success: function (categories) {
            $("#mycategories").empty();
            console.log(categories);

            //$("#mycategories").append("<a href='#' class='filter-options list-group-item active' onclick='getAllProducts()'>" + 'Alla' + "</a>")  //onclick='getAllProductsInSelectedCategories(" + category.Id + ")'

            $.each(categories, function (i, category) {

                if (category.ParentId == null) {
                    $("#mycategories").append("<a href='#' id='cat" + category.Id + "' class='filter-option list-group-item' onclick='getAllProductsInSelectedCategories(" + category.Id + ")' >" + category.Name + "</a>");
                }
                else {
                    $("#mycategories").append("<a href='#' id='cat" + category.Id + "' class='filter-option list-group-item' onclick='getAllProductsInSelectedCategories(" + category.Id + ")'>" + '&nbsp; &nbsp;' + category.Name + "</a>");
                }               
            })
        }
    });
}

function getAllColors() {
    $.ajax({
        url: apiUrl + "getcolors",
        type: "GET",
        success: function (colors) {
            $("#mycolors").empty();
            console.log(colors);

            //$("#mycolors").append("<a href='#' class='filter-option list-group-item active'>" + 'Alla' + "</a>");
            $.each(colors, function (i, color) {
                $("#mycolors").append("<a href='#' class='filter-option list-group-item' onclick='getAllProductsInSelectedColors(" + color.Id + ")'>" + color.Name + "</a>");
            })
        }
    });
}

function getAllProducts() {
    $.ajax({
        url: apiUrl,
        type: "GET",
        success: function (result) {
            $("#myproducts").empty();
            currentproducts = result;

            $.each(result, function (i, product) {
                //$.each(product.Categories, function (ip, productPrice){
                    $("#myproducts").append("<li class='products-list col-lg-4 col-md-4 col-sm-6 col-xs-6'>" +
                        '<div class="thumbnail">' +
                        '<img class="img-responsive img-circle img-thumbnail" src="/Content/images/RosRod.jpg" alt="Responsive image" />' +
                        '<div class="caption">' + '<h4 id="product-name">' + product.Name + '</h4>' +
                        '<p id="product-description">' + product.Description +
                        '</p>' +
                        '<h3>' +
                        '<span class="label label-default" id="product-price">' + product.Price + ' kr</span>' +
                        '<small>' +
                        '&nbsp;&nbsp;' +
                        '<button type="button" class="btn btn-primary btn-sm" onclick="addProductToBuquete(' + product.Id + ')">' +
                        '<span class="glyphicon glyphicon-plus-sign" ></span>' +
                        ' Lägg till' +
                        '</button>' +
                        '</small>' +
                        '</h3>' +
                        '</div>' +
                        '</div>' +
                        "</li>");
                
            })       
        }
    });
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    
    var expires = "expires=" + d.toUTCString();

    //document.cookie = cname + "=" + cvalue + "; " + expires + ";" + { path: '/' };
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

function getProductFromCookie(object) {
    $.ajax({
        url: apiUrl,
        type: "GET",
        success: function (data) {
            console.log("success")
        }
    });
}

var categoriesIds = [];
var colorsIds = [];
function getAllProductsInSelectedCategories(id) {
    if (categoriesIds != null) {
        var index = $.inArray(id, categoriesIds)
        if (index >= 0) {
            categoriesIds.splice(index, 1);
        }
        else {
            categoriesIds.push(id);
        }
    }
}

function getAllProductsInSelectedColors(id) {
    if (colorsIds != null) {
        var index = $.inArray(id, colorsIds)
        if (index >= 0) {
            colorsIds.splice(index, 1);
        }
        else {
            colorsIds.push(id);
        }
    }
}

function getAllProductsInSelectedCatAndCol() {
    var id = [];
    var cid = [];
        
    for (var i = 0; i < categoriesIds.length; i++) {
        id[i] = categoriesIds[i].toString();
    }
    var catIds = id.toString();

    for (var i = 0; i < colorsIds.length; i++) {
        cid[i] = colorsIds[i].toString();
    }
    var colIds = cid.toString();

    $.ajax({
        url: apiUrl + "getchosenproducts/" + catIds + "/" + colIds,
        type: "GET",
        success: function (products) {
            $("#myproducts").empty();

            $.each(products, function (i, product) {
                //$.each(product.Categories, function (ip, productPrice) {
                    $("#myproducts").append(
                        "<li class='products-list col-lg-4 col-md-4 col-sm-6 col-xs-6'>" +
                        '<div class="thumbnail">' +
                        '<img class="img-responsive img-circle img-thumbnail" src="/Content/images/RosRod.jpg" alt="Responsive image" />' +
                        '<div class="caption">' +
                        '<h4 id="product-name">' + product.Name + '</h4>' +
                        '<p id="product-description">' + product.Description + '</p>' +
                        '<h3>' +
                            '<span class="label label-default" id="product-price">' + product.Price + ' kr</span>' +
                            '<small>' +
                                '<button type="button" class="btn btn-primary btn-sm" onclick="addProductToBuquete(' + product.Id + ')">' +
                                    '<span class="glyphicon glyphicon-plus-sign" ></span>' + ' Lägg till' +
                                '</button>' +
                            '</small>' +
                        '</h3>' +
                        '</div>' +
                        '</div>' +
                        "</li>");
                
            })
        }
    });
}

function createNewBouqett() {
    var buquette = new Object();
    buquette.quantity = 1;
    buquette.PriceSum = 0;
    buquette.products = [];

    order.buquettes.push(buquette);

    getActiveBuqueteIndex(order.buquettes.length - 1);
}
     

function getActiveBuqueteIndex(b) {
    activeBuqueteIndex = b;
    renderCart();
}

function addProductToBuquete(id) {
    $.each(currentproducts, function (i, product) {
        //$.each(product.Categories, function (ip, productPrice) {
            if (product.Id == id) {
                currentProduct = product;
                currentProduct.Price = product.Price;
            }
        
    });

    var skipped = 0;
    for (var i = 0; i < order.buquettes[activeBuqueteIndex].products.length; i++) {
        if (order.buquettes[activeBuqueteIndex].products[i].Id == currentProduct.Id) {
            order.buquettes[activeBuqueteIndex].products[i].Quantity++;
            order.buquettes[activeBuqueteIndex].PriceSum += currentProduct.Price;
        }
        else {
            skipped++;
        }
    }

    if (skipped == order.buquettes[activeBuqueteIndex].products.length) {
        order.buquettes[activeBuqueteIndex].products.push(new Object({ Id: currentProduct.Id, Name: currentProduct.Name, Price: currentProduct.Price, Quantity: 1 }));
        order.buquettes[activeBuqueteIndex].PriceSum += currentProduct.Price;
    }
    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function addQuantity(id, b) {
    $.each(currentproducts, function (i, product) {
        //$.each(product.Categories, function (ip, productPrice) {
            if (product.Id == id) {
                currentProduct = product;
            }
        
    });

    for (var i = 0; i < order.buquettes[b].products.length; i++) {
        if (order.buquettes[b].products[i].Id == currentProduct.Id) {
            order.buquettes[b].products[i].Quantity++;
            order.buquettes[b].PriceSum += currentProduct.Price;
        }
    }
    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function loweringQuantity(id, b) {
    $.each(currentproducts, function (i, product) {
        //$.each(product.Categories, function (ip, productPrice) {
            if (product.Id == id) {
                currentProduct = product;
            }
        
    });

    for (var i = 0; i < order.buquettes[b].products.length; i++) {
        if (order.buquettes[b].products[i].Id == currentProduct.Id) {
            order.buquettes[b].products[i].Quantity--;
            order.buquettes[b].PriceSum -= currentProduct.Price;

            if (order.buquettes[b].products[i].Quantity == 0){
                order.buquettes[b].products.splice(order.buquettes[b].products.indexOf(order.buquettes[b].products[i]), 1);
            }
        }
    }
    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function deleteProductFromBuquette(id, b) {
    $.each(currentproducts, function (i, product) {
        //$.each(product.Categories, function (ip, productPrice) {
            if (product.Id == id) {
                currentProduct = product;
            }
        
    });

    for (var i = 0; i < order.buquettes[b].products.length; i++) {
        if (order.buquettes[b].products[i].Id == currentProduct.Id) {
            order.buquettes[b].PriceSum -= order.buquettes[b].products[i].Price * order.buquettes[b].products[i].Quantity;
            order.buquettes[b].products.splice(order.buquettes[b].products.indexOf(order.buquettes[b].products[i]), 1);
        }
    }
    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function decreaseBuquetteQuantity(buquetteIndex) {
    order.buquettes[buquetteIndex].quantity--;

    if (order.buquettes[buquetteIndex].quantity == 0) {
        deleteBuquetteFromOrder(buquetteIndex);
    }

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);

    calculateOrderTotalPrice()
    renderCart();
}

function increaseBuquetteQuantity(buquetteIndex) {
    order.buquettes[buquetteIndex].quantity++;

    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function deleteBuquetteFromOrder(buquetteIndex) {
    order.buquettes.splice(order.buquettes.indexOf(order.buquettes[buquetteIndex]), 1);
    activeBuqueteIndex = order.buquettes.length - 1;

    calculateOrderTotalPrice()
    renderCart();

    cvalue = JSON.stringify(order);
    setCookie(cname, cvalue, exdays);
}

function calculateOrderTotalPrice() {
    var totalPrice = 0;

    for (var b = 0; b < order.buquettes.length; b++) {
        totalPrice += order.buquettes[b].PriceSum * order.buquettes[b].quantity;
    }

    

    order.TotalSum = totalPrice;
    $("#total-order-sum").html(totalPrice + " kr");
    $("#total-sum").html(totalPrice + " kr");
}

function createOrder() {

    

    if (order.deliveryMethod > 1) {
        order.address.name = $('#nameinput').val();
        order.address.street = $('#streetinput').val();
        order.address.postalcode = $('#zipinput').val();
    }
    

    order.email = new Object();
    order.email.email = $('#emailinput').val();
    order.email.subscribe = $("input[name='subscribe']:checked").length ? true : false;

    order.comment = $('#commentinput').val();
    //order.email = $('#emailinput').val();
    //order.subscription = $("input[name='subscribe']:checked").length ? true : false;
    
    $.ajax({
        url: "http://localhost:50716/api/order/create-order/",
        type: "POST",
        data: order,
        success: function (data) {
            console.log("success")
        }
    });
}

function renderCart() {
    $("#buybag").empty();

    var index = 1;
    for (var b = 0; b < order.buquettes.length; b++) {
        $("#buybag").append("<div class='panel panel-default' id='cart-panel" + b + "'>");

        if (b == activeBuqueteIndex) {
            $("#cart-panel" + b + "").append("<div class='panel-heading active-buquette' id='header-buquette" + b + "'>");
        } else {
            $("#cart-panel" + b + "").append("<div class='panel-heading selectable' onclick='getActiveBuqueteIndex(" + b + ")' id='header-buquette" + b + "'>");
        }

        $("#header-buquette" + b + "").append(
                "<span>&nbsp;</span>" +
                "<h3 class='panel-title' style='float: left;'><span class='glyphicon glyphicon-tree-deciduous'></span>" + 'Bukett ' + index++ + "</h3>" +
                "<div style='float: right;'>" +
                    "<button class='btn btn-default btn-xs' type='button' onclick='decreaseBuquetteQuantity(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-minus-sign'></span>" +
                    "</button>" +
                    "<span class='badge'>" + order.buquettes[b].quantity + "</span>" +
                    "<button class='btn btn-default btn-xs' type='button' onclick='increaseBuquetteQuantity(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-plus-sign'></span>" +
                    "</button>" +
                    "<button value='" + b + "' class='btn btn-default btn-xs' type='button' onclick='deleteBuquetteFromOrder(" + b + ")'>" +
                        "<span class='glyphicon glyphicon-trash'></span>" +
                    "</button>");

        $("#cart-panel" + b + "").append("<div class='panel-body panel-body-menus' id='body-buquette" + b + "'>");

        $("#body-buquette" + b + "").append("<ul class='cart-list' id='productlist-buquette" + b + "'>");

        for (var p = 0; p < order.buquettes[b].products.length; p++) {
            $("#productlist-buquette" + b + "").append(
                "<li>" +
                    "<span>&nbsp;</span>" +
                    "<h3 class='panel-title' style='float: left;'>" + order.buquettes[b].products[p].Name + "</h3>" +
                    "<div style='float: right;'>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='loweringQuantity(" + order.buquettes[b].products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-minus-sign'></span>" +
                        "</button>" +
                        "<span class='badge'>" + order.buquettes[b].products[p].Quantity + "</span>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='addQuantity(" + order.buquettes[b].products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-plus-sign'></span>" +
                        "</button>" +
                        "<button class='btn btn-default btn-xs' type='button' onclick='deleteProductFromBuquette(" + order.buquettes[b].products[p].Id + ',' + b + ")'>" +
                            "<span class='glyphicon glyphicon-trash' ></span>" +
                        "</button>");
        }
        $("#body-buquette" + b + "").append('<h4><span class="label label-default label-left" id="buquette-price">' + order.buquettes[b].PriceSum + ' kr</span></h4>');
    }
}
