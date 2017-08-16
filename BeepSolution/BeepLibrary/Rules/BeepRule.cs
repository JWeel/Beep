using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using System.Linq;

namespace Beep.Rules {

    // a repository of actions
    public abstract class BeepRule {

        public abstract string RuleName { get; }

        protected Dictionary<Point, Tile> tiles;
        protected List<Brush> colorArguments;
        protected List<int> intArguments;

        public BeepRule(Dictionary<Point, Tile> tiles,
                List<Brush> colorArguments = null,
                List<int> intArguments = null) {
            this.tiles = tiles;
            this.colorArguments = colorArguments;
            this.intArguments = intArguments;
        }

        public static BeepRule CreateBeepRule(string type) {
            throw new NotImplementedException();
        }

        public abstract Dictionary<Point, Tile> Run();

        // returns a deep copy of a tiles dictionary
        protected Dictionary<Point,Tile> DeepCopyDict(Dictionary<Point,Tile> sourceDict) {
            Dictionary<Point, Tile> copyDict = new Dictionary<Point, Tile>();
            foreach (var v in tiles) copyDict[v.Key] = new Tile(v.Value);
            return copyDict;
        }

        // updates the tiles dictionary reference
        public void Update(Dictionary<Point, Tile> updatedTiles) {
            tiles = updatedTiles;
        }
    }
}
