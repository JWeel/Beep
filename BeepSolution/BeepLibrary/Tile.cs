using System.Collections.Generic;
using System.Windows.Media;
using System;
namespace Beep {
    /// <summary>
    /// Represents a tile in the two-dimensional hexagonal grid of an instance of Beep.BeepWorld
    /// </summary>
    public class Tile {

        public static readonly Color DEFAULT_COLOR = (Color)ColorConverter.ConvertFromString("#DAC9A6");

        public List<Point> Neighbors { get; set; }
        public Point Coordinates { get; set; }

        internal event EventHandler<ColorChangeEventArgs> ColorChanged;

        private Color color;
        public Color Color {
            get { return color; }
            set {
                Color oldColor = color;
                color = value;
                ColorChanged?.Invoke(this, new ColorChangeEventArgs(oldColor, value));
            }
        }

        internal Tile(int x, int y) : this(new Point(x, y)) { }

        internal Tile(Point p) {
            this.Coordinates = p;
            this.Color = DEFAULT_COLOR;
        }

        // (deep) copy constructor
        internal Tile(Tile t) {
            this.Coordinates = t.Coordinates;
            this.Color = t.Color;
            this.Neighbors = new List<Point>(t.Neighbors);
            this.ColorChanged = t.GetEventSubscriptions();
        }

        private EventHandler<ColorChangeEventArgs> GetEventSubscriptions() {
            return this.ColorChanged;
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
}
