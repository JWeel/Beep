using System.Collections.Generic;
using System.Windows.Media;
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
        }
    }

    public class Tile {
        //public List<Tile> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        public Brush Color { get; set; }
        public Point SizeBW { get; set; }
        public bool BoxedBW { get; set; }

        internal Tile(int x, int y) : this(new Point(x, y)) { }

        internal Tile(Point p) {
            this.Coordinates = p;
            this.Color = Brushes.BlanchedAlmond;
        }

        internal Tile(Point p, Point size, bool boxed) {
            this.Coordinates = p;
            this.BoxedBW = boxed;
            this.SizeBW = size;
        }

        public static List<Point> GetNeighbors(Point p) {

            List<Point> neighbors = new List<Point>();
           
            //int startX = 0 - (indexY / 2);
            //int endX = Size.X + startX;

            //if (boxed && indexY % 2 != 0) endX--; // this line makes the grid a box if SizeY is even

            neighbors.Add(new Point(p.X + 1, p.Y));

            neighbors.Add(new Point(p.X + 1, p.Y - 1));

            neighbors.Add(new Point(p.X, p.Y - 1));

            neighbors.Add(new Point(p.X - 1, p.Y));

            neighbors.Add(new Point(p.X - 1, p.Y + 1));

            neighbors.Add(new Point(p.X, p.Y + 1));

            
            return neighbors;

            
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
