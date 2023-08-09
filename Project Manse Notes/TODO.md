
# TODO

- Implement Inventory Screen

- Implement automaton script that moves and object 

- Implement Pause screen 

- Implement an interactable that takes an item from the players inventory to activate

- Experiment with removing/adjusting camera snapback

- Make footstep sounds dynamic by adding sound sources to floor objects rather than the player

- Figure out the best way to get an image representation of an item

# BUGS

- Interactables can be triggered through walls

- 'ReturnToLookAt' will return to a slightly displaced location if the camera was not looking forward when 'LookAt' was triggered

- Player can get pushed through wall box colliders when moving forward while pressed between two object (such as an interactable with a sphere collider, and the wall box collider) 

- ~~Player can get stuck with rotation momentum when colliding with objects 