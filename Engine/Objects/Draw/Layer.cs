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
        List<Sprite> Sprites = new List<Sprite>();
        List<Text> Texts = new List<Text>();
        string name;

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
            get { return Sprites.Count; }
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
            if (!Sprites.Contains(sprite))
            {
                Sprites.Add(sprite);
            }
        }

        /// <summary>
        /// Removes a drawable sprite from the draw routine
        /// </summary>
        /// <param name="sprite"></param>
        public void Remove(Sprite sprite)
        {
            if (Sprites.Contains(sprite))
            {
                Sprites.Remove(sprite);
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


        public void Update(float elapsedTime)
        {
            foreach (Sprite item in Sprites)
            {
                item.Update(elapsedTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.View);

            visibleSprites = 0;

            foreach (Sprite item in Sprites)
            {
                if (camera.IsInView(item.Position, item.Texture))
                {
                    spriteBatch.Draw(item.Texture, item.Position, item.SourceRect, item.Color, item.Rotation, item.Origin, item.Scale, item.SpriteEffect, 0);
                    visibleSprites++;
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
