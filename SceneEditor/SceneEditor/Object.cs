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
        public string slippery { get; set; }
        public bool isVisible { get; set; }
        public string endPosX { get; set; }
        public string endPosY { get; set; }
        public string endPosZ { get; set; }
        public string size { get; set; }

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
            textBoxEndX.Text = "";
            textBoxEndY.Text = "";
            textBoxEndZ.Text = "";
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
            textBoxSlippery.Text = obj.slippery;
            checkBoxVisible.Checked = obj.isVisible;
            textBoxEndX.Text = obj.endPosX;
            textBoxEndY.Text = obj.endPosY;
            textBoxEndZ.Text = obj.endPosZ;
            textBoxSize.Text = obj.size;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            type = textBoxType.Text;
            id = textBoxID.Text;
            posX = textBoxPosX.Text;
            posY = textBoxPosY.Text;
            posZ = textBoxPosZ.Text;
            endPosX = textBoxEndX.Text;
            endPosY = textBoxEndY.Text;
            endPosZ = textBoxEndZ.Text;
            if (textBoxDoorArea.Text == "")
                doorArea = "x";
            else
                doorArea = textBoxDoorArea.Text;
            if (textBoxDoorLevel.Text == "")
                doorLevel = "x";
            else
                doorLevel = textBoxDoorLevel.Text;
            slippery = textBoxSlippery.Text;
            isVisible = checkBoxVisible.Checked;
            size = textBoxSize.Text;
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
        public string slippery { get; set; }
        public bool isVisible{ get; set; }
        public string size { get; set; }

        public string endPosX { get; set; }
        public string endPosY { get; set; }
        public string endPosZ { get; set; }
        
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
            ret += "                    <object_endposition>\n";
            ret += "                      <x>" + endPosX + "</x>\n";
            ret += "                      <y>" + endPosY + "</y>\n";
            ret += "                      <z>" + endPosZ + "</z>\n";
            ret += "                    </object_endposition>\n";
            ret += "                    <object_size>" + size + "</object_size>\n";
            ret += "                    <slippery>" + slippery + "</slippery>\n";
            ret += "                    <visible>" + isVisible.ToString() + "</visible>\n";
            ret += "                  </object>\n";
            return ret;
        }
    }
}
