const urlParams = new URLSearchParams(window.location.search)
const id = urlParams.get('id') || "00000000-0000-0000-0000-000000000000"
const xhr = new XMLHttpRequest()
xhr.open('GET', 'http://localhost:5001/api/product-info/' + id, true)
xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
xhr.onload = function () {
    var response = JSON.parse(xhr.responseText)
    showProduct(response)
}
xhr.send()

function showProduct(response) {
    if (!response.Code) { // 404
        document.title = response.Name
        document.getElementById("product-name").innerHTML = response.Name
        document.getElementById("product-price").innerHTML = response.Price + " SEK"
        document.getElementById("product-quantity").innerHTML = response.Quantity + " in stock!"
    }
    else {
        window.location.href = '/404.html'
    }
}