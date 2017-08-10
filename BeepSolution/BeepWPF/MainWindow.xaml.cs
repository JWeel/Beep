using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Collections.Generic;
using static System.Math;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        private const double HEXAGON_SIDE_LENGTH = 10;
        private const double HEXAGON_VERTICAL_LENGTH = 2 * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
        private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_HORIZONTAL_HALF / Sqrt(3);
        // total size = (line + ((line/2)/sqrt(3)) *2) -> 

        public MainWindow() {
            InitializeComponent();

            //canvas.Visibility = Visibility.Hidden;

            BeepWorld bw = new BeepWorld(4, 4);

            //foreach (Line line in MakeHexagon(30, 30)) {
            //    canvas.Children.Add(line);
            //}

            double relativeX = canvas.Width / 2;
            double relativeY = canvas.Height / 2;

            for (int j = -bw.Size.Y + 1; j < bw.Size.Y; j++) {

                double posY = j * (HEXAGON_VERTICAL_LENGTH/2 + HEXAGON_SIDE_LENGTH/2) + relativeY;

                //if (j % 2 == 1) posY -= HEXAGON_VERTICAL_EDGE;
                //if (Abs(j % 2) == 1) continue;
                //if (j < 0) continue;

//                for (int i = 0; i < bw.Size.X; i++) {
                for (int i = -bw.Size.X+1; i < bw.Size.X; i++) {
                        //        Console.Write(" " + i + "," + j + " ");
                        //Console.Write(" * ");
                        double posX = i * HEXAGON_HORIZONTAL_LENGTH + relativeX;


                    if (j % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;

                    foreach (Line line in MakeHexagon(posX, posY)) {
                        canvas.Children.Add(line);
                    }
                    Label label = new Label() {
                        Foreground = new SolidColorBrush(Colors.Black),
                        Content = i + "," + j,
                        FontSize = 6,
                        RenderTransform = new TranslateTransform { X = posX - HEXAGON_SIDE_LENGTH / 4, Y = posY - HEXAGON_SIDE_LENGTH / 2 }
                    };
                    canvas.Children.Add(label);
                }
            //    Console.WriteLine("");
            }
        }

        // returns a list of six lines representing a hexagon
        private List<Line> MakeHexagon(double posX, double posY) {
            //List<System.Windows.Point> hexagonPoints = new List<System.Windows.Point>();
            Polygon p = new Polygon();
            
            System.Windows.Point[] hexagonPoints = new System.Windows.Point[6] {
                new System.Windows.Point(posX, posY),
                new System.Windows.Point(posX, posY),
                new System.Windows.Point(posX, posY),
                new System.Windows.Point(posX, posY),
                new System.Windows.Point(posX, posY),
                new System.Windows.Point(posX, posY)
            };

            #region createpoints
            Line l1 = new Line() {
                X1 = posX,
                X2 = posX,
                Y1 = posY,
                Y2 = posY + HEXAGON_SIDE_LENGTH,
                Stroke = Brushes.LightSteelBlue
            };
            Line l2 = new Line() {
                X1 = posX,
                X2 = posX + HEXAGON_HORIZONTAL_HALF,
                Y1 = posY,
                Y2 = posY - HEXAGON_VERTICAL_EDGE,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue
            };
            Line l3 = new Line() {
                X1 = posX + HEXAGON_HORIZONTAL_LENGTH,
                X2 = posX + HEXAGON_HORIZONTAL_HALF,
                Y1 = posY,
                Y2 = posY - HEXAGON_VERTICAL_EDGE,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue
            };
            Line l4 = new Line() {
                X1 = posX + HEXAGON_HORIZONTAL_LENGTH,
                X2 = posX + HEXAGON_HORIZONTAL_LENGTH,
                Y1 = posY,
                Y2 = posY + HEXAGON_SIDE_LENGTH,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue
            };
            Line l5 = new Line() {
                X1 = posX + HEXAGON_HORIZONTAL_HALF,
                X2 = posX + HEXAGON_HORIZONTAL_LENGTH,
                Y1 = posY + HEXAGON_SIDE_LENGTH + HEXAGON_VERTICAL_EDGE,
                Y2 = posY + HEXAGON_SIDE_LENGTH,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue
            };
            Line l6 = new Line() {
                X1 = posX,
                X2 = posX + HEXAGON_HORIZONTAL_HALF,
                Y1 = posY + HEXAGON_SIDE_LENGTH,
                Y2 = posY + HEXAGON_SIDE_LENGTH + HEXAGON_VERTICAL_EDGE,
                Stroke = System.Windows.Media.Brushes.LightSteelBlue
            };
            #endregion
            return new List<Line>() { l1, l2, l3, l4, l5, l6 };
        }
        
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            var p = e.GetPosition(this);
            MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);
        }
    }
}
