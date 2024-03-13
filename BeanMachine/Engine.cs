using BeanMachine.Debug;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using BeanMachine.Testing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BeanMachine
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Draw(_spriteBatch);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            this._graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            this._graphics.PreferredBackBufferHeight = Globals.ScreenHeight;

            this._graphics.IsFullScreen = Globals.IsFullscreen;

            this.Window.IsBorderless = false;
            
            this._graphics.ApplyChanges();

#if DEBUG
            StartEngineDebug();
#endif

            Thread thread = new Thread(Physics.Instance.Update);
            thread.Start();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.Content = Content;

            Globals.GraphicsDevice = this.GraphicsDevice;

            Globals.GraphicsDeviceManager = this._graphics;

            this.Open();
            this.Load();
        }

        public virtual void Load()
        {

        }

        public virtual void Open()
        {

        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Environment.Exit(0);
        }

        private static void StartEngineDebug()
        {
            Thread debugInformationThread = new Thread(DebugManager.Instance.SendDataToDebugConsole);
            debugInformationThread.Start();

            string debugPath = Environment.CurrentDirectory;

            debugPath = Directory.GetParent(debugPath).Parent.Parent.FullName;

            debugPath += "\\DebugConsole\\DebugConsole.exe";

            Process[] processes = Process.GetProcessesByName("DebugConsole");

            if (processes.Length == 0)
                Process.Start(debugPath);
        }

        public virtual void Start()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Update();

            Time.Instance.Update(gameTime);

            InputManager.Instance.Update();
        }

        public virtual void Update()
        {
        }

    }
}