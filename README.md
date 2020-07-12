# Random-Island-Generator
### Introduction
Procedural Landmass Generation is a technique used to create different shaped islands and worlds for strategy games with high efficiency. Hand designing the basic geography of landmasses for games where details of the land does not matter much is a waste of resources. 
So using PLG a geographically accurate landmass can be created with the ability to change according to preferance by changing some parameters. In this project ***Perlin Noise*** is used as the basic algorithm to develop the patterns of landmass.
[Perlin Noise](https://en.wikipedia.org/wiki/Perlin_noise) is an algorithm wich generates a pixel map of Gradient Noise which is not totaly random but has random sets of areas where noise intensity changes gradually.
In ***Random-Island-Generator*** a region coloured 2d map and a 3d mesh of the land mass is generated thorugh perlin noise. Futhermore trees and other objects are also placed randomly on correct regions according to preferance.
<p align="center">
  <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Seed%20Change.gif" height="300px" alt="Gif"/>
</br><i>Random Atolls are generated by seed value change</i>
</p>

### Parameters

Following parameters can be adjusted in the ***Random-Island-Generator*** to change the criterias of the map being rendered.

<p align="center">
  <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Capture.PNG" height="300px" alt="Gif"/>
</br><i>parameters in the inspector</i>
</p>


Parameter | Effect 
--------- | ------
Draw Mode | Noise Map - Resulting Height map from perlin noise is displayed along with Mesh<br> Colour Map - Regions are coloured as defined in the 'regions sector' in the script<br> Object Map - Vegitation and other objects can be generated on the mesh
Mesh Height | <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Mesh%20Height.gif" height="150px" alt="Gif"/>
Seed | The map will change for different seeds because the area selected from the perlin noise map is changed with the seed(shown by the GIF in introduction) 
Scale | <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Scale%20change.gif" height="150px" alt="Gif"/>
Auto Update | Update every change in inspector with out clicking Generate button
Fall-off | This causes the landmasses on the edges of the map reduce height ensuring the created land will be a set of islands with a clear sea path around them *(with and without fall-off effect)* </br> <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/With%20fall%20off.PNG" height="150px" alt="Gif"/> <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Without%20falloff.PNG" height="150px" alt="Gif"/>
Fall-off Value | Intensity of the fall-off effect. Low value generates a small island at the center of the map <br/> <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Fall%20off%20spread.gif" height="150px" alt="Gif"/>
Octaves | Changes the number of vertices on the mesh. High value creates a more detailed landscapes but use slightly more processing <br/> <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Octaves.gif" height="150px" alt="Gif"/>
Persistance | <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Persistance.gif" height="150px" alt="Gif"/>
Lacunarity | Increases the density of the perlin noise gradient change
Offset | Change the area selected from the noise map.
<br/>
<p align="center">
  <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/gen%20objects.PNG" height="300px" alt="Gif"/>
</br><i>generate objects in the inspecter</i>
</p>
Parameters in the Generate Objects section are apllied only if the selected Draw Mode is Object Map .
Land objects(trees, rocks etc.) are generated on the ground at the respective points while sea objects(ship wrecks, boulders etc.) area generated on sea level. All the prefered models shoul be added to the script as Prefabs and objects will be generated depending on the selected count randomly.
<br/>
<p align="center">
  <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Region%20colours.PNG" height="300px" alt="Gif"/>
  <img src="https://github.com/chamikaCN/Random-Island-Generator/blob/master/ReadMe%20Contents/Colour%20map.PNG" height="300px" alt="Gif"/>
</br><i>region selector in the inspector and colour map </i>
</p>
Region colours section is relevent to the colour map. All the area in between the indicated height and the previous height level will be coloured in that particular colour. blend colours option will mix the colour with previous region's colour. 


