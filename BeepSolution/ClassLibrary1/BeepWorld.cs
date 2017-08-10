using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
//using System.Drawing; // Point structure

// TODO
// -generate world
// -possible database functionality => save random generated world seed/parameters to database

namespace Beep {
    // this class is a generated world
    public class BeepWorld {

        public Point Size { get; set; }
        public Dictionary<Point, Tile> tiles;

        public BeepWorld(Point p) : this(p.X, p.Y) { }

        public BeepWorld(int sizeX, int sizeY) {
            Size = new Point(sizeX, sizeY);
            tiles = new Dictionary<Point, Tile>();
            //for (int i = 0; i < Size.X; i++) for (int j = 0; j < Size.Y; j++) tiles.Add(new Point(i, j), new Tile(i, j));
            for(int indexY=0; indexY < sizeY; indexY++) {

                int startX = 0 - (indexY / 2);
                int endX = sizeX + startX;
                for (int indexX = startX; indexX < endX; indexX++) {
                    tiles.Add(new Point(indexX, indexY), new Tile(indexX, indexY));
                    //Debug.WriteLine("ix" + indexX + "sx" + startX + "ex" + endX + "iy" + indexY);
                }
            }

        }

        public void Show() {
            for (int j = -Size.Y + 1; j < Size.Y; j++) {
                if (j % 2 == 0) Console.Write(" ");
                for (int i = 0; i < Size.X; i++) {
                    Console.Write(" " + i + "," + j + " ");
                    //Console.Write(" * ");
                }
                Console.WriteLine("");
            }
        }
    }


    public class Tile {
        public List<Tile> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        public Brush Color { get; set; }
        internal Tile(int x, int y) {
            this.Coordinates = new Point(x, y);
        }
        internal Tile(Point p) {
            this.Coordinates = p;
        }
    }

    public struct Point {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) {
            X = x;
            Y = y;
        }
    }
}
