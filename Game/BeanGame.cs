using BeanMachine;
using BeanMachine.Graphics;
using BeanMachine.Scenes;
using BeanMachine.Testing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Rename class and namespace
namespace BeanGame
{
    public class BeanGame : Engine
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            SceneManager.Instance.ActiveScene.Draw(spriteBatch);
        }

        public override void Load()
        {
            base.Load();

            GraphicsManager.Instance.SetFullScreen(false);
            GraphicsManager.Instance.SetScreenSize(1280, 720);
            GraphicsManager.Instance.ApplyChanges();

            TestScene tempScene = new TestScene("Temp Scene");

            SceneManager.Instance.AddNewScene(tempScene);

            SceneManager.Instance.LoadScene(tempScene.Name);

            SceneManager.Instance.SetActiveScene(tempScene.Name);
        }

        public override void Update()
        {
            base.Update();

            SceneManager.Instance.ActiveScene.Update();
        }
    }
}
