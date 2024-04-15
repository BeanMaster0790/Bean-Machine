using BeanMachine.Debug;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using BeanMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using BeanMachine.Graphics;

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
    

#if DEBUG
            StartEngineDebug();
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.Content = Content;

            GraphicsManager.Instance.StartGame(this.GraphicsDevice, this._graphics);
            GraphicsManager.Instance.ApplyChanges();


            this.Open();
            this.Load();
        }

        public virtual void Load()
        {

        }

        public virtual void LateUpdate()
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

            Physics.Instance.Update();

            Time.Instance.Update(gameTime);

            InputManager.Instance.Update();

            this.LateUpdate();
        }

        public virtual void Update()
        {
        }

    }
}