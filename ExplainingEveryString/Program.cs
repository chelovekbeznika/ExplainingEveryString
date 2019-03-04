using System;
using ExplainingEveryString.Core;

namespace ExplainingEveryString
{   
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (EesGame game = new EesGame())
            {
                game.Run();
            }
        }
    }
}
