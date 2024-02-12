using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace BeanMachine.Player
{
    public class InputManager
    {
        public static InputManager Instance = new InputManager();

        public KeyboardState CurrentKeyboardState;

        public MouseState CurrentMouseState;

        public KeyboardState PreviousKeyboardState;

        public MouseState PreviousMouseState;

        public bool IsKeyHeld(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);

        }

        public bool IsKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        public Vector2 MousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }

        public void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public bool WasKeyPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }

        public bool WasRightButtonPressed()
        {
            return PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released;
        }

        public bool WasLeftButtonPressed()
        {
            return PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released;
        }

    }
}
