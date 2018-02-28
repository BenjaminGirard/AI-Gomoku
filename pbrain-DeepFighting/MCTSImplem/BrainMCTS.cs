using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace pbrain_DeepFighting.MCTSImplem
{
    class BrainMCTS
    {
        public Tuple<int, int> GetNextMove(int[,]map, int player)
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            GameManager gm = new GameManager((int[,])map.Clone(), player);
            Node root = new Node(null, null, player, gm.GetMap());

            // Check if map if empty - play in 10 - 10 if true
            if (gm.GetMap().Length == root.GetUnexploredMoves().Count)
                return new Tuple<int, int>(map.GetLength(0) / 2, map.GetLength(0) / 2);

            int nbSim = 0;
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() - currentTime < 4500)
            {
                GC.Collect();
                // reset game manager to select new node to explore
                gm = new GameManager((int[,])map.Clone(), player);

                // select node to explore 
                var node = SelectNode(root, gm);
                node = node.Expand(gm);
                var rw = gm.GenerateRandomEnd();
                node.BackPropagate(rw);
            }

            return root.GetMostVisitedNode()?.GetMoveUsedToGetToNode();
        }

        private Node SelectNode(Node node, GameManager gm)
        {
            Node Selected = node;
            while (!Selected.CanExpandFromTargets() && !gm.IsGameOver())
            {
                Selected = Selected.SelectChild();
                Tuple<int, int> move = Selected.GetMoveUsedToGetToNode();
                if (move != null)
                    gm.PlayTurn(move.Item1, move.Item2);
            }
            return Selected;
        }

    }
}
