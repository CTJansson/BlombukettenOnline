$(document).ready(function () {
    //getAllProducts();
    //GetAllProducts();
})

var apiUrl = "http://localhost:50716/api/datebase/";
var apiIntraNetUrl = "http://localhost:50716/api/intranet/";

 function getAllProducts() {
     var trHTML = '';
    $.ajax({
        url: apiUrl,
        dateType: "json",
        type: "GET",
        success:function (result) {

            console.log(result);

            $.each(result, function (i, product) {
                $.each(product.Categories, function (ip, productCategory) {
                    //trHTML += '<tr><td class="tdcolor">' + product.Name + '</td><td>' + product.Description + '</td><td>' + product.Color.Name + '</td><td>' + productCategory.Name + '</td><td>' + productCategory.Price + '</td><td><a href="#" onclick="editProduct(' + product.Id + ')">' + 'Edit' + '</a></td></tr>';
                    $("#productsTbody").append('<tr><td class="tdcolor">' + product.Name + '</td><td>' + product.Description + '</td><td>' + product.Color.Name + '</td><td>' + productCategory.Name + '</td><td>' + productCategory.Price + '</td><td><a href="#" id="' + product.Id + '" onclick="editProduct(' + product.Id + ')">' + 'Edit' + ' | </a>' + '<a href="#" onclick="deleteProduct(' + product.Id + ')">' + 'Delete' + ' |</a> ' + '<a href=# onclick="getdetails(' + product.Id + ')">' + 'Details' + '</a>' +  '</td></tr>');
                })
            })
        }
    });
}

function createProduct() {
    var boxName = $("#boxName").val();
    var boxCol = $("#boxCol").val();
    var boxDesc = $("#boxDesc").val();
    var boxCat = $("#boxCat").val();

    var product = {
        Name: boxName,
        Color: boxCol,
        Description: boxDesc,
        Categories: boxCat,
    };

    $.ajax({
        url: apiIntranetUrl + "create-product/",
        type: "POST",
        data: JSON.stringify(product),
        contentType: "application/json",
        error: function (error) {
            console.log(JSON.stringify(product));
        }
    }).success(function () {
        console.log("YES!");
    });
};

function getProductById() {

    var selectedId = $("#idInput").val();

    $.ajax({
        url: apiDatebaseUrl + selectedId,
        dateType: "json",
    }).success(function (result) {
        $("#product").html(result.Id + " - " + result.Name);
    });
};

function editProduct(id) {
    console.log(id)
    $.ajax({
        url: apiUrl + id,
        dateType: "json",
        type: "GET",
    }).success(function (product) {
        $("#product").append("Namn: <input type='text' value='"+ product.Name +"' id='nameInputBox'/>" +
                           "<div> Färg <input type='' value='" + product.Color.Name + "' id='colorInputbox'/></div>" +
                           "<div> Beskrivning <input type='' value='" + product.Description + "' id='descInputBox'/></div>" +
                           "<div> Kategorier <input type='' value='" + product.Categories.Name + "' id='categoryInputBox'/></div>"
                            + "<div> <input type='submit' value='Uppdatera' onclick='updateProductInDatabase("+ id +")'/></div>");
                           
    });
}

function updateProductInDatabase(id) {
    var nameInput = $("#nameInputBox").val();
    var colorInput = $("#colorInputBox").val();
    var descInput = $("#descInputBox").val();

    var product = {
        Name: nameInput,
        Color: colorInput,
        Description: descInput,
    };

    $.ajax({
        url: apiIntraNetUrl + "edit-product/" + id,
        type: "PUT",
        dataType: "json",
        data: JSON.stringify(product),
        error: function (error) {
            console.log(error + " " + JSON.stringify(product));
        }
    }).success(function () {
        $("#product").html("<p>Product successfully updated!</p>");
    });
}


