using System;

namespace HalloweenStory
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameHalloweenStory())
                game.Run();
        }
    }
#endif
}
