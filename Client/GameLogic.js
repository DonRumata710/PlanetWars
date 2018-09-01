
var socket

var room_params = 4

var middle
var room_list_div
var input_elements


var game_data


function loadGameMap() {
	var client = new XMLHttpRequest();
	client.open('GET', '/html/GamePanel.html');
	client.onreadystatechange = function () {
		middle.innerHTML = client.responseText
		socket.send("getmap")
	}
	client.send();
}


function generatePlanetImage() {
	return "planetX.png"
}



function showInfo(element) {
	if (element.target.className === "cell") {
	
	}
}


function updateRoomList (data) {
	room_list_div.innerHTML = ""

	if (data.length === 0)
		return

	var rooms_desc = data.split(/[;]/)
	for (var i = 0; i < rooms_desc.length; i += 1) {

		if (rooms_desc[i].length === 0)
			break

		var room_div = document.createElement("div")
		room_div.className = "room"

		var players = 0
		var maxplayers = 0

		var params = rooms_desc[i].split(/[=,]/)
		for (var j = 0; j < params.length + 1; j += 2) {

			var param_name = params[j]
			var param_value = params[j + 1]

			if (param_name === "name") {
				var room_name = document.createElement("div")
				room_name.className = "text"
				room_name.textContent = param_value
				room_div.appendChild(room_name)
			}
			else if (param_name === "size") {
				var room_size = document.createElement("div")
				room_size.className = "text"
				room_size.textContent = "Size: " + param_value
				room_div.appendChild(room_size)
			}
			else if (param_name === "players") {
				players = param_value
			}
			else if (param_name === "maxplayers") {
				maxplayers = param_value
			}
		}

		var room_fullness = document.createElement("div")
		room_fullness.className = "text"
		room_fullness.textContent = "Players: " + players + "/" + maxplayers
		room_div.appendChild(room_fullness)

		var index = i
		room_div.onclick = function () {
			socket.send(index)
			loadGameMap()
		}

		room_list_div.appendChild(room_div)
	}
}


function buildMap(data) {
	room_list_div.hidden = true
	document.getElementById("new_room_panel").hidden = true

	game_data = JSON.parse(data)

	var map = document.createElement("div")
	map.className = "map"
	middle.appendChild(map)

	for (var i = 0; i < game_data.planets.length; ++i) {
		var cell = document.createElement("img")
		cell.src = generatePlanetImage()
		cell.className = "cell"
		cell.id = i.toString()
		cell.click = showInfo

		var style = cell.style
		style.top = game_data.planets[i].y * 15
		style.left = game_data.planets[i].x * 15
	}
}



function parseUpdates(event) {
	var data = event.data

	if (data === "") {
		room_list_div.innerHTML = ""
	}
	else if (data.slice(0, 6) === "rooms:") {
		updateRoomList(data.slice(6))
	}
	else if (data.slice(0, 1) === "{") {
		buildMap(data)
	}
}


window.onload = function open() {
	socket = new WebSocket("ws://" + window.location.hostname + "/auth")
	socket.onmessage = parseUpdates

	middle = document.getElementById("middle")
	room_list_div = document.getElementById("room_list")
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
	loadGameMap()
}
