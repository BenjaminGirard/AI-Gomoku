using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pbrain_DeepFighting
{
    class Program
    {
        static void Main(string[] args)
        {
           Brain brain = new Brain();
           Comm comm = new Comm(brain);


            while (true)
            {
                comm.Listen();
            }
        }
    }
}
