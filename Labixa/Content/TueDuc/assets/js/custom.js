function addToCart(prodId) {
    var count = document.getElementById('countCart').innerHTML;
    console.log("get value count: " + count);
    var countValue = parseInt(count);
    console.log("count cart: " + countValue);
    console.log("prodId: " + prodId);
    var quantity = 1;
    $.ajax({
        type: "POST",
        url: "/Products/AddToCart",
        data: { ProductId: prodId, Quantity: quantity },
        success: function (result) {
            console.log(result);
            count++;
            console.log("count cart: " + count);
            document.getElementById("countCart").innerHTML = count;
        }
    });
};