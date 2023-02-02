const xhr = new XMLHttpRequest()
xhr.open('GET', 'http://localhost:5001/api/products', true)
xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
xhr.onload = function () {
    var response = JSON.parse(xhr.responseText)
    showProducts(response)
}
xhr.send()

function showProducts(response) {
    for (i = 0; i < response.length; i++) {
        document.write(response[i].Name)
        document.write("<br>")
        document.write("<div>")
        document.write("<button onclick=goToPage('" + response[i].Id + "')>View </button>")
        document.write("</div>")
    }
}

function goToPage(id) {
    window.location.href = "http://localhost:5005/product-page.html?id=" + id
}