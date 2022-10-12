using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts
{
    class Pair
    {
        private Tile tile;
        private LineRenderer line;

        public Pair(Tile _tile, LineRenderer _line) {
            tile = _tile;
            line = _line;
        }

        public Tile getTile() { return tile; }
        public LineRenderer getLine() { return line; }
    }
}
