using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Engine
{
    public class FpsCounter : DrawableGameComponent
    {
        string text;        

        int frameRate = 0;
        int frameCounter = 0;
        float memory;

        TimeSpan elapsedTime = TimeSpan.Zero;

        public FpsCounter(Game game) : base(game)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            memory = (float)GC.GetTotalMemory(false) / 1024 / 1024;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
           
            text = string.Format("FPS: {0} \nMem: {1} MB", frameRate, memory.ToString("F"));
            Debug.WriteLine(text);
            base.Draw(gameTime);
        }
    }
}
