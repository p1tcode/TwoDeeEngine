using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;

namespace Engine.Components
{
    public class Layer
    {
        List<Sprite> sprites = new List<Sprite>();

        List<Text> Texts = new List<Text>();
        string name;
        int index = 0;

        Vector2 shadowOffset = new Vector2(2, 2);

        #region Properties

        /// <summary>
        /// The name of the Layer
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        public int NumberOfSprites
        {
            get { return sprites.Count; }
        }
        
        public int NumberOfText
        {
            get { return Texts.Count; }
        }

        private int visibleSprites = 0;
        public int VisibleSprites
        {
            get { return visibleSprites; }
        }
        #endregion


        public Layer(string n)
        {
            this.name = n;
        }


        /// <summary>
        /// Adds sprite to the draw routine
        /// </summary>
        /// <param name="sprite">The spirite to draw</param>
        public void Add(Sprite sprite)
        {
            sprite.Index = index;
            sprites.Add(sprite);
            index++;
        }

        /// <summary>
        /// Removes a drawable sprite from the draw routine
        /// </summary>
        /// <param name="sprite"></param>
        public void Remove(Sprite sprite)
        {
            for (int i = sprites.Count - 1; i >= 0; i--)
            {
                if (sprites[i].Index == sprite.Index)
                {
                    sprites.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adds sprite to the draw routine
        /// </summary>
        /// <param name="sprite">The spirite to draw</param>
        public void AddText(Text text)
        {
            if (!Texts.Contains(text))
            {
                Texts.Add(text);
            }
        }

        /// <summary>
        /// Removes a drawable sprite from the draw routine
        /// </summary>
        /// <param name="sprite"></param>
        public void RemoveText(Text text)
        {
            Texts.Remove(text);
        }


        public void Update(GameTime gameTime)
        {
            foreach (Sprite item in sprites)
            {
                if (!(item is Particle))
                {
                    item.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.View);

            visibleSprites = 0;

            foreach (Sprite item in sprites)
            {
                if (camera.IsInView(item.Position, item.Texture))
                {
                    if (item.Alive)
                    {
                        //spriteBatch.Draw(item.Texture, item.Position, item.SourceRect, item.Color, item.Rotation, item.Origin, item.Scale, item.SpriteEffect, 0);
                        item.Draw(spriteBatch);
                        visibleSprites++;
                    }
                }
            }
                

            foreach (Text item in Texts)
            {
                spriteBatch.DrawString(item.Font, item.OutputText, (item.Position + camera.Position - camera.ScreenCenter + shadowOffset), Color.Black, item.Rotation, item.Origin, item.Scale, item.SpriteEffect, item.Layer);
                spriteBatch.DrawString(item.Font, item.OutputText, (item.Position + camera.Position - camera.ScreenCenter), item.Color, item.Rotation, item.Origin, item.Scale, item.SpriteEffect, item.Layer);
            }
            spriteBatch.End();
        }
    }
}
