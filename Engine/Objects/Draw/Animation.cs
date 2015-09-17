using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public class Animation : Sprite
    {

        Vector2 TileSize;
        /// <summary>
        /// Time each frame is shown
        /// </summary>
        public float Speed;
        float timeToNewFrame;
        
        int rows;
        int columns;
        int rowPosition = 0;
        int colPosition = 0;


        public Animation(Texture2D texture, int tileWidth, int tileHeight, float speed) : base(texture)
        {
            this.Texture = texture;
            this.TileSize = new Vector2(tileWidth, tileHeight);
            this.rows = (texture.Height / tileHeight) - 1;
            this.columns = (texture.Width / tileWidth) - 1;
            this.Speed = speed;
            this.timeToNewFrame = Speed;
            this.SourceRect = new Rectangle(0, 0, (int)TileSize.X, (int)TileSize.Y);
            this.Origin = new Vector2(tileWidth / 2, tileHeight / 2);
        }

        public override void Update(float elapsedTime)
        {
            timeToNewFrame -= elapsedTime;

            if (timeToNewFrame < 0)
            {
                if (colPosition < columns)
                {
                    colPosition++;
                }
                else
                {
                    if (rowPosition < rows)
                    {
                        rowPosition++;
                    }
                    else
                    {
                        rowPosition = 0;
                    }
                    
                    colPosition = 0;
                }

                timeToNewFrame = Speed;
                
                SourceRect = new Rectangle(colPosition * (int)TileSize.X, rowPosition * (int)TileSize.Y, (int)TileSize.X, (int)TileSize.Y);
            }

            base.Update(elapsedTime);
        }
        
    }
}
