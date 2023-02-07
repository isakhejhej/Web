function getOrderItems() {
    const xhr = new XMLHttpRequest()
    xhr.open('GET', 'http://localhost:5001/api/order', true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        loopOrderItems(response.order)
        showTotalPrice(response.totalPrice)
        document.getElementById("loading").remove()
    }
    xhr.send()
}

function loopOrderItems(items) {
    for (i = 0; i < items.length; i++) {
        insertOrderItem(items[i])
    }
}

function insertOrderItem(item) {
    console.log(item)
    document.getElementById("products").insertAdjacentHTML('beforebegin', '<tr>' +
        '<td>' + item.name + '</td>' +
        '<td>' + item.price + '</td>' +
        '<td>' + item.quantity + '</td>' +
        '</tr>')
}

function showTotalPrice(price) {
    console.log(price)
    document.getElementById("total-price").innerHTML = "Total Price: " + String(price)
} 

function checkoutButton() {
    document.getElementById("checkout-button").onclick = proceedCheckout;
}

function proceedCheckout() {
    document.getElementById("checkout-button").classList.add("disabled")
    createNewOrder()
}

function createNewOrder() {
    const xhr = new XMLHttpRequest()
    xhr.open('POST', 'http://localhost:5001/api/new-order', true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        if (response.orderId) {
            window.location.href = "/order.html"
        }
        else if (response.code) {
            alreadyHasOrderRedirect()
        }
    }
    xhr.send()
}

function alreadyHasOrderRedirect() {
    const xhr = new XMLHttpRequest()
    xhr.open('GET', 'http://localhost:5001/api/order-status', true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        window.location.href = "/order.html"
    }
    xhr.send()
}

getOrderItems()
checkoutButton()