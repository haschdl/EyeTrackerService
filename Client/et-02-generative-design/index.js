
var wsUri = "ws://127.0.0.1:8080/eyetrackerservice/glaze";
var output;

var pos;
var userPresent = false;

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
    console.log("CONNECTED to " + wsUri);
}

function onClose(evt) {
    writeToScreen("DISCONNECTED");
}

function onMessage(evt) {
    let parent = document.getElementById("glaze");

    //see documentation! 
    //evt.data[0] is for message type
    
    
    let dv = new DataView(evt.data);

    let msgType = dv.getUint8(0);
    switch(msgType) {
      case 1: //Gaze message               
         //evt.data[1] is for HasLeftEyePosition
         //evt.data[2] is for HasRightEyePosition
         pos = new Float64Array(evt.data.slice(3)); 
         break;
      case 2: //User presence
         //let eye = new Uint8Array(evt.data.slice(1,3));  
         userPresent = (dv.getUint8(1) > 0);
         console.debug("User present:" + userPresent);
         break;
      default:
         break;

    }
    
    //writeToScreen('<span style="color: blue;">X: ' + pos[0] + '  Y: ' + pos[1] + '</span>',parent);
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
