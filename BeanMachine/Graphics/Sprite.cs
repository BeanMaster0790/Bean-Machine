using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BeanMachine.Graphics.Animations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BeanMachine.Graphics
{
    public class Sprite : Component
    {
        public List<Addon> Addons = new List<Addon>();

        public Color Colour = Color.White;

        public SpriteEffects Flipped;

        protected Texture2D _texture;

        public Vector2 Origin;

        public Vector2 Position;

        public Sprite Parent;

        public float Rotation;

        private int _rectWidth;

        private int _rectHeight;

        public Rectangle Rectangle
        { 
            get
            {
                if (_texture != null)
                    return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), _texture.Width, _texture.Height);
                else
                    return new Rectangle((int)Position.X, (int)Position.Y, _rectWidth, _rectHeight);
            }
        }

        public Vector2 Velocity;

        public Sprite( int rectWidth = 0, int rectHeight = 0) : base()
        {       

            if(rectWidth != 0)
                this._rectWidth = rectWidth;

            if(rectHeight != 0)
                this._rectHeight = rectHeight;

            this.Origin = new Vector2(rectWidth / 2, rectHeight / 2);
        }

        public Sprite(Texture2D texture) : this()
        {
            this._texture = texture;

            this.Origin = new Vector2(this._texture.Width / 2, this._texture.Height /2 );
        } 

        public void AddAddon(Addon addon)
        {
            addon.Parent = this;
            this.Addons.Add(addon);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager animationManager = this.GetAddon<AnimationManager>();


            if (animationManager != null)
            {
                animationManager.Draw(spriteBatch);
                return;
            }

            else if (this._texture != null)
                spriteBatch.Draw(this._texture, this.Position, null, this.Colour, MathHelper.ToRadians(this.Rotation), this.Origin, 1, this.Flipped, 0f);

        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (Addon addon in this.Addons.ToArray())
            {
                addon.Destroy();
                this.Addons.Remove(addon);
            }
        }

        public T GetAddon<T>()
        {
            foreach (Addon addon in this.Addons)
            {
                if (addon.GetType() == typeof(T))
                    return (T)Convert.ChangeType(addon, typeof(T));
            }
            return default(T);
        }

        public T[] GetAddons<T>()
        {
            List<T> list = new List<T>();

            foreach (Addon addon in this.Addons)
            {
                if (addon.GetType() == typeof(T))
                    list.Add((T)Convert.ChangeType(addon, typeof(T)));
            }
            
            return list.ToArray();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            foreach (Addon addon in this.Addons)
            {
                addon.Update();
            }
        }
    }
}
