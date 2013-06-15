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

        private void addStaticButton_Click(object sender, EventArgs e)
        {
            m_objectGenerator.InitializeNewObject();
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                Object obj = new Object();
                obj.type = m_objectGenerator.type;
                obj.id = m_objectGenerator.id;
                obj.posX = m_objectGenerator.posX;
                obj.posY = m_objectGenerator.posY;
                obj.posZ = m_objectGenerator.posZ;
                obj.endPosX = m_objectGenerator.endPosX;
                obj.endPosY = m_objectGenerator.endPosY;
                obj.endPosZ = m_objectGenerator.endPosZ;
                obj.doorArea = m_objectGenerator.doorArea;
                obj.doorLevel = m_objectGenerator.doorLevel;
                obj.isSlippery = m_objectGenerator.isSlippery;
                staticObjects.Add(obj);
                listBox1.Items.Add(obj);
            }
        }

        private void editStaticButton_Click(object sender, EventArgs e)
        {
            Object current = (Object)listBox1.SelectedItem;
            m_objectGenerator.InitializeWithObject(current);
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                current.type = m_objectGenerator.type;
                current.id = m_objectGenerator.id;
                current.posX = m_objectGenerator.posX;
                current.posY = m_objectGenerator.posY;
                current.posZ = m_objectGenerator.posZ;
                current.endPosX = m_objectGenerator.endPosX;
                current.endPosY = m_objectGenerator.endPosY;
                current.endPosZ = m_objectGenerator.endPosZ;
                current.doorArea = m_objectGenerator.doorArea;
                current.doorLevel = m_objectGenerator.doorLevel;
                current.isSlippery = m_objectGenerator.isSlippery;
            }
        }

        private void addDynamicButton_Click(object sender, EventArgs e)
        {
            m_objectGenerator.InitializeNewObject();
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                Object obj = new Object();
                obj.type = m_objectGenerator.type;
                obj.id = m_objectGenerator.id;
                obj.posX = m_objectGenerator.posX;
                obj.posY = m_objectGenerator.posY;
                obj.posZ = m_objectGenerator.posZ;
                obj.endPosX = m_objectGenerator.endPosX;
                obj.endPosY = m_objectGenerator.endPosY;
                obj.endPosZ = m_objectGenerator.endPosZ;
                obj.doorArea = m_objectGenerator.doorArea;
                obj.doorLevel = m_objectGenerator.doorLevel;
                obj.isSlippery = m_objectGenerator.isSlippery;
                dynamicObjects.Add(obj);
                listBox2.Items.Add(obj);
            }
        }

        private void editDynamicButton_Click(object sender, EventArgs e)
        {
            Object current = (Object)listBox2.SelectedItem;
            m_objectGenerator.InitializeWithObject(current);
            if (m_objectGenerator.ShowDialog() == DialogResult.OK)
            {
                current.type = m_objectGenerator.type;
                current.id = m_objectGenerator.id;
                current.posX = m_objectGenerator.posX;
                current.posY = m_objectGenerator.posY;
                current.posZ = m_objectGenerator.posZ;
                current.endPosX = m_objectGenerator.endPosX;
                current.endPosY = m_objectGenerator.endPosY;
                current.endPosZ = m_objectGenerator.endPosZ;
                current.doorArea = m_objectGenerator.doorArea;
                current.doorLevel = m_objectGenerator.doorLevel;
                current.isSlippery = m_objectGenerator.isSlippery;
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            id = textBoxID.Text;
            posX = textBoxPosX.Text;
            posY = textBoxPosY.Text;
            posZ = textBoxPosZ.Text;
            this.DialogResult = DialogResult.OK;
        }
    }

    public class Level
    {
        public string id { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
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

            XmlNodeList id = scene.GetElementsByTagName("object_id");
            XmlNodeList type = scene.GetElementsByTagName("object_type");
            XmlNodeList position = scene.GetElementsByTagName("object_position");
            XmlNodeList door_to_area = scene.GetElementsByTagName("is_door_to_area");
            XmlNodeList door_to_level = scene.GetElementsByTagName("is_door_to_level");
            XmlNodeList slippery = scene.GetElementsByTagName("slippery");
            XmlNodeList endPosition = scene.GetElementsByTagName("object_endposition");
            int count = 0;

            foreach (XmlNode node in id)
            {
                Object obj = new Object();

                // set id
                obj.id = node.InnerText;

                // get object type
                obj.type = type[count].InnerText;

                // get x, y, z position
                obj.posX = position[count].SelectSingleNode("x").InnerText;
                obj.posY = position[count].SelectSingleNode("y").InnerText;
                obj.posZ = position[count].SelectSingleNode("z").InnerText;

                try
                {
                    // get x, y, z position
                    obj.endPosX = endPosition[count].SelectSingleNode("x").InnerText;
                    obj.endPosY = endPosition[count].SelectSingleNode("y").InnerText;
                    obj.endPosZ = endPosition[count].SelectSingleNode("z").InnerText;
                }
                catch { }

                // get bool value for slippery
                obj.isSlippery = bool.Parse(slippery[count].InnerText);

                // get isDoorToArea
                obj.doorArea = door_to_area[count].InnerText;

                // get isDoorToLevel
                obj.doorLevel = door_to_level[count].InnerText;

                // add object to list
                returnList.Add(obj);

                // increase count as it is used to access the not-id xml elements of the correct level
                // (the one currently being parsed)
                count++;
            }

            return returnList;
        }

        public string Write()
        {
            string ret = "";
            ret += "        <!--- LEVEL "+id+" -->\n";
            ret += "        <level>\n";
            ret += "          <level_id>" + id + "</level_id>\n";
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
