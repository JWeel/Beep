using System.Collections.Generic;
using System.Drawing;
//using System.Drawing; // Point structure

// TODO
// -generate painting
// -possible database functionality => save random generated painting seed/parameters to database

namespace Beep {
    // this class is a generated painting
    public class BeepWorld {

        public Point Size { get; set; }
        public Dictionary<Point, Tile> tiles;

        public BeepWorld(Point p) : this(p.X, p.Y) { }

        public BeepWorld(int sizeX, int sizeY) {
            Size = new Point(sizeX, sizeY);
            tiles = new Dictionary<Point, Tile>();
            for(int indexY=0; indexY < sizeY; indexY++) {

                int startX = 0 - (indexY / 2);
                int endX = sizeX + startX;

                if (indexY % 2 != 0) endX--; // this line makes the grid a box if SizeY is even

                for (int indexX = startX; indexX < endX; indexX++) {
                    tiles.Add(new Point(indexX, indexY), new Tile(indexX, indexY));
                }
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
