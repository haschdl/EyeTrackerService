# Example 2 - Generative art

Control a generative art drawing with your eyes. [See HD video in Vimeo](https://vimeo.com/309360509).

![](../docs/EyeTrackerTest02.gif)

Based on example from book [*Generative design*](http://www.generative-gestaltung.de/2/sketches/?01_P/P_2_1_2_03).

It assumes *EyeTrackerService* is running at 
ws://127.0.0.1:8080/eyetrackerservice/glaze*.  If you change the server URL, make the adjustments in the javascript code.


## How to run 

* Make sure *EyeTracker.Service.Host* is running.

   For example, from Visual Studio, in the solution explorer right-click *EyeTracker.Service.Host* and cihose Debug-> 
   Start new instance. A new console windows should appear, with the following content:

   ![](docs/2019-01-03-08-42-37.png)

   ```
   Initiating host for Eye Tracker...
   Assembly version: 1.0.0.0
   The service Service is ready at http://127.0.0.1:8080/eyetrackerservice/glaze
   Press <Enter> to stop the service.
   ```

* Open index.html in the browser.