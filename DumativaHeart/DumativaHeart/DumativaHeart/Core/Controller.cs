/*XNA Game - Pirate Blast
 * Developed by Marcos T.B. Soarers
 * e-mail: markosthiago@gmail.com
 * Controller class
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DumativaHeart.Core
{
    static class Controller
    {
        //Variables to store the current and previous states of both the Keyboard and Mouse
        static KeyboardState previousKeyboardState, currentKeyboardState;
        static MouseState previousMouseState, currentMouseState;

        //X-Coordinate of the mouse cursor
        public static int MouseX
        {
            get { return currentMouseState.X; }
            set { Mouse.SetPosition(value, currentMouseState.X); }
        }

        //Y-coordinate of the mouse cursor
        public static int MouseY
        {
            get { return currentMouseState.Y; }
            set { Mouse.SetPosition(currentMouseState.Y, value); }
        }
        
        //Updates the mouse and keyboard states
        public static void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        public static bool KeyReleased(Keys key)
        {
            return (currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key));
        }


        /// <summary>
        /// Detects if key has just been pressed
        /// </summary>
        /// <param name="key">The key to be analyzed</param>
        /// <returns>Returns true if pressed</returns>
        public static bool KeyPressed(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Detects if Key is currently pressed
        /// </summary>
        /// <param name="key">The key to be analyzed</param>
        /// <returns>Returns true if pressed</returns>
        public static bool KeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Detects if left button of the mouse is currently pressed
        /// </summary>
        /// <returns></returns>
        public static bool MouseLeftPressed()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Detects if right button of the mouse is currently pressed
        /// </summary>
        /// <returns></returns>
        public static bool MouseRightPressed()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Detects if middle mouse button is currently pressed
        /// </summary>
        /// <returns></returns>
        public static bool MouseMiddlePressed()
        {
            return (currentMouseState.MiddleButton == ButtonState.Pressed);
        }

        public static Rectangle MouseRect
        {
            get
            {
                return new Rectangle(MouseX, MouseY, 1, 1);
            }
        }

        //List of default commands and key binds
        public struct KeySet
        {
            public static Keys Up = Keys.Up;
            public static Keys Down = Keys.Down;
            public static Keys Left = Keys.Left;
            public static Keys Right = Keys.Right;
            public static Keys Confirm = Keys.Enter;


        }

    }
}
