
# Tron't Stop Me Now

Tron't stop me now is a two player arcade game made during an introduction class to Unity. It has been developped by Lucas Soubigou & Pierre Bigey and contains a gammeplay loop with UI, sound effects, visual effects and a main menu screen. 

Each player has a lightcycle (a bike that leaves a trail of light behind it) and has to avoid the walls and the other player's trail. The goal is to make the other player crash into a wall or a trail. To do so, the player can steer his bike left or right. 

You can play the game on itch.io [here](https://couteauxabeurre.itch.io/tront-stop-me-now).

## Screenshots

![Main menu](/FirstScreen.png)

![Gameplay](/GamePLayScreen.png)

![Death](/Death.png)

![ScoreScreen](/ScoreScreen.png)

## Development

The game has been developped in C# with Unity 2022.3.46 in less than 6 hours. 

The trail behind the lightcycle is made with a mesh that is frequently updated. You can find the implementation in the 
[`TrailMeshGenerator.cs`](/Assets/Pierre/TrailMeshGenerator.cs) script.


