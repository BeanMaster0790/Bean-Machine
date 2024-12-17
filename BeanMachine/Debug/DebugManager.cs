using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMachine.Player;
using Microsoft.Xna.Framework.Input;
using BeanMachine.PhysicsSystem;
using BeanMachine.Graphics;
using BeanMachine.Scenes;

namespace BeanMachine.Debug
{
	public class DebugManager
	{
		public static DebugManager Instance = new DebugManager();
		
		private List<IDebuggable> _debuggables = new List<IDebuggable>();

		private SpriteFont _font;

		private bool ShowDebugInfo = false;
		private bool ShowColliders = false;
		private bool ShowLines = false;
		private bool FreeCam = false;

		private Texture2D _pixel;

		private List<Line> _linesToDraw = new List<Line>();

		public void Start()
		{
			this._font = Globals.Content.Load<SpriteFont>("Fonts/DebugFont");
		}

		public void AddDebugabble(IDebuggable debuggable)
		{
			this._debuggables.Add(debuggable);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if(this._pixel == null)
			{
				this._pixel = new Texture2D(GraphicsManager.Instance.GraphicsDevice, 1, 1);
				this._pixel.SetData(new Color[] {Color.White});
			}

			if (this.ShowDebugInfo)
				DrawDebugInfo(spriteBatch);

			if(this.ShowColliders)
				Physics.Instance.DrawColliders(spriteBatch);

			if(this.ShowLines)
				DrawLines(spriteBatch);

		}

		public void Update()
		{
			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.D))
				this.ShowDebugInfo = !this.ShowDebugInfo;

			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.C))
				this.ShowColliders = !this.ShowColliders;

			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.R))
				this.ShowLines = !this.ShowLines;

			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.F))
				this.FreeCam = !this.FreeCam;

			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.G))
				SceneManager.Instance.ActiveScene.LightingManager.SetMapSize(this, null);

			if (InputManager.Instance.IsKeyHeld(Keys.F12) && InputManager.Instance.WasKeyPressed(Keys.L))
				SceneManager.Instance.ActiveScene.Camera.EnableLighting = !SceneManager.Instance.ActiveScene.Camera.EnableLighting;

			if (this.FreeCam)
				MoveCam();
		}

		private void MoveCam()
		{
			if (InputManager.Instance.IsKeyHeld(Keys.W))
			{
				SceneManager.Instance.ActiveScene.Camera.Position.Y -= 5;
			}
			if (InputManager.Instance.IsKeyHeld(Keys.S))
			{
				SceneManager.Instance.ActiveScene.Camera.Position.Y += 5;
			}
			if (InputManager.Instance.IsKeyHeld(Keys.A))
			{
				SceneManager.Instance.ActiveScene.Camera.Position.X -= 5;
			}
			if (InputManager.Instance.IsKeyHeld(Keys.D))
			{
				SceneManager.Instance.ActiveScene.Camera.Position.X += 5;
			}

			if (InputManager.Instance.IsKeyHeld(Keys.Add))
			{
				SceneManager.Instance.ActiveScene.Camera.MoveZ(3);
			}
			if (InputManager.Instance.IsKeyHeld(Keys.Subtract))
			{
				SceneManager.Instance.ActiveScene.Camera.MoveZ(-3);
			}
		}

		private void DrawLines(SpriteBatch spriteBatch)
		{
			foreach (Line line in this._linesToDraw.ToArray())
			{
				Vector2 lineScale = new Vector2(line.Length, line.Thickness);

				spriteBatch.Draw(this._pixel, line.StartPoint - SceneManager.Instance.ActiveScene.Camera.Position, null, line.Colour, MathHelper.ToRadians(line.Angle), new Vector2(0, line.Thickness / 2), lineScale, SpriteEffects.None, 0.91f);

				this._linesToDraw.Remove(line);
			}
		}

		public void DrawLine(Line line)
		{
			if(this.ShowLines)
				this._linesToDraw.Add(line);
		}

		public void DrawLine(Line line, Color colour, int thickness)
		{
			line.Colour = colour;
			line.Thickness = thickness;

			this.DrawLine(line);
		}

		public void DrawLine(Vector2 origin, Vector2 direction ,float length, Color colour, int thickness)
		{
			Vector2 endPoint = origin + direction * length;

			Line line = new Line(origin, endPoint, colour, thickness);

			this.DrawLine(line);
		}

		public void DrawLine(Vector2 origin, float direction, float length, Color colour, int thickness)
		{
			Vector2 vectorDirection = new Vector2(MathF.Cos(direction), MathF.Sin(direction));

			this.DrawLine(origin, vectorDirection, length, colour, thickness);	
		}

		private void DrawDebugInfo(SpriteBatch spriteBatch)
		{
			foreach (IDebuggable debugItem in this._debuggables)
			{
				if (!debugItem.ShowDebugInfo)
					continue;

				Vector2 lastPosition = debugItem.GetDebugDrawPosition();

				foreach (string debugValue in debugItem.DebugValueNames)
				{
					string displayedText = "Error";


					displayedText = debugValue + ": " + debugItem.GetType().GetField(debugValue).GetValue(debugItem);

					Vector2 drawPosition = new Vector2(lastPosition.X, lastPosition.Y);
					lastPosition = new Vector2(drawPosition.X, drawPosition.Y + this._font.MeasureString(displayedText).Y);

					spriteBatch.DrawString(this._font, displayedText, drawPosition - SceneManager.Instance.ActiveScene.Camera.Position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
				}
			}
		}
	}
}
