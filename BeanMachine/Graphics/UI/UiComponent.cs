using BeanMachine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.UI
{
    public class UiComponent : Component, IUiTextCompatable
    {
        new public UiScene Scene;

        public Vector2 Position;

        public float Scale = 1;

        public int Width;
        public int Height;

        private Rectangle Rectangle
        {
            get 
            { 
                return new Rectangle((int)(Position.X * this.Scale * this.Scene.GetScaleToScreenSize()), (int)(Position.Y * this.Scale * this.Scene.GetScaleToScreenSize()), 
                    (int)(Width * this.Scale * this.Scene.GetScaleToScreenSize()), (int)(Height * this.Scale * this.Scene.GetScaleToScreenSize()));
            }       
        }

        public UiComponent(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool IsMouseOverRect()
        {
            Rectangle MouseRectangle = new Rectangle((int)InputManager.Instance.MousePosition().X, (int)InputManager.Instance.MousePosition().Y, 1, 1);

            return this.Rectangle.Intersects(MouseRectangle);
        }


        public bool HasMouseClickedRect()
        {
            return IsMouseOverRect() && InputManager.Instance.WasLeftButtonPressed();
        }

        public override void Update()
        {
            if (!base._started)
                this.Start();
        }

        public override void Start()
        {
            if (this.Scene == null)
                throw new Exception("Ui component needs to be added using the scene manager");

            base._started = true;
        }

        public virtual string GetUiString()
        {
            return "Value Not Set";
        }
    }
}
