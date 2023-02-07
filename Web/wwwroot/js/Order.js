function getOrderStatus() {
    const xhr = new XMLHttpRequest()
    xhr.open('GET', 'http://localhost:5001/api/order-status', true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        console.log(response)
        showOrderStatus(response)
    }
    xhr.send()
}

function getOrderItems() {
    const xhr = new XMLHttpRequest()
    xhr.open('GET', 'http://localhost:5001/api/order', true)
    xhr.setRequestHeader('Content-Type', 'application/json, charset=UTF-8')
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText)
        loopOrderItems(response.order)
        showTotalPrice(response.totalPrice)
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


function showOrderStatus(order) {
    document.getElementById("order-id").innerHTML = "orderId " + order.orderId
    document.getElementById("paid").innerHTML = "paid " + order.paid
    document.getElementById("order-date").innerHTML = "created " + order.created

    var statusText = document.getElementById("status")
    if (order.paid == 0) { //not paid
        statusText.innerHTML = "Order is unpaid!"
        statusText.classList.add("text-danger")
    }
    else {
        statusText.innerHTML = "Order is paid!"
        statusText.classList.add("text-success")
    }
}

getOrderStatus()
getOrderItems()