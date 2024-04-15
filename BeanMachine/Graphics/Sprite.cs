using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BeanMachine.Graphics.Animations;
using System.Collections.Generic;
using System.Linq;
using System;
using BeanMachine.Debug;
using System.Diagnostics;

namespace BeanMachine.Graphics
{
    public class Sprite : Component
    {
        public List<Addon> Addons = new List<Addon>();

        public Color Colour = Color.White;

        public float layer = 0;

        public Vector2 Origin
        {
            get
            {
                if (this._userChangedOrigin)
                    return Origin;

                if (this.HasAnimationManager())
                    return this.GetAddon<AnimationManager>().CurrentAnimation.GetCenterOrigin();

                if (this._texture == null)
                    return Vector2.Zero;

                return new Vector2(this._texture.Width / 2, this._texture.Height / 2);

            }

            private set { }
        }
        private bool _userChangedOrigin;

        public Vector2 Position;

        public Sprite Parent;

        public float Rotation;

        public float Scale;

        protected Texture2D _texture;

        private int _rectWidth;

        private int _rectHeight;

        public bool HasAnimationManager()
        {
            AnimationManager animationManager = this.GetAddon<AnimationManager>();

            if (animationManager != null)
                return true;
            else
                return false;
        }

        public Rectangle GetSpriteRectangle()
        {
            if (this.HasAnimationManager())
            {
                Vector2 widthAndHeight = this.GetAddon<AnimationManager>().CurrentAnimation.GetFrameDimentions();
                return new Rectangle((int)(Position.X - Origin.X * this.Scale), (int)(Position.Y - Origin.Y * this.Scale), (int)(widthAndHeight.X * this.Scale), (int)(widthAndHeight.Y * this.Scale));
            }

            if (_texture != null)
                return new Rectangle((int)(Position.X - Origin.X * this.Scale), (int)(Position.Y - Origin.Y * this.Scale), (int)(this._texture.Width * this.Scale), (int)(this._texture.Height * this.Scale));
            else
                return new Rectangle((int)Position.X, (int)Position.Y, _rectWidth, _rectHeight);
        }

        public Vector2 Velocity;

        public Sprite(int rectWidth = 0, int rectHeight = 0) : base()
        {

            if (rectWidth != 0)
                this._rectWidth = rectWidth;

            if (rectHeight != 0)
                this._rectHeight = rectHeight;
        }

        public Sprite(Texture2D texture, float scale = 1) : this()
        {
            this._texture = texture;

            this.Scale = scale;
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
                spriteBatch.Draw(this._texture, this.Position - base.Scene.Camera.Position, null, this.Colour, MathHelper.ToRadians(this.Rotation), this.Origin, this.Scale, SpriteEffects.None, this.layer);

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

        public override void LateUpdate()
        {
            base.LateUpdate();

            foreach (Addon addon in this.Addons)
            {
                addon.LateUpdate();
            }

        }


        public override void Start()
        {
            base.Start();
        }

        public void SetOrigin(Vector2 origin)
        {
            this.Origin = origin;
            this._userChangedOrigin = true;
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
