using BeanMachine.Debug;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BeanMachine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Globals.GraphicsDevice = GraphicsDevice;
            Globals.Content = Content;

#if DEBUG
            Thread debugInformationThread = new Thread(DebugManager.SendDataToDebugConsole);
            debugInformationThread.Start();

            string debugPath = Environment.CurrentDirectory;

            debugPath = Directory.GetParent(debugPath).Parent.Parent.FullName;

            debugPath += "\\DebugConsole\\DebugConsole.exe";

            Process[] processes = Process.GetProcessesByName("DebugConsole");

            if(processes.Length == 0)
                Process.Start(debugPath);
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Time.Update(gameTime);

            this.Update();

            Physics.Update();

        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Environment.Exit(0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Draw(_spriteBatch);
            Physics.DrawColliders(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Update()
        {

        }
    }
}