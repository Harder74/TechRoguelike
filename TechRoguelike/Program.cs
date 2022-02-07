using System;

namespace TechRoguelike
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TechRoguelikeGame())
                game.Run();
        }
    }
}
