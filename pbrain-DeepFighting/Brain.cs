using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbrain_DeepFighting.MCTSImplem;

namespace pbrain_DeepFighting
{
    class Brain
    {
        public Brain() { }

        public Tuple<int, int> Play(int[,] map)
        {
            return new BrainMCTS().GetNextMove(map, 1);
        }
    }
}
