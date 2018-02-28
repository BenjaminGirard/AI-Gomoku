using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbrain_DeepFighting.GomokuGame
{
    class Map
    {
        private int[,] _gameMap;
        private LinkedList<Tuple<int, int>> _weightList = new LinkedList<Tuple<int, int>>();
        private int _size;
        private int _listSize;
        public int[,] GameMap { get => _gameMap; set => _gameMap = value; }
        public int Size { get => _size; set => _size = value; }

        enum Val
        {
            me = -2,
            ennemy = -1,
            empty = 0
        }

        private int Max(int one, int two)
        {
            if (one > two)
                return one;
            return two;
        }

        public Map(int[,] map, int listSize)
        {
            _listSize = listSize;
            _size = map.GetLength(0);
            _gameMap = new int[_size, _size];

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (map[y, x] == 1)
                        _gameMap[y, x] = (int)Val.me;
                    else if (map[y, x] == 2)
                        _gameMap[y, x] = (int)Val.ennemy;
                    else
                        _gameMap[y, x] = map[y, x];
                }
            }
            //Display(_gameMap, _size);
        }

        public Map(int size)
        {
            _size = size;
            _gameMap = new int[_size, _size];

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    _gameMap[y, x] = 0;
                }
            }

        }

        private bool ExistCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _size || y >= _size)
            {
                return false;
            }
            return true;
        }

        private int Pow(int nb)
        {
            return nb * nb * nb;
        }

        private int Adj(int x, int y, Val search) //OK
        {
            int ret = 0;
            if (ExistCell(x, y) && _gameMap[y, x] == 0)
            {
                int s = (int)search;
                if (ExistCell(x - 1, y) && _gameMap[y, x - 1] == (int)s)
                { ret++; }
                if (ExistCell(x + 1, y) && _gameMap[y, x + 1] == (int)s)
                { ret++; }
                if (ExistCell(x, y - 1) && _gameMap[y - 1, x] == (int)s)
                { ret++; }
                if (ExistCell(x, y + 1) && _gameMap[y + 1, x] == (int)s)
                { ret++; }
                if (ExistCell(x - 1, y - 1) && _gameMap[y - 1, x - 1] == (int)s)
                { ret++; }
                if (ExistCell(x + 1, y + 1) && _gameMap[y + 1, x + 1] == (int)s)
                { ret++; }
                if (ExistCell(x + 1, y - 1) && _gameMap[y - 1, x + 1] == (int)s)
                { ret++; }
                if (ExistCell(x - 1, y + 1) && _gameMap[y + 1, x - 1] == (int)s)
                { ret++; }
                return ret;
            }
            else
                return (_gameMap[y, x]);
        }

        private int checkRight(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x + 1, y))
            {
                if (_gameMap[y, x + 1] == (int)search)
                {
                    ret++;
                    x++;
                    i++;
                    b = false;
                }
                else if (_gameMap[y, x + 1] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    x++;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkLeft(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x - 1, y))
            {
                if (_gameMap[y, x - 1] == (int)search)
                {
                    b = false;
                    ret++;
                    x--;
                    i++;
                }
                else if (_gameMap[y, x - 1] >= (int)Val.empty)
                {
                    x--;
                    i++;
                    if (b)
                        return ret;
                    else
                        b = true;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkUp(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x, y - 1))
            {
                if (_gameMap[y - 1, x] == (int)search)
                {
                    b = false;
                    ret++;
                    y--;
                    i++;
                }
                else if (_gameMap[y - 1, x] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    y--;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkDown(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x, y + 1))
            {
                if (_gameMap[y + 1, x] == (int)search)
                {
                    b = false;
                    ret++;
                    y++;
                    i++;
                }
                else if (_gameMap[y + 1, x] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    y++;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkUpRight(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x + 1, y - 1))
            {
                if (_gameMap[y - 1, x + 1] == (int)search)
                {
                    b = false;
                    ret++;
                    x++;
                    y--;
                    i++;
                }
                else if (_gameMap[y - 1, x + 1] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    x++;
                    y--;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkUpLeft(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x - 1, y - 1))
            {
                if (_gameMap[y - 1, x - 1] == (int)search)
                {
                    b = false;
                    ret++;
                    x--;
                    y--;
                    i++;
                }
                else if (_gameMap[y - 1, x - 1] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    x--;
                    y--;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkDownRight(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x + 1, y + 1))
            {
                if (_gameMap[y + 1, x + 1] == (int)search)
                {
                    b = false;
                    ret++;
                    x++;
                    y++;
                    i++;
                }
                else if (_gameMap[y + 1, x + 1] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    x++;
                    y++;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int checkDownLeft(int x, int y, Val search) // OK
        {
            int i = 0;
            int ret = 0;
            bool b = false;
            while (i < 4 && ExistCell(x - 1, y + 1))
            {
                if (_gameMap[y + 1, x - 1] == (int)search)
                {
                    b = false;
                    ret++;
                    x--;
                    y++;
                    i++;
                }
                else if (_gameMap[y + 1, x - 1] >= (int)Val.empty)
                {
                    if (b)
                        return ret;
                    else
                        b = true;
                    x--;
                    y++;
                    i++;
                }
                else
                {
                    return ret - 1;
                }
            }
            return ret;
        }

        private int Horizontal(int x, int y, Val search) // OK
        {
            return (checkRight(x, y, search) + checkLeft(x, y, search));
        }
        private int Vertical(int x, int y, Val search) // OK
        {
            return (checkUp(x, y, search) + checkDown(x, y, search));
        }
        private int Diago(int x, int y, Val search)
        {
            int tmp1 = checkDownLeft(x, y, search) + checkUpRight(x, y, search);
            int tmp2 = checkDownRight(x, y, search) + checkUpLeft(x, y, search);
            return Max(tmp1, tmp2);
        }

        private int Alignement(int x, int y, Val search)
        {
            int ret = 0;
            if (_gameMap[y, x] == 0 && Adj(x, y, search) > 0)
            {
                int a = Diago(x, y, search);
                int b = Vertical(x, y, search);
                int c = Horizontal(x, y, search);
                ret = Max(a, Max(b, c));
            }
            return (ret);
        }

        public int[,] Eval()
        {
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (_gameMap[y, x] == (int)Val.empty)
                    {
                        _gameMap[y, x] = Adj(x, y, Val.me) + Pow(Alignement(x, y, Val.me)) +            // attack
                                         Adj(x, y, Val.ennemy) + Pow(Alignement(x, y, Val.ennemy));     // defense
                        AddToWeightList(Tuple.Create(x, y));
                    }
                }
            }

            // Display(_gameMap, _size);
            return _gameMap;
        }
        private void AddToWeightList(Tuple<int, int> nb)
        {
            var actualSize = _weightList.Count();
            if (actualSize < 1)
            {
                _weightList.AddFirst(nb);
                return;
            }
            Tuple<int, int> tmp;
            for (int i = 0; i < _listSize; i++)
            {
                if (i < actualSize)
                {
                    tmp = _weightList.ElementAt(i);
                    if (_gameMap[nb.Item1, nb.Item2] > _gameMap[tmp.Item1, tmp.Item2])
                    {
                        _weightList.AddBefore(_weightList.Find(tmp), nb);
                        if (_weightList.Count() > _listSize)
                        {
                            _weightList.RemoveLast();
                        }
                        return;
                    }
                }
                else
                {
                    _weightList.AddLast(nb);
                }
            }
        }

        /* public void Debug()
         {
             string line = "";
             for (int y = 0; y < _size; y++)
             {
                 line = "";
                 for (int x = 0; x < _size; x++)
                 {
                     line = line + _gameMap[y,x] + " ";
                 }
                 Comm.Message(line + "end");

             }
             Comm.Message("size :" + _size);
         }
         */

        public void SetValue(int x, int y, int val)
        {
            if (_gameMap[y, x] != 0)
                return;
            if (val == 1)
            {
                _gameMap[y, x] = (int)Val.me;
            }
            if (val == 2)
            {
                _gameMap[y, x] = (int)Val.ennemy;
            }
        }

        public void Display()
        {
            var map = _gameMap;
            var size = _size;
            string str = "";

            for (int y = 0; y < size; y++)
            {
                str = "\t";
                for (int x = 0; x < size; x++)
                {
                    if (map[x, y] == -1)
                        str = str + "O\t";
                    else if (map[x, y] == -2)
                        str = str + "X\t";
                    else
                        str = str + map[x, y] + "\t";
                }
                Console.WriteLine("MESSAGE " + str);
                Console.WriteLine("");
            }
        }

        public LinkedList<Tuple<int, int>> GetWeightList()
        {
            return _weightList;
        }
    }
}
