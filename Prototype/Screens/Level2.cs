using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Prototype
{
    class Level2 : GameScreen
    {
        Sprite player;
        Sprite fallingSprite;

        public Level2()
        {
        }

        public override void LoadContent()
        {
            player = new Sprite(ScreenManager.Content.Load<Texture2D>("crate"));
            player.Position = new Vector2(100, 100);
            ObjectManager["Player"].Add(player);

            ScreenManager.Camera.JumpToTarget(ScreenManager.Camera.Origin);


            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (ScreenManager.InputManager["Next"].IsClicked)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new Level1());
            }

            
            if (ScreenManager.InputManager["Trigger"].IsClicked)
            {
                Texture2D tex = ScreenManager.Content.Load<Texture2D>(@"crate");
                Body body = BodyFactory.CreateRectangle(ScreenManager.World, ConvertUnits.ToSimUnits(tex.Width), ConvertUnits.ToSimUnits(tex.Height), 1);
                fallingSprite = new Sprite(tex, body);
                fallingSprite.Position = ScreenManager.InputManager.MouseWorldPosition;
                fallingSprite.Body.Friction = 1f;
                fallingSprite.Body.BodyType = BodyType.Dynamic;
                ObjectManager["Player"].Add(fallingSprite);
            }

            ScreenManager.InputManager.MouseGrabWorld("CameraMove");

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
