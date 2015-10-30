using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Objects;

namespace Engine.Components
{
    interface IInputManager
    {
        Vector2 MousePosition { get; }
        int ScrollWheelValue { get; set; }
        Vector2 LeftStick(PlayerIndex player);
        Vector2 RightStick(PlayerIndex player);
        float LeftTrigger(PlayerIndex player);
        float RightTrigger(PlayerIndex player);
        Vector2 MouseWorldPosition { get; }
        void MouseGrabSprite(Sprite sprite, string action);
    }

    public class InputManger : IInputManager
    {
        List<Action> actions = new List<Action>();

        KeyboardState kbState = new KeyboardState();
        GamePadState[] gpState = new GamePadState[4];
        MouseState msState = new MouseState();

        private bool mouseGrabbedSprite = false;
        Vector2 mouseGrabOffsetSprite = Vector2.Zero;
        Vector2 mouseGrabOffsetWorld = Vector2.Zero;

        ICamera2D camera;


        #region Properties
        /// <summary>
        /// Retrive the position of the mouse
        /// </summary>
        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(msState.X, msState.Y);
            }
        }

        public int ScrollWheelValue
        {
            get
            {
                return msState.ScrollWheelValue;
            }
            set
            {
                ScrollWheelValue = value;
            }
        }

        /// <summary>
        /// The deltavalue of the left stick movement.
        /// </summary>
        /// <param name="player">Whitch controller to get data from</param>
        /// <returns>Returns a Vector2</returns>
        public Vector2 LeftStick(PlayerIndex player)
        {
            return new Vector2(gpState[(int)player].ThumbSticks.Left.X, -gpState[(int)player].ThumbSticks.Left.Y);
        }
        /// <summary>
        /// The deltavalue of the right stick movement.
        /// </summary>
        /// <param name="player">Whitch controller to get data from</param>
        /// <returns>Returns a Vector2</returns>
        public Vector2 RightStick(PlayerIndex player)
        {
            return new Vector2(gpState[(int)player].ThumbSticks.Right.X, gpState[(int)player].ThumbSticks.Right.Y);
        }
        /// <summary>
        /// Returns the deltavalue of the left trigger
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Returns a float</returns>
        public float LeftTrigger(PlayerIndex player)
        {
            return gpState[(int)player].Triggers.Left;
        }
        /// <summary>
        /// Returns the deltavalue of the Right trigger
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Returns a float</returns>
        public float RightTrigger(PlayerIndex player)
        {
            return gpState[(int)player].Triggers.Right;
        }
        #endregion

        public Vector2 MouseWorldPosition
        {
            get
            {
                return camera.Position + ((this.MousePosition - camera.ScreenCenter) / camera.Scale);
            }
        }

        public InputManger(Game game)
        {
            game.Services.AddService(typeof(IInputManager), this);

            // Add some default keys
            AddAction("Left");
            this["Left"].Add(Keys.A);
            this["Left"].Add(Buttons.DPadLeft);

            AddAction("Right");
            this["Right"].Add(Keys.D);
            this["Right"].Add(Buttons.DPadRight);

            AddAction("Up");
            this["Up"].Add(Keys.W);
            this["Up"].Add(Buttons.DPadUp);

            AddAction("Down");
            this["Down"].Add(Keys.S);
            this["Down"].Add(Buttons.DPadDown);
        }

        public void Initialize(Game game)
        {
            // Here is a good place to grab services.
            this.camera = (ICamera2D)game.Services.GetService(typeof(ICamera2D));

        }

        /// <summary>
        /// List of actions
        /// </summary>
        /// <param name="ActionName"></param>
        /// <returns></returns>
        public Action this[string ActionName]
        {
            get
            {
                return actions.Find((Action a)=>{ return a.Name == ActionName; });
            }
        }


        /// <summary>
        /// Adds an action to the input manager
        /// </summary>
        /// <param name="actionName"></param>
        public void AddAction(string actionName)
        {
            actions.Add(new Action(actionName));
        }


        /// <summary>
        /// Update all actions
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            msState = Mouse.GetState();
            for (int i = 0; i < 4; i++)
            {
                gpState[i] = GamePad.GetState((PlayerIndex)i);   
            }

            foreach (Action a in actions)
            {
                a.Update(kbState, msState, gpState);
            }
        }


        /// <summary>
        /// Grabs a specified sprite if the mouse is hoovering the sprite
        /// </summary>
        /// <param name="sprite">Sprite to grab</param>
        /// <param name="action">What action that defines the keys used to trigger the grab</param>
        public void MouseGrabSprite(Sprite sprite, string action)
        {
            if (this[action].IsDown)
            {

                Rectangle containRect = new Rectangle((int)sprite.Position.X  - (int)sprite.Texture.Width / 2, 
                                            (int)sprite.Position.Y - (int)sprite.Texture.Height / 2, 
                                            containRect.Width = sprite.Texture.Width,
                                            containRect.Height = sprite.Texture.Height);

                if (containRect.Contains(MouseWorldPosition))
                {
                    if (!mouseGrabbedSprite)
                    {
                        mouseGrabOffsetSprite = sprite.Position - MouseWorldPosition;
                        mouseGrabbedSprite = true;
                    }
                    sprite.Position = MouseWorldPosition + mouseGrabOffsetSprite;
                }
                else
                    mouseGrabbedSprite = false;
            }
            else
                mouseGrabbedSprite = false;
        }

        /// <summary>
        /// Move camera based on grabbing the world and dragging around
        /// Will conflict with other camera.SetTarget or camera.JumptoTarget calls.
        /// </summary>
        /// <param name="action">What action(key binding) will activate the dragging</param>
        public void MouseGrabWorld(string action)
        {
            if (this[action].IsDown)
            {
                camera.JumpToTarget(-MousePosition + mouseGrabOffsetWorld);
            }
            else
            {
                mouseGrabOffsetWorld = MousePosition + camera.Position;
            }
        }
    }
}
