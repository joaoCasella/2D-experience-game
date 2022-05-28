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

# External assets
The text font used is available at this [domain](https://fonts.google.com/specimen/Press+Start+2P) and is licensed under the [OFL](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL). The author is cody@zone38.net and the licensing file can be found in the project, at `Assets/Runner/Fonts/OFL`
The main menu music (copyright free) was downloaded from [pixabay]<https://pixabay.com/> and the music author is [nojisuma]<https://pixabay.com/users/nojisuma-23737290/?tab=audio>

# Possible Improvements
My goal is to learn about the engine and its workflow. So, I consider the game in a state were there is not much to be done that can teach me a lot (you can tell me why I'm wrong by creating an issue). In spite of those facts, here are some points that could be improved in the game:

- [X] Adapt game (mainly the controls) to work on mobile
- [X] Tune some parameters, like the ones related to the jump
- [X] Fix some visual glitches on the moving floor
- [X] Persist the high score in the hard drive (currently, the game only "remembers" the high score from the current session)
- [X] Add configuration screen to adjust sound and music volumes
- [X] Add localizations to the game
- [X] Add countdown after game pause (so that the player can have a moment after the pause to concentrate before the game restarts)
- [X] Make it possible to navigate inside the menus using the back button (escape or android back)
- [X] Make the language changeable through the main menu
- [X] Add music to the main menu
- [X] Change UI
    - [X] Make UI be fully inside the game screen space (currently, it fills the whole screen)
    - [X] In game menu positions (difficult to press pause button and to see current score)
- [X] Minor optimizations:
    - [X] Replace tag comparation with the `CompareTag` method
- [ ] Future improvements (after v2)
    - [ ] Fix the way the pontuation is inserted in the localizations
    - [ ] Change UI appearance
    - [ ] Remove unused assets from the package imported
    - [ ] Play a sound effect when the player dies
    - [ ] Use a shader on the floor to optimize the way its drawn (and to fix visual glitches)
    - [ ] Change the UI to use prefabs (and to create a pattern)
    - [ ] Add background parallax
- [ ] Game design ideas
    - [ ] Add an enemy that flies (similar to the aerodactyl in the chrome game, used as inspiration), and adapt the player logic accordingly to make it work
    - [ ] Add some sort of bonus to the player (like coins that add a bonus to the player pontuation but are difficult to be caught)
