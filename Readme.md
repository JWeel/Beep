## BeepWorld

Procedural painting generation in Visual C# as part of MCSD 70-483 Certificate training

-----

The idea is to generate a painting from scratch, using various techniques.

This project stemmed from a combination of ideas involving procedural map generation, music generation using Console.Beep, and a user-driven system that predicted celestial movement and applied this to the workplace. Hence the obvious name - BeepWorld.

Our algorithm is as follows:

1. Given parameters for size of the painting, generate a hexagonal grid
2. Based on user input, find a number of tiles within the grid
3. Change the color of surrounding tiles according to various rules specified by the user
4. Repeat point 3 iteratively or continuously based on user input
5. Notice pretty things being made?
