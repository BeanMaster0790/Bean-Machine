using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Player
{
    public static class InputManager
    {
        public static KeyboardState PreviousKeyboardState;
        public static KeyboardState CurrentKeyboardState;

        public static MouseState PreviousMouseState;
        public static MouseState CurrentMouseState;

        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyHeld(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);

        }
        public static bool IsKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        public static bool WasRightButtonPressed()
        {
            return PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released;
        }

        public static bool WasLeftButtonPressed()
        {
            return PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released;
        }

        public static Vector2 MousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }
    }
}
