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
                    tiles.Add(new Point(indexX, indexY), new Tile(indexX, indexY));
                }
            }

            // TEMPORARY TEST STUFF please ignore
            BeepRule.ChangeColor(tiles.Values.ToList(), null, null);
        }
    }

    // TODO change Tile to struct, would make life simpler
    public class Tile {
        public List<Point> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        public Brush Color { get; set; }
        internal Tile(int x, int y) : this(new Point(x, y)) { }
        internal Tile(Point p) {
            this.Coordinates = p;
            this.Color = Brushes.BlanchedAlmond;
        }

        // (deep) copy constructor
        public Tile(Tile t) {
            //this.Neighbors = t.Neighbors;
            this.Coordinates = t.Coordinates;
            this.Color = t.Color;
        }

        private static List<Point> GetNeighbors(Point p) {
            return null;
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
