using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Collections.Generic;
using DrawPoint = System.Windows.Point;
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

            BeepWorld bw = new BeepWorld(22, 26);

            //foreach (Line line in MakeHexagon(30, 30)) {
            //    canvas.Children.Add(line);
            //}

            double relativeX = 0;
            double relativeY = HEXAGON_VERTICAL_EDGE;

            //for (int j = -bw.Size.Y + 1; j < bw.Size.Y; j++) {
            for (int j = 100000; j < bw.Size.Y; j++) {

                double posY = j * (HEXAGON_VERTICAL_LENGTH/2 + HEXAGON_SIDE_LENGTH/2) + relativeY;

                //if (j % 2 == 1) posY -= HEXAGON_VERTICAL_EDGE;
                //if (Abs(j % 2) == 1) continue;
                //if (j < 0) continue;

                for (int i = 0; i < bw.Size.X; i++) {
                //for (int i = -bw.Size.X+1; i < bw.Size.X; i++) {
                        //        Console.Write(" " + i + "," + j + " ");
                        //Console.Write(" * ");
                    double posX = i * HEXAGON_HORIZONTAL_LENGTH + relativeX;

                    if (j % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;
                    
                    canvas.Children.Add(MakeHexagon(posX, posY));
                    Label label = new Label() {
                        Foreground = new SolidColorBrush(Colors.White),
                        Content = i + "," + j,
                        FontSize = 6,
                        RenderTransform = new TranslateTransform { X = posX - HEXAGON_SIDE_LENGTH / 4, Y = posY - HEXAGON_SIDE_LENGTH / 2 }
                    };
                    canvas.Children.Add(label);
                }
            //    Console.WriteLine("");
            }

            foreach (Tile t in bw.tiless) {
                int xCoordinate = t.Coordinates.X;
                int yCoordinate = t.Coordinates.Y;
                double posY = yCoordinate * (HEXAGON_VERTICAL_LENGTH / 2 + HEXAGON_SIDE_LENGTH / 2) + relativeY;
                double posX = xCoordinate * HEXAGON_HORIZONTAL_LENGTH + relativeX;
                if (yCoordinate % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;

                canvas.Children.Add(MakeHexagon(posX, posY));
                Label label = new Label() {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = xCoordinate + "," + yCoordinate,
                    FontSize = 6,
                    RenderTransform = new TranslateTransform { X = posX - HEXAGON_SIDE_LENGTH / 8, Y = posY - HEXAGON_SIDE_LENGTH / 2 }
                };
                canvas.Children.Add(label);
            }
        }

        // returns a polygon of six points representing a hexagon
        private Polygon MakeHexagon(double posX, double posY) {
            return new Polygon() {
                Points = new PointCollection {
                    new DrawPoint(posX, posY),
                    new DrawPoint(posX, posY + HEXAGON_SIDE_LENGTH),
                    new DrawPoint(posX + HEXAGON_HORIZONTAL_HALF, posY + HEXAGON_SIDE_LENGTH + HEXAGON_VERTICAL_EDGE),
                    new DrawPoint(posX + HEXAGON_HORIZONTAL_LENGTH, posY + HEXAGON_SIDE_LENGTH),
                    new DrawPoint(posX + HEXAGON_HORIZONTAL_LENGTH, posY),
                    new DrawPoint(posX + HEXAGON_HORIZONTAL_HALF, posY - HEXAGON_VERTICAL_EDGE)
                },
                Stroke = Brushes.LightSteelBlue,
                Fill = Brushes.Black
            };
        }
        
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            var p = e.GetPosition(this);
            MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);
        }
    }
}
