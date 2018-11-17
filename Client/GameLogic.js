
var socket
var user_id

var room_params = 4

var middle

var room_list_markup
var game_markup
var fleet_creation_markup

var room_list_div

var room_name_le
var size_sb
var players_sb
var planets_sb


var resources
var description
var mil_sb
var civ_sb
var sience_sb


var game_data
var current_planet
var planet_choise


function loadMarkup() {
	var main_page_request = new XMLHttpRequest()
	main_page_request.open('GET', '/html/RoomListPanel.html', false)
	main_page_request.send()
	if (main_page_request.status !== 200)
		return
	room_list_markup = main_page_request.responseText

	prepare_main_page()

	var game_markup_request = new XMLHttpRequest()
	game_markup_request.open('GET', '/html/GamePanel.html');
	game_markup_request.onreadystatechange = function () {
		if (game_markup_request.readyState !== 4)
			return
		game_markup = game_markup_request.responseText
	}
	game_markup_request.send()

	var fleet_creation_markup_request = new XMLHttpRequest()
	fleet_creation_markup_request.open('GET', '/html/FleetCreation.html');
	fleet_creation_markup_request.onreadystatechange = function () {
		if (fleet_creation_markup_request.readyState !== 4)
			return
		fleet_creation_markup = fleet_creation_markup_request.responseText
	}
	fleet_creation_markup_request.send()
}



function loadGameMap() {
	middle.innerHTML = game_markup
	socket.send("getmap")

	var input_elements = document.getElementsByTagName("input")

	for (var i = 0; i < input_elements.length; ++i) {
		var name = input_elements[i].getAttribute("name")
		if (name === "mil")
			mil_sb = input_elements[i]
		else if (name === "civ")
			civ_sb = input_elements[i]
		else if (name === "science")
			sience_sb = input_elements[i]
	}

	resources = document.getElementById("resources")
	description = document.getElementById("description")
}


function generatePlanetImage() {
	return "planetX.png"
}



function showInfo(element) {
	if (planet_choise !== undefined) {
		planet_choise = element.target.id
		middle.style.cursor = "auto"
		document.getElementById("fleetcreation").innerHTML += "<br />to " + planet_choise;
		document.getElementById("send").disabled = false;
		return
	}

	var input_elements = document.getElementsByTagName("button")
	var move_button = document.getElementById("move")
	var finance_button = document.getElementById("finance")
	var army_desc = document.getElementById("army_desc")

	army_desc.innerHTML = ""

	for (var i = 0; i < input_elements.length; ++i) {
		var name = input_elements[i].getAttribute("name")
		if (name === "finance") {
			finance_button = input_elements[i]
			break
		}
	}

	if (element.target.className === "cell") {
		current_planet = element.target.id
		var planet = game_data[current_planet]

		if (planet.Owner !== user_id) {
			description.innerHTML = "Planet " + element.target.id + "<br /> " +
				"Size: " + planet.size + "<br /> " +
				"Owner: " + planet.Owner

			mil_sb.disabled = true
			civ_sb.disabled = true
			sience_sb.disabled = true
			finance_button.disabled = true
			move_button.disabled = true;

			return
		}
		else {
			description.innerHTML = "Planet " + element.target.id + "<br /> " +
				"Size: " + planet.size + "<br /> " +
				"Military industry: " + planet.MilitaryIndustryLevel + "<br />" +
				"Civil industry: " + planet.CivilIndustryLevel + "<br />" +
				"Science: " + planet.ScienceLevel

			mil_sb.disabled = false
			civ_sb.disabled = false
			sience_sb.disabled = false
			finance_button.disabled = false
			move_button.disabled = false

			for (var item in planet.Guardians.Ships) {
				army_desc.innerHTML += item + " - " + planet.Guardians.Ships[item] + "<br />";
			}
			
			return
		}
	}
}


function finance() {
	socket.send("finance;planet=" + current_planet + ";mil=" + mil_sb.value + ";civ=" + civ_sb.value + ";science=" + sience_sb.value)
}


function createFleet() {
	var commands_panel = document.getElementById("commands")

	var fleet_panel = document.getElementById("fleetcreation")
	if (fleet_panel === null) {
		fleet_panel = htmlToElement(fleet_creation_markup)
		commands_panel.parentNode.insertBefore(fleet_panel, commands_panel.nextSibling)
	}

	var ships_list = document.getElementById("ships_list")

	for (var item in game_data[current_planet].Guardians.Ships) {
		var elem = document.createElement("input");
		elem.type = "number";
		elem.defaultValue = 0
		elem.min = 0
		elem.max = game_data[current_planet].Guardians.Ships[item]
		ships_list.innerHTML += item + ": ";
		ships_list.appendChild(elem)
		ships_list.appendChild(document.createElement("br"))
	}

	planet_choise = "choose"
	middle.style.cursor = "pointer"
}


function sendFleet() {
	var ships = {}
	var ships_list = document.getElementById("ships_list")

	var count = 0
	for (var i = 1; i < ships_list.childElementCount; i += 3) {
		var val = ships_list.childNodes[i].value
		ships[count.toString()] = (val === "" ? 0 : val)
		++count
	}

	socket.send("move:" + JSON.stringify(ships) + ";" + current_planet + ">" + planet_choise)
	planet_choise = undefined
	document.getElementById("send").disabled = true;
	document.getElementById("game_panel").removeChild(document.getElementById("fleetcreation"))
}


function startStep(money) {
	resources.innerHTML = "Money: " + money
	Array.prototype.map.call(document.getElementById("game_panel").getElementsByClassName("game_panel_part"), function (element, index) {
		element.disabled = false
	})
}


function step() {
	Array.prototype.map.call(document.getElementById("game_panel").getElementsByClassName("game_panel_part"), function (element, index) {
		element.disabled = true
	})
	socket.send("step")
}


function updateRoomList(data) {
	if (room_list_div === undefined || middle.innerHTML === "")
		loadMarkup()

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
		var roomname = ""

		var params = rooms_desc[i].split(/[=,]/)
		for (var j = 0; j < params.length + 1; j += 2) {

			var param_name = params[j]
			var param_value = params[j + 1]

			if (param_name === "name") {
				var room_name = document.createElement("div")
				room_name.className = "text"
				room_name.textContent = param_value
				room_div.appendChild(room_name)

				roomname = param_value
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

		var index = roomname
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

	for (var planet_info in game_data) {
		var cell = document.createElement("img")
		cell.src = '/png/' + generatePlanetImage()
		cell.className = "cell"
		cell.id = planet_info
		cell.onclick = showInfo

		var coordinates = planet_info.split('-')

		var style = cell.style
		style.top = coordinates[1] * 60
		style.left = coordinates[0] * 60
		
		map.appendChild(cell)
	}
}



function parseUpdates(event) {
	var data = event.data

	if (data === "") {
		room_list_div.innerHTML = ""
	}
	else if (data.slice(0, 3) === "id:") {
		user_id = Number(data.slice(3))
	}
	else if (data.slice(0, 4) === "map:") {
		buildMap(data.slice(4))
	}
	else if (data.slice(0, 5) === "turn:") {
		var map_desc = data.indexOf("map")
		buildMap(data.slice(map_desc + 4))
		startStep(data.slice(5, map_desc))
	}
	else if (data.slice(0, 6) === "rooms:") {
		updateRoomList(data.slice(6))
	}
}


window.onload = function open() {
	socket = new WebSocket("ws://" + window.location.hostname + "/auth")
	socket.onmessage = parseUpdates

	middle = document.getElementById("middle")
}


function prepare_main_page() {
	middle.innerHTML = room_list_markup

	room_list_div = document.getElementById("room_list")
	var input_elements = document.getElementsByTagName("input")

	for (var i = 0; i < input_elements.length; ++i) {
		var name = input_elements[i].getAttribute("name")
		if (name === "name")
			room_name_le = input_elements[i]
		else if (name === "size")
			size_sb = input_elements[i]
		else if (name === "planets")
			planets_sb = input_elements[i]
		else if (name === "players")
			players_sb = input_elements[i]

		input_elements[i].onchange = checkButton
		input_elements[i].oninput = checkButton
	}

	checkButton()
}


function checkButton() {
	var button_element = document.getElementsByName("create_room")[0]
	button_element.disabled = false;

	var size = size_sb.value
	var planets = planets_sb.value
	var players = players_sb.value

	if (room_name_le.value === "" || size < 6 || planets < 2 || players < 2 || size * size / 4 < planets || players > planets)
		button_element.disabled = true
}


function createRoom() {
	socket.send("create;name=" + room_name_le.value + ";size=" + size_sb.value + ";players=" + players_sb.value + ";planets=" + planets_sb.value)
	loadGameMap()
}
