using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SceneEditor
{
    public partial class ObjectGenerator : Form
    {
        public string id { get; set; }
        public string type { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public string doorArea { get; set; }
        public string doorLevel { get; set; }
        public bool isSlippery { get; set; }

        public ObjectGenerator()
        {
            InitializeComponent();
        }

        public void InitializeNewObject()
        {
            // keep the old data for convenience, don't change anything
            //textBoxType.Text = "platform";
            //textBoxID.Text = "";
            //textBoxPosX.Text = "";
            //textBoxPosY.Text = "";
            //textBoxPosZ.Text = "";
            //textBoxDoorArea.Text = "";
            //textBoxDoorLevel.Text = "";
            //checkBoxSlippery.Checked = false;
        }

        public void InitializeWithObject( Object obj )
        {
            textBoxType.Text = obj.type;
            textBoxID.Text = obj.id;
            textBoxPosX.Text = obj.posX;
            textBoxPosY.Text = obj.posY;
            textBoxPosZ.Text = obj.posZ;
            textBoxDoorArea.Text = obj.doorArea;
            textBoxDoorLevel.Text = obj.doorLevel;
            checkBoxSlippery.Checked = obj.isSlippery;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            type = textBoxType.Text;
            id = textBoxID.Text;
            posX = textBoxPosX.Text;
            posY = textBoxPosY.Text;
            posZ = textBoxPosZ.Text;
            if (textBoxDoorArea.Text == "")
                doorArea = "x";
            else
                doorArea = textBoxDoorArea.Text;
            if (textBoxDoorLevel.Text == "")
                doorLevel = "x";
            else
                doorLevel = textBoxDoorLevel.Text;
            isSlippery = checkBoxSlippery.Checked;
            this.DialogResult = DialogResult.OK;
        }
    }

    public class Object
    {
        public string id { get; set; }
        public string type { get; set; }
        public string posX { get; set; }
        public string posY { get; set; }
        public string posZ { get; set; }
        public string doorArea { get; set; }
        public string doorLevel { get; set; }
        public bool isSlippery { get; set; }

        public Object() { }

        public override string ToString()
        {
            return "ID: "+ id + "; X: " + posX + ", Y: " + posY + ", Z: " + posZ;
        }

        public string Write()
        {
            string ret = "";
            ret += "                  <object>\n";
            ret += "                    <object_type>" + type + "</object_type>\n";
            ret += "                    <object_id>" + id + "</object_id>\n";
            ret += "                    <is_door_to_area>" + doorArea + "</is_door_to_area>\n";
            ret += "                    <is_door_to_level>" + doorLevel + "</is_door_to_level>\n";
            ret += "                    <object_position>\n";
            ret += "                      <x>" + posX + "</x>\n";
            ret += "                      <y>" + posY + "</y>\n";
            ret += "                      <z>" + posZ + "</z>\n";
            ret += "                    </object_position>\n";
            ret += "                    <slippery>" + isSlippery.ToString() + "</slippery>\n";
            ret += "                  </object>\n";
            return ret;
        }
    }
}
