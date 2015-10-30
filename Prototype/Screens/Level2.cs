using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype
{
    class Level2 : GameScreen
    {
        Sprite player;

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

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
