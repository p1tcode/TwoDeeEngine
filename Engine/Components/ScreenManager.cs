using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Engine.Objects;

namespace Engine.Components
{
    interface IScreenManager
    {

    }

    public class ScreenManager : DrawableGameComponent, IScreenManager
    {

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();
        List<GameScreen> screensToDraw = new List<GameScreen>();

        IInputManager inputManager;
        ContentManager content;

        new public Game Game
        {
            get { return base.Game; }
        }

        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }
        

        public ScreenManager(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            inputManager = (IInputManager)Game.Services.GetService(typeof(IInputManager));
            content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }

            base.LoadContent();
        }


        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }

            base.UnloadContent();
        }

        /// <summary>
        /// Add a screen to the ScreenManager
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(GameScreen screen)
        {
            screen.LoadContent();
            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the ScreenManager
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        /// <summary>
        /// Update all screens
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
            {
                screensToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (screensToUpdate.Count > 0)
            {
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            screensToDraw.Clear();

            foreach (GameScreen screen in screens)
            {
                screensToDraw.Add(screen);
            }

            foreach (GameScreen screen in screensToDraw)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

    }
}
