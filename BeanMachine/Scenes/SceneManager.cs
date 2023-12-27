using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Scenes
{
    public class SceneManager
    {
        public static SceneManager ActiveManager = new SceneManager();

        private Dictionary<string,Scene> _scenes = new Dictionary<string, Scene>();

        public Scene ActiveScene { get; private set; }

        public void AddNewScene(string sceneName, Scene scene)
        {
            DoesSceneExist(sceneName, flip: true);

            _scenes.Add(sceneName, scene);
        }

        public void RemoveScene(string sceneName)
        { 
            DoesSceneExist(sceneName);

            _scenes.Remove(sceneName);
        }

        public void LoadScene(string sceneName)
        {
            DoesSceneExist(sceneName);

            _scenes[sceneName].LoadScene(caller: this);
        }

        public void UnloadScene(string sceneName)
        {
            DoesSceneExist(sceneName);

            _scenes[sceneName].UnloadScene(caller: this);
        }

        public void SetActiveScene(string sceneName)
        {
            DoesSceneExist(sceneName);

            Scene scene = _scenes[sceneName];

            if (!scene._loaded)
                throw new Exception("Scene has to be loaded before it can be active");

            this.ActiveScene = scene;
        }

        public void DoesSceneExist(string sceneName, bool flip = false)
        {
            if(this._scenes.TryGetValue(sceneName, out Scene scene))
            {
                if(scene == null && !flip)
                {
                    throw new NullReferenceException($"'{sceneName}' Does not excit! You may need to add your scene using 'AddNewScene()'.");
                }
                else if(scene != null && flip)
                {
                    throw new Exception($"'{sceneName}' Already exists!");
                }
            }
        }

    }
}
