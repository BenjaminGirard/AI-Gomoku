using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using pbrain_DeepFighting.GomokuGame;

namespace pbrain_DeepFighting.MCTSImplem
{
    internal class Node
    {
        private readonly Node _parent;
        private int _numSimulations = 0;
        private readonly Reward _reward;
        readonly LinkedList<Node> _children = new LinkedList<Node>();
        readonly LinkedList<Tuple<int, int>> _unexploredMoves;
        private readonly LinkedList<Tuple<int, int>> _targetedMoves;
        private readonly int _player;
        readonly Tuple<int, int> _moveUsedToGetNode;

        private Tuple<int, int> GetMostValuablePos(int[,] map)
        {
            var target = new Tuple<int, int>(-1, -1);
            int currentTargetWeight = int.MinValue;

            var size = map.GetLength(0);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (currentTargetWeight < map[y, x])
                    {
                        target = new Tuple<int, int>(y, x);
                        currentTargetWeight = map[y, x];
                    }
                }
            }
            if (currentTargetWeight == int.MinValue || currentTargetWeight == 0)
                return null;
            map[target.Item1, target.Item2] = int.MinValue;
            return target;
        }

        // temporary to test weights
        private LinkedList<Tuple<int, int>> GetTargetedMoves(Tile[,] tiles, int limit)
        {
            int mapSize = tiles.GetLength(0);
            var mapToAnalyze = new int[mapSize, mapSize];

            for (int y = 0; y < mapSize; y++)
                for (int x = 0; x < mapSize; x++)
                    mapToAnalyze[y, x] = tiles[y, x].GetPlayer();

            var AnalyzedMap = new Map(mapToAnalyze, limit);
            AnalyzedMap.Eval();

            var weightMap = AnalyzedMap.GameMap;
            var TargetedMoves = new LinkedList<Tuple<int, int>>();

            for (int l = 0; l < limit; l++)
            {
                var target = GetMostValuablePos(weightMap);
                if (target == null)
                    break;
                TargetedMoves.AddFirst(target);
            }
            return TargetedMoves;
        }

        public Node(Node parent, Tuple<int, int> move, int player, Tile[,] map)
        {
            _parent = parent;
            _moveUsedToGetNode = move;
            _player = player;
            _unexploredMoves = GetEmptyTiles(map);
            _targetedMoves = GetTargetedMoves(map, 4);
            _reward = new Reward(0, 0);
        }

        LinkedList<Tuple<int, int>> GetEmptyTiles(Tile[,] map)
        {
            LinkedList<Tuple<int, int>> emptyTiles = new LinkedList<Tuple<int, int>>();

            var sizeMap = map.GetLength(0);
            for (int y = 0; y < sizeMap; y++)
            {
                for (int x = 0; x < sizeMap; x++)
                {
                    if (map[y, x].GetPlayer() == 0)
                        emptyTiles.AddFirst(new Tuple<int, int>(y, x));
                }
            }
            return emptyTiles;
        }


        public Node SelectChild()
        {
            Node bestNode = this;
            double max = double.MinValue;
            // bestNode.GetNumberOfSimulations() == 0 ? int.MaxValue : int.MinValue;

            foreach (var child in _children)
            {
                double childUctValue = GetUctValue(child);
                if (childUctValue > max)
                {
                    max = childUctValue;
                    bestNode = child;
                }
            }
            return bestNode;
        }

        public void BackPropagate(Reward reward)
        {
            _reward.AddReward(reward);
            _numSimulations++;
            _parent?.BackPropagate(reward);
        }

        public Node Expand(GameManager gm)
        {
            if (!CanExpandFromTargets())
                return this;

            Tuple<int, int> movePos = _targetedMoves.First();
            _targetedMoves.RemoveFirst();
            gm.PlayTurn(movePos.Item1, movePos.Item2);
            Node child = new Node(this, movePos, _player == 2 ? 1 : 2, gm.GetMap());
            _children.AddFirst(child);
            return child;
        }

        private double GetUctValue(Node child)
        {
            double uctValue;

            if (child.GetNumberOfSimulations() == 0)
            {
                uctValue = 1;
            }
            else
            {
                double Wi = child.GetRewardForPlayer(_player);
                double ni = child.GetNumberOfSimulations();
                double c = Math.Sqrt(2);
                double Ni = GetNumberOfSimulations();

                uctValue
                    = Wi / ni + c * (Math.Sqrt(Math.Log(Ni) / ni));
            }

            return uctValue;
        }

        public int GetNumberOfSimulations()
        {
            return _numSimulations;
        }

        public double GetRewardForPlayer(int player)
        {
            return _reward.GetRewardForPlayer(player);
        }

        public int GetPlayer()
        {
            return _player;
        }

        public bool CanExpand()
        {
            return _unexploredMoves.Count > 0;
        }

        public bool CanExpandFromTargets()
        {
            return _targetedMoves.Count > 0;
        }

        public Tuple<int, int> GetMoveUsedToGetToNode()
        {
            return _moveUsedToGetNode;
        }

        public Node GetMostValuableNode()
        {
            double mostValuable = double.MinValue;
            Node bestChild = null;
            foreach (var child in _children)
            {
                if (child.GetRewardForPlayer(child._player) > mostValuable)
                {
                    mostValuable = child.GetRewardForPlayer((child._player));
                    bestChild = child;
                }
            }
            return bestChild;
        }

        public Node GetMostVisitedNode()
        {
            int mostVisitCount = 0;
            Node bestChild = null;

            foreach (var child in _children)
            {
                if (child.GetNumberOfSimulations() > mostVisitCount)
                {
                    bestChild = child;
                    mostVisitCount = child.GetNumberOfSimulations();
                }
            }
            return bestChild;
        }

        public LinkedList<Tuple<int, int>> GetUnexploredMoves()
        {
            return _unexploredMoves;
        }

        public LinkedList<Tuple<int, int>> GetTargetedMoves()
        {
            return _targetedMoves;
        }

        public void LogChildren()
        {
            Comm.Message("NBSimParent = " + _numSimulations);
            foreach (var child in _children)
            {

                Comm.Message("player n°" + child._player + " - y = " + child._moveUsedToGetNode.Item1 + " - x = " + child._moveUsedToGetNode.Item2
                    + " - reward = " + child._reward.GetRewardForPlayer(child.GetPlayer()) + " - targetedMovesLeft = " + child._targetedMoves.Count + " - NumberOfSimulations = " + child._numSimulations + " - UTCValue = " + child.GetUctValue(child));
            }
        }
    }
}