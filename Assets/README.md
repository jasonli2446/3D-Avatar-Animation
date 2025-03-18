# 3D Avatar Animation - Zombie Shooter

This project is created for **CSDS 290: Introduction to Computer Game Design and Implementation** at Case Western Reserve University.

## Overview

This is a third-person shooter game featuring animated humanoid characters. The player must eliminate all zombies scattered across a level to save Earth from the zombie apocalypse. The game demonstrates the integration of third-person character controls, orbit camera mechanics, and animated humanoid avatars.

## Features

- Third-person character controller with orbit camera
- Humanoid avatar with 5 animations from Mixamo
- Zombie AI with different behavior based on distance from player
- Shooting mechanics with reticle
- Victory screen when all zombies are eliminated
- Play Again and Quit Game options

## Installation

1. Clone the repository:

```sh
git clone https://github.com/yourusername/3D-Avatar-Animation
```

2. Navigate to the project directory:

```sh
cd 3D-Avatar-Animation
```

3. Open the project in Unity.

4. Alternatively, run the standalone executable:

- Launch 3D Avatar Animation.exe

## How to Play

- Use WASD keys to move the player
- Hold right mouse button and move mouse to orbit the camera around the player
- Left-click to shoot
- Eliminate all zombies to win the game

## Game Controls

`W` - Move forward
`A` - Move left
`S` - Move backward
`D` - Move right
`Space` - Jump
`Left Mouse Button` - Shoot

## Technical Implementation

The game uses:

- Unity's Character Controller system
- Custom orbit camera implementation
- Humanoid animation rigging
- Animation state machines with transitions
- Event-based messaging system for game state management
