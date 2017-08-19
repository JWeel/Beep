using System.Windows.Media;
using System;
namespace Beep {
    /// <summary>
    /// Provides two color values for the Beep.Tile.ColorChanged event.
    /// </summary>
    internal class ColorChangeEventArgs : EventArgs {
        internal Color OldColor { get; }
        internal Color NewColor { get; }
        internal ColorChangeEventArgs(Color oldColor, Color newColor) : base() {
            OldColor = oldColor;
            NewColor = newColor;
        }
    }
}
