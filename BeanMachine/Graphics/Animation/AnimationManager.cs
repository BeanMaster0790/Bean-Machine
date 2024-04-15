using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using BeanMachine.Graphics.Animation;
using System.IO;

namespace BeanMachine.Graphics.Animations
{
    public class AnimationManager : Addon
    {
        public Dictionary<string, Animation> Animations;

        public Animation CurrentAnimation;

        public EventHandler<FrameEventArgs> FrameEvent;

        private bool _isPlaying;

        private float _timer;

        private string _animationsDirectory;

        private Texture2D _texture;

        public AnimationManager(Texture2D SheetTexture, string dataDirectory) : base()
        {
            this._texture = SheetTexture;

            this._animationsDirectory = Directory.GetCurrentDirectory() + "/Content/" + dataDirectory;

            LoadAnimations();

            this._isPlaying = false;
        }

        private void LoadAnimations()
        {
            this.Animations = new Dictionary<string, Animation>();

            string data = File.OpenText(this._animationsDirectory).ReadToEnd();

            ImageFrames imageFrames = JsonConvert.DeserializeObject<ImageFrames>(data);

            List<string> frameTags = new List<string>();

            foreach (FrameTag tag in imageFrames.meta.frameTags)
            {
                frameTags.Add(tag.name);

                Animation animation = new Animation(tag.name);

                this.Animations.Add(tag.name, animation);
            }

            foreach (Frame frame in imageFrames.frames)
            {
                foreach (string animationName in frameTags)
                {
                    string frameName = frame.filename.Split('#')[1];
                    frameName = frameName.Split('.')[0];

                    if (frameName.Contains(" "))
                        frameName = frameName.Split(" ")[0];

                    this.Animations[frameName].Frames.Add(frame);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (this.CurrentAnimation.Frames == 0)
            //{
            //    spriteBatch.Draw(this.CurrentAnimation.Texture, base.Parent.Position - base.Parent.Scene.Camera.Position,
            //    new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, 0, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
            //    Parent.Colour, MathHelper.ToRadians(base.Parent.Rotation), base.Parent.Origin, base.Parent.Scale, SpriteEffects.FlipVertically, 0);
            //}
            //else
            //{
            //    spriteBatch.Draw(this.CurrentAnimation.Texture, base.Parent.Position - base.Parent.Scene.Camera.Position,
            //    new Rectangle(this.CurrentAnimation.CurrentFrame * this.CurrentAnimation.FrameWidth, this.CurrentAnimation.AnimationRow * this.CurrentAnimation.FrameHeight, this.CurrentAnimation.FrameWidth, this.CurrentAnimation.FrameHeight),
            //    Parent.Colour, MathHelper.ToRadians(base.Parent.Rotation), base.Parent.Origin, base.Parent.Scale, SpriteEffects.FlipVertically, 0);
            //}

            spriteBatch.Draw(this._texture, base.Parent.Position - base.Parent.Scene.Camera.Position,
                CurrentAnimation.GetTextureRectangle(),
                Parent.Colour, MathHelper.ToRadians(base.Parent.Rotation), base.Parent.Origin, base.Parent.Scale, SpriteEffects.None, base.Parent.layer);
        }

        public void Play(Animation animation)
        {
            if (this.CurrentAnimation == animation)
                return;

            this._isPlaying = true;

            this.CurrentAnimation = animation;
            this.CurrentAnimation.CurrentFrame = 0;

            this._timer = 0;
        }

        public void Pause()
        {
            this._isPlaying = false;
        }

        public void Resume()
        {
            this._isPlaying = true;
        }

        public void Stop()
        {
            this._isPlaying = false;

            this.CurrentAnimation.CurrentFrame = 0;
            this._timer = 0;
        }

        public override void Update()
        {

            if (!this._isPlaying)
                return;

            this._timer += Time.Instance.DeltaTime;

            if (this._timer >= CurrentAnimation.FrameSpeed)
            {
                this._timer = 0;

                this.CurrentAnimation.CurrentFrame++;

                FrameEventArgs args = new FrameEventArgs();
                args.AnimationName = this.CurrentAnimation.AnimationName;
                args.CurrentFrame = this.CurrentAnimation.CurrentFrame;

                FrameEvent?.Invoke(this, args);

                if (this.CurrentAnimation.CurrentFrame >= this.CurrentAnimation.FrameCount && this.CurrentAnimation.IsLooping)
                {
                    this.CurrentAnimation.CurrentFrame = 0;
                }
                else if (this.CurrentAnimation.CurrentFrame > this.CurrentAnimation.FrameCount)
                {
                    Stop();
                }
            }
        }

    }

    public class FrameEventArgs : EventArgs
    {
        public string AnimationName;

        public int CurrentFrame;
    }
}
