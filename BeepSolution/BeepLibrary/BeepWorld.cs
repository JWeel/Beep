using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Diagnostics;
using System;
//using System.Drawing; // Point structure

// TODO
// -generate painting
// -possible database functionality => save random generated painting seed/parameters to database

namespace Beep {
    // this class is a generated painting
    public class BeepWorld {

        public Point Size { get; set; }
        public bool Boxed { get; set; }
        public Dictionary<Point, Tile> tiles;
        public List<Color> UsedColors;

        public event EventHandler<ColorChangeEventArgs> ColorsChanged;

        public BeepWorld(Point p) : this(p, false) { }
        public BeepWorld(int sizeX, int sizeY) : this(new Point(sizeX, sizeY), false) { }
        public BeepWorld(int sizeX, int sizeY, bool b) : this(new Point(sizeX, sizeY), b) { }

        public BeepWorld(Point p, bool boxed) {
            Size = p;
            Boxed = boxed;
            UsedColors = new List<Color>();
            tiles = new Dictionary<Point, Tile>();
            for (int indexY = 0; indexY < Size.Y; indexY++) {
                int startX = 0 - (indexY / 2);

                int endX = Size.X + startX;
                if (Boxed && indexY % 2 != 0) endX--; // this line makes the grid a box if SizeY is even

                for (int indexX = startX; indexX < endX; indexX++) {
                    Tile t = new Tile(indexX, indexY);
                    t.SetNeighbors(Size, Boxed);
                    //t.ColorChanged += RaiseColorsChanged;
                    tiles.Add(new Point(indexX, indexY), t);
                }
            }
            PrepareColorList();
        }
        public void Resize(Point newSize, bool b) {
            this.Boxed = b;
            this.Size = newSize;
            tiles = new Dictionary<Point, Tile>();
            for (int indexY = 0; indexY < Size.Y; indexY++) {
                int startX = 0 - (indexY / 2);

                int endX = Size.X + startX;
                if (Boxed && indexY % 2 != 0) endX--; // this line makes the grid a box if SizeY is even

                for (int indexX = startX; indexX < endX; indexX++) {
                    Tile t = new Tile(indexX, indexY);
                    t.SetNeighbors(Size, Boxed);
                    tiles.Add(new Point(indexX, indexY), t);
                }
            }
            PrepareColorList();
        }

        private void PrepareColorList() {
            foreach (Tile t in tiles.Values) if (!UsedColors.Contains(t.Color)) UsedColors.Add(t.Color);
        }

        private void RaiseColorsChanged(object sender, ColorChangeEventArgs e) {
            ColorsChanged(this, e);
        }

        private void UpdateColorList(object sender, ColorChangeEventArgs e) {
            if (!UsedColors.Contains(e.NewColor)) UsedColors.Add(e.NewColor);
            foreach (Tile t in tiles.Values) if (t.Color == e.OldColor) return;
            UsedColors.Remove(e.OldColor);
            Debug.WriteLine(UsedColors.Count);
            UsedColors.ForEach(color => Debug.Write(color));
            Debug.WriteLine("");
        }
    }
}
