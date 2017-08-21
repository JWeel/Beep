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
using Beep.RuleUI;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using System.Threading.Tasks;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        //private static readonly Point BEEP_SIZE = new Point(8, 7); // best with 42.8
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // best with 20
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // 10
        private static Point BEEP_SIZE = new Point(49, 45); // 7
        //private static readonly Point BEEP_SIZE = new Point(46, 53); // 5

        private const bool BEEP_BOXED = true;

        // hexagon length values. only change HEXAGON_SIDE_LENGTH 
        private const double HEXAGON_SIDE_LENGTH = 7;
        private static readonly double HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
        private static readonly double HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
        private static readonly double HEXAGON_VERTICAL_EDGE = HEXAGON_SIDE_LENGTH / 2;
        
        //
        private static readonly Color HEXAGON_BORDER_COLOR = (Color)ColorConverter.ConvertFromString("#FF89FB89");
        private static readonly Color HEXAGON_FILL_COLOR = (Color)ColorConverter.ConvertFromString("#FFDEAD"); // NavajoWhite LOL
        private static readonly Color HEXAGON_FUN_COLOR = (Color)ColorConverter.ConvertFromString("#FFFFD700");

        public static Color MouseClickColor = (Color)ColorConverter.ConvertFromString("#FFFFD700");

        // point that is selected by user
        private List<Point> SelectedPointList = new List<Point>();
        private List<Point> ColouredPointList = new List<Point>();

        private bool useMouseDownColorDrag = true;
        private bool isMouseDownColorDragging = false;

        private bool useRelativeBorderColor = false;
        private Color fixedBorderColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        public List<string> RuleMenuItems { get; set; }

        internal ObservableCollection<ColorItem> StandardColorItems = new ObservableCollection<ColorItem>() {
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFFF0000"),"#FFFF0000"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFFFA500"),"#FFFFA500"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFFFFF00"),"#FFFFFF00"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF00FF00"),"#FF00FF00"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF00FFFF"),"#FF00FFFF"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF0000FF"),"#FF0000FF"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFFF00FF"),"#FFFF00FF"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFFFFFFF"),"#FFFFFFFF"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF888888"),"#FF888888"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF000000"),"#FF000000")
        };

        //private List<Color> UsedColors;
        //internal ObservableCollection<ColorItem> UsedColorItems = new ObservableCollection<ColorItem>();

        // random number
        Random rand = new Random();

        // list containing the rules used for painting generation, and list for corresponding UI
        private List<BeepRule> beepRules;
        private ObservableCollection<BeepRuleUserControl> BeepRulesUIComponents;

        private BeepWorld bw;
        private Polygon selectedHexagon;

        public MainWindow() {
            InitializeComponent();
            
            bw = new BeepWorld(BEEP_SIZE, BEEP_BOXED);

            beepRules = new List<BeepRule>();
            BeepRulesUIComponents = new ObservableCollection<BeepRuleUserControl>();

            lbRules.ItemsSource = BeepRulesUIComponents;

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

            RuleMenuItems = new List<String> {
                BeepRule.RULE_CHANGE_COLOR, 
                BeepRule.RULE_CHANGE_NEIGHBOR_COLOR,
                BeepRule.RULE_RANDOM_CHANGE,
                BeepRule.RULE_VIRUS
            };

            Refresh();
            UpdateUsedColors();
            clrPickMouse.StandardColors = StandardColorItems;
        }

        //private void UpdateUsedColors(object sender, ColorChangeEventArgs e) {
        private void UpdateUsedColors() {
            List<Color> usedColors = new List<Color>();
            Parallel.ForEach(bw.tiles.Values, tile => {
                if (!usedColors.Contains(tile.Color)) usedColors.Add(tile.Color);
            });
            UpdateColorPickers(ColorsToColorItems(usedColors));
        }

        // TODO rename to UpdateUsedColorPickers
        private void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickMouse.AvailableColors = usedColorItems;
            foreach (BeepRuleUserControl bruc in BeepRulesUIComponents) bruc.UpdateColorPickers(usedColorItems);
        }

        private ObservableCollection<ColorItem> ColorsToColorItems(List<Color> usedColors) {
            ObservableCollection<ColorItem> usedColorItems = new ObservableCollection<ColorItem>();
            usedColors.ForEach(color => usedColorItems.Add(new ColorItem(color, color.ToString())));
            return usedColorItems;
        }

        private void ColourListTiles(List<Point> listPoint) {
            foreach (Point m in listPoint) {
                Polygon temp = (Polygon)this.FindName(HexagonPointToName(m));
                if (temp != null) temp.Fill = new SolidColorBrush(HEXAGON_FUN_COLOR);
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
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5
            };
        }

        //
        private void Refresh() {
            foreach (Tile t in bw.tiles.Values) {
                Polygon po = (Polygon)FindName(HexagonPointToName(t.Coordinates));
                if ((po.Fill as SolidColorBrush).Color != t.Color) po.Fill = new SolidColorBrush(t.Color);
                if (useRelativeBorderColor && (po.Stroke as SolidColorBrush).Color != t.Color) po.Stroke = po.Fill;
            }
            //UpdateRules();
            //UpdateColorPickers();
        }

        //
        private void UpdateRules() {
            foreach (BeepRule rule in beepRules) rule.Update(bw.tiles);
        }

        //
        private void OnMouseMove(object sender, MouseEventArgs e) {
            PixelPoint p = e.GetPosition(sender as IInputElement);
            Point axialPoint = MouseCoordinatesToAxialCoordinates(p.X, p.Y);
            Polygon po = (Polygon)this.FindName(HexagonPointToName(axialPoint));

            MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);

            if (po != null) {
                if (useMouseDownColorDrag && isMouseDownColorDragging) {
                    if ((po.Fill as SolidColorBrush).Color != MouseClickColor) {
                        bw.tiles[axialPoint].Color = MouseClickColor;
                        po.Fill = new SolidColorBrush(MouseClickColor);
                        if (useRelativeBorderColor && (po.Stroke as SolidColorBrush).Color != MouseClickColor) po.Stroke = po.Fill;
                    }
                } else {
                    if (po != selectedHexagon) {
                        po.Fill = Brushes.SlateGray;
                        po.Stroke = Brushes.SlateGray;

                        if (selectedHexagon != null && (selectedHexagon.Fill as SolidColorBrush).Color == (Brushes.BlueViolet as SolidColorBrush).Color) {
                            selectedHexagon = null;
                            //return;
                        }

                        if (selectedHexagon != null) {
                            selectedHexagon.Fill = new SolidColorBrush(bw.tiles[HexagonNameToPoint(selectedHexagon.Name)].Color);
                            if (useRelativeBorderColor) selectedHexagon.Stroke = selectedHexagon.Fill;
                            else selectedHexagon.Stroke = new SolidColorBrush(fixedBorderColor);
                        }
                        selectedHexagon = po;
                    }
                }
            }
        }

        //
        private void OnMouseLeftDown(object sender, MouseButtonEventArgs e) {
            PixelPoint p = e.GetPosition(sender as IInputElement);
            Point axialPoint = MouseCoordinatesToAxialCoordinates(p.X, p.Y);
            if (bw.tiles.ContainsKey(axialPoint)) {
                bw.tiles[axialPoint].Color = MouseClickColor;
                UpdateRules();
                UpdateUsedColors();
            }
            MouseTextCopy.Text = axialPoint.X + " , " + axialPoint.Y;

            if (useMouseDownColorDrag) isMouseDownColorDragging = true;
        }

        // 
        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e) {
            if (useMouseDownColorDrag && isMouseDownColorDragging) isMouseDownColorDragging = false;
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

        //
        private Brush PickBrush() {
            Brush result = Brushes.Transparent;
            
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rand.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;
        }

        // sets the color property of all tiles to the default color
        private void BtnClearClick(object sender, RoutedEventArgs e) {
            Parallel.ForEach(bw.tiles.Values, tile => { tile.Color = Tile.DEFAULT_COLOR; });
            //foreach (Tile t in bw.tiles.Values) t.Color = Tile.DEFAULT_COLOR;
            Refresh();
            //UpdateRules(); // <= not needed because tiles reference is shared with rules
            UpdateUsedColors();
        }

        private void BtnNewRuleClick(object sender, RoutedEventArgs e) {

            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;

            //BeepRule virus = BeepRule.CreateBeepRule(BeepRule.RULE_VIRUS, bw.tiles);
            //beepRules.Add(virus);

            //BeepRuleUserControl bruc = new VirusRuleUserControl(virus as VirusRule);

            //bruc.SelectedRule += RuleUserControlRuleSelection;
            //bruc.Deleting += DeleteRuleUserControl;
            //BeepRulesUIComponents.Add(bruc);
        }
        
        private void AddRuleClick(object sender, RoutedEventArgs e) { 
            BeepRule br = BeepRule.Create((sender as MenuItem).Header.ToString(), bw.tiles);
            beepRules.Add(br);

            BeepRuleUserControl bruc = BeepRuleUserControl.Create(br);
            bruc.SelectedRule += RuleUserControlRuleSelection;
            bruc.Deleting += DeleteRuleUserControl;
            bruc.PrepareColorPickers(StandardColorItems);
            bruc.UpdateColorPickers(clrPickMouse.AvailableColors);
            BeepRulesUIComponents.Add(bruc);
        }

        //
        private void RuleUserControlRuleSelection(object sender, EventArgs e) {
            BeepRuleUserControl bruc = sender as BeepRuleUserControl;
            //return;
            // selected rule must be different
            if (bruc.SelectedRuleName == bruc.RuleName) return;

            beepRules.Remove(bruc.Rule);
            BeepRulesUIComponents.Remove(bruc);

            BeepRule br = BeepRule.Create(bruc.SelectedRuleName, bw.tiles);
            beepRules.Add(br);

            bruc = BeepRuleUserControl.Create(br);
            bruc.SelectedRule += RuleUserControlRuleSelection;
            bruc.Deleting += DeleteRuleUserControl;
            bruc.PrepareColorPickers(StandardColorItems);
            bruc.UpdateColorPickers(clrPickMouse.AvailableColors);
            BeepRulesUIComponents.Add(bruc);
        }

        // deletes a rule
        private void DeleteRuleUserControl(object sender, EventArgs e) {
            BeepRuleUserControl bruc = sender as BeepRuleUserControl;
            beepRules.Remove(bruc.Rule);
            BeepRulesUIComponents.Remove(bruc);
        }

        // paints according to defined rules
        private async void BtnPaintClick(object sender, RoutedEventArgs e) {
            int? number = iudAmountPicker1.Value;
            await Task.Run(() => {
                for (int i = 0; i < number; i++) {
                    //Task.Run(() => {
                        foreach (BeepRule rule in beepRules) {
                            bw.tiles = rule.Run();
                            UpdateRules();
                        }
                        //    Refresh();

                    //});
                    this.Dispatcher.Invoke(() => {
                        Refresh();
                    });
                    //Task.WaitAll();
                    // TODO async Refresh();
                }
            });
            //t.Wait();
            Refresh();
            UpdateUsedColors();
        }
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
          
        }

        // experimental drag drop code
        private void ListBoxPreviewMouseMoved(object sender, MouseEventArgs e) {
            return;
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed) {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, new DataObject("abc", draggedItem.DataContext), DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }
        private void ListBoxRuleDropped(object sender, DragEventArgs e) {
            return;
            BeepRuleUserControl droppedData = e.Data.GetData("abc") as BeepRuleUserControl;
            BeepRuleUserControl target = ((ListBoxItem)(sender)).DataContext as BeepRuleUserControl;

            int removedIdx = lbRules.Items.IndexOf(droppedData);
            int targetIdx = lbRules.Items.IndexOf(target);

            if (removedIdx < targetIdx) {
                BeepRulesUIComponents.Insert(targetIdx + 1, droppedData);
                BeepRulesUIComponents.RemoveAt(removedIdx);
            }
            else {
                int remIdx = removedIdx + 1;
                if (BeepRulesUIComponents.Count + 1 > remIdx) {
                    BeepRulesUIComponents.Insert(targetIdx, droppedData);
                    BeepRulesUIComponents.RemoveAt(remIdx);
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {

            SaveFileDialog sfd = new SaveFileDialog() {
                Filter = "Text Document|*.txt",
                FileName = "Painting.txt",
                DefaultExt = ".txt"
            };
            bool? result = sfd.ShowDialog();
            string createText = "" + Environment.NewLine;
            if (result.HasValue && result.Value) {
                
                foreach (Point Key in bw.tiles.Keys) {

                    createText = createText + String.Format("{0}:{1}", Key, bw.tiles[Key].Color) + Environment.NewLine;
                }
                Debug.WriteLine(createText);
                string path = sfd.FileName;

                File.WriteAllText(path, createText);
            }

        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog open = new OpenFileDialog() {
                Filter = "Text Document|*.txt"
            };
            string line;
            bool? result = open.ShowDialog();

            if(result == true) {
               

                StreamReader file = new StreamReader(open.FileName);
                while((line = file.ReadLine()) != null) {

                    foreach (Point Key in bw.tiles.Keys) {
                        line = file.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line)) {


                            Debug.WriteLine(line);

                            string[] lines = line.Split(':');


                            bw.tiles[Key].Color = (Color)ColorConverter.ConvertFromString(lines[1]); 
                        }

                    }
                       

                }
                    Refresh();
                file.Close();
            }
               
            
        }

        private void btnBeepSize_Click(object sender, RoutedEventArgs e) {
            
        }

        private void MouseColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            MouseClickColor = (Color)clrPickMouse.SelectedColor;
        }
    }
}
