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
        public float TimeToLive;
        public float RemainingTimeToLive;
        public float Growth;
        
        public Color OriginalColor { get; set; }

        float elapsedTime;
                
        //Ctor
        public Particle(Texture2D texture) : base(texture) { }


        public override void Update(GameTime gameTime)
        {
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            RemainingTimeToLive -= elapsedTime;

            if (RemainingTimeToLive <= 0)
            {
                Alive = false;
            }

            Position += Velocity * elapsedTime;
            Scale += new Vector2(Growth * elapsedTime);

            base.Update(gameTime);
        }
    }
}
