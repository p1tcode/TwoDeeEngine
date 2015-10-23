using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using Engine.Components;

namespace Engine
{
    public class FpsCounter : DrawableGameComponent
    {
        Text text;
        IObjectManager objectManager;
        

        int frameRate = 0;
        int frameCounter = 0;
        float memory;

        TimeSpan elapsedTime = TimeSpan.Zero;

        public FpsCounter(Game game, SpriteFont Font) : base(game)
        {
            this.text = new Text(Font);
            this.text.Color = Color.Red;
            this.text.Position = new Vector2(10, 5);
            
        }

        public override void Initialize()
        {
            this.objectManager = (IObjectManager)Game.Services.GetService(typeof(IObjectManager));
            objectManager["Text"].AddText(text);

            base.Initialize();
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
           
            this.text.OutputText = string.Format("FPS: {0} \nMem: {1} MB", frameRate, memory.ToString("F"));

            base.Draw(gameTime);
        }
    }
}
