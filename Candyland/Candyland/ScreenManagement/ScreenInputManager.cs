using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public enum InputState
    {
        Left,
        Right,
        Up,
        Down,
        Enter,
        None
    }

    class ScreenInputManager
    {
        KeyboardState oldKeyboardState;
        GamePadState oldGamepadState;

        public const int KEYBOARDMOUSE = 0; //Bewegungsstates
        public const int GAMEPADONLY = 1;
        public const int GAMEPADBOARD = 2;
        private int inputMode = GameConstants.inputManagerMode;

        public InputState getInput()
        {
            switch (inputMode)
            {
                case 0: return getKeyboardInput();
                case 1: return getGamepadInput();
            }
            return InputState.None;
        }


        public InputState getKeyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))
            {
                oldKeyboardState = keyState;
                return InputState.Left;
            }
            if (keyState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                oldKeyboardState = keyState;
                return InputState.Right;
            }
            if (keyState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W))
            {
                oldKeyboardState = keyState;
                return InputState.Up;
            }
            if (keyState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                oldKeyboardState = keyState;
                return InputState.Down;
            }
            if (keyState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
            {
                oldKeyboardState = keyState;
                return InputState.Enter;
            }
            else
            {
                oldKeyboardState = keyState;
                return InputState.None;
            }
        }


        private InputState getGamepadInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            oldGamepadState = gamepadState;
            return InputState.None;
        }
    }
}
