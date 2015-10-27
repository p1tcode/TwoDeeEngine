using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Components;

namespace Engine.Objects
{
    public enum ScreenState
    {
        Active,
        Hidden,
    }

    public abstract class GameScreen
    {

        /// <summary>
        /// What state the screen is in
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }
        ScreenState screenState = ScreenState.Hidden;

        /// <summary>
        /// Checks if the screen is exiting
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected set { isExiting = value; }
        }
        bool isExiting;

        /// <summary>
        /// If a screen is a popup it will not remove the screen underneath in the screen stack
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }
        bool isPopup;


        /// <summary>
        /// Checks if a screen is active
        /// </summary>
        public bool IsActive
        {
            get { return !otherScreenHasFocus &&
                         (screenState == ScreenState.Active);
                }
        }
        bool otherScreenHasFocus;

        /// <summary>
        /// Get the manager this screen belongs to
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        ScreenManager screenManager;

        
        public ObjectManager ObjectManager
        {
            get { return objectManager; }
            protected set { objectManager = value; }
        }
        ObjectManager objectManager;


        public GameScreen()
        {
            objectManager = new ObjectManager(screenManager.Game);
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                ScreenManager.RemoveScreen(this);
            }
            else if(coveredByOtherScreen)
            {
                screenState = ScreenState.Hidden;
            }
            else
            {
                screenState = ScreenState.Active;
            }
        }

        public abstract void Draw(GameTime gameTime);

    }
}
