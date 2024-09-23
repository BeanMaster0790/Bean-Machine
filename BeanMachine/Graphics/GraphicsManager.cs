using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics
{
    public class GraphicsManager
    {
        public static GraphicsManager Instance = new GraphicsManager();

        public GraphicsDevice GraphicsDevice;

        public GraphicsDeviceManager GraphicsDeviceManager;

        public EventHandler<EventArgs> GraphicsChanged;

        private int screenWidth
        {
            get
            {
                return this.GraphicsDeviceManager.PreferredBackBufferWidth;
            }
        
        }

        private int screenHeight
        {
            get
            {
                return this.GraphicsDeviceManager.PreferredBackBufferHeight;
            }

        }

        public void ApplyChanges()
        {
            this.GraphicsDeviceManager.ApplyChanges();
            
            this.GraphicsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void StartGame(GraphicsDevice device, GraphicsDeviceManager deviceManager)
        {
            this.GraphicsDevice = device;
            this.GraphicsDeviceManager = deviceManager;

            Vector2 screenSize = this.GetUserDisplaySize();

            this.SetScreenSize((int)screenSize.X, (int)screenSize.Y);
            this.SetFullScreen(true);
        }

        public void SetScreenSize(int width, int height)
        {
            this.GraphicsDeviceManager.PreferredBackBufferWidth = width;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = height;
        }

        public Vector2 GetScreenSize()
        {
            return new Vector2(this.screenWidth, this.screenHeight);
        }

        public Vector2 GetUserDisplaySize()
        {
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            return new Vector2(width, height);
        }

        public float GetScreenAspectRatio()
        {
            Vector2 screenSize = GetScreenSize();

            return screenSize.X / screenSize.Y;
        }

        public void SetFullScreen(bool state)
        {
            this.GraphicsDeviceManager.IsFullScreen = state;
        }

    }
}
