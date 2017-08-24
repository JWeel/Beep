﻿using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using PixelPoint = System.Windows.Point;
using static System.Math;
using System.Collections.Generic;
using System;
using Beep.Rules;
using Microsoft.Win32;
using System.IO;
using Beep.RuleUI;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace Beep {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {

        //private static readonly Point BEEP_SIZE = new Point(8, 7); // best with 42.8
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // best with 20
        //private static readonly Point BEEP_SIZE = new Point(23, 26); // 10
         // 7
        //private static readonly Point BEEP_SIZE = new Point(46, 53); // 5

        private bool isBoxed = true;
        private bool isBoxedCheckboxControl = false;

        // hexagon length values. only change HEXAGON_SIDE_LENGTH 

        private  double HEXAGON_SIDE_LENGTH = 7; //7
        private  double HEXAGON_HORIZONTAL_LENGTH;
        private  double HEXAGON_HORIZONTAL_HALF;
        private  double HEXAGON_VERTICAL_EDGE;

        
        //
        private static readonly Color HEXAGON_BORDER_COLOR = (Color)ColorConverter.ConvertFromString("#FF89FB89");
        private static readonly Color HEXAGON_FILL_COLOR = (Color)ColorConverter.ConvertFromString("#FFDEAD"); // NavajoWhite LOL
        private static readonly Color HEXAGON_FUN_COLOR = (Color)ColorConverter.ConvertFromString("#FFFFD700");

        public static Color MouseClickColor = (Color)ColorConverter.ConvertFromString("#FFFFD700");

        // point that is selected by user
        private List<Point> SelectedPointList = new List<Point>();
        private List<Point> ColouredPointList = new List<Point>();
        private Point BEEP_SIZE = new Point(49, 45);

        private bool useMouseDownColorDrag = true;
        private bool isMouseDownColorDragging = false;

        private bool paintIndefinitely = false;
        private bool isPaintingIndefinitely = false;

        private bool useRelativeBorderColor = true;
        
        private Color fixedBorderColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        private List<string> registeredHexPolygons = new List<string>();

        public List<string> RuleMenuItems { get; set; }

       // public Brush CanvasBackgroundColor { get; set; }

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
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF000000"),"#FF000000"),

            new ColorItem((Color)ColorConverter.ConvertFromString("#FF880000"),"#FF880000"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF885200"),"#FF885200"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF888800"),"#FF888800"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF008800"),"#FF008800"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF008888"),"#FF008888"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF000088"),"#FF000088"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF880088"),"#FF880088"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FFBBBBBB"),"#FFBBBBBB"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#FF444444"),"#FF444444"),
            new ColorItem((Color)ColorConverter.ConvertFromString("#00FFFFFF"),"#00FFFFFF")
        };

        //private List<Color> UsedColors;
        //internal ObservableCollection<ColorItem> UsedColorItems = new ObservableCollection<ColorItem>();

        // random number
        Random rand = new Random();

        // list containing the rules used for painting generation, and list for corresponding UI
        private List<BeepRule> beepRules;
        private ObservableCollection<BeepRuleUserControl> BeepRulesUIComponents;

        private BeepWorld bw;
        private Polygon highlightedHexPolygon;

        public MainWindow() {
            
            InitializeComponent();
            CalculateHexPolygonSize();

            bw = new BeepWorld(BEEP_SIZE, isBoxed);
            beepRules = new List<BeepRule>(); // TODO should be part of beepworld object

            BeepRulesUIComponents = new ObservableCollection<BeepRuleUserControl>();
            lbRules.ItemsSource = BeepRulesUIComponents;

            PrepareBeepWorldCanvas();

            RuleMenuItems = new List<String> {
                BeepRule.RULE_CHANGE_COLOR, 
                BeepRule.RULE_CHANGE_NEIGHBOR_COLOR,
                BeepRule.RULE_RANDOM_CHANGE,
                BeepRule.RULE_VIRUS,
                BeepRule.RULE_VINCENT,
                BeepRule.RULE_LIFE
            };
            foreach (string ruleMenuItem in RuleMenuItems) {
                MenuItem mi = new MenuItem { Header = ruleMenuItem };
                mi.Click += AddRuleClick;
                mnuAddRule.Items.Add(mi);
            }

            Refresh();
            UpdateUsedColors();
            clrPickMouse.StandardColors = StandardColorItems;
            clrPickBackground.StandardColors = StandardColorItems;
            clrPickBorderColor.StandardColors = StandardColorItems;
        }

        private void CalculateHexPolygonSize() {
         HEXAGON_HORIZONTAL_LENGTH = Sqrt(3) * HEXAGON_SIDE_LENGTH;
         HEXAGON_HORIZONTAL_HALF = HEXAGON_HORIZONTAL_LENGTH / 2;
         HEXAGON_VERTICAL_EDGE = HEXAGON_SIDE_LENGTH / 2;

    }
        // prepares hexpolygons corresponding to tiles in the beepworld on a canvas
        private void PrepareBeepWorldCanvas() {
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
                registeredHexPolygons.Add(name);
                canvas.Children.Add(hexPolygon);
            }
        }

        //private void UpdateUsedColors(object sender, ColorChangeEventArgs e) {
        private void UpdateUsedColors() {
            List<Color> usedColors = new List<Color>();
            Parallel.ForEach(bw.tiles.Values, tile => {
                if (!usedColors.Contains(tile.Color)) usedColors.Add(tile.Color);
            });
            UpdateColorPickers(ColorsToColorItems(usedColors));
        }

        // forwards a new list of coloritems to the various colorpickers in the application
        private void UpdateColorPickers(ObservableCollection<ColorItem> usedColorItems) {
            clrPickMouse.AvailableColors = usedColorItems;
            clrPickBackground.AvailableColors = usedColorItems;
            clrPickBorderColor.AvailableColors = usedColorItems;
            foreach (BeepRuleUserControl bruc in BeepRulesUIComponents) bruc.UpdateColorPickers(usedColorItems);
        }

        // converts a list of colors to a list of coloritems that a xctk colorpicker can use
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
                Stroke = Brushes.Black
                //,StrokeThickness = 0.5
            };
        }

        // updates the color of the hexpolygons corresponding to tiles in the beepworld
        private void Refresh() {
            foreach (Tile t in bw.tiles.Values) {
                Polygon po = (Polygon)FindName(HexagonPointToName(t.Coordinates));
                if ((po.Fill as SolidColorBrush).Color != t.Color) po.Fill = new SolidColorBrush(t.Color);
                if (useRelativeBorderColor) {
                    if ((po.Stroke as SolidColorBrush).Color != t.Color) po.Stroke = po.Fill;
                }
                else if ((po.Stroke as SolidColorBrush).Color != fixedBorderColor) po.Stroke = new SolidColorBrush(fixedBorderColor);
            }
        }

        // forwards the reference to the beepworld's tiles to the beeprules
        // TODO shouldnt this be done by beepworld object? shouldnt beepworld object have the beeprules?
        private void UpdateRules() {
            foreach (BeepRule rule in beepRules) rule.Update(bw.tiles);
        }

        //
        private void OnMouseMove(object sender, MouseEventArgs e) {
            PixelPoint p = e.GetPosition(sender as IInputElement);
            Point axialPoint = MouseCoordinatesToAxialCoordinates(p.X, p.Y);
            Polygon po = (Polygon)this.FindName(HexagonPointToName(axialPoint));

            //MouseText.Text = (int)(p.X) + " , " + (int)(p.Y);

            if (po != null) {
                if (useMouseDownColorDrag && isMouseDownColorDragging) {
                    if ((po.Fill as SolidColorBrush).Color != MouseClickColor) {
                        bw.tiles[axialPoint].Color = MouseClickColor;
                        po.Fill = new SolidColorBrush(MouseClickColor);
                        if (useRelativeBorderColor) {
                            if ((po.Stroke as SolidColorBrush).Color != MouseClickColor) po.Stroke = po.Fill;
                        }
                        else if ((po.Stroke as SolidColorBrush).Color != fixedBorderColor) po.Stroke = new SolidColorBrush(fixedBorderColor);
                    }
                } else {
                    if (po != highlightedHexPolygon) {
                        po.Fill = Brushes.SlateGray;
                        po.Stroke = Brushes.SlateGray;

                        if (highlightedHexPolygon != null && (highlightedHexPolygon.Fill as SolidColorBrush).Color == (Brushes.BlueViolet as SolidColorBrush).Color) {
                            highlightedHexPolygon = null;
                        }

                        if (highlightedHexPolygon != null) {
                            highlightedHexPolygon.Fill = new SolidColorBrush(bw.tiles[HexagonNameToPoint(highlightedHexPolygon.Name)].Color);
                            if (useRelativeBorderColor) highlightedHexPolygon.Stroke = highlightedHexPolygon.Fill;
                            else highlightedHexPolygon.Stroke = new SolidColorBrush(fixedBorderColor);
                        }
                        highlightedHexPolygon = po;
                    }
                }
            } else {
                if (highlightedHexPolygon != null) {
                    highlightedHexPolygon.Fill = new SolidColorBrush(bw.tiles[HexagonNameToPoint(highlightedHexPolygon.Name)].Color);
                    if (useRelativeBorderColor) highlightedHexPolygon.Stroke = highlightedHexPolygon.Fill;
                    else highlightedHexPolygon.Stroke = new SolidColorBrush(fixedBorderColor);
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
            //MouseTextCopy.Text = axialPoint.X + " , " + axialPoint.Y;

            if (useMouseDownColorDrag) isMouseDownColorDragging = true;
        }

        // stops mouse drag functionality, but only if mouse up happened inside the application window
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

        // sets the color property of all tiles to the default color
        private void BtnClearClick(object sender, RoutedEventArgs e) {
            foreach (Tile t in bw.tiles.Values) t.Color = Tile.DEFAULT_COLOR;
            Refresh();
            //UpdateRules(); // <= not needed because tiles reference is shared with rules
            UpdateUsedColors();
        }

        // opens a context menu from which a rule can be chosen
        private void BtnNewRuleClick(object sender, RoutedEventArgs e) {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
        
        // adds a rule and matching UI component based on user selection
        private void AddRuleClick(object sender, RoutedEventArgs e) { 
            BeepRule br = BeepRule.Create((sender as MenuItem).Header.ToString(), bw.tiles);
            beepRules.Add(br);

            BeepRulesUIComponents.Add(CreateBeepRuleUserControl(br));
        }

        //
        private BeepRuleUserControl CreateBeepRuleUserControl(BeepRule rule) {
            BeepRuleUserControl bruc = BeepRuleUserControl.Create(rule);
            bruc.SelectedRule += RuleUserControlRuleSelection;
            bruc.Deleting += DeleteRuleUserControl;
            bruc.Dragging += RuleUserControlDragPreviewMouseDown;
            bruc.PrepareColorPickers(StandardColorItems);
            bruc.UpdateColorPickers(clrPickMouse.AvailableColors);
            return bruc;
        }

        //
        private void RuleUserControlRuleSelection(object sender, EventArgs e) {
            BeepRuleUserControl bruc = sender as BeepRuleUserControl;

            // selected rule must be different
            if (bruc.SelectedRuleName == bruc.RuleName) return;

            beepRules.Remove(bruc.Rule);

            int index = BeepRulesUIComponents.IndexOf(bruc);
            BeepRulesUIComponents.Remove(bruc);

            BeepRule br = BeepRule.Create(bruc.SelectedRuleName, bw.tiles);
            beepRules.Add(br);

            BeepRulesUIComponents.Insert(index, CreateBeepRuleUserControl(br));
        }

        // deletes the UI component that corresponds to a rule
        private void DeleteRuleUserControl(object sender, EventArgs e) {
            BeepRuleUserControl bruc = sender as BeepRuleUserControl;
            beepRules.Remove(bruc.Rule);
            BeepRulesUIComponents.Remove(bruc);
        }

        // paints according to defined rules
        private async void BtnPaintClick(object sender, RoutedEventArgs e) {
            if (isPaintingIndefinitely) {
                isPaintingIndefinitely = false;
                btnPaint.Content = "Paint";
                return;
            }
            int? number = iudAmountPicker1.Value;
            await Task.Run(() => {
                if (paintIndefinitely) {
                    isPaintingIndefinitely = true;
                    Dispatcher.Invoke(() => { btnPaint.Content = "Pause"; });
                    while (isPaintingIndefinitely) {
                        foreach (BeepRule rule in beepRules) {
                            bw.tiles = rule.Run();
                            UpdateRules();
                        }
                        Thread.Sleep(20); // ¿¿¿????
                        this.Dispatcher.Invoke(() => {
                            Refresh();
                        });
                    }
                } else {
                    for (int i = 0; i < number; i++) {
                        foreach (BeepRule rule in beepRules) {
                            bw.tiles = rule.Run();
                            UpdateRules();
                        }
                        Thread.Sleep(20); // ¿¿¿????
                        this.Dispatcher.Invoke(() => {
                            Refresh();
                        });
                    }
                }
            });
            Refresh();
            UpdateUsedColors();
        }

        // ???? what is this ¿¿¿¿
        private void AmountChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
          
        }

        // experimental drag drop code
        private void RuleUserControlDragPreviewMouseDown(object sender, MouseEventArgs e) {
            if (sender is BeepRuleUserControl && e.LeftButton == MouseButtonState.Pressed) {
                BeepRuleUserControl draggedItem = sender as BeepRuleUserControl;
                DragDrop.DoDragDrop(draggedItem, new DataObject("drag", draggedItem.DataContext), DragDropEffects.Move);
            }
        }

        private void RuleUserControlDragDropped(object sender, DragEventArgs e) {
            BeepRuleUserControl draggedBruc = e.Data.GetData("drag") as BeepRuleUserControl;
            BeepRuleUserControl targetBruc = ((ListBoxItem)(sender)).DataContext as BeepRuleUserControl;

            int removedIdx = lbRules.Items.IndexOf(draggedBruc);
            int targetIdx = lbRules.Items.IndexOf(targetBruc);

            if (removedIdx < targetIdx) {
                BeepRulesUIComponents.Insert(targetIdx + 1, draggedBruc);
                BeepRulesUIComponents.RemoveAt(removedIdx);
            } else {
                int remIdx = removedIdx + 1;
                if (BeepRulesUIComponents.Count + 1 > remIdx) {
                    BeepRulesUIComponents.Insert(targetIdx, draggedBruc);
                    BeepRulesUIComponents.RemoveAt(remIdx);
                }
            }
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e) {

            //using (var db = new BeepWorldContext()) {

            //    var v = new SavableBeepWorld() {
            //        Name = "Joe"
            //    };
            //    db.SavedBeepWorlds.Add(v);

            //    db.SaveChanges();

            //    var query = from b in db.SavedBeepWorlds
            //                orderby b.Name
            //                select b;
                
            //    foreach (var item in query) {
            //        Debug.WriteLine(item.Name);
            //    }
            //}

            ///*
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
            //*/
        }

        private void BtnLoadClick(object sender, RoutedEventArgs e) {
            OpenFileDialog open = new OpenFileDialog() { Filter = "Text Document|*.txt" };
            string line;
            bool? result = open.ShowDialog();
            if (result == true) {
                StreamReader file = new StreamReader(open.FileName);
                while ((line = file.ReadLine()) != null) {
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

        private void MouseColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            MouseClickColor = (Color)clrPickMouse.SelectedColor;
        }

        private void UnprepareBeepWorldCanvas() {
            canvas.Children.Clear();
            foreach (string name in registeredHexPolygons) UnregisterName(name);
            registeredHexPolygons.Clear();
            highlightedHexPolygon = null;
        }

        //private void ResizeCanvasToFullScreen() {
        //    Point newSize = new Point();

        //    bool b = true;


        //    newSize.X = (int)CanvasGrid.ActualWidth;
        //    newSize.Y = (int)CanvasGrid.ActualHeight;
        //    ResizeCanvas(newSize, b);


        //}

        //private void ResizeCanvas(Point p, bool b) {
        //    Point newSize = new Point();
        //    bw.Resize(newSize, b);
        //    UnprepareBeepWorldCanvas();

        //    PrepareBeepWorldCanvas();

        //    Refresh();
        //    UpdateUsedColors();
        //}

        private void BtnSize_Click(object sender, RoutedEventArgs e) {
            int result;
            Point newSize = new Point();

            
            if (iudAmountPickerWidth != null && iudAmountPickerHeigth != null) {
                newSize.X = (int)iudAmountPickerWidth.Value;
                newSize.Y = (int)iudAmountPickerHeigth.Value;
            }



            

            
            bw.Resize(newSize, isBoxed);
            UnprepareBeepWorldCanvas();

            PrepareBeepWorldCanvas();

            Refresh();
            UpdateUsedColors();

            //foreach(UIElement c in canvas.Children) {
            //    canvas.Children.Remove(c);
            //}
        }

        // closes the application
        private void BtnQuitClick(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void clrPickBackground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            canvas.Background = new SolidColorBrush((Color)clrPickBackground.SelectedColor);
            //CanvasBackgroundColor
            
        }

        private void clrPickBorderColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            //if(useRelativeBorderColor== false)
            fixedBorderColor = (Color)clrPickBorderColor.SelectedColor;
            if (bw != null) Refresh();
        }


        private void FixedBorderColorChecked(object sender, RoutedEventArgs e) {

            useRelativeBorderColor = false;
            fixedBorderColor = (Color)clrPickBorderColor.SelectedColor;
            Refresh();
        }

        private void FixedBorderColorUnchecked(object sender, RoutedEventArgs e) {
            useRelativeBorderColor = true;
            Refresh();
        }

        private void IsCheckedIndefinite(object sender, RoutedEventArgs e) {
            paintIndefinitely = true;
            iudAmountPicker1.IsEnabled = false;
        }

        private void IsUncheckedIndefinite(object sender, RoutedEventArgs e) {
            paintIndefinitely = false;
            isPaintingIndefinitely = false;
            btnPaint.Content = "Paint";
            iudAmountPicker1.IsEnabled = true;
        }
        private void AmountHexSizeChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (bw != null && iudAmountPickerHexagonSize.Value != null && iudAmountPickerHexagonSize.Value <= iudAmountPickerHexagonSize.Maximum) {
                HEXAGON_SIDE_LENGTH = (double)iudAmountPickerHexagonSize.Value;
                CalculateHexPolygonSize();
                UnprepareBeepWorldCanvas();
                PrepareBeepWorldCanvas();
                Refresh();
            }
        }

        private void SetFullscreenClick(object sender, RoutedEventArgs e) {
            ControlMenuGrid.Visibility = Visibility.Collapsed;
            ParentGrid.ColumnDefinitions.RemoveAt(0);
            //ResizeCanvasToFullScreen();
           
            
           

        }

        // 
        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            Debug.WriteLine(string.Format("{0},{1}", e.NewSize.Width, ControlMenuGrid.ActualWidth));
            if (e.NewSize.Width < ControlMenuGrid.ActualWidth) ControlMenuGrid.Visibility = Visibility.Collapsed;
            else ControlMenuGrid.Visibility = Visibility.Visible;
        }

        // opens the color picker window, allowing a user to click anywhere in the button to open it
        private void ClrPickMouseButtonClick(object sender, RoutedEventArgs e) {
            clrPickMouse.IsOpen = true;
        }


        

        private void IsBoxedChecked(object sender, RoutedEventArgs e) {
            

            if (bw != null) {
                if (!bw.Boxed) {
                    var result = System.Windows.MessageBox.Show("Be careful, by boxing the canvas you will lose your current painting. Would you like to proceed?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes) {
                        isBoxed = true;
                        bw.Resize(bw.Size, isBoxed);
                        UnprepareBeepWorldCanvas();

                        PrepareBeepWorldCanvas();

                        Refresh();
                        UpdateUsedColors(); ;
                    }
                    else {
                        CheckIsBoxed.Checked -= IsBoxedChecked;
                        CheckIsBoxed.Unchecked -= IsBoxedUnchecked;
                        CheckIsBoxed.IsChecked = false;
                        CheckIsBoxed.Unchecked += IsBoxedUnchecked;
                        CheckIsBoxed.Checked += IsBoxedChecked;

                    }

                }

            }
           



        }



        private void IsBoxedUnchecked(object sender, RoutedEventArgs e) {
            var result = System.Windows.MessageBox.Show("Be careful, by unboxing the canvas you will lose your current painting. Would you like to proceed?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if(result == MessageBoxResult.Yes) {
                isBoxed = false;
                bw.Resize(bw.Size, isBoxed);
                UnprepareBeepWorldCanvas();

                PrepareBeepWorldCanvas();
                Refresh();
                UpdateUsedColors();
               
            }
            else {
                CheckIsBoxed.Checked -= IsBoxedChecked;
                CheckIsBoxed.Unchecked -= IsBoxedUnchecked;
                CheckIsBoxed.IsChecked = true;
                CheckIsBoxed.Checked += IsBoxedChecked;
                CheckIsBoxed.Unchecked += IsBoxedUnchecked;
                // isBoxedCheckboxControl = true;
            }
            
            

        }

        



        //private void SwitchFullScreen() {
        //    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        //    if(window!= null) {
        //        if(window.WindowStyle== WindowStyle.None) {
        //            window.WindowStyle = WindowStyle.SingleBorderWindow;
        //            window.WindowState = WindowState;

        //        }
        //        else {
        //            WindowState = window.WindowState;
        //            window.WindowStyle = WindowStyle.None;
        //            window.WindowState = System.Windows.WindowState.Maximized;
        //            window.Topmost = true;

        //        }
        //    }
        //}
    }

    /*
     * 
     * 
            if (k) {
                var watch = Stopwatch.StartNew();
                Parallel.ForEach(bw.tiles.Values, tile => { tile.Color = Tile.DEFAULT_COLOR; });
                watch.Stop();
                Debug.WriteLine(watch.ElapsedTicks);
            }
     * 
     * 
     */
}
