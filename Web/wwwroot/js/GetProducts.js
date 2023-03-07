const xhr = new XMLHttpRequest()
xhr.open('GET', 'https://woizservice.xyz/api/api/products', true)
xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
xhr.onload = function () {
    var response = JSON.parse(xhr.responseText)
    showProducts(response)
}
xhr.send()

function showProducts(response) {
    for (i = 0; i < response.length; i++) {
        document.getElementById("products").insertAdjacentHTML('beforebegin', '<tr>' +
            '<td>' + response[i].name + '</td>' +
            '<td><button id="' + response[i].id + '" class="btn btn-success">View</button></td > ' +
            '</tr>')
        viewButton(response[i].id)
    }
}

function viewButton(id) {
    document.getElementById(id).onclick = function () { window.location.href = "/product-page.html?id=" + id }
}