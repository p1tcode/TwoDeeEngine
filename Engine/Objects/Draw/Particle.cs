using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public class Particle : Sprite
    {
        public Vector2 Vector;
        public Vector2 Acceleration;

        public Particle(Texture2D texture) : base(texture)
        {
            
        }
    }
}
