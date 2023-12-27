using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProtectTheCenter.Managers;
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
            Thread debugThread = new Thread(DebugManager.Update);
            debugThread.Start();
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Globals.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            InputManager.Update();
            Physics.Update();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            SceneManager.ActiveManager.ActiveScene.Draw(_spriteBatch);
            Physics.DrawColliders(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}