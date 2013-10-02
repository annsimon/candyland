﻿using System;
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

        float amplifyingWalkingSpeedFactor = 5;
        float camSlow = 0.17f;
        float camFast = 0.7f;
        float maxSpeed = 0.54f;

        float cutCam = 0.04f;
        float cutCamFast = 0.07f;
        float cutWalk = 0.11f;

        KeyboardState oldKeyState;

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
                    case 2: boardMovement(player, keystate); break;
                }
            }

        public void update(Playable candy)
        {

            KeyboardState keystate = Keyboard.GetState();
            GamePadState padstate = GamePad.GetState(PlayerIndex.One);
            MouseState mousestate = Mouse.GetState();

            updateinfo.currentpushedKeys.Clear();
            
            
            /*add if-statements*/

            if (updateinfo.locked) {

                /* Menu, Dialogues, nonmovementstuff*/
            }

            if(! updateinfo.locked )
                movePlayable(candy, padstate, mousestate, keystate);
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


        private void boardMovement(Playable player, KeyboardState keystate)
        {
            
            BalanceBoardState boardstate = m_balanceBoard.getState();
            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            float camRotation = 0;

            //adjust parameters

            if (keystate.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right)) cutCam += 0.01f;
            if (keystate.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left)) cutCam -= 0.01f;

            if (keystate.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S)) camSlow += 0.01f;
            if (keystate.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A)) camSlow -= 0.01f;

            if (keystate.IsKeyDown(Keys.F) && oldKeyboardState.IsKeyUp(Keys.F)) camFast += 0.01f;
            if (keystate.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D)) camFast -= 0.01f;

            if (keystate.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W)) amplifyingWalkingSpeedFactor += 0.01f;
            if (keystate.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q)) amplifyingWalkingSpeedFactor -= 0.01f;

            if (keystate.IsKeyDown(Keys.X) && oldKeyboardState.IsKeyUp(Keys.X)) cutCamFast += 0.01f;
            if (keystate.IsKeyDown(Keys.Y) && oldKeyboardState.IsKeyUp(Keys.Y)) cutCamFast -= 0.01f;

            if (keystate.IsKeyDown(Keys.M) && oldKeyboardState.IsKeyUp(Keys.M)) maxSpeed += 0.01f;
            if (keystate.IsKeyDown(Keys.N) && oldKeyboardState.IsKeyUp(Keys.N)) maxSpeed -= 0.01f;

            if(updateinfo.alwaysRun)
            {
                float speed = 0.5f;
                float dmovex = boardstate.X * amplifyingWalkingSpeedFactor;
                updateinfo.playerIsWalking = true;
                // lean back to stand still
                if (boardstate.Y < -cutWalk)
                {
                    updateinfo.playerIsWalking = false;
                    speed = 0;
                    dmovex = 0;
                }
                player.movementInput(dmovex, speed, 0, 0);
                return;
            }


            // no camera rotation in a small area, then slow and then fast rotation
            if (Math.Abs(boardstate.X) >= cutCam)
            {
                if(Math.Abs(boardstate.X) >= cutCamFast)
                    camRotation = - boardstate.X * camFast;
                else
                    camRotation = - boardstate.X * camSlow;
            }

             bbPlayerMovementWithoutCamera(player, boardstate, alpha, amplifyingWalkingSpeedFactor, maxSpeed, camRotation, keystate);

             //bbPlayerMovementInCamDirection(player, boardstate, alpha, amplifyingWalkingSpeedFactor, maxSpeed, camRotation, keystate);

        }

        private void bbPlayerMovementWithoutCamera(Playable player, BalanceBoardState boardstate, float alpha, float amplifyingWalkingSpeedFactor, float maxSpeed, float camRotation, KeyboardState keystate)
        {
            if (keystate.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up)) cutWalk += 0.01f;
            if (keystate.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down)) cutWalk -= 0.01f;

            //rotate the movementvector to kamerakoordinates
            float dmovex = (float)Math.Cos(alpha) * boardstate.X
                - (float)Math.Sin(alpha) * boardstate.Y;
            float dmovey = (float)Math.Sin(alpha) * boardstate.X
                + (float)Math.Cos(alpha) * boardstate.Y;
            Console.WriteLine("board.X" + boardstate.X);
            Console.WriteLine("board.Y" + boardstate.Y);
            Console.WriteLine("dmovex" + dmovex);
            Console.WriteLine("dmovey" + dmovey);
            Console.WriteLine("");

            float movementScale = Math.Abs(dmovex) + Math.Abs(dmovey);
            updateinfo.playerIsWalking = false;

            // only move, when input is strong enough, then accelerate up to a maximum speed
            if (Math.Abs(boardstate.Y) >= cutWalk)
            {
                updateinfo.playerIsWalking = true;
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

        private void bbPlayerMovementInCamDirection(Playable player, BalanceBoardState boardstate, float alpha, float amplifyingWalkingSpeedFactor, float maxSpeed, float camRotation, KeyboardState keystate)
        {
            updateinfo.playerIsWalking = false;

            if (keystate.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up)) cutWalk += 0.01f;
            if (keystate.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down)) cutWalk -= 0.01f;

            //rotate the movementvector to kamerakoordinates
            float dmovex = -(float)Math.Sin(alpha) * boardstate.Y;
            float dmovey = (float)Math.Cos(alpha) * boardstate.Y;

            float movementScale = Math.Abs(dmovex) + Math.Abs(dmovey);

            // only move, when input is strong enough, then accelerate up to a maximum speed
            if (Math.Abs(boardstate.Y) >= cutWalk)
            {
                updateinfo.playerIsWalking = true;
                if (movementScale > maxSpeed)
                {
                    // put down to max speed
                    dmovex *= (maxSpeed / movementScale);
                    dmovey *= (maxSpeed / movementScale);
                }

                player.movementInput(dmovex * amplifyingWalkingSpeedFactor, dmovey * amplifyingWalkingSpeedFactor, camRotation, 0);
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
