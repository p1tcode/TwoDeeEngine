using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype
{
    class Level1 : GameScreen
    {
        Sprite player;

        public override void LoadContent()
        {
            player = new Sprite(ScreenManager.Content.Load<Texture2D>("crate"));
            player.Position = new Vector2(500, 500);
            ObjectManager["Player"].Add(player);


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
            

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
