using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    public partial class AssetManager
    {
        public Texture2D hudIcons { get; set; }
        public Texture2D hudIconsWithCharChange { get; set; }
        public Texture2D chocoChip { get; set; }

        public Texture2D distanceDisplay0 { get; set; }
        public Texture2D distanceDisplay1 { get; set; }
        public Texture2D distanceDisplay2 { get; set; }
        public Texture2D distanceDisplay3 { get; set; }

        public Texture2D titleImage { get; set; }

        public Texture2D optionsScreen { get; set; }
        public Texture2D loadingTexture { get; set; }

        public Texture2D captionMain { get; set; }
        public Texture2D captionCredits { get; set; }
        public Texture2D captionBonus { get; set; }
        public Texture2D menuSelection { get; set; }

        public Texture2D dialogTL { get; set; }
        public Texture2D dialogTR { get; set; }
        public Texture2D dialogBL { get; set; }
        public Texture2D dialogBR { get; set; }
        public Texture2D dialogT { get; set; }
        public Texture2D dialogB { get; set; }
        public Texture2D dialogL { get; set; }
        public Texture2D dialogR { get; set; }
        public Texture2D dialogC { get; set; }

        public Texture2D yes { get; set; }
        public Texture2D yesActive { get; set; }
        public Texture2D no { get; set; }
        public Texture2D noActive { get; set; }

        public Texture2D dialogArrow { get; set; }

        public Texture2D map { get; set; }
        public Texture2D pinCurrent { get; set; }
        public Texture2D pinSelected { get; set; }
        public Texture2D pinAvailable { get; set; }

        public Texture2D acagamicsLogo { get; set; }

        public Dictionary<String, Texture2D> dialogImages { get; set; }

        public void LoadScreenTextures(ContentManager content)
        {
            hudIcons = content.Load<Texture2D>("Images/HUD/HudFull");
            hudIconsWithCharChange = content.Load<Texture2D>("Images/HUD/HudFullWithChange");
            chocoChip = content.Load<Texture2D>("Images/HUD/Choco");

            distanceDisplay0 = content.Load<Texture2D>("Images/HUD/chaseSafe");
            distanceDisplay1 = content.Load<Texture2D>("Images/HUD/chaseCareful");
            distanceDisplay2 = content.Load<Texture2D>("Images/HUD/chaseDanger");
            distanceDisplay3 = content.Load<Texture2D>("Images/HUD/chaseCritical");

            titleImage = content.Load<Texture2D>("ScreenTextures/Main");

            optionsScreen = content.Load<Texture2D>("ScreenTextures/optionsScreen");
            loadingTexture = content.Load<Texture2D>("ScreenTextures/loading");

            captionMain = content.Load<Texture2D>("Images/Captions/MainMenu");
            captionCredits = content.Load<Texture2D>("Images/Captions/Credits");
            captionBonus = content.Load<Texture2D>("Images/Captions/Bonus");
            menuSelection = content.Load<Texture2D>("ScreenTextures/transparent");

            dialogTL = content.Load<Texture2D>("Images/Dialog/DialogTopLeft");
            dialogTR = content.Load<Texture2D>("Images/Dialog/DialogTopRight");
            dialogBL = content.Load<Texture2D>("Images/Dialog/DialogBottomLeft");
            dialogBR = content.Load<Texture2D>("Images/Dialog/DialogBottomRight");
            dialogT = content.Load<Texture2D>("Images/Dialog/DialogTop");
            dialogB = content.Load<Texture2D>("Images/Dialog/DialogBottom");
            dialogL = content.Load<Texture2D>("Images/Dialog/DialogLeft");
            dialogR = content.Load<Texture2D>("Images/Dialog/DialogRight");
            dialogC = content.Load<Texture2D>("Images/Dialog/DialogMiddle");
            dialogArrow = content.Load<Texture2D>("Images/Dialog/DialogArrow");

            yes = content.Load<Texture2D>("Images/Dialog/JaInactive");
            yesActive = content.Load<Texture2D>("Images/Dialog/JaActive");
            no = content.Load<Texture2D>("Images/Dialog/NeinInactive");
            noActive = content.Load<Texture2D>("Images/Dialog/NeinActive");

            map = content.Load<Texture2D>("ScreenTextures/travelScreen");
            pinCurrent = content.Load<Texture2D>("Images/Map/CurrentPos");
            pinSelected = content.Load<Texture2D>("Images/Map/SelectedPos");
            pinAvailable = content.Load<Texture2D>("Images/Map/AvailablePos");

            dialogImages["BonbonRed"] = content.Load<Texture2D>("Images/DialogImages/BonbonFairyRed");
            dialogImages["BonbonBlue"] = content.Load<Texture2D>("Images/DialogImages/BonbonFairyBlue");
            dialogImages["AcaHelper"] = content.Load<Texture2D>("Images/DialogImages/AcaHelper");
            dialogImages["Buddy"] = content.Load<Texture2D>("Images/DialogImages/Helper");
            dialogImages["Boss"] = content.Load<Texture2D>("Images/DialogImages/Boss");
            dialogImages["Salesman"] = content.Load<Texture2D>("Images/DialogImages/Salesman");

            acagamicsLogo = content.Load<Texture2D>("ScreenTextures/acagamicsLogo");
        }
    }
}
