using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    class InputManager
    {
        MouseState oldMouseState;
        KeyboardState oldKeyboardState;
        GamePadState oldGamepadstate;
        BalanceBoard m_balanceBoard;

        public const int KEYBOARDMOUSE = 0; //Bewegungsstates
        public const int GAMEPADONLY = 1;
        public const int GAMEPADBOARD = 2;
        private int inputMode = 0;
        int screenWidth;
        int screenHeight;
        UpdateInfo updateinfo;

        public InputManager(GraphicsDevice graphics, int initialInputmode, UpdateInfo info) 
        
        {
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            inputMode = initialInputmode;
            updateinfo = info;
            m_balanceBoard = new BalanceBoard();
        }

        public void wndProc(ref System.Windows.Forms.Message mes)
        {
            if (m_balanceBoard != null)
                m_balanceBoard.wndproc(ref mes);
        }

        private void movePlayable(Playable player, GamePadState padstate, MouseState mousestate, KeyboardState keystate) 
        {
            switch (inputMode)
                {
                    case 0: mouseMovement(player, keystate, mousestate); break;
                    case 1: gamePadMovement(player, padstate); break;
                    case 2: boardMovement(player); break;
                }
            }

        public void update(Playable candy, Playable helper) {

            KeyboardState keystate = Keyboard.GetState();
            GamePadState padstate = GamePad.GetState(PlayerIndex.One);
            MouseState mousestate = Mouse.GetState();

            updateinfo.currentpushedKeys.Clear();
            
            
            /*add if-statements*/

            if (updateinfo.locked) {

                /* Menu, Dialogues, nonmovementstuff*/
            }

            if(updateinfo.candyselected && ! updateinfo.locked )
                movePlayable(candy, padstate, mousestate, keystate);
            else if(! updateinfo.candyselected && ! updateinfo.locked)
                movePlayable(helper, padstate, mousestate, keystate);


        }
        
        /// <summary>
        /// Moves the player with keyboard and mouse input
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keystate"></param>
        /// <param name="mousestate"></param>
        /// 

        private void mouseMovement(Playable player, KeyboardState keystate, MouseState mousestate)
        {
            //Calculate the difference for Camera Movement and normalize it
            float dcamx = mousestate.X - screenWidth / 2;
            float dcamy = mousestate.Y - screenHeight / 2;
            //normalize the movement difference
            dcamx /= screenWidth / 4;
            dcamy /= screenHeight / 4;


            //get the Keyboard Input to Move the player
            float dmovextemp = 0;
            float dmoveytemp = 0;

            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            if (!updateinfo.alwaysRun)
            {
                if (keystate.IsKeyDown(Keys.W)) dmoveytemp += 1f;
                if (keystate.IsKeyDown(Keys.S)) dmoveytemp -= 1f;
                if (keystate.IsKeyDown(Keys.A)) dmovextemp += 1f;
                if (keystate.IsKeyDown(Keys.D)) dmovextemp -= 1f;
            }
            else
            {
                alpha = 0;
                dmoveytemp += 1.2f;
                if (keystate.IsKeyDown(Keys.S)) dmoveytemp -= 1.2f;
                if (keystate.IsKeyDown(Keys.A)) dmovextemp += 1.2f;
                if (keystate.IsKeyDown(Keys.D)) dmovextemp -= 1.2f;
            }
            if (keystate.IsKeyDown(Keys.Space)
                && keystate.IsKeyDown(Keys.Space) != oldKeyboardState.IsKeyDown(Keys.Space))
            {
                 updateinfo.currentpushedKeys.Add(Keys.Space);
                 player.uniqueskill();
            }
            if (keystate.IsKeyDown(Keys.M)
                && oldKeyboardState != keystate) player.switchCameraPerspective(); 
            if(keystate.IsKeyDown(Keys.LeftAlt)
                && oldKeyboardState != keystate) updateinfo.currentpushedKeys.Add(Keys.LeftAlt);
            if (keystate.IsKeyDown(Keys.Q)
                && oldKeyboardState != keystate) updateinfo.currentpushedKeys.Add(Keys.Q);
            if (keystate.IsKeyDown(Keys.C)
                && oldKeyboardState != keystate
                &&player.isInThirdP()) updateinfo.switchPlayer();

            if (keystate.IsKeyDown(Keys.R)
                 &&  !oldKeyboardState.IsKeyDown(Keys.R)) updateinfo.reset = true;




            //rotate the movementvector to kamerakoordinates
            float dmovex =(float) Math.Cos(alpha) * dmovextemp - (float)Math.Sin(alpha) * dmoveytemp;
            float dmovey = (float) Math.Sin(alpha) * dmovextemp + (float) Math.Cos(alpha) * dmoveytemp;


            //move the player
            if (player.getIsThirdPersonCam())
                player.movementInput(dmovex, dmovey, dcamx, dcamy);
            else
                player.movementInput(dmovextemp * 0.1f, dmoveytemp * 0.1f, 0, 0);
            //reset mouse to the center of the screen, to rotate freely
            Mouse.SetPosition(screenWidth / 2, screenHeight / 2);

            oldKeyboardState = keystate;
            oldMouseState = mousestate;
        }
        

        private void boardMovement(Playable player)
        {
            
            BalanceBoardState boardstate = m_balanceBoard.getState();
            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            //rotate the movementvector to kamerakoordinates
            float dmovex =  -(float)Math.Sin(alpha) * boardstate.Y;
            float dmovey =  (float)Math.Cos(alpha) * boardstate.Y;


            //move the player
            float amplifyingWalkingSpeedFactor = 5;
            float camSlow = 0.25f;
            float camFast = 0.7f;
            float maxSpeed = 1;
            float movementScale = Math.Abs(dmovex) + Math.Abs(dmovey);
            float camRotation = 0;

            // no camera rotation in a small area, then slow and then fast rotation
            if (Math.Abs(boardstate.X) >= 0.04f)
            {
                if(Math.Abs(boardstate.X) >= 0.1f)
                    camRotation = - boardstate.X * camFast;
                else
                    camRotation = - boardstate.X * camSlow;
            }

            // only move, when input is strong enough, then accelerate up to a maximum speed
            if (Math.Abs(boardstate.Y) >= 0.05f)
            {
                if (movementScale > maxSpeed)
                {
                    // put down to max speed
                    dmovex *= (maxSpeed / movementScale);
                    dmovey *= (maxSpeed / movementScale);
                }

                player.movementInput(dmovex * amplifyingWalkingSpeedFactor, dmovey * amplifyingWalkingSpeedFactor, 0, 0);
            }
            else
                player.movementInput(0, 0, camRotation, 0);

        }

        private void gamePadMovement(Playable player, GamePadState padstate)
        {
            if (padstate.IsButtonDown(Buttons.A)
                && padstate.IsButtonDown(Buttons.A) != oldGamepadstate.IsButtonDown(Buttons.A))
            {
                updateinfo.currentpushedKeys.Add(Keys.Space); //Space because its the equivalent to Buttons.A
                player.uniqueskill();
            }
            if (padstate.IsButtonDown(Buttons.Y)
                && oldGamepadstate.IsButtonDown(Buttons.Y) 
                != padstate.IsButtonDown(Buttons.Y)) player.switchCameraPerspective();
            if (padstate.IsButtonDown(Buttons.B)
                && oldGamepadstate != padstate) updateinfo.currentpushedKeys.Add(Keys.LeftAlt);
            if (padstate.IsButtonDown(Buttons.X)
                && oldGamepadstate != padstate) updateinfo.currentpushedKeys.Add(Keys.Q);
            if (padstate.Triggers.Left > 0.7f
                && oldGamepadstate.Triggers.Left < 0.5f && player.isInThirdP()) updateinfo.switchPlayer();

            if (padstate.IsButtonDown(Buttons.LeftShoulder) && padstate.IsButtonDown(Buttons.RightShoulder)) player.Reset();

            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            //rotate the movementvector to kamerakoordinates
            float dmovex = (float)Math.Cos(alpha) * -padstate.ThumbSticks.Left.X 
                - (float)Math.Sin(alpha) * padstate.ThumbSticks.Left.Y;
            float dmovey = (float)Math.Sin(alpha) * -padstate.ThumbSticks.Left.X
                + (float)Math.Cos(alpha) * padstate.ThumbSticks.Left.Y;
            //move the player
            player.movementInput(dmovex, dmovey,
                0.6f * -padstate.ThumbSticks.Right.X, 0.6f * padstate.ThumbSticks.Right.Y);
            //reset mouse to the center of the screen, to rotate freely

            oldGamepadstate = padstate;
            
        }


        public int getInputMode(){return inputMode;}

        public void setInputMode(int state)
        {
            if (state <= 2 && 0 <= state) inputMode = state;
            else throw new NotImplementedException();
        }

    }
}
