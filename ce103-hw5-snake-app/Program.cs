using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ce103_hw5_snake_dll;

namespace ce103_hw5_snake_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            snakedll snakegame = new snakedll();
            snakegame.main();
        }
    }
}
