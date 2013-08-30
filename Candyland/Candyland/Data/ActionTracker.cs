using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// This class tracks the one-time-events that occured during the game.
    /// TODO: It is saved.
    /// On Load we have to trigger all actions that are true, somehow, to get the correct
    /// positions/states... but let's worry about that later :')
    /// </summary>
    public class ActionTracker
    {
        // actionState is a dictionary that saves action id and if it already has been triggered
        public Dictionary<string, bool> actionState {get; set;}

        public ActionTracker()
        {
            actionState = new Dictionary<string, bool>();
        }

        // ToDo on load: check if action with actionID "helperActivated" is in dictionary with value "true"
        // if it is, set updateInfo.helperavailable to true, else to false
    }
}
