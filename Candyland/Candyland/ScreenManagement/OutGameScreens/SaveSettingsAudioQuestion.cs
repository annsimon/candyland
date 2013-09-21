using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    class SaveSettingsAudioQuestion : YesNoScreen
    {
        int musicVolume;
        int soundVolume;

        public SaveSettingsAudioQuestion(int music, int sound)
        {
            musicVolume = music;
            soundVolume = sound;
        }

        public override void Open(Game game, AssetManager assets)
        {
            base.Open(game, assets);

            question = "Einstellungen speichern?";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (enterPressed && answer)
            {
                SaveSettingsData data = ScreenManager.Settings;

                data.musicVolume = musicVolume;
                data.soundVolume = soundVolume;
                ScreenManager.SceneManager.getUpdateInfo().musicVolume = musicVolume;
                ScreenManager.SceneManager.getUpdateInfo().soundVolume = soundVolume;

                MediaPlayer.Volume = ((float)musicVolume) / 10;

                string filename = "settings.sav";

                // Convert the object to XML data
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    IntermediateSerializer.Serialize(writer, data, null);
                }

                ScreenManager.ResumeLast(this);
            }
        }
    }
}
