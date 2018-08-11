
var socket

var room_params = 4

var input_elements


function parseUpdates(event) {
	if (event.data.substring(0, 4) === "name") {
		var params = event.data.split(/[,=]/)
		for (var room = 0; room < params.length; room += room_params * 2) {
			var room_list_div = document.getElementById("room_list")

			var room_div = document.createElement("div")
			room_div.className = "room"

			var room_name = document.createElement("div")
			room_name.className = "text"
			room_name.textContent = params[room + 1]

			var room_size = document.createElement("div")
			room_size.className = "text"
			room_size.textContent = "Size: " + params[room + 3]

			var room_fullness = document.createElement("div")
			room_fullness.className = "text"
			room_fullness.textContent = "Players: " + params[room + 5] + "/" + params[room + 7]
		}
	}
	else {
		window.location.href = "room" + event.data
	}
}


window.onload = function open() {
	socket = new WebSocket("ws://" + window.location.hostname)
	socket.onmessage = parseUpdates

	input_elements = document.getElementsByTagName("input")
	for (var i = 0; i < input_elements.length; ++i) {
		input_elements[i].onchange = checkButton
	}

	checkButton()
}


function checkButton() {
	var button_element = document.getElementsByName("create_room")[0]
	button_element.disabled = false;
	for (var i = 0; i < input_elements.length; ++i) {
		if (input_elements[i].value <= 0)
			button_element.disabled = true;
	}
}


function createRoom() {
	socket.send("create;size=" + input_elements[0].value + ";players=" + input_elements[1].value + ";planets=" + input_elements[2].value + ";");
}
