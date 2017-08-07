using System;
using System.Collections.Generic;

// TODO
// -generate world
// -possible database functionality => save random generated world seed/parameters to database

namespace Beep {

    // this class is designed to show the generated world
    class BeepConsole {

        // 
        static void Main(string[] args) {
            Console.WriteLine("Hello (generated) world!");
            //BeepWorld bw = new BeepWorld();

            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(698, 350);
            Console.Beep(523, 150);
            Console.Beep(415, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            
            Console.ReadKey();
        }
    }

    // this class is a generated world
    class BeepWorld {

        public int Size { get; set; }
        private List<Tile> tiles;

        internal BeepWorld() {
            tiles = new List<Tile>();



            for (int i=0; i<5; i++) {
                for (int j=0; j<5; j++) {
                    Tile t1 = new Tile(i, j);

                    if (i != 0 && j != 0) {
                        Tile t2 = new Tile(-i, -j);
                    }
                }
            }
        }
    }

    class Tile {
        public List<Tile> Neighbors { get; set; }
        public Point Coordinates { get; set; }
        internal Tile(int x, int y) {
            this.Coordinates = new Point(x, y);
        }
    }

    struct Point {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) {
            X = x;
            Y = y;
        }
    }
}
