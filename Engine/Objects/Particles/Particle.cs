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

        public Particle(Texture2D texture) : base(texture)
        {
            Acceleration = Vector2.One;
        }
    }
}
