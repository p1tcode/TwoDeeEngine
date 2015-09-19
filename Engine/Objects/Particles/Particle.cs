using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public class Particle : Sprite
    {
        public Vector2 Acceleration;
        public Vector2 Velocity;
        public Vector2 Direction;

        float elapsedTime;

        public float TimeToLive;

        public Particle(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            TimeToLive -= elapsedTime;

            if (TimeToLive <= 0)
            {
                Alive = false;
            }

            Position += Velocity * elapsedTime;

            base.Update(gameTime);
        }
    }
}
