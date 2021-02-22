# Summary
-The project is developed in Unity 2019.4.6f1

-It is ready to be compiled in Standalone and you already have a compiled version in the folder

# Knowed Issues
The game has a system to control that when the Player is instantiated, it does not find any asteroid near the spawn position. It would be necessary to implement a similar system for each change of round, in the current version, when completing a round, the new asteroids that are generated can instantiate on the player.

The Atlas sprite with the asteroid sprites should be edited so that all the sprites get closer to the box collider so that there are no cases where the shots cross the asteroid in areas of the edge where the collider does not reach.

# Details, configurations and notes
-The code has small explanations in some places

-The managers, the shoots and the Asteroids have public variables in the editor for configure different options.

-UI not animated for a good performance. Using Animators in the UI causes the canvas to render all the frames, so it is recommended to use LeanTween.

-Built-in Text Mesh Pro to optimize UI performance and cost

-Disabled in the TexttMeshProUGUI components no needed options, "Rich Text", "RaycastTarget","Parse Escape Character", "Visible Descender" and "Kerning". Little changes like this are done in different components in al the game 

-The sprite material is a duplicate of the default material but with the "Enable GPU instancing" option checked.

-The riggidBody of the asteroids and shoots only are wake up when are being used.

-Audios compressed with ADPCM

-Pools used in the asteroids, particles and shoots.

-Player Settings with some small Changes

-No use of light

-HDR disabled

# Controls

Mouse controls:

- Rotate - xAxis

-	Move forward - Hold right click

-	Shoot - Hold left click

keyboard controls:

-	Rotate - Left and right arrow

-	Move forward – Up arrow

-	Shoot - Hold Space

The keyboard controls can be changed in the public variables of the InputManager, in the GameObject "Managers".

You can close the compiled application in any moment with the Esc button.
