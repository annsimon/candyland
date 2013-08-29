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
    public partial class LevelGenerator : Form
    {
        public string id { get; set; }
        public string startMainID { get; set; }
        public string startSecondaryID { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public List<Object> staticObjects { get; set; }
        public List<Object> dynamicObjects { get; set; }

        ObjectGenerator m_objectGenerator;

        public LevelGenerator()
        {
            InitializeComponent();
            m_objectGenerator = new ObjectGenerator();
            staticObjects = new List<Object>();
            dynamicObjects = new List<Object>();
        }

        public void InitializeNewLevel()
        {
            textBoxID.Text = "";
            textBoxPosX.Text = "";
            textBoxPosY.Text = "";
            textBoxPosZ.Text = "";
            textBoxStartMain.Text = "";
            textBoxStartSecondary.Text = "";

            listBox1.Items.Clear();
            staticObjects.Clear();
            listBox2.Items.Clear();
            dynamicObjects.Clear();
        }

        public void InitializeWithLevel(Level level)
        {
            listBox1.Items.Clear();
            staticObjects.Clear();
            listBox2.Items.Clear();
            dynamicObjects.Clear();

            textBoxID.Text = level.id;
            textBoxPosX.Text = level.posX;
            textBoxPosY.Text = level.posY;
            textBoxPosZ.Text = level.posZ;
            textBoxStartMain.Text = level.startMainID;
            textBoxStartSecondary.Text = level.startSecondaryID;

            foreach( Object obj in level.staticObjects )
            {
                listBox1.Items.Add(obj);
                staticObjects.Add(obj);
            }
            foreach (Object obj in level.dynamicObjects)
            {
                listBox2.Items.Add(obj);
                dynamicObjects.Add(obj);
            }
        }

        private void initObject( Object obj )
        {
            obj.type = m_objectGenerator.type;
            obj.id = m_objectGenerator.id;
            obj.posX = m_objectGenerator.posX;
            obj.posY = m_objectGenerator.posY;
            obj.posZ = m_objectGenerator.posZ;
            obj.doorArea = m_objectGenerator.doorArea;
            obj.doorLevel = m_objectGenerator.doorLevel;
            obj.slippery = m_objectGenerator.slippery;
            obj.isVisible = m_objectGenerator.isVisible;
			obj.endPosX = m_objectGenerator.endPosX;
            obj.endPosY = m_objectGenerator.endPosY;
            obj.endPosZ = m_objectGenerator.endPosZ;
            obj.size = m_objectGenerator.size;
            obj.dialog = m_objectGenerator.dialog;
        }

        private void addStaticButton_Click(object sender, EventArgs e)
        {
            m_objectGenerator.InitializeNewObject();
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                Object obj = new Object();
                initObject(obj);
                staticObjects.Add(obj);
                listBox1.Items.Add(obj);
            }
        }

        private void editStaticButton_Click(object sender, EventArgs e)
        {
            try
            {
                Object current = (Object)listBox1.SelectedItem;
                m_objectGenerator.InitializeWithObject(current);
                if (m_objectGenerator.ShowDialog() == DialogResult.OK)
                {
                    initObject(current);
                }
            }
            catch { }
        }

        private void addDynamicButton_Click(object sender, EventArgs e)
        {
            m_objectGenerator.InitializeNewObject();
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                Object obj = new Object();
                initObject(obj);
                dynamicObjects.Add(obj);
                listBox2.Items.Add(obj);
            }
        }

        private void editDynamicButton_Click(object sender, EventArgs e)
        {
            try
            {
                Object current = (Object)listBox2.SelectedItem;
                m_objectGenerator.InitializeWithObject(current);
                if (m_objectGenerator.ShowDialog() == DialogResult.OK)
                {
                    initObject(current);
                }
            }
            catch { }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            id = textBoxID.Text;
            posX = textBoxPosX.Text;
            posY = textBoxPosY.Text;
            posZ = textBoxPosZ.Text;
            if (textBoxStartMain.Text == "")
                startMainID = "x";
            else
                startMainID = textBoxStartMain.Text;
            if (textBoxStartSecondary.Text == "")
                startSecondaryID = "x";
            else
                startSecondaryID = textBoxStartSecondary.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // replace content with the content specified in chosen tmx file
                TmxParser.parseObjects(openFileDialog.FileName, textBoxID.Text, staticObjects, dynamicObjects);
                listBox1.Items.Clear();
                foreach (Object obj in staticObjects)
                {
                    listBox1.Items.Add(obj);
                }
                listBox2.Items.Clear();
                foreach (Object obj in dynamicObjects)
                {
                    listBox2.Items.Add(obj);
                }
            }
        }
    }

    public class Level
    {
        public string id { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public string startMainID { get; set; }
        public string startSecondaryID { get; set; }
        public List<Object> staticObjects { get; set; }
        public List<Object> dynamicObjects { get; set; }

        public Level() {
            staticObjects = new List<Object>();
            dynamicObjects = new List<Object>();
        }

        public void Parse(string xml)
        {
            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("dynamic_objects");

            dynamicObjects = ParseDetails(objects[0].InnerXml);

            objects = scene.GetElementsByTagName("static_objects");

            staticObjects = ParseDetails(objects[0].InnerXml);
        }

        public override string ToString()
        {
            return "ID: " + id + "; X: " + posX + ", Y: " + posY + ", Z: " + posZ;
        }

        List<Object> ParseDetails(string xml)
        {
            XmlDocument scene = new XmlDocument();
            List<Object> returnList = new List<Object>();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("object");

            foreach (XmlNode node in objects)
            {
                Object obj = new Object();

                // set id
                obj.id = node.Attributes["id"].InnerText;

                // get object type
                obj.type = node.Attributes["type"].InnerText;

                // get x, y, z position
                obj.posX = node.Attributes["posX"].InnerText;
                obj.posY = node.Attributes["posY"].InnerText;
                obj.posZ = node.Attributes["posZ"].InnerText;

                try
                {
                    // get x, y, z position
                    obj.endPosX = node.Attributes["endPosX"].InnerText;
                    obj.endPosY = node.Attributes["endPosY"].InnerText;
                    obj.endPosZ = node.Attributes["endPosZ"].InnerText;
                }
                catch { }

                // get value for slippery
                obj.slippery = node.Attributes["slippery"].InnerText;

                // get bool value for isVisible
                try
                {
                    obj.isVisible = bool.Parse(node.Attributes["visible"].InnerText);
                }
                catch
                {
                    obj.isVisible = true;
                }

                // get isDoorToArea
                obj.doorArea = node.Attributes["is_door_to_area"].InnerText;

                // get isDoorToLevel
                obj.doorLevel = node.Attributes["is_door_to_level"].InnerText;
                
                // get object size
                obj.size = node.Attributes["size"].InnerText;

                // get object's dialog text
                obj.dialog = node.Attributes["dialog"].InnerText;

                // add object to list
                returnList.Add(obj);
            }

            return returnList;
        }

        public string Write()
        {
            string ret = "";
            ret += "        <!--- LEVEL "+id+" -->\n";
            ret += "        <level>\n";
            ret += "          <level_id>" + id + "</level_id>\n";
            ret += "          <main_start_platform>" + startMainID + "</main_start_platform>\n";
            ret += "          <secondary_start_platform>" + startSecondaryID + "</secondary_start_platform>\n";
            ret += "          <level_starting_position>\n";
            ret += "            <x>" + posX + "</x>\n";
            ret += "            <y>" + posY + "</y>\n";
            ret += "            <z>" + posZ + "</z>\n";
            ret += "          </level_starting_position>\n";
            ret += "          <objects>\n";
            ret += "            <the_objects>\n";
            ret += "            <!--- OBJECTS OF LEVEL "+id+" -->\n";
            ret += "            <!--- STATIC-->\n";
            ret += "              <static_objects>\n";
            ret += "                <statics>\n";
            foreach (Object obj in staticObjects)
                ret += obj.Write();
            ret += "                </statics>\n";
            ret += "              </static_objects>\n";
            ret += "            <!--- DYNAMIC-->\n";
            ret += "              <dynamic_objects>\n";
            ret += "                <dynamics>\n";
            foreach (Object obj in dynamicObjects)
                ret += obj.Write();
            ret += "                </dynamics>\n";
            ret += "              </dynamic_objects>\n";
            ret += "            </the_objects>\n";
            ret += "          </objects>\n";
            ret += "        </level>\n";
            return ret;
        }
    }
}
