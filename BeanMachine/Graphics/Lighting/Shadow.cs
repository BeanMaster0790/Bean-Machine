using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.Lighting
{
	public class Shadow : Addon
	{
		public Vector2[] points;

		public Shadow(Vector2[] shadowPoints) 
		{ 
			this.points = shadowPoints;
		}

		public override void Start()
		{
			base.Start();

			base.Parent.Scene.LightingManager.AddShadow(this);

		}

		public override void Destroy()
		{
			base.Destroy();

			base.Parent.Scene.LightingManager.RemoveShadow(this);
		}
	}
}
