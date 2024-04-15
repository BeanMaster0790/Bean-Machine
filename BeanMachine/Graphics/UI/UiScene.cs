using BeanMachine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.UI
{
    public class UiScene
    {
        public bool Show;

        public string Name { get; set; }

        private List<UiComponent> _sceneComponents;

        public int SceneWidth;
        public int SceneHeight;

        public UiScene(string name, int width, int height)
        {
            this._sceneComponents = new List<UiComponent>();

            this.Name = name;

            this.SceneWidth = width;
            this.SceneHeight = height;

            Vector2 screenBounds = GraphicsManager.Instance.GetScreenSize();

            if (screenBounds.Y < this.SceneHeight)
                this.SceneHeight = (int)screenBounds.Y;

            if (screenBounds.X < this.SceneWidth)
                this.SceneWidth = (int)screenBounds.X;
        }

        public virtual void AddToScene(UiComponent component)
        {
            component.Scene = this;
            this._sceneComponents.Add(component);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (UiComponent component in this._sceneComponents)
            {
                component.Draw(spriteBatch);
            }
        }

        public T GetComponentWith<T>(string tag = null, string name = null)
        {
            if (tag != null && name == null)
            {
                foreach (UiComponent component in this._sceneComponents.ToArray())
                {
                    if (component.Tag != tag)
                        continue;

                    if (component.GetType() == typeof(T))
                        return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            else if (name != null && tag == null)
            {
                foreach (UiComponent component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name)
                        continue;

                    if (component.GetType() == typeof(T))
                        return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            else if (name != null && tag != null)
            {
                foreach (UiComponent component in this._sceneComponents.ToArray())
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
                foreach (UiComponent component in this._sceneComponents.ToArray())
                {
                    if (component.Tag != tag)
                        continue;

                    components.Add((T)Convert.ChangeType(component, typeof(T)));
                }
            }

            else if (name != null && tag == null)
            {
                foreach (UiComponent component in this._sceneComponents.ToArray())
                {
                    if (component.Name != name)
                        continue;

                    components.Add((T)Convert.ChangeType(component, typeof(T)));
                }
            }

            else if (name != null && tag != null)
            {
                foreach (UiComponent component in this._sceneComponents.ToArray())
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

        public float GetScaleToScreenSize()
        {
            float scale = GraphicsManager.Instance.GetScreenSize().X / (float)this.SceneWidth;

            return scale;
        }

        public virtual void LateUpdate()
        {
            foreach (Component component in this._sceneComponents.ToArray())
            {
                component.LateUpdate();
            }
        }

        public virtual void UnloadScene()
        {
            foreach (Component component in this._sceneComponents)
            {
                component.Destroy();
            }
        }

        public virtual void Update()
        {

            foreach (UiComponent component in this._sceneComponents.ToArray())
            {
                if (component.ToRemove)
                {
                    this._sceneComponents.Remove(component);
                    continue;
                }

                component.Update();
            }
        }

    }
}
