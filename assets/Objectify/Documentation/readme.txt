Hi!

Thank you so much for purchasing Objectify. Here you will find definitions for shader parameters as well
as a couple quick tips.


SHADER PARAMETER DEFINITIONS:

Diffuse - This is where you put the texture that acts as the color of the surface of the material.

Specular - This is where you put the texture that controls how shiny your object is and what color that reflected light is.

Specular Power - This value controls how intense the shininess is on your material. Higher values can make the object look wet.

Normal - This is where you put your normal maps.

Normal Intensity - this value magnifies how intense your normals are.

Emissive On/Off - This check box decides weather or not the object is using the Emissive texture parameter. Off state simply
puts a value of 0 into this part of the shader. On state adds the emissive texture to the material.

Emissive - This is where you put your texture for any glow or color you want to see that will ignore lighting.

Pulse Speed - This parameter controls the speed of the cycle of which the material becomes bright and then dark again.

Fresnel Width - This slider controls how far the edge glow on the object reaches into the center of your object.

Fresnel Color - This parameter controls what color your outline and edge glow appear as.

Highlight Color - This parameter controls what color your outline and edge glow appear as.

Outline Width - This value controls how far the outline extrudes out from the original mesh this shader is applied to.

Glow/Extrusion Texture - This is where you put a noise texture that will add variation to the extrusion amount of outline
shaders. make sure to add textures that have a wide range of values from black to white preferably with no hard edges. This 
can give the outline a nice jelly feeling

Pan Speed X - This value controlls how fast the Glow/Extrusion Texture pans on it's X coordinate.

Pan Speed Y - This value controlls how fast the Glow/Extrusion Texture pans on it's Y coordinate.




QUICK TIPS:

Outline Shader Fracturing -
One known issue that can occur is that the outline shader can appear to seperate from itself. It's caused by vertices 
in the same location, but with different normals. A solution for this is to smooth your entire mesh, and make the 
hard-edges in your normal map instead. 

If you have any issues please contact me at sidekickapplications@gmail.com
I am a one man team so your patience is appreciated ^_^!
