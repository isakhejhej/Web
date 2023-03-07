window.onload = function () {
    console.log("LOADED")
    pageLoaded()
}

var productId

function pageLoaded() {
    const urlParams = new URLSearchParams(window.location.search)
    const id = urlParams.get('id') || "00000000-0000-0000-0000-000000000000"
    const xhr = new XMLHttpRequest()
    xhr.open('GET', 'https://woizservice.xyz/api/api/product-info/' + id, true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        productId = response.id
        showProduct(response)
    }
    xhr.send()
}

function showProduct(response) {
    if (!response.code) { // 404
        console.log(response)
        document.title = response.name
        document.getElementById("product-name").innerHTML = response.name
        document.getElementById("product-price").innerHTML = response.price + " SEK"
        document.getElementById("product-quantity").innerHTML = response.quantity + " in stock!"
        document.getElementById("add-to-cart").onclick = addProductToCart;
    }
    else {
        window.location.href = '/404.html'
    }
}

function addProductToCart() {
    const xhr = new XMLHttpRequest()
    xhr.open('POST', 'https://woizservice.xyz/api/api/add-to-cart/' + productId, true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        console.log(response)
        if (response) {
            if (response.code == 200) {
                document.getElementById("cart-result").innerHTML = "Added to cart"
                window.location.reload()
            }
        }
    }
    xhr.send(JSON.stringify({ "quantity": 1 }))
}