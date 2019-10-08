Fire Simulation Application

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

I've initially made a coroutine in Combustible class which was raycasting two Physics cassts. 
- SphereCast + OverlapSphere
- This was used to determine adjacent objects, in front of the plant and then around the burning plant. 

Unfortunatelly this solution was quite performence consuming so I decided to create separate Fire class to have only one gameobject responsible for fire. 
This gameobject moves around the map within region of the terrain and spread its radius to ignige all adjacent plants within fire object radius. 
