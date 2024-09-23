using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanMachine.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BeanMachine.Graphics.Tiles
{
    public class Map : Component
    {
        protected string _mapDirectory { get; set; }

        protected RawMapData _rawMapData;

        protected Texture2D[] _textures { get; set; }

        protected int _tileWidth;
        protected int _tileHeight;

        private float _tileScale;

        private int _numberOfTiles;

        protected float _frameSpeed;
        private float _frameTimer;
        private int _currentFrame;

        public float TileScale
        {
            get
            {
                return this._tileScale;
            }
            set
            {
                _tileScale = value;

                this._tileWidth = (int)(this._rawMapData.tilewidth * this._tileScale);
                this._tileHeight = (int)(this._rawMapData.tileheight * this._tileScale);

                this._rectangles = this.GetTileRectangles();
            }
        }

        protected Rectangle[] _rectangles;

        protected List<Tile> _tiles = new List<Tile>();


        public Map(string mapDirectory, string textureDirectory)
        {
            GetTexturesFromDirectory(textureDirectory);

            this._mapDirectory = Directory.GetCurrentDirectory() + "/Content/" + mapDirectory;

            string data = File.OpenText(this._mapDirectory).ReadToEnd();

            this._rawMapData = JsonConvert.DeserializeObject<RawMapData>(data);

            this.TileScale = 1;
            this._numberOfTiles = this._rawMapData.width * this._rawMapData.height;
        }

        private void GetTexturesFromDirectory(string textureDirectory)
        {
            try
            {
                Texture2D texture = Globals.Content.Load<Texture2D>(textureDirectory);

                this._textures = new Texture2D[1];

                this._textures[0] = texture;
            }
            catch
            {
                int index = 1;

                string directory = Directory.GetCurrentDirectory() + "/Content/" + textureDirectory;

                List<Texture2D> textures = new List<Texture2D>();

                while (true)
                {
                    try
                    {
                        Texture2D texture = Globals.Content.Load<Texture2D>(textureDirectory + index);

                        textures.Add(texture);

                        index++;

                        continue;
                    }
                    catch
                    {
                        break;
                    }
                }

                this._textures = textures.ToArray();
            }
        }

        protected Rectangle[] GetTileRectangles()
        {
            int tilesOnWidth = this._textures[0].Width / (int)(this._tileWidth / this._tileScale);
            int tilesOnHeight = this._textures[0].Height / (int)(this._tileHeight / this._tileScale);

            int numberOfTiles = tilesOnWidth * tilesOnHeight;

            Rectangle[] rectangles = new Rectangle[numberOfTiles];

            int xIndex = -1;
            int yIndex = 0;

            for (int i = 0; i < numberOfTiles; i++)
            {

                xIndex++;

                if (xIndex >= tilesOnWidth)
                {
                    xIndex = 0;
                    yIndex++;
                }

                rectangles[i] = new Rectangle(xIndex * (int)(this._tileWidth / this._tileScale),
                    yIndex * (int)(this._tileHeight / this._tileScale),
                    (int)(this._tileWidth / this._tileScale), (int)(this._tileHeight / this._tileScale));

            }

            return rectangles;
        }

        public virtual void LoadMap()
        {
            throw new NotImplementedException("You must create a function that uses the \"LayerData\" property to load tiles");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in this._tiles)
            {
                spriteBatch.Draw(this._textures[this._currentFrame], tile.Position -
                    new Vector2(this._rawMapData.width / 2 * this._tileWidth, this._rawMapData.height / 2 * this._tileHeight) -
                    new Vector2(this._tileWidth / 2, this._tileHeight / 2) -
                    base.Scene.Camera.Position, tile.TextureRectangle, Color.White, 0, Vector2.Zero, this._tileScale, SpriteEffects.None, tile.Layer);
            }
        }

        public override void Update()
        {
            base.Update();

            if (this._frameSpeed == 0)
                return;

            this._frameTimer += Time.Instance.DeltaTime;

            if (this._frameTimer > this._frameSpeed)
            {
                this._currentFrame += 1;
                this._frameTimer = 0;
            }


            if (this._currentFrame >= this._textures.Length)
                this._currentFrame = 0;
        }
    }
}
