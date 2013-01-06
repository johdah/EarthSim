using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace EarthSim.Components
{
    /*
     * #Add to Properties
     * private static Input _input = null;
     * /// <summary>
     * /// The input helper for menus, gamepads, keyboard and mouse.
     * /// </summary>
     * public static Input Input
     * {
     *      get { return _input; }
     * }
     * 
     * #Add to Game constructor
     * // Init the Input
     * _input = new Input(this);
     * Components.Add(_input);
     */
    public class InputComponent : GameComponent
    {
        public KeyboardState CurrentKeyboardState;
        public GamePadState CurrentGamePadState;
        public MouseState CurrentMouseState;

        public KeyboardState LastKeyboardState;
        public GamePadState LastGamePadState;
        public MouseState LastMouseState;

        private Point _lastMouseLocation;

        private Vector2 _mouseMoved;
        public Vector2 MouseMoved
        {
            get { return _mouseMoved; }
        }

        public InputComponent(Game game)
            : base(game)
        {
            Enabled = true;
        }

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;
            LastMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            CurrentMouseState = Mouse.GetState();

            _mouseMoved = new Vector2(LastMouseState.X - CurrentMouseState.X, LastMouseState.Y - CurrentMouseState.Y);
            _lastMouseLocation = new Point(CurrentMouseState.X, CurrentMouseState.Y);
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyUp(key) &&
                    LastKeyboardState.IsKeyDown(key));
            /*return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyUp(key));*/
        }

        #region Actions

        public bool IsDownPressed()
        {
            return IsNewKeyPress(Keys.Down);
        }

        public bool IsUpPressed()
        {
            return IsNewKeyPress(Keys.Up);
        }

        public bool IsLeftPressed()
        {
            return IsNewKeyPress(Keys.Left);
        }

        public bool IsRightPressed()
        {
            return IsNewKeyPress(Keys.Right);
        }

        public bool IsSwitchPressed()
        {
            return IsNewKeyPress(Keys.Home);
        }

        /// <summary>
        /// Checks for a "pause the game" input action (on either keyboard or gamepad).
        /// </summary>
        public bool PauseGame
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       (CurrentGamePadState.Buttons.Back == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Back == ButtonState.Released) ||
                       (CurrentGamePadState.Buttons.Start == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Start == ButtonState.Released);
            }
        }

        #endregion
    }
}
