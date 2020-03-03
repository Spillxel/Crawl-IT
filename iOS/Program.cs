using CrawlIT.Shared;
using Foundation;
using UIKit;

namespace iOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static CrawlIt game;

        internal static void RunGame()
        {
            game = new CrawlIt();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
