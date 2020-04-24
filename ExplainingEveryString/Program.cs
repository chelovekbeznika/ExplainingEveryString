using System;
using ExplainingEveryString.Core;
using ExplainingEveryString.Editor;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString
{
    public static class Program
    {
        [STAThread]
        public static void Main(String[] args)
        {
#if !DEBUG
            try
            {
#endif
                using (var game = GetAppToStart(args))
                {
                    game.Run();
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("last_crash.txt",
                    $"Game crushed with {ex.GetType().Name} exception\nError message:\n{ex.Message}\nStacktrace:\n{ex.StackTrace}");
                throw;
            }
#endif
        }

        private static EesApp GetAppToStart(String[] args)
        {
            if (args.Length > 1 && args[0] == "-e")
                return new EesEditor(args[1]);
            else
                return new EesGame();
        }
    }
}
