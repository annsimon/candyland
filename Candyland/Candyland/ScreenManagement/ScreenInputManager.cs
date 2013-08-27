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
        Continue,
        Back,
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

            if (keyState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A)
                || keyState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))
            {
                oldKeyboardState = keyState;
                return InputState.Left;
            }
            if (keyState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D)
                || keyState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))
            {
                oldKeyboardState = keyState;
                return InputState.Right;
            }
            if (keyState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W)
                || keyState.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up))
            {
                oldKeyboardState = keyState;
                return InputState.Up;
            }
            if (keyState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S)
                || keyState.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down))
            {
                oldKeyboardState = keyState;
                return InputState.Down;
            }
            if (keyState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter)
                || keyState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
                oldKeyboardState = keyState;
                return InputState.Continue;
            }
            if (keyState.IsKeyDown(Keys.Back) && oldKeyboardState.IsKeyUp(Keys.Back)
                || keyState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
            {
                oldKeyboardState = keyState;
                return InputState.Back;
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
