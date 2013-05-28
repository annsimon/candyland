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

        public const int KEYBOARDMOUSE = 0; //Bewegungsstates
        public const int GAMEPADONLY = 1;
        public const int GAMEPADBOARD = 2;
        private int inputMode = 0;
        GraphicsDeviceManager graphicDevice;

        public InputManager(int initialInputmode, GraphicsDeviceManager graphicDevice) 
        
        {
            inputMode = initialInputmode;
            this.graphicDevice = graphicDevice;
        }

        public void movePlayable(Playable player, GamePadState padstate, MouseState mousestate, KeyboardState keystate) 
        {
            switch (inputMode){
            case 0: mouseMovement(player, keystate,mousestate); break;
            case 1: gamePadMovement(player); break;
            case 2: boardMovement(player); break;
            }
        }

        
        /// <summary>
        /// Moves the player with keybourd and mouse input
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keystate"></param>
        /// <param name="mousestate"></param>
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

            if(keystate.IsKeyDown(Keys.W))      dmoveytemp += 0.7f;
            if(keystate.IsKeyDown(Keys.S))      dmoveytemp -= 0.7f;
            if(keystate.IsKeyDown(Keys.A))      dmovextemp += 0.7f;
            if(keystate.IsKeyDown(Keys.D))      dmovextemp -= 0.7f;


            //Get the direction of the players camera
            float alpha = player.getCameraDir();

            //rotate the movementvector to kamerakoordinates
            float dmovex =(float) Math.Cos(alpha) * dmovextemp - (float)Math.Sin(alpha) * dmoveytemp;
            float dmovey = (float) Math.Sin(alpha) * dmovextemp + (float) Math.Cos(alpha) * dmoveytemp;
            //move the player
            player.movementInput(dmovex, dmovey, dcamx, dcamy);
            //reset mouse to the center of the screen, to rotate freely
            Mouse.SetPosition(graphicDevice.PreferredBackBufferWidth / 2, graphicDevice.PreferredBackBufferHeight / 2);
        }
        

        private void boardMovement(Playable player)
        {
            throw new NotImplementedException();
        }

        private void gamePadMovement(Playable player)
        {
            throw new NotImplementedException();
        }



        public int getInputMode(){return inputMode;}

        public void setInputMode(int state)
        {
            if (state <= 2 && 0 <= state) inputMode = state;
            else throw new NotImplementedException();
        }


    }
}
