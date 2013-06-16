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

        public const int KEYBOARDMOUSE = 0; //Bewegungsstates
        public const int GAMEPADONLY = 1;
        public const int GAMEPADBOARD = 2;
        public const int RUN = 3;
        private int inputMode = 0;
        GraphicsDeviceManager graphicDevice;
        UpdateInfo updateinfo;

        public InputManager(int initialInputmode, GraphicsDeviceManager graphicDevice,UpdateInfo info) 
        
        {
            inputMode = initialInputmode;
            this.graphicDevice = graphicDevice;
            updateinfo = info;
        }

        private void movePlayable(Playable player, GamePadState padstate, MouseState mousestate, KeyboardState keystate) 
        {
            if (updateinfo.currentAreaID.Equals("5"))
            {
                inputMode = 3;
            }
            switch (inputMode){
            case 0: mouseMovement(player, keystate,mousestate); break;
            case 1: gamePadMovement(player, padstate); break;
            case 2: boardMovement(player); break;
            case 3: mouseMovementRun(player, keystate, mousestate); break;
            }
        }

        public void update(Playable candy, Playable helper) {

            KeyboardState keystate = Keyboard.GetState();
            GamePadState padstate = GamePad.GetState(PlayerIndex.One);
            MouseState mousestate = Mouse.GetState();

            updateinfo.currentpushedKeys.Clear();
            
            
            /*add if-statements*/

            if(updateinfo.candyselected)
                movePlayable(candy, padstate, mousestate, keystate);
            else
                movePlayable(helper, padstate, mousestate, keystate);
        }
        
        /// <summary>
        /// Moves the player with keybourd and mouse input
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keystate"></param>
        /// <param name="mousestate"></param>
        /// 

        private void mouseMovementRun(Playable player, KeyboardState keystate, MouseState mousestate)
        {

            //Calculate the difference for Camera Movement and normalize it
            float dcamx = mousestate.X - graphicDevice.PreferredBackBufferWidth / 2;
            float dcamy = mousestate.Y - graphicDevice.PreferredBackBufferHeight / 2;
            //normalize the movement difference
            dcamx /= graphicDevice.PreferredBackBufferWidth / 4;
            dcamy /= graphicDevice.PreferredBackBufferHeight / 4;


            //get the Keyboard Input to Move the player
            float dmovextemp = 0;
            float dmoveytemp = 0;

            if ((keystate.IsKeyDown(Keys.W))||(keystate.IsKeyUp(Keys.W))) dmoveytemp += 1;
            if(keystate.IsKeyDown(Keys.S))      dmoveytemp -= 1f;
            if(keystate.IsKeyDown(Keys.A))      dmovextemp += 1f;
            if (keystate.IsKeyDown(Keys.Space)
                && keystate.IsKeyDown(Keys.Space) != oldKeyboardState.IsKeyDown(Keys.Space))
            {
                updateinfo.currentpushedKeys.Add(Keys.Space);
                player.uniqueskill();
            }
            if (keystate.IsKeyDown(Keys.M)
                && oldKeyboardState != keystate) player.switchCameraPerspective();
            if (keystate.IsKeyDown(Keys.LeftAlt)
                && oldKeyboardState != keystate) updateinfo.currentpushedKeys.Add(Keys.LeftAlt);
            if (keystate.IsKeyDown(Keys.Q)
                && oldKeyboardState != keystate) updateinfo.currentpushedKeys.Add(Keys.Q);
            if (keystate.IsKeyDown(Keys.Tab)
                && oldKeyboardState != keystate) updateinfo.switchPlayer();

            if (keystate.IsKeyDown(Keys.R)) player.Reset();

            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            //rotate the movementvector to kamerakoordinates
            float dmovex = (float)Math.Cos(alpha) * dmovextemp - (float)Math.Sin(alpha) * dmoveytemp;
            float dmovey = (float)Math.Sin(alpha) * dmovextemp + (float)Math.Cos(alpha) * dmoveytemp;
            //move the player
            player.movementInput(dmovex, dmovey, 0, 0);
            //reset mouse to the center of the screen, to rotate freely
            Mouse.SetPosition(graphicDevice.PreferredBackBufferWidth / 2, graphicDevice.PreferredBackBufferHeight / 2);

            oldKeyboardState = keystate;
            oldMouseState = mousestate;
        }


        private void mouseMovement(Playable player, KeyboardState keystate, MouseState mousestate)
        {
            //Calculate the difference for Camera Movement and normalize it
            float dcamx = mousestate.X - graphicDevice.PreferredBackBufferWidth / 2;
            float dcamy = mousestate.Y - graphicDevice.PreferredBackBufferHeight / 2;
            //normalize the movement difference
            dcamx /= graphicDevice.PreferredBackBufferWidth / 4;
            dcamy /= graphicDevice.PreferredBackBufferHeight / 4;


            //get the Keyboard Input to Move the player
            float dmovextemp = 0;
            float dmoveytemp = 0;

                if (keystate.IsKeyDown(Keys.W)) dmoveytemp += 0.7f;
                if (keystate.IsKeyDown(Keys.S)) dmoveytemp -= 0.7f;
                if (keystate.IsKeyDown(Keys.A)) dmovextemp += 0.7f;
                if (keystate.IsKeyDown(Keys.D)) dmovextemp -= 0.7f;
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
            if (keystate.IsKeyDown(Keys.Tab)
                && oldKeyboardState != keystate) updateinfo.switchPlayer();

            if (keystate.IsKeyDown(Keys.R)) player.Reset();

            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            //rotate the movementvector to kamerakoordinates
            float dmovex =(float) Math.Cos(alpha) * dmovextemp - (float)Math.Sin(alpha) * dmoveytemp;
            float dmovey = (float) Math.Sin(alpha) * dmovextemp + (float) Math.Cos(alpha) * dmoveytemp;
            //move the player
            player.movementInput(dmovex, dmovey, dcamx, dcamy);
            //reset mouse to the center of the screen, to rotate freely
            Mouse.SetPosition(graphicDevice.PreferredBackBufferWidth / 2, graphicDevice.PreferredBackBufferHeight / 2);

            oldKeyboardState = keystate;
            oldMouseState = mousestate;
        }
        

        private void boardMovement(Playable player)
        {
            throw new NotImplementedException();
        }

        private void gamePadMovement(Playable player, GamePadState padstate)
        {
            if (padstate.IsButtonDown(Buttons.A)
                && padstate.IsButtonDown(Buttons.A) != oldGamepadstate.IsButtonDown(Buttons.A))
            {
                updateinfo.currentpushedKeys.Add(Keys.Space); //SPace because its the epuivalent to Buttons.A
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
