using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class VirusRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required:
        //- one (or n) colors in colorArguments
        //  -first color determines virus color
        //  -each iteration, original color stays the same, targetcolor changes shade
        //- one int to determine contagion rate
        // - one bool argument specifying whether to infect other tiles
        // TODO: 
        // 1. Change neighbor(s) color depending on contagion speed
        // 2. Lock colors for each generation, so that each new generation has a slightly different color
        // 3. Lock generations, so that older generation loose their ability to spread colors
        //

        Random rand = new Random();

        public VirusRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {

            MatchColor = colorArguments[0];
            //TargetColor = colorArguments[2];
            //PreviousColor = colorArguments[3];
            MouseColor = colorArguments[1];
            ContagionRate = intArguments[0];
            ColorNeighboringMatchers = boolArguments[0];
            IsAbleToInfect = boolArguments[1];

        }
        

        public override string RuleName { get => RULE_VIRUS; }
        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }
        public Color PreviousColor { get; set; }
        public Color MouseColor { get; set; }
        public int ContagionRate { get; set; }
        public bool IsAbleToInfect { get; set; }
        public bool ColorNeighboringMatchers { get; set; }

        int counter = 0;
        bool brightnessDown;


        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
           
                alteredTiles = CalculateVirus(alteredTiles, brightnessDown);
                brightnessDown = brightnessCheck(brightnessDown);

            
            return alteredTiles;
        }

        public bool brightnessCheck(bool b) {
            if(b && counter < 10) {
                counter++;
                return true;
            }else if(b && counter == 10) {
                counter = 0;
                return  false;
            }else if (!b && counter < 10) {
                counter++;
                return false;
            } else if(!b && counter == 10) {
                counter = 0;
                return true;
            }
            else {
                return b;
            }
           
        }

        public Dictionary<Point,Tile> CalculateVirus(Dictionary<Point,Tile> alteredTiles, bool b) {

           

            List<Tile> TileList = new List<Tile>();
            List<Color> PreviousColors = new List<Color>();
            foreach (Tile t in tiles.Values) {
               
            }

            foreach (Tile t in tiles.Values) {
                if (t.Color == MatchColor) {
                    if (t.Color == MouseColor) {
                        TileList.Add(t);
                    }
                    // contagionrate randomizer here
                    for (int i = 1; i < ContagionRate + 1; i++) {

                        Point p = t.Neighbors[rand.Next(0, t.Neighbors.Count)];

                        if (tiles[p].Color == MatchColor || PreviousColors.Contains(tiles[p].Color)) {
                            //Debug.WriteLine("previous color hit");
                            continue;
                        }
                        TileList.Add(tiles[p]);
                        alteredTiles[p].Color = MatchColor;
                    }
                }
            }
            PreviousColors.Add(MatchColor);


            if (b) MatchColor = ChangeColorBrightness(MatchColor, -0.010F);
            else {
                MatchColor = ChangeColorBrightness(MatchColor, 0.020F);
            }
            foreach (Tile t in TileList) {

                alteredTiles[t.Coordinates].Color = MatchColor;
            }
            TileList.Clear();
            return alteredTiles;
        }

        public static Color ChangeColorBrightness(Color c, float correctionFactor) {
            float red = (float)c.R;
            float green = (float)c.G;
            float blue = (float)c.B;
            if (correctionFactor < 0) {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;

            }
            else {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            return Color.FromArgb(c.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}