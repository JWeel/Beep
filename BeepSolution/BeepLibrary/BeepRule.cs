using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using System.Linq;

namespace Beep {

    // a repository of actions
    public class BeepRule {

        public const string RULE_CHANGE_COLOR = "Change Color";
        public const string RULE_CHANGE_NEIGHBOR_COLOR = "Change Neighbor Color";
        public const string RULE_VIRUS = "Virus";

        public string Name { get; set; }
        
        private Brush color1, color2, color3;
        private int nNeighbors;
        private Dictionary<Point, Tile> tilesDict;

        public BeepRule(string name,
                Brush color1 = null,
                Brush color2 = null,
                Brush color3 = null,
                int nNeighbors = 0,
                Dictionary<Point, Tile> tilesDict = null) {
            this.Name = name;
            this.color1 = color1;
            this.color2 = color2;
            this.color3 = color3;
            this.nNeighbors = nNeighbors;
            this.tilesDict = tilesDict;
        }

        public Dictionary<Point, Tile> Run() {
            switch (Name) {
                case RULE_CHANGE_COLOR:
                    return ChangeColor(tilesDict, color1, color2);
                case RULE_CHANGE_NEIGHBOR_COLOR:
                    return ChangeNeighborColor(tilesDict, nNeighbors, color1, color2);
                case RULE_VIRUS:
                    return null;
                default:
                    return null;
            }
        }

        // Tiles with designated color become target color
        // TODO possible extension: percentage reliability, percentage of tiles to change
        public static Dictionary<Point, Tile> ChangeColor(Dictionary<Point, Tile> tiles, Brush originalColor, Brush targetColor) {
            //List<Tile> alteredTiles = tiles.ConvertAll(tile => new Tile(tile));

            // deep copy dict
            Dictionary<Point, Tile> alteredTiles = new Dictionary<Point, Tile>();
            foreach (var v in tiles) {
                alteredTiles[v.Key] = new Tile(v.Value);
            }

            foreach (Tile t in alteredTiles.Values) {
                if (t.Color == originalColor) t.Color = targetColor;
            }
            return alteredTiles;
        }

        // Specified number of neighbors of tiles with designated color become target color
        public Dictionary<Point, Tile> ChangeNeighborColor(Dictionary<Point, Tile> tiles, int nNeighbors, Brush originalColor, Brush targetColor) {
            // deep copy dict
            Dictionary<Point, Tile> alteredTiles = new Dictionary<Point, Tile>();
            foreach (var v in tiles) {
                alteredTiles[v.Key] = new Tile(v.Value);
            }

            foreach (Tile t in tiles.Values) {
                if (t.Color == originalColor) {
                    foreach (Point p in t.Neighbors) {
                        //Debug.WriteLine(p + " " + t.Neighbors.ToString());
                        alteredTiles[p].Color = targetColor;
                    }
                }
            }
            return alteredTiles;
        }

        //virus: takes parameters Contagion rate and (power)
        public Dictionary<Point, Tile> SpreadVirus (Dictionary<Point, Tile> tiles, int nNeighbors, Brush originalColor, Brush targetColor) {

           //deep copy dict
           Dictionary<Point, Tile> alteredTiles = new Dictionary<Point, Tile>();
           foreach(var v in tiles) {
                alteredTiles[v.Key] = new Tile(v.Value);
            }

            return alteredTiles;

        }

        public void Update(Dictionary<Point, Tile> updatedTiles) {
            tilesDict = updatedTiles;
        }
    }
}
