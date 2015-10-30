using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using System.Diagnostics;

namespace Engine.Objects
{
    public class Sprite
    {
        /// <summary>
        /// Texture for the sprite
        /// </summary>
        public Texture2D Texture;

        private Vector2 position;
        /// <summary>
        /// Position of the sprite
        /// </summary>
        public Vector2 Position
        {
            get
            {
                if (Body != null)
                {
                    return Body.Position;
                }
                else
                {
                    return position;
                }
            }
            set
            {
                if (Body != null)
                {
                    Body.Position = value;
                }
                else
                {
                    position = value;
                }
            }
        }

        /// <summary>
        /// What part of the texture to be shown
        /// </summary>
        public Rectangle SourceRect;

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
        public int Layer;

        /// <summary>
        /// If an sprites is alive and active.
        /// </summary>
        public bool Alive;

        /// <summary>
        /// A unique searchable index.
        /// </summary>
        public int Index;

        /// <summary>
        /// Body containing all physics information
        /// </summary>
        public Body Body = null;


        public Sprite(Texture2D texture, Body body) : this(texture)
        {
            this.Body = body;
        }
        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
            this.Position = Vector2.Zero;
            this.SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            this.Color = Color.White;
            this.Rotation = 0;
            this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.Scale = Vector2.One;
            this.SpriteEffect = SpriteEffects.None;
            this.Alive = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            if (Body != null)
            {
                this.Position = Body.Position;
                this.Rotation = Body.Rotation;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.SourceRect, this.Color, this.Rotation, this.Origin, this.Scale, this.SpriteEffect, 0);
        }
    }
}
