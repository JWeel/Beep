using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using PixelPoint = System.Windows.Point;
using static System.Math;
using System.Collections.Generic;
using System;
using System.Reflection;
using Beep.Rules;
using Microsoft.Win32;
using System.IO;
using System.Xml;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        //private static readonly Point BEEP_SIZE = new Point(8, 7); // best with 42.8
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // best with 20
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // 10
        private static readonly Point BEEP_SIZE = new Point(49, 45);
        //private static readonly Point BEEP_SIZE = new Point(46, 53); // 5

        private const bool BEEP_BOXED = true;

        // hexagon length values. only change HEXAGON_SIDE_LENGTH 
        private const double HEXAGON_SIDE_LENGTH = 7;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
        private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_SIDE_LENGTH / 2;

        //
        private static readonly Brush HEXAGON_BORDER_COLOR = Brushes.LightGray;
        private static readonly Brush HEXAGON_FILL_COLOR = Brushes.NavajoWhite; // NavajoWhite LOL
		private static readonly Brush HEXAGON_FUN_COLOR = Brushes.Gold;

        // point that is selected by user
        private List<Point> SelectedPointList = new List<Point>();
        private List<Point> ColouredPointList = new List<Point>();

        private List<MenuItem> RuleMenuItems;

        // random number
        Random rand = new Random();

        private List<BeepRule> rules;

        private BeepWorld bw;
        private Polygon selectedHexagon;

        public MainWindow() {
            InitializeComponent();
            
            bw = new BeepWorld(BEEP_SIZE, BEEP_BOXED);

            rules = new List<BeepRule>();

            lbRules.ItemsSource = rules;

            double relativeX = 0;
            double relativeY = HEXAGON_VERTICAL_EDGE;

            foreach (Tile t in bw.tiles.Values) {
                int xCoordinate = t.Coordinates.X;
                int yCoordinate = t.Coordinates.Y;

                // apply axial conversion
                double posX = HEXAGON_SIDE_LENGTH * Sqrt(3) * (xCoordinate + yCoordinate / 2);
                double posY = HEXAGON_SIDE_LENGTH * (3 / 2) * yCoordinate;

                // add offset
                posX += relativeX;
                posY += relativeY + (yCoordinate * relativeY);

                // apply odd row offset
                if (yCoordinate % 2 != 0) posX += HEXAGON_HORIZONTAL_HALF;

                // create polygon to be placed on canvas
                Polygon hexPolygon = MakeHexagon(posX, posY);
                string name = HexagonPointToName(t.Coordinates);
                hexPolygon.Name = name;
                RegisterName(name, hexPolygon);
                canvas.Children.Add(hexPolygon);

                continue;
                Label label = new Label() {
                    Foreground = new SolidColorBrush(Colors.Indigo),
                    Content = xCoordinate + "," + yCoordinate,
                    FontSize = 6,
                    RenderTransform = new TranslateTransform { X = posX - HEXAGON_SIDE_LENGTH / 8, Y = posY - HEXAGON_SIDE_LENGTH / 2 }
                };
                canvas.Children.Add(label);
            }

            RuleMenuItems = new List<MenuItem>() {
                new MenuItem() {
                    Header = BeepRule.RULE_CHANGE_COLOR
                },
                new MenuItem() {
                    Header = BeepRule.RULE_CHANGE_NEIGHBOR_COLOR
                },
                new MenuItem() {
                    Header = BeepRule.RULE_RANDOM_CHANGE
                },
                new MenuItem() {
                    Header = BeepRule.RULE_VIRUS
                }
            };


            Refresh();
        }

        private void ColourListTiles(List<Point> listPoint) {
            foreach (Point m in listPoint)
            {
                Polygon temp = (Polygon)this.FindName(HexagonPointToName(m));
                if (temp != null)
                    temp.Fill = HEXAGON_FUN_COLOR;
                 ColouredPointList.Add(m);
                
            }
        }
												
        // returns a polygon of six points representing a hexagon
        private Polygon MakeHexagon(double posX, double posY) {
            return new Polygon() {
                Points = new PointCollection {
                    new PixelPoint(posX, posY),
                    new PixelPoint(posX, posY + HEXAGON_SIDE_LENGTH),
                    new PixelPoint(posX + HEXAGON_HORIZONTAL_HALF, posY + HEXAGON_SIDE_LENGTH + HEXAGON_VERTICAL_EDGE),
                    new PixelPoint(posX + HEXAGON_HORIZONTAL_LENGTH, posY + HEXAGON_SIDE_LENGTH),
                    new PixelPoint(posX + HEXAGON_HORIZONTAL_LENGTH, posY),
                    new PixelPoint(posX + HEXAGON_HORIZONTAL_HALF, posY - HEXAGON_VERTICAL_EDGE)
                },
                Stroke = HEXAGON_BORDER_COLOR,
                Fill = HEXAGON_FILL_COLOR
            };
        }

        //
        private void Refresh() {
            foreach (Tile t in bw.tiles.Values) {
                Polygon po = (Polygon)FindName(HexagonPointToName(t.Coordinates));
                po.Fill = t.Color;
            }
        }

        //
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
			//return;
            PixelPoint p = e.GetPosition(sender as IInputElement);
            MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);

            Point axialPoint = MouseCoordinatesToAxialCoordinates(p.X, p.Y);
			
            //SelectedPointList.Add(axialPoint);

            Polygon po = (Polygon)this.FindName(HexagonPointToName(axialPoint));

            if (po != null && po != selectedHexagon) {
                po.Fill = Brushes.SlateGray;


                if (selectedHexagon != null && (selectedHexagon.Fill as SolidColorBrush).Color == (Brushes.BlueViolet as SolidColorBrush).Color) {
                    selectedHexagon = null;
                    //return;
                }


                if (selectedHexagon != null) selectedHexagon.Fill = bw.tiles[HexagonNameToPoint(selectedHexagon.Name)].Color;
                selectedHexagon = po;
            }
        }

        //
        private void OnMouseLeftClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            //return;
            PixelPoint p = e.GetPosition(sender as IInputElement);

            Point axialPoint = MouseCoordinatesToAxialCoordinates(p.X, p.Y);
            //SelectedPointList.Add(axialPoint);								  
            //Polygon po = (Polygon)this.FindName(HexagonPointToName(axialPoint));
            //if (po != null) {
            //    po.Fill = Brushes.BlueViolet;
            //    //if (selectedHexagon != null) selectedHexagon.Fill = HEXAGON_FILL_COLOR;
            //    //selectedHexagon = po;
            //}
            if (bw.tiles.ContainsKey(axialPoint)) bw.tiles[axialPoint].Color = Brushes.Orange;
            MouseTextCopy.Text = axialPoint.X + " , " + axialPoint.Y;
            Refresh();
        }

        // converts coordinates of mouse to axial coordinates
        private Point MouseCoordinatesToAxialCoordinates(double mouseX, double mouseY) {
            // invert offset
            double offsetX = mouseX - HEXAGON_HORIZONTAL_HALF; // DONT ASK ME WHY
            double offsetY = mouseY - HEXAGON_SIDE_LENGTH; // I HAVE NO IDEA

            // apply axial conversion
            double axialX = (offsetX * Sqrt(3) / 3 - offsetY / 3) / HEXAGON_SIDE_LENGTH;
            double axialY = offsetY * 2 / 3 / HEXAGON_SIDE_LENGTH;

            // convert axial to cube coordinates
            double cubeX = axialX;
            double cubeZ = axialY;
            double cubeY = -axialX - axialY;
            double roundX = Round(cubeX);
            double roundY = Round(cubeY);
            double roundZ = Round(cubeZ);

            // find structure so roundX + roundY + roundZ == 0
            double xdif = Abs(roundX - cubeX);
            double ydif = Abs(roundY - cubeY);
            double zdif = Abs(roundZ - cubeZ);
            if (xdif > ydif && xdif > zdif) roundX = -roundY - roundZ;
            else if (ydif > zdif) roundY = -roundX - roundZ;
            else roundZ = -roundX - roundY;

            // cast to int and return
            int trueX = (int) roundX;
            int trueY = (int) roundZ;
            return new Point(trueX, trueY);
        }

        // creates string representation of hexagon coordinates that can be used f0r WPF component name
        private string HexagonPointToName(Point p) {
            return string.Format("hexX{0}{1}Y{2}{3}", (p.X < 0 ? "n" : ""), Abs(p.X), (p.Y < 0 ? "n" : ""), Abs(p.Y));
        }

        // extracts hexagon coordinates from WPF component name defined by HeaxgonPointToName(Point)
        private Point HexagonNameToPoint(string name) {
            string[] s = name.Substring(4).Split('Y'); // skip hexX and split over Y
            int x, y;
            if (s[0][0] == 'n') x = int.Parse(s[0].Substring(1)) * -1;
            else x = int.Parse(s[0]);
            if (s[1][0] == 'n') y = int.Parse(s[1].Substring(1)) * -1;
            else y = int.Parse(s[1]);
            return new Point(x, y);
        }


        

       
        
        //private void btnRandomizeClick(object sender, RoutedEventArgs e) {
        //    Random rnd = new Random();
        //    Random rnd2 = new Random();
        //    int percentage = 50;
        //    int numTiles = (BEEP_SIZE.X * BEEP_SIZE.Y)/2;
        //    Brush randomColor = Brushes.HotPink;

        //    List<Tile> ListTiles = new List<Tile>();

        //    foreach (Tile t in bw.tiles.Values) {
        //        int chooser = rnd.Next(0, 2);
        //        if(chooser == 1) {
        //            //t.Color = HEXAGON_FILL_COLOR;
                                 
        //        }
        //        else {
                  
        //            t.Color = PickBrush();

        //        }                         
        //    }
        //    Refresh();   
        //}

        private void SpreadVirus() {
            foreach (Tile t in bw.tiles.Values) {
                if (SelectedPointList.Contains(t.Coordinates)) {
                    
                    ColorVirusTiles(t);
                    

                }
                
            }
            SelectedPointList.Clear();
            UpdateVirusTiles();
        }
        private void ColorVirusTiles(Tile t) {

            List<Point> Neighbors = t.Neighbors;
            int i = rand.Next(0, 6);

            foreach (Tile m in bw.tiles.Values) {
                if (m.Coordinates.Equals(Neighbors[i])) {
                    m.Color = Brushes.Firebrick;
                    Refresh();
                }

            }


            //Neighbors.Clear();


        }

        private void UpdateVirusTiles() {
            foreach(Tile t in bw.tiles.Values) {
                if(t.Color != HEXAGON_FILL_COLOR && t.Color != Brushes.BlanchedAlmond) {
                    SelectedPointList.Add(t.Coordinates);
                }
            }
        }

        private Brush PickBrush() {
            Brush result = Brushes.Transparent;
            
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rand.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;

        }

        private void BtnNewRuleClick(object sender, RoutedEventArgs e) {
            rules.Add(BeepRule.CreateBeepRule(BeepRule.RULE_CHANGE_NEIGHBOR_COLOR, bw.tiles, colorArguments: new List<Brush> { Brushes.Orange, Brushes.Azure }, boolArguments: new List<bool> { true }));
            lbRules.ItemsSource = null;
            lbRules.ItemsSource = rules;
        }

        private void BtnPaintClick(object sender, RoutedEventArgs e) {
            foreach (BeepRule rule in rules) {
                bw.tiles = rule.Run();
            }
            //bw.tiles = BeepRule.ChangeColor(bw.tiles, Brushes.BlanchedAlmond, Brushes.MediumAquamarine);
            Refresh();

            foreach(BeepRule rule in rules) {
                rule.Update(bw.tiles);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            List<Tile> temp = new List<Tile>();
            Point y = new Point(1, 1);
            Tile x = new Tile(y);

            x.Color = Brushes.Black;
           

            x.Coordinates = y;
            
            temp.Add(x);
            try {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XML documents|*.xml";
                sfd.FileName = "Painting";
                sfd.DefaultExt = ".xml";

                Nullable<bool> result = sfd.ShowDialog();

                if(result.HasValue && result.Value) {

                    MemoryStream ms = FormatAsXMLStream(temp);
                    
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "Error generating painting", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private MemoryStream FormatAsXMLStream(List<Tile> p) {
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);


            writer.WriteStartDocument();
            //writer.WriteStartElement(p[0].Coordinates.ToString());
           // writer.WriteAttributeString("BeepWorld", String.Format("{0}", B));

        foreach(Tile t in p) {
                writer.WriteStartElement(t.Coordinates.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;

        }
    }
}
