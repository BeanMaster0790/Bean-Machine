using BeanMachine.Graphics;
using BeanMachine.Graphics.Lighting;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using BeanMachine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BeanMachine.Testing
{
    public class TestScene : Scene
    {

        private CustomShape _shape;

        private Sprite _ed;
        private Sprite _light;
        private Sprite _shadow;
        private Sprite _floor;

        public TestScene(string name) : base(name)
        {

        }

        public override void LoadScene(object caller = null)
        {
            base.LoadScene(caller);

            base.LightingManager.DarknessColour = new Color(5, 5, 5);

            this.Camera.SetZ(this.Camera.GetZFromHeight(1440));

            List<Vector2> points = new List<Vector2>() {new Vector2(-40,-40), new Vector2(40, -40), new Vector2(40,40), new Vector2(-40,40)};

            this._shape = new CustomShape();

            this._shape.SetPoints(points);


            this._floor = new Sprite(this._shape.Texture);
            this._floor.Scale = 1000f;
            this._floor.Colour = Color.DarkOliveGreen;
            this._floor.layer = 0f;

            this._ed = new Sprite(Globals.Content.Load<Texture2D>("Textures/Debug/TestImage"));

            this._ed.Scale = 1;

            this._light = new Sprite();

            this._light.AddAddon(new Light(10f, 500, 100, new Color(255,255,255)));

            base.AddToScene(this._floor);
            base.AddToScene(this._ed);
            base.AddToScene(this._light);

            for (int i = 0; i < 20; i++)
            {
                this._shadow = new Sprite(this._shape.Texture);

                this._shadow.Scale = 1f;

                this._shadow.AddAddon(new Shadow(points.ToArray()));

                this._shadow.Position = new Vector2(Random.RandomInt(-500, 500), Random.RandomInt(-500, 500));
                //this._shadow.Position = new Vector2(0,0);

                base.AddToScene(this._shadow);
            }
        }

        public override void Update()
        {
            base.Update();

            this._ed.Rotation += 1;

            this._light.Position = this.Camera.ScreenToWorld(InputManager.Instance.MousePosition());

            if (InputManager.Instance.WasLeftButtonPressed())
            {
				Sprite light = new Sprite();

				light.AddAddon(new Light(10f, 300, 100, new Color(255, 255, 255)));
                
                light.Position = this._light.Position;

                base.AddToScene(light);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
            base.Draw(spriteBatch);
		}
	}
}
