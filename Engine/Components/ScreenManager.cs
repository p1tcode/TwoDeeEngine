using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Engine.Objects;
using Engine.Helpers;

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

        
        Camera2D camera;
        ParticleManager particleManager;
        FpsCounter fps;

        bool traceEnabled = true;


        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        new public Game Game
        {
            get { return base.Game; }
        }

        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }

        public InputManger InputManager
        {
            get { return inputManager; }
        }
        InputManger inputManager;


        public ScreenManager(Game game) : base(game) { }

        public override void Initialize()
        {
            // Load content
            content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            // Load Camera
            camera = new Camera2D(this.Game, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Game.Components.Add(camera);
            camera.JumpToTarget(camera.Origin);

            // Load input manager
            inputManager = new InputManger(this.Game);
            Game.Components.Add(inputManager);
            PreFabs.Initialize_Input(inputManager);

            // Load particleManager
            particleManager = new ParticleManager(this.Game);
            Game.Components.Add(particleManager);
            PreFabs.Initialize_ParticleEffects(particleManager);

            fps = new FpsCounter(this.Game);
            Game.Components.Add(fps);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            PreFabs.LoadContent_ParticleEffects(particleManager);

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
            screen.ScreenManager = this;
            screen.Initialize();
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
            screen = null;
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

            if (traceEnabled)
                TraceScreens();

            base.Update(gameTime);
        }

        /// <summary>
        /// Prist a list of all the screens for debugging
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
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
