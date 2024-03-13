using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BeanMachine
{
    public static class Globals
    {
        public static ContentManager Content;

        public static GraphicsDevice GraphicsDevice;

        public static GraphicsDeviceManager GraphicsDeviceManager;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        public static bool IsFullscreen = false;

        public static int VirtualWidth = 3840;
        public static int VirtualHeight = 2160;
    }
}
