using BeanMachine.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.UI
{
    public class UiImage : UiComponent
    {
        private Texture2D _texture;

        public UiImage(Texture2D texture) : base(texture.Width, texture.Height)
        {
            this._texture = texture;

        }

        public override void Start()
        {
            base.Start();

            float totalPixels =  base.Scale * (this._texture.Width * base.Scene.GetScaleToScreenSize()) * (this._texture.Height * base.Scene.GetScaleToScreenSize());
            float screenPixels = GraphicsManager.Instance.GetScreenSize().X * GraphicsManager.Instance.GetScreenSize().Y;
            float percentScreenTaken = (totalPixels / screenPixels) * 100;

            DebugManager.Instance.Monitor(totalPixels, "Image Pixels");
            DebugManager.Instance.Monitor(screenPixels, "Screen Pixels");
            DebugManager.Instance.Monitor(percentScreenTaken.ToString("#0.000"), "% of screen taken");

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, base.Position * base.Scale *base.Scene.GetScaleToScreenSize(), null, Color.White, 0, Vector2.Zero, base.Scale * base.Scene.GetScaleToScreenSize(), SpriteEffects.None, 0);
        }

        public override void Update()
        {
            base.Update();

            if (base.HasMouseClickedRect())
            {
                DebugManager.Instance.Log(base.HasMouseClickedRect());
            }
        }

        public override string GetUiString()
        {
            return this._texture.Name;
        }
    }
}
