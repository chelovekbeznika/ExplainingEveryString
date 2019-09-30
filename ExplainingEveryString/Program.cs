using System;
using ExplainingEveryString.Core;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Game game = new EesGame())
            {
                game.Run();
            }
        }
    }
}
