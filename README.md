# 2D-experience-game
First 2D project, trying to learn about 2D game development using Unity

![](Demo.gif)

# Objective
Develop a simple 2D game, using Unity, to learn about the basics of the engine

# Game Setup
The game proposal is to be similar to the Dino Chrome, the game present on Google Chrome when the user is offline. The desired setup is to emulate it's behaviour, adding a few more features and polishment to learn about the engine possibilities.

# Configurations
The machine used to develop this game was a Acer Nitro 5, with a Intel I7 7700H processor, a Nvidia Geforce GTX 1050Ti and 16Gb of RAM. The Unity version used was the 5.0 one

# Feedback
This is my first 2D project (I have also developed a 3D game, available at https://github.com/joaoCasella/FPS), so any feedbacks and suggestions are apreciated. All I ask is respect :)

# Possible Improvements
My goal is to learn about the engine and its workflow. So, I consider the game in a state were there is not much to be done that can teach me a lot (you can tell me why I'm wrong by creating an issue). In spite of those facts, here are some points that could be improved in the game:

- [X] Adapt game (mainly the controls) to work on mobile
- [X] Tune some parameters, like the ones related to the jump
- [X] Fix some visual glitches on the moving floor
- [X] Persist the high score in the hard drive (currently, the game only "remembers" the high score from the current session)
- [X] Add configuration screen to adjust sound and music volumes
    - [ ] Make this screen accessible through the main menu
- [ ] Add localizations to the game
- [ ] Add countdown after game pause (so that the player can have a moment after the pause to concentrate before the game restarts)
- [X] Make it possible to navigate inside the menus using the back button (escape or android back)
- [ ] Change UI
    - [X] Make UI be fully inside the game screen space (currently, it fills the whole screen)
    - [X] In game menu positions (difficult to press pause button and to see current score)
    - [ ] Change button appearance
- [ ] Minor optimizations:
    - [X] Replace tag comparation with the `CompareTag` method
    - [ ] Remove unused assets from the package imported
- [ ] Add an enemy that flies (similar to the aerodactyl in the chrome game, used as inspiration), and adapt the player logic accordingly to make it work
- [ ] Play a sound effect when the player dies
- [ ] Add some sort of bonus to the player (like coins that add a bonus to the player pontuation but are difficult to be caught)
