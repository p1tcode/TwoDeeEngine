using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public class Text
    {
        /// <summary>
        /// Font to be used to draw the text
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// The text to be drawn
        /// </summary>
        public string OutputText;

        /// <summary>
        /// Position of the sprite
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Color tint of the sprite
        /// </summary>
        public Color Color;

        /// <summary>
        /// Rotation of the sprite
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Origin of the sprite
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Scale of the sprite
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// SpriteEffect to be used on the sprite
        /// </summary>
        public SpriteEffects SpriteEffect;

        /// <summary>
        /// At what layer to draw the sprite
        /// </summary>
        public float Layer;


        public Text(SpriteFont font)
        {
            this.Font = font;
            this.OutputText = "";
            this.Position = Vector2.Zero;
            this.Color = Color.White;
            this.Rotation = 0;
            this.Origin = Vector2.Zero;
            this.Scale = Vector2.One;
            this.SpriteEffect = SpriteEffects.None;
            this.Layer = 0;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.Font, this.OutputText, this.Position, this.Color, this.Rotation, this.Origin, this.Scale, this.SpriteEffect, this.Layer);
        }

    }
}
