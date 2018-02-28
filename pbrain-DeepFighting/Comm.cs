using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbrain_DeepFighting
{
    class Comm
    {
        private int[,] _map;
        private Brain _brain;

        public Comm(Brain brain)
        {
            _brain = brain;
            Listen();
        }

        public void Listen()
        {
            String cmd;

            cmd = Console.ReadLine();
            String[] cmds = cmd.Split(' ');
            switch (cmds[0])
            {
                case "START":
                    try { Start(int.Parse(cmds[1])); } catch { NoArgumentError(); }
                    break;

                case "TURN":
                    try { Turn(cmds[1]); } catch { NoArgumentError(); }
                    break;

                case "BEGIN":
                    Begin();
                    break;

                case "BOARD":
                    Board();
                    break;

                case "INFO":
                    break;

                case "END":
                    Environment.Exit(0);
                    break;

                case "ABOUT":
                    Console.WriteLine("name=\"DeepFighting\", version=\"1.0\", author=\"xXx~~Sniperdu91~~xXx\", country=\"France\"");
                    break;
                
                default:
                    Console.WriteLine("ERROR Comm: Listen: Can't find command " + cmds[0] + ".");
                    Environment.Exit(84);
                    break;
            }
        }

        private void NoArgumentError()
        {
            Console.WriteLine("Error: No argument given.");
            Environment.Exit(84);
        }

        private void Start(int size)
        {
            _map = new int[size, size];
            Console.WriteLine("OK");
        }

        private void Turn(String arg) // PLAY
        {
            String[] args = arg.Split(',');
            int x = int.Parse(args[0]);
            int y = int.Parse(args[1]);
            _map[x, y] = 2;
            Play();
        }

        private void Begin()
        {
            int middle = _map.GetLength(0) / 2;
            Play();
        }

        private void Board() // PLAY
        {
            int x, y, player;
            x = y = player = 0;

            String line = "";
            while ((line = Console.ReadLine()) != "DONE" && line != "")
            {
                String[] args = line.Split(',');
                try
                {
                    x = int.Parse(args[0]);
                    y = int.Parse(args[1]);
                    player = int.Parse(args[2]);
                }
                catch
                {
                    Console.WriteLine("ERROR: Board line incorrect.");
                    Environment.Exit(84);
                }
                _map[x, y] = player;
            }
            Play();
        }

        private void Play()
        {
            Tuple<int, int> pos = _brain.Play(_map);
            _map[pos.Item1, pos.Item2] = 1;
            Console.WriteLine(pos.Item1 + "," + pos.Item2);
        }

        public void Debug(String msg)
        {
            Console.WriteLine("DEBUG " + msg);
        }

        public static void Message(String msg)
        {
            Console.WriteLine("MESSAGE " + msg);
        }
    }
}
