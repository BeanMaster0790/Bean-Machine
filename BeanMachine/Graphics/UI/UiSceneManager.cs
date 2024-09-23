using BeanMachine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.UI
{
    public class UiSceneManager
    {
        public static UiSceneManager Instance = new UiSceneManager();

        public UiScene ActiveScene { get; private set; }

        private Dictionary<string, UiScene> _scenes = new Dictionary<string, UiScene>();

        private RenderTarget2D _renderTarget;

        public UiSceneManager()
        {
            Vector2 screenBounds = GraphicsManager.Instance.GetScreenSize();

            this._renderTarget = new RenderTarget2D(GraphicsManager.Instance.GraphicsDevice, (int)screenBounds.X, (int)screenBounds.Y);
        }

        public void AddNewScene(UiScene scene)
        {
            DoesSceneExist(scene.Name, flip: true);

            _scenes.Add(scene.Name, scene);
        }

        public void RemoveScene(string sceneName)
        {
            DoesSceneExist(sceneName);

            _scenes.Remove(sceneName);
        }

        public UiScene GetScene(string sceneName)
        {
            this.DoesSceneExist(sceneName);

            return _scenes[sceneName];
        }


        public void DrawScenes(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (KeyValuePair<string, UiScene> scene in this._scenes)
            {
                if(scene.Value.Show)
                    scene.Value.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void DoesSceneExist(string sceneName, bool flip = false)
        {
            if (this._scenes.TryGetValue(sceneName, out UiScene scene))
            {
                if (scene == null && !flip)
                {
                    throw new NullReferenceException($"'{sceneName}' Does not excit! You may need to add your scene using 'AddNewScene()'.");
                }
                else if (scene != null && flip)
                {
                    throw new Exception($"'{sceneName}' Already exists!");
                }
            }
        }

        public void Update()
        {
            foreach (KeyValuePair<string, UiScene> scene in this._scenes)
            {
                if (scene.Value.Show)
                    scene.Value.Update();
            }
        }
    }
}
