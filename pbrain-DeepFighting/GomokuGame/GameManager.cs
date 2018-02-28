using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using pbrain_DeepFighting.GomokuGame;
using pbrain_DeepFighting.MCTSImplem;

namespace pbrain_DeepFighting
{
    class GameManager
    {
        private Tile[,] _map;
        private int _playerTurn;

        public GameManager(int[,] map, int playerTurn = 1)
        {
            _playerTurn = playerTurn;
            int sizeMap = map.GetLength(0);
            _map = new Tile[sizeMap, sizeMap];
            for (int y = 0; y < sizeMap; y++)
            {
                for (int x = 0; x < sizeMap; x++)
                    _map[y, x] = new Tile(new Tuple<int, int>(y, x), map[y, x]);
            }
        }

        public bool IsGameOver()
        {
            ResetWeights();
            int nbEmpty = 0;
            int mapSize = _map.GetLength(0);
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (_map[y, x].GetPlayer() == 0)
                    {
                        nbEmpty += 1;
                        continue;
                    }
                    if (_map[y, x].IsLineOver())
                        return true;
                    SpreadInfo(y, x);
                }
            }

            return nbEmpty == 0;
        }

        private void ResetWeights()
        {
            int mapSize = _map.GetLength(0);

            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                    _map[y, x].ResetWeights();
            }
        }

        private void SpreadInfo(int y, int x)
        {
            Tile current = _map[y, x];
            int mapSize = _map.GetLength(0);

            // horizontal
            if (x + 1 < mapSize && current.GetPlayer() == _map[y, x + 1].GetPlayer())
                _map[y, x + 1].Horizontal += 1 + _map[y, x].Horizontal;
            if (y + 1 >= mapSize) return;

            // diag from up left to down right
            if (x + 1 < mapSize && current.GetPlayer() == _map[y + 1, x + 1].GetPlayer())
                _map[y + 1, x + 1].DiagRight += 1 + _map[y, x].DiagRight;
            // diag from up right to down left
            if (x - 1 >= 0 && current.GetPlayer() == _map[y + 1, x - 1].GetPlayer())
                _map[y + 1, x - 1].DiagLeft += 1 + _map[y, x].DiagLeft;
            // vertical
            if (current.GetPlayer() == _map[y + 1, x].GetPlayer())
                _map[y + 1, x].Vertical += 1 + _map[y, x].Vertical;
        }

        public void PrintMap()
        {
            int sizeMap = _map.GetLength(0);
            for (int y = 0; y < sizeMap; y++)
            {
                for (int x = 0; x < sizeMap; x++)
                {
                    Console.Write(_map[y, x].GetTotalWeight());
                }
                Console.Write('\n');
            }
            Console.Write('\n');
        }

        public void PlayTurn(int y, int x)
        {
            _map[y, x].SetPlayer(_playerTurn);
            SwitchPlayerTurn();
        }

        private void SwitchPlayerTurn()
        {
            _playerTurn = _playerTurn == 1 ? 2 : 1;
        }

        public Tile[,] GetMap()
        {
            return _map;
        }

        public Reward GenerateRandomEnd()
        {
            Random rdm = new Random();
            List<Tile> emptyTiles = GetEmptyTiles();

            while (emptyTiles.Count != 0 && !IsGameOver())
            {
                int nextMove = rdm.Next(emptyTiles.Count);

                PlayTurn(emptyTiles.ElementAt(nextMove).GetPos().Item1, emptyTiles.ElementAt(nextMove).GetPos().Item1);
                emptyTiles.RemoveAt(nextMove);
            }
            return GetStatusEndOfGame();
        }

        private Reward GetStatusEndOfGame()
        {
            int winner = 0;
            int sizeMap = _map.GetLength(0);

            for (int y = 0; y < sizeMap; y++)
            {
                for (int x = 0; x < sizeMap; x++)
                {
                    if (_map[y, x].IsLineOver())
                        winner = _map[y, x].GetPlayer();
                }
            }
            return winner == 0 ? new Reward(0, 0) : winner == 1 ? new Reward(1, 0) : new Reward(0, 1);
        }

        private List<Tile> GetEmptyTiles()
        {
            var emptyTiles = new List<Tile>();
            int sizeMap = _map.GetLength(0);

            for (int y = 0; y < sizeMap; y++)
            {
                for (int x = 0; x < sizeMap; x++)
                {
                    if (_map[y, x].GetPlayer() == 0)
                        emptyTiles.Add(_map[y, x]);
                }
            }
            return emptyTiles;
        }
    }
}

