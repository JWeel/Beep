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

        public VirusRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles, colorArguments, intArguments, boolArguments) {

            MatchColor = colorArguments[0];
            TargetColor = colorArguments[1];
            PreviousColor = colorArguments[2];
            ContagionRate = intArguments[0];
            ColorNeighboringMatchers = boolArguments[0];
            IsAbleToInfect = boolArguments[1];

        }
        

        public override string RuleName { get => RULE_VIRUS; }
        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }
        public Color PreviousColor { get; set; }
        public int ContagionRate { get; set; }
        public bool IsAbleToInfect { get; set; }
        public bool ColorNeighboringMatchers { get; set; }



        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            foreach (Tile t in tiles.Values) {
                if(t.Color == (Color)ColorConverter.ConvertFromString("#FFFFA500")) {
                    t.Color = MatchColor;
                    

                }
            }
           
            List<Tile> TileList = new List<Tile>();
            List<Color> PreviousColors = new List<Color>();


            foreach (Tile t in tiles.Values) {
                if (t.Color == MatchColor) {
                    //add a contagionrate randomizer here

                    Point p = t.Neighbors[rand.Next(0, t.Neighbors.Count)];

                    if (tiles[p].Color == MatchColor || PreviousColors.Contains(tiles[p].Color)) {
                        Debug.WriteLine("previous color hit");
                        continue;
                    }
                    TileList.Add(tiles[p]);
                    alteredTiles[p].Color = MatchColor;
                }
            }
            PreviousColors.Add(MatchColor);
            MatchColor = ChangeColorBrightness(MatchColor, -0.010F);
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