# fire-spreading

HELP:

- Use left panel to tweak the simulation. 
- To simulate the fire propagation, click on the generate button in left panel. You can either use grid spawn or random spawn. 
- By using grid spawn, do not forget to set spawn radius.  
- Blue arrow on top of the screen represents wind direction.
- Ignite random value is default 4. You can change this value in Unity Inspector in Vegetation Handler component.

Namespaces:
Control - Control and user input classes responsible for interaction
Core - Core functionality like fire physics and combustible class
UI - User Interfaces classes
Vegetation - Flower class and VegetationFactory responsible for instantiating of vegetation prefabs

Method of fire propagation:
My first approach was to make each flower/plant responsible for itself but this solution led 
to high performence issues when the scene had more gameobjects. 

Each plant had a coroutine to cast Rays in distance with growing radius, I had also a method for side spreading and directional spreading. 
Unfotunatelly I had to remove this because high cpu loads. 

I decided to create one game object responsible for the whole fire element so the Weather Controller instantiates a FirePrefab, which has 
only RigidBody and Sphere collider. This gameobject is moved by wind direction and some of the aspects which can be changed either
through inspector or through ingame UI. 

This gameobject has component Fire.cs which is responsible for radius growing. Whenever a collider is detected through this sphere collider, 
it's determined whether the object is combusible or not and ignited based on several conditions. I think this solution is really nice and 
easy to handle. Maybe a little bit more tweak would be great. 

Unfortunatelly I got stuck on first solution and also by tweaking performance issues, especially Rendering issues with a lot of meshes 
in the scene. I don't have a lot of experience with graphic optimization so it took a while to figure out how to use GPU instanced materials.
All meshes are properly batched and when color of the flower needs to be changed, it access its own property block. 
