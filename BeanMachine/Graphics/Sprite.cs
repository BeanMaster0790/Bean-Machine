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

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

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
                else if (_animationManager != null)
                    return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), _animationManager.CurrentAnimation.FrameWidth, _animationManager.CurrentAnimation.FrameHeight);
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
        }

        public Sprite(Texture2D texture) : this()
        {
            this._texture = texture;

            this.Origin = new Vector2(this._texture.Width / 2, this._texture.Height /2 );
        } 

        public Sprite(Dictionary<string, Animation> animations) : this()
        {
            this._animations = new Dictionary<string, Animation>();

            foreach (KeyValuePair<string, Animation> animation in animations)
            {
                Animation ani = new Animation(animation.Value.Texture, animation.Value.AnimationName, animation.Value.FrameCount, animation.Value.FrameWidth, animation.Value.FrameHeight, animation.Value.AnimationRow)
                { 
                    FrameSpeed = animation.Value.FrameSpeed,
                    IsLooping = animation.Value.IsLooping,
                };

                this._animations.Add(ani.AnimationName, ani);
            }

            this._animationManager = new AnimationManager(this, _animations.First().Value);

            this.Origin = new Vector2(this._animationManager.CurrentAnimation.FrameWidth / 2, this._animationManager.CurrentAnimation.FrameHeight / 2);
        }

        public void AddAddon(Addon addon)
        {
            addon.Parent = this;
            this.Addons.Add(addon);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._texture != null) 
                spriteBatch.Draw(this._texture, this.Position, null, Color.White, MathHelper.ToRadians(this.Rotation), this.Origin, 1, this.Flipped, 0);
            else if(this._animationManager != null)
                this._animationManager.Draw(spriteBatch);
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

        public override void Update()
        {
            base.Update();

            if (this._animationManager != null)
                this._animationManager.Update();

            foreach (Addon addon in this.Addons)
            {
                addon.Update();
            }
        }
    }
}
