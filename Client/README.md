# Eye Tracker Client (Javascript)
	This is not a Visual Studio project! Open the folder in any editor or open **index.html** directly from the browser.

An example in Javascript which connects to the Eye Tracking service and displays the glaze positions in the screen. 
It is used only as quick test to check the websockets connectivity to the main project *EyeTracker.Service.Core*.

It assumes *EyeTrackerService* is 
running at *ws://127.0.0.1:8080/eyetrackerservice/glaze*. 

# How to test it

* Make sure *EyeTracker.Service.Host* is running.
For example, from Visual Studio, in the solution explorer right-click *EyeTracker.Service.Host* and cihose Debug-> 
Start new instance. A new console windows should appear, with the following content:
	Initiating host for Eye Tracker...
	Assembly version: 1.0.0.0
	The service Service is ready at http://127.0.0.1:8080/eyetrackerservice/glaze
	Press <Enter> to stop the service.

* Open index.html in the browser. You should start seeing the glaze positions displayed in the web page, as follows:

	WebSocket Test
	X: 921.3090237831944 Y: 336.3348781484637
	CONNECTED to ws://127.0.0.1:8080/eyetrackerservice/glaze

	