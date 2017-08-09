using System;
using System.Windows;
using System.Windows.Shapes;
using static System.Math;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        private const double HEXAGON_SIDE_LENGTH = 10;
        private const double HEXAGON_VERTICAL_LENGTH = 2 * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        // total size = (line + ((line/2)/sqrt(3)) *2) -> 

        public MainWindow() {
            InitializeComponent();
            BeepWorld bw = new BeepWorld(4, 4);

            Line line = new Line() {
                X1 = 20,
                X2 = 20,
                Y1 = 20,
                Y2 = 20 + HEXAGON_SIDE_LENGTH,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                StrokeThickness = 100
            };
            canvas.Children.Add(line);

            for (int j = -bw.Size.Y + 1; j < bw.Size.Y; j++) {
                if (j % 2 == 0) Console.Write(" ");
                for (int i = 0; i < bw.Size.X; i++) {
            //        Console.Write(" " + i + "," + j + " ");
                    //Console.Write(" * ");
                }
            //    Console.WriteLine("");
            }
        }
    }
}
