using System;
using System.Collections.Generic;
//using System.Drawing; // Point structure

// TODO
// -generate world
// -possible database functionality => save random generated world seed/parameters to database

namespace Beep {
    // this class is a generated world
    public class BeepWorld {

        public Point Size { get; set; }
        //private List<Tile> tiles;
        //private Tile[,] tiles;
        private Dictionary<Point, Tile> tiles;
        //private NegativeArray<Tile> tiles;

        public BeepWorld(Point p) : this(p.X, p.Y) { }

        public BeepWorld(int sizeX, int sizeY) {
            this.Size = new Point(sizeX, sizeY);

            //tiles = new List<Tile>();
            //tiles = new Tile[sizeX, sizeY*2 -1];
            tiles = new Dictionary<Point, Tile>();
            //tiles = new NegativeArray<Tile>(sizeY);

            // add tiles
            for (int i = 0; i < Size.X; i++) {
                for (int j = 0; j < Size.Y; j++) tiles[new Point(i, j)] = new Tile(i, j);
                for (int j = -Size.Y + 1; j < 0; j++) tiles[new Point(i, j)] = new Tile(i, j);
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


    class Tile {
        public List<Tile> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        internal Tile(int x, int y) {
            this.Coordinates = new Point(x, y);
        }
        internal Tile(Point p) {
            this.Coordinates = p;
        }
    }

    class NegativeArray<ArrayType> {
        private ArrayType[] array;
        private int offset;
        public NegativeArray(int offset) {
            array = new ArrayType[offset * 2 - 1];
            this.offset = offset;
        }
        public ArrayType this[int index] {
            get { return this.array[index - offset]; }
            set { this.array[index - offset] = value; }
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
