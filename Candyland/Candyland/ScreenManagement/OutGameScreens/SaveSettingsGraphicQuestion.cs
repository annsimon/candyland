using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Candyland
{
    class SaveSettingsGraphicQuestion : YesNoScreen
    {
        bool fullscreen;
        int shadowQuality;

        public SaveSettingsGraphicQuestion(bool isFullscreen, int shadowQuali)
        {
            fullscreen = isFullscreen;
            shadowQuality = shadowQuali;
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

                data.isFullscreen = fullscreen;
                data.shadowQuality = shadowQuality;

                ScreenManager.SceneManager.getUpdateInfo().shadowQuality = shadowQuality;

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
