using System;
using System.IO;
using ExplainingEveryString.Core;

namespace ExplainingEveryString
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new EesGame())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("last_crash.txt",
                    $"Game crushed with {ex.GetType().Name} exception\nError message:\n{ex.Message}\nStacktrace:\n{ex.StackTrace}");
            }
        }
    }
}
