using BeanMachine.Graphics;
using BeanMachine.Graphics.Lighting;
using BeanMachine.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BeanMachine.Scenes
{
    public class Scene
    {
        public Camera Camera;

        public bool _loaded { get; private set; }

        public string Name { get; set; }

        private List<Component> _sceneComponents;

        public SoundManager SoundManager { get; private set; }

        public LightingManager LightingManager { get; private set; }


        public Scene(string name)
        {
            this._sceneComponents = new List<Component>();

            this.Name = name;

            this.Camera = new Camera(GraphicsManager.Instance.GraphicsDevice);
            this.Camera.Scene = this;

            this.SoundManager = new SoundManager();

            this.LightingManager = new LightingManager(this.Camera);
        }

        public virtual void AddToScene(Component component)
        {
            component.Scene = this;
            this._sceneComponents.Add(component);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Camera.Draw(spriteBatch, this._sceneComponents);
        }

        public T GetComponentWith<T>(string tag = null, string name = null)
        {
            if (tag != null && name == null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Tag != tag)
                        continue;

                    if (component.GetType() == typeof(T))
                        return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            else if (name != null && tag == null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name)
                        continue;

                    if (component.GetType() == typeof(T))
                        return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            else if (name != null && tag != null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name || component.Tag != tag)
                        continue;

                    if (component.GetType() == typeof(T))
                        return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            else
                throw new Exception("'GetComonentWith' Needs an argument");

            return default(T);
        }

        public T[] GetComponentsWith<T>(string tag = null, string name = null)
        {
            List<T> components = new List<T>();

            if (tag != null && name == null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Tag != tag)
                        continue;

                    components.Add((T)Convert.ChangeType(component, typeof(T)));
                }
            }

            else if (name != null && tag == null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name)
                        continue;

                    components.Add((T)Convert.ChangeType(component, typeof(T)));
                }
            }

            else if (name != null && tag != null)
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name || component.Tag != tag)
                        continue;

                    components.Add((T)Convert.ChangeType(component, typeof(T)));
                }
            }

            else
                throw new Exception("'GetComonentsWith()' Needs an argument!");

            return components.ToArray();
        }

        public virtual void LoadScene(object caller = null)
        {
            if (caller.GetType() != typeof(SceneManager) || caller == null)
                throw new Exception("'LoadScene()' should only be called by the scene manager!");

            if (this._loaded)
                return;

            this._loaded = true;
        }

        public virtual void LateUpdate()
        {
            foreach (Component component in this._sceneComponents.ToArray())
            {
                if(component.IsActive)
                    component.LateUpdate();
            }
        }

        public virtual void UnloadScene(object caller = null)
        {
            if (caller.GetType() != typeof(SceneManager) || caller == null)
                throw new Exception("'UnloadScene()' should only be called by the scene manager!");

            if (this._loaded == false)
                return;

            foreach (Component component in this._sceneComponents)
            {
                component.Destroy();
            }

            this.SoundManager.Destroy();

            this._loaded = false;
        }

        public virtual void Update()
        {
            this.SoundManager.AudioListener.Position = new Vector3(this.Camera.Position, 0);

            foreach (Component component in this._sceneComponents.ToArray())
            {
                if (component.ToRemove)
                {
                    this._sceneComponents.Remove(component);
                    continue;
                }

                if(component.IsActive)
                    component.Update();
            }
        }

    }
}
