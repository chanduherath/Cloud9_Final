using Home;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace PreCloud9
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //using (Game1 game = new Game1())
            //{
            //    game.Run();
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
#endif
}

