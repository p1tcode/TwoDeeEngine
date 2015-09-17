using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Engine.Objects
{
    public class Action
    {
        string name;
        List<Keys> keys = new List<Keys>();
        List<MouseButtons> mouseButtons = new List<MouseButtons>();
        List<Buttons> buttons = new List<Buttons>();
        bool currentState = false;
        bool previousState = false;

        /// <summary>
        /// Check if a key is held down
        /// </summary>
        public bool IsDown
        {
            get
            {
                return currentState;
            }
        }

        /// <summary>
        /// Check if a key is held down, but it will only be triggered once.
        /// </summary>
        public bool IsClicked
        {
            get
            {
                return (currentState) && (!previousState);
            }
        }

        /// <summary>
        /// The name of the Action
        /// </summary>
        public string Name
        {
            get 
            {
                return name;
            }
        }

        /// <summary>
        /// Describes an action whitch you can bind keys, buttons or mouseactions to
        /// </summary>
        /// <param name="n">Name of the action</param>
        public Action(string n)
        {
            this.name = n;
        }

        /// <summary>
        /// Add a key to the action
        /// </summary>
        /// <param name="key"></param>
        public void Add(Keys key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
        }
        /// <summary>
        /// Add a button to the action
        /// </summary>
        /// <param name="button"></param>
        public void Add(Buttons button)
        {
            if (!buttons.Contains(button))
            {
                buttons.Add(button);
            }
        }
        /// <summary>
        /// Add a mousebutton to the action
        /// </summary>
        /// <param name="button"></param>
        public void Add(MouseButtons button)
        {
            if (!mouseButtons.Contains(button))
            {
                mouseButtons.Add(button);
            }
        }

        /// <summary>
        /// Check for key/button/mouse presses and update the action
        /// </summary>
        /// <param name="kbState"></param>
        /// <param name="msState"></param>
        /// <param name="gpState"></param>
        internal void Update(KeyboardState kbState, MouseState msState, GamePadState[] gpState)
        {
            previousState = currentState;
            currentState = false;

            foreach (Keys k in keys)
            {
                if (kbState.IsKeyDown(k))
                {
                    currentState = true;
                }
            }
            foreach (GamePadState gamepad in gpState)
            {
                foreach (Buttons button in buttons)
                {
                    
                    if (gamepad.IsConnected)
                    {
                        if (gamepad.IsButtonDown(button))
                        {
                            currentState = true;
                        }
                    }
                }
            }
            foreach (MouseButtons button in mouseButtons)
            {
                switch (button)
	            {
                    case MouseButtons.Left:
                        if (msState.LeftButton == ButtonState.Pressed) currentState = true;
                        break;
                    case MouseButtons.Right:
                        if (msState.RightButton == ButtonState.Pressed) currentState = true;
                        break;
                    case MouseButtons.Middle:
                        if (msState.MiddleButton == ButtonState.Pressed) currentState = true;
                        break;
                    case MouseButtons.Extra1:
                        if (msState.XButton1 == ButtonState.Pressed) currentState = true;
                        break;
                    case MouseButtons.Extra2:
                        if (msState.XButton2 == ButtonState.Pressed) currentState = true;
                        break;
	            }
            }
        }
    }

    /// <summary>
    /// A enum of the diffrent mouse behaviors
    /// </summary>
    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        Extra1,
        Extra2,
    }
}
