Build and Runtime Instructions:

Find the build on: https://github.com/majohns/faceplay/

There should be 2 release builds located in: faceplay\emotion_viewer.cs\bin\x64\Release.

faceplay_mouse is initialized as the mouse-driven experience.
faceplay_webdriver is initialized as the webdriver experience.

If you cannot run the standalone builds, try building by opening the project in Visual Studio through the project file:
faceplay\emotion_viewer.cs\emotion_viewer.cs_vs2010-12

If there are symbol-loading issues when building/running, please contact any of:

rjchan@umich.edu
majohns@umich.edu
aniketk@umich.edu

for instructions on downloading the Intel Perceptual Computing SDK as it no longer has support from Intel.

User Instructions:

Please have Firefox installed as the application currently opens a Firefox window.

A firefox window will be opened when the application is run.  This will be driven by the WebDriver in WebDriver mode, but it is not imperative for mouse mode.

When the application window opens and you are ready to use the application, press the start button and keep your head in a neutral position.
In the bottom left-hand corner, there should be a text field that tells the calibration percentage.  Please keep head in central position until it is done calibrating and reads "Streaming."

Emotions Reference:
Smile: Smile (showing teeth tends to work slightly better, but a grin should work as well)
Contempt: Half-smile (bring one corner of your mouth up)
Surprise: Open your mouth as wide as possible

General:

Surprise to switch between modes (note that we know mode-switching isn't perfect which is why we have 2 executables)

Mouse mode: 

Tilt your head in a given direction to move the mouse.
Smile to left click
Contempt to right-click

Webdriver mode:

Tilt your head up/down and back to neutral to scroll the page up/down.
Tilt your head left/right and back to neutral to tab forward/back to the next applicable web element.
Smile to simulate the "Enter" key
Contempt to focus the location bar (the next text field that can be filled)

Notes:
The program will throw an exception if you close the firefox window and you are in WebDriver mode.
Mouse mode literally controls the system mouse, so if you want to type, you should use a virtual keyboard.  Note that there is currently no boundaries for where the mouse can be used, so you can actually use it to control your entire system rather than just your browser
