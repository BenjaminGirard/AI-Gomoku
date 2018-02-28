using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace pbrain_DeepFighting.GomokuGame
{
    class Tile
    {
        private Tuple<int, int> _pos;
        private int _player;
        public int DiagLeft;
        public int DiagRight;
        public int Vertical;
        public int Horizontal;

        public Tile(Tuple<int, int> pos, int player = 0)
        {
            _pos = pos;
            _player = player;
            DiagLeft = 0;
            DiagRight = 0;
            Vertical = 0;
            Horizontal = 0;
        }

        public int GetPlayer()
        {
            return _player;
        }

        public Tuple<int, int> GetPos()
        {
            return _pos;
        }

        public bool IsLineOver()
        {
            return DiagLeft == 4 || DiagRight == 4 || Vertical == 4 || Horizontal == 4;
        }

        public int GetTotalWeight()
        {
            return DiagLeft + DiagRight + Vertical + Horizontal;
        }

        public void SetPlayer(int playerTurn)
        {
            _player = playerTurn;
        }

        public void ResetWeights()
        {
            DiagLeft = 0;
            DiagRight = 0;
            Horizontal = 0;
            Vertical = 0;
        }
    }
}
