# A Day in the Life of a Delivery Robot
*Created by The Impeccable Robot Simulation Team (The I.R.S)*  
*Riley Elwell, Kevin Huynh, Ethan Largent, and Connor Olsen*

---
## Introduction
In this game you take the role of a delivery robot on the OSU campus. You are given deliveries to fulfill in a safe and timely manner.  
The main goals of our game are to:  
- Provide a fun and enjoyable experience for players
- Have realistic visuals that are accurate to the actual OSU campus
- Represent an accurate simulation of the ADAS present in delivery robots
- Win the game design competition (of course!)

---
## How to Play
The game has an easy to understand tutorial when you first start the game that will teach you the controls and what everything on your screen means. Here is a list of the controls:  
| PC | Xbox | Input |
| --- | --- | --- |
| Q | Left Trigger | Left Drive Track Forward |
| A | Left Bumper | Left Drive Track Backward |
| W | Right Trigger | Right Drive Track Forward |
| S | Right Bumper | Right Drive Track Backward |
| Enter | A Button | Continue |
| Space | B Button | Brake |
| Left Shift | X Button | RoboBoost |
| R | Y Button | Flip Robot Rightside Up |
| ESC | Start Button | Pause Game |

Drive your robot to pick up and deliver orders across campus quickly while avoiding pedestrians, vehicles, and trains (RIP). You’ll pick up an order, deliver the order, pick up a new order, etc.  

---
## Development
During the development of this game we had three primary roles:
- A Map/Environment/UI Developer (Makes everything look good)
- A Gameplay Developer (Make everything work good)
- An ADAS developer (Focuses on making our ADAS simulation realistic).
 
The four of us worked a little in each category, but we each had a primary role and one of us joined later and worked as a “free agent”. Development started slow since many features relied on other features to be completed before we could start working on them. Once the bulk of these bottleneck features were completed, working on the game was an easy task that we were able to finish, fine tune, and add some stretch goals too.

---
## The Map and Environment
When working on the map for this game, these were the steps that were taken:
1. Gather textures for objects and edit to be seamless or tileable if necessary
2. Model objects in Blender
3. Apply textures and edit UVs in Blender
4. Export models from Blender as .fbx format
5. Import to Unity 
6. Unity last extra settings

1 - The textures can be acquired in different ways, however, I found it best to capture my own pictures to get the right angles and best quality. This can be difficult due to weather or lighting conditions, but this is much simpler than using screenshots from Google Earth. Some textures will require some additional edits using either Shoebox (Adobe AIR app) or an online image seamless convertor.

2 - Modeling objects can be done in whatever way you prefer, however, I would recommend trying to keep the geometry simple and use as few polygons as possible. 

3 - Applying textures in Blender is simple because exporting them to Unity only allows for using image texture nodes. Any other nodes will not properly export, so we must only stick with this. Editing UVs takes a large chunk of time, however, if your texture images from previous steps have been edited well, this will be simpler. Also, I personally like to use the “Unwrap” and “SmartUV Wrap” features in Blender to make it even easier.

4 - When exporting models from Blender, you must make sure all the models you want are selected and then choose the .fbx format. Some of the settings you need to check are “Mesh”, “Apply Transform”, and “Copy Path” mode to ensure you get everything you need. Lastly, I liked to separate the models being exported into groups so that the file size was smaller and would still be able to be uploaded to GitHub (100MB max limit).

5 - When finally importing the .fbx file into Unity, you can simply copy and paste it into our Assets folder. Then, on the import settings page you need to “Extract Textures” and “Extract Materials.” It needs to be in this order for it to work, then you can hit apply and drag these models into the Unity scene view. 

6 - From here the map is ready to be used and we just check the “Static” box in the top right hand corner of the Inspector (when selecting a model GameObject) to enable batching that helps with performance. Also, make sure to attach a mesh collider to each model so our player robot correctly interacts with the environment!

---
## The UI
These were the steps taken when creating UI for the game:
1. Gather transparent clip art images for icons
2. Position the image and a text asset within a panel in Unity
3. Handle updating the UI within a script

1 - As we are not professional artists and have limited time, we opted for finding clip art online to use for our icons. The only criteria here was trying to find neutral coloring, good resolution/size, and a transparent background for each icon.

2 - In Unity, I decided to make an empty GameObject that would hold each part of the UI. This made it more organized, but also allowed each section to be turned off independently (as used in the Instructions/Tutorial sections). Positioning this was just a matter of experimentation and seeing what looked the best, while also keeping in mind accessibility standards. The Rule of Three was a factor as well, as all UI elements can be seen in the most efficient parts of the screen (so not to detract attention from the gameplay too much).

3 - This differed between each of the UI elements, but each one required getting a reference to its section and then usually updating a text value when a certain event occurred. For example, the boost bar goes down when a button is pressed, the health bar goes down when a specific pedestrian collider is hit, and the compass updates every frame to point towards your objective. Overall, the UI is all but done and would not require many functional updates. However, some artistic flair could be added to spice up the user experience if necessary.

---
## The Gameplay
The gameplay was developed following these steps:
1. Created a physics based player controller
2. Created a simple pathing system
3. Created a delivery systems
4. Incorporated sensors into robot movement
5. Updated the pathing systems and more

1 - The first thing we needed in our game was a player controller. To do this we used a Unity library called Chaos Vehicles that made it easy to apply torque to wheel collider objects that would rotate and cause the robot to move. This allowed our robot to accurately simulate the real physical movements of delivery robots on campus. We would later add more controls like a brake button and sprint button.

2 - In order to start work on ADAS, a quick pathing system had to be created for testing purposes. For this prototype, we set up strict paths of nodes that went in a loop that we would have stationary human models move on. It didn’t look the prettiest, but it worked for what we needed then.

3 - The delivery system is a key component that makes up the main goal of the game. The creation of the delivery system was like making a tool that would allow us to add and remove specific locations on the map as pick up and drop off locations for our player’s deliveries. This made it easy to add more deliveries as the map expanded.

4 - Once ADAS was complete, we were able to take in output from the sensor readings to interact with the player controller. For example, when the front sensor activates and the back sensor isn’t active, a modifier will be applied to each drive track to halve the speed. We made multiple scenarios like this for the ADAS and made it easy to add in more.

5 - Once our core features were in the game, we were able to go back and improve on what we’ve already done. One major improvement was the pathing system. The pedestrian model changed to an animated dummy, the paths were more realistic, the number of paths was expanded to fit the full map, the movement between nodes was made smoother, the paths were more random, and the spawn rates were balanced. 

---
## The ADAS
For the robot ADAS, we created it following these steps
1. (RESERVED FOR THE CREATIVE VISIONARY)

1 - A short description of each step (RESERVED FOR THE CREATIVE VISIONARY)

---
## Future Goals
Most of us don’t plan on working on this project further after graduation. However, if anyone decides to pick up where we left off, here are some features we wanted for the game but didn’t end up having time for (but may have added since the last update to this document):
- Retexture the dummy models to look like a variety of humans
- Vary the kinds of cars that can spawn
- Expand the map to have more of the campus
- Add new maps of other campuses
- Improve AI traffic to become more self governing and alive
- Add more customization options
- Add a silly compelling story
- Add robot tricks
- Add in-game events
- Add power ups
- Add easter eggs
- Add sounds 

---
## Conclusion
We hope that this document gives readers a full understanding of what we have been working on. We encourage anyone interested to play our game and maybe even continue on with its development.
