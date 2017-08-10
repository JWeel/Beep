using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using DrawPoint = System.Windows.Point;
using static System.Math;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        // hexagon length values. only change HEXAGON_SIDE_LENGTH !
        private const double HEXAGON_SIDE_LENGTH = 20;
        private static readonly double HEXAGON_VERTICAL_LENGTH = 2 * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
        //private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_HORIZONTAL_HALF / Sqrt(3); // <- does same thing as sidelength /2
        private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_SIDE_LENGTH / 2;

        //
        private static readonly Brush HEXAGON_BORDER_COLOR = Brushes.LavenderBlush;
        private static readonly Brush HEXAGON_FILL_COLOR = Brushes.Ivory;

        private BeepWorld bw;

        public MainWindow() {
            InitializeComponent();

            //canvas.Visibility = Visibility.Hidden;

            //bw = new BeepWorld(22, 26);
            bw = new BeepWorld(11, 11);

            double relativeX = 0;
            double relativeY = HEXAGON_VERTICAL_EDGE;

            foreach (Tile t in bw.tiles.Values) {
                int xCoordinate = t.Coordinates.X;
                int yCoordinate = t.Coordinates.Y;

                double posX = HEXAGON_SIDE_LENGTH * Sqrt(3) * (xCoordinate + yCoordinate / 2) + relativeX;
                double posY = HEXAGON_SIDE_LENGTH * (3 / 2) * yCoordinate + relativeY + (yCoordinate * HEXAGON_VERTICAL_EDGE);

                //double trueX = HEXAGON_SIDE_LENGTH * (3 / 2) * xCoordinate;
                //double trueY = HEXAGON_SIDE_LENGTH * Sqrt(3) * (yCoordinate + xCoordinate / 2);

                //double posY = trueY * (HEXAGON_VERTICAL_LENGTH / 2 + HEXAGON_SIDE_LENGTH / 2) + relativeY;
                //double posX = trueX * HEXAGON_HORIZONTAL_LENGTH + relativeX;
                if (yCoordinate % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;

                string name = "x" + (xCoordinate < 0 ? "n" : "") + Abs(xCoordinate) + "y" + (yCoordinate < 0 ? "n" : "") + Abs(yCoordinate);

                Polygon p = MakeHexagon(posX, posY);
                RegisterName(name, p);
                canvas.Children.Add(p);

                Label label = new Label() {
                    Foreground = new SolidColorBrush(Colors.Indigo),
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
                Stroke = HEXAGON_BORDER_COLOR,
                Fill = HEXAGON_FILL_COLOR
            };
        }
        
        //
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            var p = e.GetPosition(this);
            MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);
        }

        //
        private void OnMouseLeftClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var p = e.GetPosition(this);

            // find clicked hexagon
            double x = p.X;
            double y = p.Y;

            double nearestTileX = (x * Sqrt(3) / 3 - y / 3) / HEXAGON_SIDE_LENGTH;
            double nearestTileY = y * 2 / 3 / HEXAGON_SIDE_LENGTH;


            //double nearestTileX = x - (x % HEXAGON_HORIZONTAL_LENGTH);
            //double nearestTileY = y - (y % (HEXAGON_SIDE_LENGTH + 2 * HEXAGON_VERTICAL_EDGE));


            MouseTextCopy.Text = (int)(nearestTileX) + " , " + (int)(nearestTileY);


            foreach (Tile t in bw.tiles.Values) ;

            Polygon po = (Polygon) this.FindName("x5y10");
            po.Fill = Brushes.Aqua;
        }
    }
}
