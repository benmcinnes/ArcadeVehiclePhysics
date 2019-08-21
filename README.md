# ArcadeVehiclePhysics 0.1V
Made by Ben McInnes 
Email: hello[at]benmcinnes.com
Twitter: @BSMcInnes

A repo for creating arcade inspired physics system for vehicles in Unity.

The scripts involved in the project could be used for several types of vehicles if need be but the use and test case has been that of a car.

This is a side project and while I have a lot of ideas on how to improve this, it mostly depends on available time. If you have any feedback or thoughts, please contact me via email or on twitter. Also if you use this at all I would love to see what you make!

## Information

This project was built and tested for use with a controller. In the interest of keeping the dependencies light, I have removed the input manager used during the development.

I would highly recommend [Sinput](https://sophieh.itch.io/sinput) by [Sophie Houlden](www.sophiehoulden.com)

### Credits 

The awesome [Muscle Car](https://sketchfab.com/3d-models/flat-colored-muscle-car-5220c6fa750841d9a674d963a697bee4) model was created by [Asuza](https://sketchfab.com/azusanyan1992)

### Dependencies
ArcadeVehiclePhysics.package
* Unity Cinemachine

Sample Scene
* TextMeshPro

### Unity Version
This project was built and tested in Unity 2018.4.5f1 LTS
Once the project is further along, it will be tested in different versions in order to make sure there are no issues. 
Contact me at hello@benmcinnes.com if you run into any issues.

### References
I have learned a great deal from available information on the internet. The biggest influences on this project are the following:

* [It IS Rocket Science! The Physics of Rocket League Detailed](https://www.youtube.com/watch?v=ueEmiDM94IE)
* [Supercharged! Vehicle Physics in 'Skylanders'](https://www.gdcvault.com/browse/gdc-16/play/1023219)
* [The Science of Off-Roading: 'Uncharted 4's' 4x4](https://www.gdcvault.com/browse/gdc-17/play/1024004)

## Setup

# Componenets
The implementation has been broken up into small single responsibility scripts. This way you can pick and choose what you are interested in using.

## Car Physics (CP)
For the best results (but please consider the performance costs) it is advised to change the Unity default physics settings to the following:

Edit -> Project Settings 
**Physics**
Default Solver Iterations: 12
Default Solver Velocity Iterations: 12

**Time**
Fixed Timestep = 0.01666667 (60Hz)

### CpMain
This is the main hub for information. All the other components reference it and update the data so that it is available to the other components.

### CpInput
Gets the main input from the player and keeps track of any ability inputs the player has access to.
The current implementation uses Sinput for the sake of convenience by it is easy to change out with another input scheme.

### CpAcceleration
This scripts contains two approaches to applying acceleration to the vehicle and it also calculates the speed data of the vehicle.

The first was to use scale the applied force depending on the current velocity of the vechile. This was simpler to implement but less intuitive to design for unless you are well aquinted with physics. The second uses a Velocity-Time curve to calculate what force should be applied. This was more complicated to calculate but far more designer friendly. The goal of this project is to be more design-centric and so I have opted to use the second approach. It was outlined in the Skylanders GDC talk mentioned above but here are the steps. 

The designer only has to create a Vel-Time graph which outlines the behaviour they want out of the vehicle, eg they can set the Animation Curve to go from 0 - 100km in 5 seconds and that is exactly how it will respond.

The Process works using reverse evaluation of the given Velocity-Time [AnimationCurve](https://docs.unity3d.com/ScriptReference/AnimationCurve.html).
1. Binary search using current forward speed to find the time value on the graph.
2. Add one time step onto that time value and evaluate the graph to get the new velocity.
3. Calculate a = (Vf - Vi)/deltaTime
4. Return the new force to apply (Must use ForceMode.Acceleration to ignore mass)

NOTE: To be able to reverse it has to be factored into the Velocity-Time graph. 

### CpTurning
Turning is implemented by applying a Torque to the Rigidbody around the Y-Axis. The torque is applied using Forcemode.VelocityChange so that angular velocity is affected directly and that the mass is ignored.

A speed factor is applied to the turning force so that the vehicle cannot turn at full force if it is standing still. An offset can be added to this speed factor to adjust how quickly the max turn force is achieved.

TODO: 
1. Use an AnimationCurve to scale torque so that the designer has more visual control.
2. Test implementation of the Vel-Force curve mentioned in the CP_Acceleration component

### CpColliderData
Uses the colliders of the vehicle to determine the average normal of the surface the vehicle is on.

This is useful for...
* applying angular stability forces. 
* checking if the vehicle is on the ground but not on its wheels. 

### CpDrag
Adjusts the drag and angular drag of the vehicles rigidbody.

**NOTE:** The collider physics material used has no friction and so currently, drag is the force that slows the vehicle down.

### CpLateralFriction
*Implemented based on understanding of the Rocket League Talk.

Lateral friction is how much the tyres will slip sideways. This implementation is simple and aims to give the player good control while driving but some skid when it comes to land at an angle. Otherwise it can be quite jarring if the car stops dead.

When grounded the side speed is calculated and the force required to cancel that movement is applied as an Forcemode.Impulse. T

To make the vehicle slide more or less, the designer can adjust the base tyre stickiness. 
* 0 - The tyres have no grip
* 1 - The tyres stick perfectly

**NOTE:** Currently, at high speeds this functionality does not work correctly

TODO: 
1. Apply surface normal factor to the lateral friction calculations so that the tyres are more slippery on angled surfaces

### CpStabilityForces
*Implemented based on understanding of the Rocket League Talk

The player loses some degree of control when the vehicle is airborn or rolling. The stability forces are used to get the vehicle into a controllable state as soon as possible. 

**"Linear" Stability**
* Applied when between 1 and 3 tyres are grounded.
* Applies a downward force from each wheel position. 
* Snaps the wheels down to the ground so that the player can be in control again as soon as possible.

**"Angular" Stability**
* Only applied when the vehicle colliders are in contact with the ground but the wheels aren't grounded
* Calculates the angle between the AverageSurfaceNormal and the Vehicles local up direction
* Applies a roll torque to try to align the car Up with the surface normal to get the car back onto it's wheels.

The angular stability only calculates the roll torque as trying to stabilise multiple axis can be unpredictable. -Rocket League Talk

### CpWheels
Keeps a list of the physics wheel transforms
Each wheel raycasts downward in order to get the surface normal and whether the wheel is grounded or not.

At this point in time the height of the physics rig is set by a pair of capsule colliders. This is definitely a weakness in this framework and needs some iteration and improvements.

TODO:
1. Experiment with using physics based suspension but need to determine the trade off between control and being able to handle wheel collisions more accurately
2. Look into using [Physics.ComputePenetration](https://docs.unity3d.com/ScriptReference/Physics.ComputePenetration.html) to handle collisions with sub suspension height objects. Similar to how it is handled in the Unchartered 4x4 talk.

## Car Visuals (CV)
When experimenting and looking into various approaches for building a vehicle system, it was generally agreed that it is better to seperate the phyiscs rig from that of the visual rig.

**Pros**
* Less parameter tuning
* More consistent control and more accessible
* Quicker iteration time on visual changes
* Can fake the physics reactions so that most players wouldnt notice

**Cons**
* Loss of procedural more reactive animation
* Have to maintain seperate physics and visual rig

### CvWheels
Controls where the wheel models are placed and the tire particles

### CvBodyMovement
Fakes the effect of momentum/intertia on the car. It rolls the vehicles body depending on the side speed/steering input and pitches it depending on the accel input and forward speed.

Can easily tweak the amount of angle from the input and speed

## Camera System - Cinemachine-based system
The camera system is quite basic at the moment - There are two virtual cameras, one follows the vehicle from behind while grounded and focuses slightly ahead of where the vehicle is heading. The second follows the player in air and focuses in the direction of movement but does not rotate so that if the vehicle is spinning in the air, the camera remains stable and manageable.

## Car Abilities (CA)
Now that the Physics and Visual rigs have been explained and set up, it is time to give the vehicle some abilities. I mean it is for a video game after all... 

Still experimenting with the best way to implement this but ideally the abilities should be modular and as self contained as possible. If you have some ideas/feedback on how to better keep this decoupled please let me know!

### CaAirControl
Allows player to turn the vehicle in the air. Implementation is the same as CP_Turning

### CaJump
The force is applied as an Impulse at the center of mass so that the vehicle jumps directly upwards without any rotation. Press the input down to charge the jump to a max force and jump on release.

### CaDrift
Allows the vehicle to slide. This is done by reducing the TyreStickiness which decreases the corrective side Impulses in the CP_LateralFiction script. The TyreStickiness is reduced along the DriftCurve set by the designer. The duration in and out of the drift can be set by the designer and this scales the movement up and down the Drift Curve.

To reduce the amount of forward speed lost when coming out of a drift, the corrective side impulse is added as a forward acceleration.

### CaBoost
Increases the top speed of the vehicle and the rate at which it accelerates while boosting

# End Note
This is a work in progress and if you try it out please let me know if you have any thought or feedback on how I can improve this system. hello[at]benmcinnes[dot]com


