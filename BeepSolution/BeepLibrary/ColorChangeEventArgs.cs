using System.Windows.Media;
using System;
namespace Beep {
    /// <summary>
    /// Provides two color values for the Beep.Tile.ColorChanged event.
    /// </summary>
    public class ColorChangeEventArgs : EventArgs {
        public Color OldColor { get; }
        public Color NewColor { get; }
        public ColorChangeEventArgs(Color oldColor, Color newColor) : base() {
            OldColor = oldColor;
            NewColor = newColor;
        }
    }
}
