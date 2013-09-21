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
        public Texture2D sun { get; set; }

        public Texture2D teleport { get; set; }

        public Texture2D chocoBillboard { get; set; }
        public Texture2D chocoBillboardForMap { get; set; }

        public Texture2D buddyTexture { get; set; }
        public Texture2D actionBuddyTexture { get; set; }
        public Texture2D heroTexture { get; set; }
        public Texture2D bossTexture { get; set; }
        public Texture2D tutorialGuyTexture { get; set; }

        public Texture2D fairyBlueTexture { get; set; }
        public Texture2D fairyRedTexture { get; set; }
        public Texture2D salesmanTexture { get; set; }

        public Texture2D obstacleTexture { get; set; }
        public Texture2D obstacleBreakTexture { get; set; }
        public Texture2D obstacleMoveTexture { get; set; }

        public Texture2D chocoTexture { get; set; }

        public Texture2D platformTexture { get; set; }
        public Texture2D platformTextureSlippery { get; set; }
        public Texture2D platformTextureVerySlippery { get; set; }
        public Texture2D platformTextureTeleport { get; set; }
        public Texture2D platformTextureBreak { get; set; }

        public Texture2D switchPermanentActiveTexture { get; set; }
        public Texture2D switchPermanentTexture { get; set; }
        public Texture2D switchTemporaryActiveTexture { get; set; }
        public Texture2D switchTemporaryTexture { get; set; }
        public Texture2D switchTimedActiveTexture { get; set; }
        public Texture2D switchTimedTexture { get; set; }

        public void LoadTextures(ContentManager content)
        {
            sun = content.Load<Texture2D>("Images/Billboards/Sun/Sun1");

            teleport = content.Load<Texture2D>("Images/Billboards/Teleport");

            chocoBillboard = content.Load<Texture2D>("Images/Billboards/Choco1");
            chocoBillboardForMap = content.Load<Texture2D>("Images/Billboards/ChocoMap");

            buddyTexture = content.Load<Texture2D>("NPCs/Helper/buddytextur");
            actionBuddyTexture = content.Load<Texture2D>("NPCs/HelperActor/buddytextur");
            heroTexture = content.Load<Texture2D>("NPCs/Spieler/Candyguytextur");
            bossTexture = content.Load<Texture2D>("NPCs/Lakritze/bosstextur");
            tutorialGuyTexture = content.Load<Texture2D>("NPCs/TutorialGuy/tutorialtexture");

            fairyBlueTexture = content.Load<Texture2D>("NPCs/Fee/bonbon_blau");
            fairyRedTexture = content.Load<Texture2D>("NPCs/Fee/bonbon_rot");
            salesmanTexture = content.Load<Texture2D>("NPCs/Salesman/shoptexture");

            obstacleTexture = content.Load<Texture2D>("Objekte/Obstacles/obstacletextur");
            obstacleBreakTexture = content.Load<Texture2D>("Objekte/Obstacles/Breakable/blockmovabletexture");
            obstacleMoveTexture = content.Load<Texture2D>("Objekte/Obstacles/Movable/blockmovabletextur");
            
            chocoTexture = content.Load<Texture2D>("Objekte/Schokolinse/schokolinsetextur");
        
            platformTexture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_klein");
            platformTextureSlippery = content.Load<Texture2D>("Objekte/Plattformen/Slippery/plattformtexturslippery_klein");
            platformTextureVerySlippery = content.Load<Texture2D>("Objekte/Plattformen/Slippery/plattformtexturslippery_klein2");
            platformTextureTeleport = content.Load<Texture2D>("Objekte/Plattformen/Teleport/plattformtextur_porter");
            platformTextureBreak = content.Load<Texture2D>("Objekte/Plattformen/breakingplatformtextur");
        
            switchPermanentActiveTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schalterpermanent_aktiv");
            switchPermanentTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schalterpermanent_inaktiv");
            switchTemporaryActiveTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schaltertemp_aktiv");
            switchTemporaryTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schaltertemp_inaktiv");
            switchTimedActiveTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/zeitschalteraktivtextur");
            switchTimedTexture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/zeitschaltertextur");
        }
    }
}
