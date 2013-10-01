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
        public Texture2D[] skyboxTextures { get; set; }

        public Model hero { get; set; }
        public Model heroslipping { get; set; }
        public Model boss { get; set; }
        public Model tutorialGuy { get; set; }

        public Model fairy { get; set; }

        public Model obstacleHalf { get; set; }
        public Model obstacle { get; set; }
        public Model obstacleLarge { get; set; }
        public Model obstacleMovable { get; set; }

        public Model choco { get; set; }

        public Model platformSmall { get; set; }
        public Model platformMedium { get; set; }
        public Model platformLarge { get; set; }
        public Model platformBreaking { get; set; }

        public void LoadModels(ContentManager content)
        {
            skybox = content.Load<Model>("Skybox/skybox2");
            skyboxTextures = new Texture2D[6];
            int i = 0;
            foreach (ModelMesh mesh in skybox.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    skyboxTextures[i++] = currentEffect.Texture;

            hero = content.Load<Model>("NPCs/Spieler/candyguy");
            heroslipping = content.Load<Model>("NPCs/Spieler/candyguyrutsch");
            tutorialGuy = content.Load<Model>("NPCs/TutorialGuy/tutorial");

            fairy = content.Load<Model>("NPCs/Fee/bonbon");

            obstacleHalf = content.Load<Model>("Objekte/Obstacles/obstacle_half");
            obstacle = content.Load<Model>("Objekte/Obstacles/lakritzblock_klein");
            obstacleLarge = content.Load<Model>("Objekte/Obstacles/lakritzblock_gross");
            obstacleMovable = content.Load<Model>("Objekte/Obstacles/Movable/blockmovable");

            choco = content.Load<Model>("Objekte/Schokolinse/schokolinse");

            platformSmall = content.Load<Model>("Objekte/Plattformen/plattform_klein");
            platformMedium = content.Load<Model>("Objekte/Plattformen/plattform_mittel");
            platformLarge = content.Load<Model>("Objekte/Plattformen/plattform_gross");
            platformBreaking = content.Load<Model>("Objekte/Plattformen/breakingplatform");
        }
    }
}
