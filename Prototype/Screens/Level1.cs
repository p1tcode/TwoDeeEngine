using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace Prototype
{
    class Level1 : GameScreen
    {
        ParticleEmitter emitterSmoke;
        ParticleEmitter emitterSpark;

        Sprite fallingSprite;

        public override void LoadContent()
        {
            Texture2D tex = ScreenManager.Content.Load<Texture2D>(@"crate");

            for (int i = 0; i < 40; i++)
            {
                Body body = BodyFactory.CreateRectangle(ScreenManager.World, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(64 * i + 32, 1048);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                ObjectManager["Player"].Add(staticSprite);
            }
            for (int i = 0; i < 25; i++)
            {
                Body body = BodyFactory.CreateRectangle(ScreenManager.World, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(32, 1048 - 64 * i);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                ObjectManager["Player"].Add(staticSprite);
            }
            for (int i = 0; i < 25; i++)
            {
                Body body = BodyFactory.CreateRectangle(ScreenManager.World, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(29 * 64 + 32, 1048 - 64 * i);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                ObjectManager["Player"].Add(staticSprite);
            }

            ScreenManager.InputManager.AddAction("Trigger");
            ScreenManager.InputManager["Trigger"].Add(MouseButtons.Left);

            //emitterSmoke = new ParticleEmitter(Vector2.Zero, 1, "Foreground", true, false);
            //ScreenManager.ParticleManager["Smoke"].AddEmitter(emitterSmoke, ObjectManager);
            //emitterSpark = new ParticleEmitter(Vector2.Zero, 1, "Foreground", true, false);
            //ScreenManager.ParticleManager["Spark"].AddEmitter(emitterSpark, ObjectManager);


            base.LoadContent();
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Debug.WriteLine(coveredByOtherScreen);
            if (!otherScreenHasFocus)
            {
                if (ScreenManager.InputManager["Next"].IsClicked)
                {
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.AddScreen(new Level2());
                }
            }

            if (ScreenManager.InputManager["Trigger"].IsClicked)
            {
                Texture2D tex = ScreenManager.Content.Load<Texture2D>(@"crate");
                Body body = BodyFactory.CreateRectangle(ScreenManager.World, tex.Width, tex.Height, 1);
                fallingSprite = new Sprite(tex, body);
                fallingSprite.Position = ScreenManager.InputManager.MouseWorldPosition;
                fallingSprite.Body.Friction = 1f;
                fallingSprite.Body.BodyType = BodyType.Dynamic;
                ObjectManager["Player"].Add(fallingSprite);
            }


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
