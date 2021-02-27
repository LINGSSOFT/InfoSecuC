PauseController

Component should work as is - just drag PauseController on your scene.

Extension points:

1. Visuals
Enable the Canvas under PauseController > PauseCanvas and edit the visuals as you like.

2. Changing the Pause / Quit logic
Copy TimePause & AppQuit to your own GameController and replace the reference in PauseController. 
Implement IPausable & IQuittable interfaces in your GameController scripts as needed.

Known issues:
 - If there are multiple canvases on your scene, PauseCanvas might be placed under them. 
   As PauseCanvas is disabled most of the time, it should be placed as the last canvas on the scene