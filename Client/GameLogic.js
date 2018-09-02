
var socket

var room_params = 4

var middle
var room_list_div

var size_sb
var players_sb
var planets_sb


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

	game_data = JSON.parse(data)

	var map = document.getElementById("map")

	var i = 0
	for (var planet_info in game_data) {
		var cell = document.createElement("img")
		cell.src = '/png/' + generatePlanetImage()
		cell.className = "cell"
		cell.id = i.toString()
		cell.click = showInfo

		var coordinates = planet_info.split('-')

		var style = cell.style
		style.top = coordinates[1] * 60
		style.left = coordinates[0] * 60
		
		map.appendChild(cell)

		++i;
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
	else if (data.slice(0, 8) === "planets:") {
		buildMap(data.slice(8))
	}
}


window.onload = function open() {
	socket = new WebSocket("ws://" + window.location.hostname + "/auth")
	socket.onmessage = parseUpdates

	middle = document.getElementById("middle")
	room_list_div = document.getElementById("room_list")
	var input_elements = document.getElementsByTagName("input")
	
	for (var i = 0; i < input_elements.length; ++i) {
		var name = input_elements[i].getAttribute("name")
		if (name === "size")
			size_sb = input_elements[i]
		else if (name === "planets")
			planets_sb = input_elements[i]
		else if (name === "players")
			players_sb = input_elements[i]
		
		input_elements[i].onchange = checkButton
	}

	checkButton()
}


function checkButton() {
	var button_element = document.getElementsByName("create_room")[0]
	button_element.disabled = false;

	var size = size_sb.value
	var planets = planets_sb.value
	var players = players_sb.value

	if (size < 6 || planets < 2 || players < 2 || size * size / 4 < planets || players > planets)
		button_element.disabled = true
}


function createRoom() {
	socket.send("create;size=" + size_sb.value + ";players=" + players_sb.value + ";planets=" + planets_sb.value + ";");
	loadGameMap()
}
