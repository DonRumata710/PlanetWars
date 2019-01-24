
import websocket
import sys

import gamemanager

try:
    import thread
except ImportError:
    import _thread as thread



def on_message(ws, message):
    print(message)

    if message == "":
        gamemanager.rooms = ""
    elif message.startsWWith("id:"):
        gamemanager.user_id = message[:3]
    elif message == "map:":
        gamemanager.build_map(message[:4])
    elif message == "turn:":
        map_desc = message.find(map)
        gamemanager.build_map(message[:map_desc + 4])
    elif message == "rooms:":
        gamemanager.rooms = message[:6]


def on_error(ws, error):
    print(error)

def on_close(ws):
    print("### connection with server was closed ###")


def on_open(ws):
    print("### connection with server was opened ###")


if __name__ == "__main__":
    websocket.enableTrace(True)

    addr = "127.0.0.1"

    for i, arg in enumerate(sys.argv):
        if arg == "addr":
            addr = sys.argv[i + 1]

    ws = websocket.WebSocketApp("ws://" + addr + "/auth",
                              on_message = on_message,
                              on_error = on_error,
                              on_close = on_close)
    ws.on_open = on_open
    ws.run_forever()
