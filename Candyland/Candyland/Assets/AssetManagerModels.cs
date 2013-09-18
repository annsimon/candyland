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
        public Model skybox { get; set; }

        public Model buddy { get; set; }
        public Model buddybreaking { get; set; }
        public Model actionBuddy { get; set; }
        public Model hero { get; set; }
        public Model boss { get; set; }
        public Model tutorialGuy { get; set; }

        public Model fairy { get; set; }
        public Model salesman { get; set; }

        public Model obstacleHalf { get; set; }
        public Model obstacle { get; set; }
        public Model obstacleLarge { get; set; }
        public Model obstacleBreakable { get; set; }
        public Model obstacleMovable { get; set; }

        public Model choco { get; set; }

        public Model platformSmall { get; set; }
        public Model platformMedium { get; set; }
        public Model platformLarge { get; set; }
        public Model platformBreaking { get; set; }

        public void LoadModels(ContentManager content)
        {
            skybox = content.Load<Model>("Skybox/skybox2");

            buddy = content.Load<Model>("NPCs/Helper/buddyrunning");
            buddybreaking = content.Load<Model>("NPCs/Helper/buddybreaking");
            actionBuddy = content.Load<Model>("NPCs/HelperActor/buddy");
            hero = content.Load<Model>("NPCs/Spieler/candyguy");
            boss = content.Load<Model>("NPCs/Lakritze/boss");
            tutorialGuy = content.Load<Model>("NPCs/TutorialGuy/tutorial");

            fairy = content.Load<Model>("NPCs/Fee/bonbon");
            salesman = content.Load<Model>("NPCs/Salesman/shopguy");

            obstacleHalf = content.Load<Model>("Objekte/Obstacles/obstacle_half");
            obstacle = content.Load<Model>("Objekte/Obstacles/lakritzblock_klein");
            obstacleLarge = content.Load<Model>("Objekte/Obstacles/lakritzblock_gross");
            obstacleBreakable = content.Load<Model>("Objekte/Obstacles/Breakable/blockbreakable");
            obstacleMovable = content.Load<Model>("Objekte/Obstacles/Movable/blockmovable");

            choco = content.Load<Model>("Objekte/Schokolinse/schokolinse");

            platformSmall = content.Load<Model>("Objekte/Plattformen/plattform_klein");
            platformMedium = content.Load<Model>("Objekte/Plattformen/plattform_mittel");
            platformLarge = content.Load<Model>("Objekte/Plattformen/plattform_gross");
            platformBreaking = content.Load<Model>("Objekte/Plattformen/breakingplatform");
        }
    }
}
