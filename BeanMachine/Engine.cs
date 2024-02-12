using BeanMachine.Debug;
using BeanMachine.Graphics;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using BeanMachine.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private Sprite _sprite;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Draw(_spriteBatch);
            Physics.Instance.DrawColliders(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            SceneManager.Instance.ActiveScene.Draw(spriteBatch);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Globals.GraphicsDevice = GraphicsDevice;
            Globals.Content = Content;

#if DEBUG
            StartEngineDebug();
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.Load();
        }
        public virtual void Load()
        {
            Scene tempScene = new Scene("Temp Scene");

            SceneManager.Instance.AddNewScene(tempScene);

            SceneManager.Instance.LoadScene(tempScene.Name);

            SceneManager.Instance.SetActiveScene(tempScene.Name);

            this._sprite = new Sprite(Content.Load<Texture2D>("Textures\\Debug\\Square"))
            {
                Position = new Vector2(100,100)
            };

            tempScene.AddToScene(this._sprite);
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

            Time.Instance.Update(gameTime);

            this.Update();

            Physics.Instance.Update();

            InputManager.Instance.Update();
        }

        public virtual void Update()
        {
            SceneManager.Instance.ActiveScene.Update();


            DebugManager.Instance.Monitor(Time.Instance.Fps, "FPS");
            DebugManager.Instance.Monitor(this._sprite.Position, "Pos");

            if(InputManager.Instance.IsKeyHeld(Keys.W))
            {
                this._sprite.Position.Y -= 1;
            }
            if (InputManager.Instance.IsKeyHeld(Keys.S))
            {
                this._sprite.Position.Y += 1;
            }
            if (InputManager.Instance.IsKeyHeld(Keys.A))
            {
                this._sprite.Position.X -= 1;
            }
            if (InputManager.Instance.IsKeyHeld(Keys.D))
            {
                this._sprite.Position.X += 1;
            }
        }



    }
}