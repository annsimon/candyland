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
    public partial class SceneGenerator : Form
    {
        List<Area> m_areas;
        AreaGenerator m_areaGenerator;

        public SceneGenerator()
        {
            InitializeComponent();

            m_areas = new List<Area>();
            m_areaGenerator = new AreaGenerator();

            Parse();
        }

        private void addAreaButton_Click(object sender, EventArgs e)
        {
            m_areaGenerator.InitializeNewArea();
            if (m_areaGenerator.ShowDialog() == DialogResult.OK)
            {
                Area area = new Area();
                area.id = m_areaGenerator.id;
                area.idNext = m_areaGenerator.idNext;
                area.idPrev = m_areaGenerator.idPrev;
                area.posX = m_areaGenerator.posX;
                area.posY = m_areaGenerator.posY;
                area.posZ = m_areaGenerator.posZ;

                area.levels = new List<Level>();
                foreach (Level lvl in m_areaGenerator.levels)
                    area.levels.Add(lvl);

                m_areas.Add(area);
                listBox1.Items.Add(area);
            }
        }

        private void editAreaButton_Click(object sender, EventArgs e)
        {
            Area current = (Area)listBox1.SelectedItem;
            m_areaGenerator.InitializeWithArea(current);
            if (m_areaGenerator.ShowDialog() == DialogResult.OK)
            {
                current.id = m_areaGenerator.id;
                current.idNext = m_areaGenerator.idNext;
                current.idPrev = m_areaGenerator.idPrev;
                current.posX = m_areaGenerator.posX;
                current.posY = m_areaGenerator.posY;
                current.posZ = m_areaGenerator.posZ;
                current.levels = new List<Level>();
                foreach (Level lvl in m_areaGenerator.levels)
                    current.levels.Add(lvl);
            }
        }

        private void Parse()
        {
            XmlDocument scene = new XmlDocument();
            scene.Load("SceneTest.xml");

            XmlNodeList id = scene.GetElementsByTagName("area_id");
            XmlNodeList prev = scene.GetElementsByTagName("area_prev");
            XmlNodeList next = scene.GetElementsByTagName("area_next");
            XmlNodeList start = scene.GetElementsByTagName("area_starting_position");
            XmlNodeList areaContent = scene.GetElementsByTagName("levels");

            int count = 0;

            foreach (XmlNode node in id)
            {
                Area area = new Area();

                // set id
                area.id = node.InnerText;

                // get x, y, z position
                area.posX = start[count].SelectSingleNode("x").InnerText;
                area.posY = start[count].SelectSingleNode("y").InnerText;
                area.posZ = start[count].SelectSingleNode("z").InnerText;

                // set previous
                string prevID = prev[count].InnerText;
                area.idPrev = prevID;

                // set next
                string nextID = next[count].InnerText;
                area.idNext = nextID;

                // let area parse its levels
                area.Parse(areaContent[count].InnerXml);

                // add completed area to area list
                m_areas.Add(area);
                listBox1.Items.Add(area);

                // increase count as it is used to access the not-id xml elements of the correct area
                // (the one currently being parsed)
                count++;
            }

        }

        private void generateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
