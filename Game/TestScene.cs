using BeanMachine.Graphics;
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

        public TestScene(string name) : base(name)
        {

        }

        public override void LoadScene(object caller = null)
        {
            base.LoadScene(caller);

            List<Vector2> points = new List<Vector2>() {new Vector2(5,0), new Vector2(4, 10), new Vector2(20, 17) };

            this._shape = new CustomShape(points);

            Sprite sprite = new Sprite(_shape.Texture);

            sprite.Scale = 10;

            base.AddToScene(sprite);
        }

        public override void Update()
        {
            base.Update();
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
            base.Draw(spriteBatch);
		}
	}
}
