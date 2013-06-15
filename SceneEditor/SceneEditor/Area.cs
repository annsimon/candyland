using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SceneEditor
{
    public partial class AreaGenerator : Form
    {
        public string id { get; set; }
        public string idNext { get; set; }
        public string idPrev { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public List<Level> levels { get; set; }

        LevelGenerator m_levelGenerator;

        public AreaGenerator()
        {
            InitializeComponent();
            levels = new List<Level>();
            m_levelGenerator = new LevelGenerator();
        }

        public void InitializeNewArea()
        {
            textBoxID.Text = "";
            textBoxNextID.Text = "";
            textBoxPrevID.Text = "";
            textBoxPosX.Text = "";
            textBoxPosY.Text = "";
            textBoxPosZ.Text = "";

            listBox1.Items.Clear();
            levels.Clear();
        }

        public void InitializeWithArea( Area area )
        {
            levels.Clear();
            listBox1.Items.Clear();

            textBoxID.Text = area.id;
            textBoxNextID.Text = area.idNext;
            textBoxPrevID.Text = area.idPrev;
            textBoxPosX.Text = area.posX;
            textBoxPosY.Text = area.posY;
            textBoxPosZ.Text = area.posZ;

            foreach (Level level in area.levels)
            {
                listBox1.Items.Add(level);
                levels.Add(level);
            }
        }

        private void addLevelButton_Click(object sender, EventArgs e)
        {
            m_levelGenerator.InitializeNewLevel();
            if (m_levelGenerator.ShowDialog() == DialogResult.OK)
            {
                Level level = new Level();
                level.id = m_levelGenerator.id;
                level.posX = m_levelGenerator.posX;
                level.posY = m_levelGenerator.posY;
                level.posZ = m_levelGenerator.posZ;
                level.startMainID = m_levelGenerator.startMainID;
                level.startSecondaryID = m_levelGenerator.startSecondaryID;
                level.dynamicObjects = new List<Object>();
                foreach (Object obj in m_levelGenerator.dynamicObjects)
                    level.dynamicObjects.Add(obj);
                level.staticObjects = new List<Object>();
                foreach (Object obj in m_levelGenerator.staticObjects)
                    level.staticObjects.Add(obj);
                levels.Add(level);
                listBox1.Items.Add(level);
            }
        }

        private void editLevelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Level current = (Level)listBox1.SelectedItem;
                m_levelGenerator.InitializeWithLevel(current);
                if (m_levelGenerator.ShowDialog() == DialogResult.OK)
                {
                    current.id = m_levelGenerator.id;
                    current.posX = m_levelGenerator.posX;
                    current.posY = m_levelGenerator.posY;
                    current.posZ = m_levelGenerator.posZ;
                    current.startMainID = m_levelGenerator.startMainID;
                    current.startSecondaryID = m_levelGenerator.startSecondaryID;
                    current.dynamicObjects = new List<Object>();
                    foreach (Object obj in m_levelGenerator.dynamicObjects)
                        current.dynamicObjects.Add(obj);
                    current.staticObjects = new List<Object>();
                    foreach (Object obj in m_levelGenerator.staticObjects)
                        current.staticObjects.Add(obj);
                }
            }
            catch { }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            id = textBoxID.Text;
            if (textBoxNextID.Text == "")
                idNext = "x";
            else
                idNext = textBoxNextID.Text;
            if (textBoxPrevID.Text == "")
                idPrev = "x";
            else
                idPrev = textBoxPrevID.Text;
            posX = textBoxPosX.Text;
            posY = textBoxPosY.Text;
            posZ = textBoxPosZ.Text;
            this.DialogResult = DialogResult.OK;
        }
    }

    public class Area
    {
        public string id { get; set; }
        public string idNext { get; set; }
        public string idPrev { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public List<Level> levels { get; set; }

        public Area() 
        {
            levels = new List<Level>();
        }

        public void Parse(string xml)
        {
            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList id = scene.GetElementsByTagName("level_id");
            XmlNodeList start = scene.GetElementsByTagName("level_starting_position");
            XmlNodeList idStartMain = scene.GetElementsByTagName("main_start_platform");
            XmlNodeList idStartSecondary = scene.GetElementsByTagName("secondary_start_platform");
            XmlNodeList levelContent = scene.GetElementsByTagName("objects");

            int count = 0;

            foreach (XmlNode node in id)
            {
                Level level = new Level();

                // set id
                level.id = node.InnerText;

                // get x, y, z position
                level.posX = start[count].SelectSingleNode("x").InnerText;
                level.posY = start[count].SelectSingleNode("y").InnerText;
                level.posZ = start[count].SelectSingleNode("z").InnerText;

                // get start positions for player and support
                if( idStartMain[count] == null )
                    level.startMainID = "x";
                else
                    level.startMainID = idStartMain[count].InnerText;
                if (idStartSecondary[count] == null)
                    level.startSecondaryID = "x";
                else
                    level.startSecondaryID = idStartSecondary[count].InnerText;

                // let level parse its objects
                level.Parse(levelContent[count].InnerXml);

                // add completed level to level list
                levels.Add(level);

                // increase count as it is used to access the not-id xml elements of the correct level
                // (the one currently being parsed)
                count++;
            }
        }

        public override string ToString()
        {
            return id;
        }

        public string[] Write()
        {
            string[] retArr = new string[ levels.Count() + 2];
            int count = 0;

            string ret = "";
            ret += "  <!--- AREA " + id + " -->\n";
            ret += "  <area>\n";
            ret += "    <area_id>" + id + "</area_id>\n";
            ret += "    <area_prev>" + idPrev + "</area_prev>\n";
            ret += "    <area_next>" + idNext + "</area_next>\n";
            ret += "    <area_starting_position>\n";
            ret += "      <x>" + posX + "</x>\n";
            ret += "      <y>" + posY + "</y>\n";
            ret += "      <z>" + posZ + "</z>\n";
            ret += "    </area_starting_position>\n";
            ret += "    <levels>\n";
            ret += "      <the_levels>\n";
            retArr[count] = ret;


            foreach (Level lvl in levels)
            {
                count++;
                retArr[count] = lvl.Write();
            }

            string ret2 = "      </the_levels>\n";
            ret2 += "    </levels>\n";
            ret2 += "  </area>\n";
            retArr[count+1] = ret2;

            return retArr;
        }
    }
}
