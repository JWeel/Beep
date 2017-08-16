using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
//using System.Drawing; // Point structure

// TODO
// -generate painting
// -possible database functionality => save random generated painting seed/parameters to database

namespace Beep {
    // this class is a generated painting
    public class BeepWorld {

        public Point Size { get; set; }
        public Dictionary<Point, Tile> tiles;

        public BeepWorld(Point p) : this(p, false) { }
        public BeepWorld(int sizeX, int sizeY) : this(new Point(sizeX, sizeY), false) { }
        public BeepWorld(int sizeX, int sizeY, bool b) : this(new Point(sizeX, sizeY), b) { }

        public BeepWorld(Point p, bool boxed) {
            Size = p;
            tiles = new Dictionary<Point, Tile>();
            for (int indexY = 0; indexY < Size.Y; indexY++) {

                int startX = 0 - (indexY / 2);
                int endX = Size.X + startX;
                if (boxed && indexY % 2 != 0) endX--; // this line makes the grid a box if SizeY is even

                for (int indexX = startX; indexX < endX; indexX++) {
                    Tile t = new Tile(indexX, indexY);
                    t.SetNeighbors(Size, boxed);
                    tiles.Add(new Point(indexX, indexY), t);
                }
            }
        }
    }

    // TODO change Tile to struct, would make life simpler
    public class Tile {

        public List<Point> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        public Brush Color { get; set; }

        internal Tile(int x, int y) : this(new Point(x, y)) { }

        public Tile(Point p) {
            this.Coordinates = p;
            this.Color = Brushes.BlanchedAlmond;
        }

        // (deep) copy constructor
        public Tile(Tile t) {
            this.Coordinates = t.Coordinates;
            this.Color = t.Color;
            this.Neighbors = new List<Point>(t.Neighbors);
        }

        // returns true if point exists within dimensions of a beep world
        private bool IsValidPoint(int x, int y, Point beepWorldSize, bool boxedBeepWorld) {
            int startX = 0 - (y / 2);
            int endX = beepWorldSize.X + startX;
            if (boxedBeepWorld && y % 2 != 0) endX--;
            return (x >= startX && x < endX && y >= 0 && y < beepWorldSize.Y);
        }

        // painstakingly sets all valid neighbor points for a hexagonal tile
        internal void SetNeighbors(Point bwSize, bool boxedBW) {
            int x, y;
            Neighbors = new List<Point>();

            x = Coordinates.X + 1;
            y = Coordinates.Y;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));

            x = Coordinates.X - 1;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));

            x = Coordinates.X;
            y = Coordinates.Y + 1;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));

            y = Coordinates.Y - 1;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));

            x = Coordinates.X + 1;
            y = Coordinates.Y - 1;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));

            x = Coordinates.X - 1;
            y = Coordinates.Y + 1;
            if (IsValidPoint(x, y, bwSize, boxedBW)) Neighbors.Add(new Point(x, y));
        }
    }

   public struct Point {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) {
            X = x;
            Y = y;
        }
        public override string ToString() {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
