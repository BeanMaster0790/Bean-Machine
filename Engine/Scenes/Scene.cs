using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeanMachine.Scenes
{
    public class Scene
    {

        public string Name { get; set; }

        public bool _loaded { get; private set; }

        private List<Component> _sceneComponents;

        public Scene(string name)
        { 
            this._sceneComponents = new List<Component>();
            this.Name = name;
        }

        public virtual void LoadScene(object caller = null)
        {
            if (caller.GetType() != typeof(SceneManager) || caller == null)
                throw new Exception("'LoadScene()' should only be called by the scene manager!");

            if(this._loaded)
                return;

            this._loaded = true;
        }

        public virtual void UnloadScene(object caller = null)
        {
            if (caller.GetType() != typeof(SceneManager) || caller == null)
                throw new Exception("'UnloadScene()' should only be called by the scene manager!");

            if (_loaded == false)
                return;

            foreach (Component component in _sceneComponents)
            {
                component.Destroy();
            }

            this._loaded = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Component component in this._sceneComponents.ToArray())
            {
                if (component.ToRemove)
                {
                    _sceneComponents.Remove(component);
                    continue;
                }

                component.Update(gameTime);
            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component component in this._sceneComponents.ToArray())
            {
                component.Draw(spriteBatch);
            }
        }

        public virtual void AddToScene(Component component)
        {
            component.Scene = this;
            this._sceneComponents.Add(component);
        }

        public Component GetComponentWith(string tag = "None", string name = "None")
        {
            if (tag != "None")
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Tag == tag)
                        return component;
                }
            }

            else if (name != "None")
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name == name)
                        return component;
                }
            }

            else
                throw new Exception("'GetComonentWith' Needs an argument");

            return null;
        }

        public Component[] GetComponentsWith(string tag = "None", string name = "None")
        {
            List<Component> components = new List<Component>();

            if (tag != "None")
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Tag == tag)
                        components.Add(component);
                }
            }

            else if (name != "None")
            {
                foreach (Component component in this._sceneComponents.ToArray())
                {
                    if (component.Name == name)
                        components.Add(component);
                }
            }

            else
                throw new Exception("'GetComonentsWith()' Needs an argument!");

            return components.ToArray();
        }
    }
}
