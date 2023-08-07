Important Qualities:
-   Low poly models and low resolution textures
	- Suggestions:
		- 6 bones per character
		- < 500 polygons per char, <3,000 for scene on screen
		- 32 max texture rez
-   Polygon jittering
	- Snap vertex positions to pixel locations in the output resolution and disable all anti-aliasing methods
-   Warped textures
-   Popping, and jittering texture mapping
-   Per vertex lighting
	- Do your lighting calculations in the vertex shader and output a colour per vertex to mix with the texture mapped colour in the fragment shader.
-   Depth cueing fog
	- Emulated in a modern shader by calculating the depth of the vertex in view space and then doing an inverse lerp to get back a fog density value which we output to the fragment shader. In the fragment shader we just mix this with the output colour we’d usually render out
- CRT image effect
  
  Environment art:

  Break the environment up into tiles and then each tile maps to a specific section of a texture, such as shown below.
  ![[rgz5b72y.bmp]]
  ![[cuabzpot.bmp]]
  ![[ojbl3xmt.bmp]]