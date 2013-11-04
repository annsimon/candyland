using System;
using System.IO;

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
            try
            {
                using (Game1 game = new Game1())
                {
                        game.Run();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Exception while loading!")
                {
                    // we need to tell the user something here... :c
                    // for now: explain somewhere (on the website?) that
                    // if the game just closes, they should check for a log file (log.txt)
                    // in the game folder and send it over if there is one
                    // for now, just make the game crash...
                    throw new Exception();
                }
                else
                {
                    using (StreamWriter outfile = new StreamWriter("log.txt"))
                    {
                        outfile.Write("Game closed with Exception: \n");
                        outfile.Write(ex.ToString());
                    }
                    // we need to tell the user something here... :c
                    // for now: explain somewhere (on the website?) that
                    // if the game just closes, they should check for a log file (log.txt)
                    // in the game folder and send it over if there is one
                    // for now, just make the game crash...
                    throw new Exception();
                }
            }
        }
    }
#endif
}

