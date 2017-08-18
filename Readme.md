## BeepWorld

Procedural painting generation in Visual C# as part of MCSD 70-483 Certificate training

-----

The idea is to generate a painting from scratch, using various techniques.

This project stemmed from a combination of ideas involving procedural map generation, music generation using Console.Beep, and a user-driven system that predicted celestial movement and applied this to the workplace. Hence the obvious name - BeepWorld.

Our algorithm is as follows:

1. Given parameters for size of the painting, generate a hexagonal grid
2. Based on user input, select a number of tiles within the grid and give these a specific color
3. Change the color of surrounding tiles according to various rules specified by the user
4. Continuously loop over 3. where reference tiles are now the affected tiles
5. Notice pretty things being made?
