using System;

namespace Candyland
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Form1 form = new Form1();
            BalanceBoard.initialize(form.Handle);
            form.Show();
            form.game = new Game1(
                form.surface.Handle,
                form,
                form.surface);
            form.game.Run();
        }
    }
#endif
}

