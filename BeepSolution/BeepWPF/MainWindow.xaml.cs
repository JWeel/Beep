using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Text;
using DrawPoint = System.Windows.Point;
using static System.Math;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        // hexagon length values. only change HEXAGON_SIDE_LENGTH !
        private const double HEXAGON_SIDE_LENGTH = 30;
        private static readonly double HEXAGON_VERTICAL_LENGTH = 2 * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
        //private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_HORIZONTAL_HALF / Sqrt(3); // <- does same thing as sidelength /2
        private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_SIDE_LENGTH / 2;

        //
        private static readonly Brush HEXAGON_BORDER_COLOR = Brushes.LavenderBlush;
        private static readonly Brush HEXAGON_FILL_COLOR = Brushes.Ivory;

        private BeepWorld bw;
        private Polygon previousHexagon;

        public MainWindow() {
            InitializeComponent();

            //canvas.Visibility = Visibility.Hidden;

            //bw = new BeepWorld(22, 26);
            bw = new BeepWorld(5, 5);

            double relativeX = 0;
            double relativeY = HEXAGON_VERTICAL_EDGE;

            foreach (Tile t in bw.tiles.Values) {
                int xCoordinate = t.Coordinates.X;
                int yCoordinate = t.Coordinates.Y;

                double posX = HEXAGON_SIDE_LENGTH * Sqrt(3) * (xCoordinate + yCoordinate / 2) + relativeX;
                double posY = HEXAGON_SIDE_LENGTH * (3 / 2) * yCoordinate + relativeY + (yCoordinate * relativeY);

                //double trueX = HEXAGON_SIDE_LENGTH * (3 / 2) * xCoordinate;
                //double trueY = HEXAGON_SIDE_LENGTH * Sqrt(3) * (yCoordinate + xCoordinate / 2);

                //double posY = trueY * (HEXAGON_VERTICAL_LENGTH / 2 + HEXAGON_SIDE_LENGTH / 2) + relativeY;
                //double posX = trueX * HEXAGON_HORIZONTAL_LENGTH + relativeX;
                if (yCoordinate % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;


                Polygon hexPolygon = MakeHexagon(posX, posY);
                RegisterName(HexagonPointToName(t.Coordinates), hexPolygon);
                canvas.Children.Add(hexPolygon);

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


            /*
            double clickedX = p.X;
            double clickedY = p.Y;

            double axialX = (clickedX * Sqrt(3) / 3 - clickedY / 3) / HEXAGON_SIDE_LENGTH;
            double axialY = clickedY * 2 / 3 / HEXAGON_SIDE_LENGTH;

            Polygon po = (Polygon)this.FindName(HexagonPointToName(new Point((int)axialX, (int)axialY)));
            if (po != null && po != previousHexagon) {
                po.Fill = Brushes.Aqua;
                if (previousHexagon != null) previousHexagon.Fill = HEXAGON_FILL_COLOR;
                previousHexagon = po;
            }
            */
        }

        //
        private void OnMouseLeftClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var p = e.GetPosition(this);

            // find clicked hexagon
            double clickedX = p.X - HEXAGON_HORIZONTAL_HALF; // DONT ASK ME WHY
            double clickedY = p.Y - HEXAGON_SIDE_LENGTH; // I HAVE NO IDEA

            double axialX = (clickedX * Sqrt(3) / 3 - clickedY / 3) / HEXAGON_SIDE_LENGTH;
            double axialY = clickedY * 2 / 3 / HEXAGON_SIDE_LENGTH;

            
            var cubeX = axialX;
            var cubeZ = axialY;
            var cubeY = -axialX - axialY;

            var roundX = Round(cubeX);
            var roundY = Round(cubeY);
            var roundZ = Round(cubeZ);

            Debug.WriteLine("aX" + axialX + "aY" + axialY + "rx" + roundX + "ry" + roundY + "rz" + roundZ);

            var xdif = Abs(roundX - cubeX);
            var ydif = Abs(roundY - cubeY);
            var zdif = Abs(roundZ - cubeZ);

            if (xdif > ydif && xdif > zdif) roundX = -roundY - roundZ;
            else if (ydif > zdif) roundY = -roundX - roundZ;
            else roundZ = -roundX - roundY;

            double trueX = roundX;
            double trueY = roundZ;
            

            //double nearestTileX = x - (x % HEXAGON_HORIZONTAL_LENGTH);
            //double nearestTileY = y - (y % (HEXAGON_SIDE_LENGTH + 2 * HEXAGON_VERTICAL_EDGE));


            //MouseTextCopy.Text = (int)(axialX) + " , " + (int)(axialY);
            MouseTextCopy.Text = (int)(trueX) + " , " + (int)(trueY);



            Polygon po = (Polygon)this.FindName(HexagonPointToName(new Point((int)trueX, (int)trueY)));
            if (po != null && po != previousHexagon) {
                po.Fill = Brushes.Aqua;
                if (previousHexagon != null) previousHexagon.Fill = HEXAGON_FILL_COLOR;
                previousHexagon = po;
            }
        }

        private string HexagonPointToName(Point p) {
            return string.Format("hexX{0}{1}Y{2}{3}", (p.X < 0 ? "n" : ""), Abs(p.X), (p.Y < 0 ? "n" : ""), Abs(p.Y));
        }

        private Point HexagonNameToPoint(string name) {
            string[] s = name.Substring(4).Split('Y'); // skip hexX and split over Y
            int x, y;
            if (s[0][0] == 'n') x = int.Parse(s[0].Substring(1)) * -1;
            else x = int.Parse(s[0]);
            if (s[1][0] == 'n') y = int.Parse(s[1].Substring(1)) * -1;
            else y = int.Parse(s[1]);
            return new Point(x, y);
        }
    }
}
