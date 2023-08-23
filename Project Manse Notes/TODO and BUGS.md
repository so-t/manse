
# TODO

- ~~Implement Inventory Display and Controls

- ~~Angle calculations when determining how far to rotate and inventory display can be reused as long (as inventory stays the same size) rather than recalculated 

- ~~investigate whether or not the main circle/game object of an inventory display needs a mesh filter like it does not need a material, or if even the circle is unnecessary of all we care about is

- Implement automaton script that moves an objects

- Implement Pause screen ~~background and~~ commands

- Implement Inventory Screen background

- Implement an interactable that takes an item from the players inventory to activate

- Consider revisiting interactable start condition functionality. Switch to UnityEvents? Delegates? 

- Experiment with removing/adjusting camera snapback

- Make footstep sounds dynamic by adding sound sources to floor objects rather than the player (UnityEvents?)

- ~~Add lighting to the PSX shader 

- ~~Figure out the best way to get an image representation of an item

- ~~Add game object to Item class

- ~~Add game objects from items in inventory to boundary points of inventory display, adjusted for scale 

- ~~Add “interact” input check for selecting highlighted item from inventory display

- ~~Make highlighted item from inventory display rotate

- ~~Does the circle object need to be created for the inventory display, or can we slip it and use the coords found for the items if all we care about is the boundary points



# BUGS

- ~~Interactables can be triggered through walls

- ~~Inventory displays of various sized (E.G. 7, 8) will never stop rotating once they've started

- 'ReturnToLookAt' will return to a slightly displaced location if the camera was not looking forward when 'LookAt' was triggered

- 'SmoothLookAt' will sometimes rotate in an inhuman seeming way 

- Player can get pushed through wall box colliders when moving forward while pressed between two object (such as an interactable with a sphere collider, and the wall box collider) 

- ~~Player can get stuck with rotation momentum when colliding with objects 

- ~~Player camera rotation and head-bob can trigger in inventory and pause menus