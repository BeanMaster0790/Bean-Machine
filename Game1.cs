using BeanMachine.Graphics;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProtectTheCenter.Managers;
using System.Diagnostics;

namespace BeanMachine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Scene _testScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals.GraphicsDevice = GraphicsDevice;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _testScene = new Scene("Test");

            SceneManager.ActiveManager.AddNewScene("Test", _testScene);
            SceneManager.ActiveManager.LoadScene("Test");
            SceneManager.ActiveManager.SetActiveScene("Test");

            SceneManager.ActiveManager.UnloadScene("Test");
            
            Component[] testerSprites = SceneManager.ActiveManager.ActiveScene.GetComponentsWith(tag: "Tester");
            Component jeff = SceneManager.ActiveManager.ActiveScene.GetComponentWith(name: "Jeff");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DebugManager.Update();

            // TODO: Add your update logic here
            SceneManager.ActiveManager.ActiveScene.Update(gameTime);
            InputManager.Update();
            Physics.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            SceneManager.ActiveManager.ActiveScene.Draw(_spriteBatch);
            Physics.DrawColliders(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}