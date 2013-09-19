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
        #region sfx
        public SoundEffect menuButtonSound { get; set; }
        public SoundEffect menuButtonError { get; set; }

        public SoundEffect chocoSound { get; set; }
        public SoundEffect obstacleBreakSound { get; set; }
        public SoundEffect switchActivateSound { get; set; }
        public SoundEffect switchDeactivateSound { get; set; }
        public SoundEffect switchTickingSound { get; set; }
        public SoundEffect platformBreakSound { get; set; }
        #endregion

        #region songs
        public Song song1 { get; set; }
        public Song song2 { get; set; }
        #endregion

        #region fonts
        public SpriteFont mainTextFullscreen { get; set; }
        public SpriteFont mainText { get; set; }
        public SpriteFont mainRegular { get; set; }
        public SpriteFont smallText { get; set; }
        #endregion

        #region effects
        public Effect skyboxEffect { get; set; }
        public Effect billboardEffect { get; set; }
        public Effect commonShader { get; set; }
        #endregion

        public AssetManager() 
        {
            dialogImages = new Dictionary<string, Texture2D>();
        }

        public void Load(ContentManager content) 
        {
            menuButtonSound = content.Load<SoundEffect>("Sfx/MenuButton8bit");
            menuButtonError = content.Load<SoundEffect>("Sfx/MenuError8bit");
            chocoSound = content.Load<SoundEffect>("Sfx/coin");
            obstacleBreakSound = content.Load<SoundEffect>("Sfx/CrackingBlock8bit");
            switchActivateSound = content.Load<SoundEffect>("Sfx/SwitchActivate8bit");
            switchDeactivateSound = content.Load<SoundEffect>("Sfx/SwitchDeactivate8bit");
            switchTickingSound = content.Load<SoundEffect>("Sfx/Ticking8bit");
            platformBreakSound = content.Load<SoundEffect>("Sfx/CrackingPlatform8bit");  

            song1 = content.Load<Song>("Music/bgmusic");
            song2 = content.Load<Song>("Music/bossmusic");

            mainTextFullscreen = content.Load<SpriteFont>("Fonts/MainTextFullscreen");
            mainText = content.Load<SpriteFont>("Fonts/MainText");
            mainRegular = content.Load<SpriteFont>("Fonts/MainTextRegular");
            smallText = content.Load<SpriteFont>("Fonts/smallText");

            LoadScreenTextures(content);

            LoadTextures(content);

            LoadModels(content);

            skyboxEffect = content.Load<Effect>("Skybox/effects");
            billboardEffect = content.Load<Effect>("Shaders/Billboard");
            commonShader = content.Load<Effect>("Shaders/Shader");
        }

    }
}
