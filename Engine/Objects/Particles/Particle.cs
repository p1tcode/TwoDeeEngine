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
        public bool Alive;

        public float TimeToLive;

        public Particle(Texture2D texture) : base(texture)
        {
            Acceleration = Vector2.One;
        }

        public override void Update(float elapsedTime)
        {

            TimeToLive -= elapsedTime;

            if (TimeToLive <= 0)
            {
                Alive = false;
            }

            Position += Velocity * elapsedTime;

            base.Update(elapsedTime);
        }
    }
}
