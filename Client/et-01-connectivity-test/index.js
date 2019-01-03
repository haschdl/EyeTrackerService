
var wsUri = "ws://127.0.0.1:8080/eyetrackerservice/glaze";
var output;

function init() {
    output = document.getElementById("output");
    testWebSocket();
}

function testWebSocket() {
    websocket = new WebSocket(wsUri);
    websocket.binaryType = "arraybuffer";
    websocket.onopen = function (evt) { onOpen(evt) };
    websocket.onclose = function (evt) { onClose(evt) };
    websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };
}

function onOpen(evt) {
    writeToScreen("CONNECTED to " + wsUri);
}

function onClose(evt) {
    writeToScreen("DISCONNECTED");
}

function onMessage(evt) {
    let parent = document.getElementById("glaze");
    writeToScreen('<span style="color: blue;">RESPONSE: ' + evt.data + '</span>', parent);

    //see documentation! First element of evt.data is for message type, then next is for position
    let pos = new Float64Array(evt.data.slice(3)); 
    writeToScreen('<span style="color: blue;">X: ' + pos[0] + '  Y: ' + pos[1] + '</span>',parent);
}

function onError(evt) {
    writeToScreen('<span style="color: red;">ERROR:</span> ' + evt.data);
}



function writeToScreen(message, parent) {
    var pre = document.createElement("p");
    pre.style.wordWrap = "break-word";
    pre.innerHTML = message;
    if (parent !== undefined)
        parent.innerHTML = message
    else
        output.appendChild(pre);
}

window.addEventListener("load", init, false);
