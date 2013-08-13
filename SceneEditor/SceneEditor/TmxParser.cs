using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;

namespace SceneEditor
{
    static class TmxParser
    {
        public static void parseObjects(String filename, String idFront, 
                                        List<Object> statics, List<Object> dynamics)
        {
            // empty the list, we only want the new objects in it
            statics.Clear();
            dynamics.Clear();

            // parse level
            XmlDocument level = new XmlDocument();
            level.Load(filename);

            XmlNodeList objectgroups = level.GetElementsByTagName("objectgroup");

            // create ID for the objects
            int objectID = 0;

            foreach (XmlNode layer in objectgroups)
            {
                // base layer only contains "dummy" objects, don't parse it
                String name = layer.Attributes["name"].InnerText;
                if ( name == "BaseLayer")
                    continue;
                // the layer's name equals the y-position of all objects in that layer
                String posY = name;

                XmlNodeList objects = layer.ChildNodes;
                foreach (XmlNode obj in objects)
                {
                    // we only care about objects here
                    if (obj.Name != "object")
                        continue;

                    String objName = obj.Attributes["name"].InnerText;

                    String id = "";
                    if (objName == "-") id = idFront + "." + objectID;
                    else id = idFront + "." + objName;

                    String type = obj.Attributes["type"].InnerText;
                    double offset = Convert.ToDouble(obj.Attributes["width"].InnerText) / 2;
                    String posX = (Convert.ToDouble(obj.Attributes["x"].InnerText) + offset) / 20 + "";
                    String posZ = (Convert.ToDouble(obj.Attributes["y"].InnerText) + offset) / 20 + "";
                    String endPositionX = "";
                    String endPositionY = "";
                    String endPositionZ = "";
                    String isDoorToArea = "";
                    String isDoorToLevel = "";
                    String isVisible = "true";
                    String isSlippery = "false";
                    String size = "1";
                    
                    // get the property nodes
                    XmlNodeList properties = obj.ChildNodes[0].ChildNodes;
                    foreach (XmlNode property in properties)
                    {
                        if (property.Attributes["name"].InnerText == "endpositionX" && property.Attributes["value"].InnerText != "-")
                            endPositionX = (Convert.ToDouble(property.Attributes["value"].InnerText) + (offset/10)) / 2 + "";
                        else
                        if (property.Attributes["name"].InnerText == "endPositionY" && property.Attributes["value"].InnerText != "-")
                            endPositionY = property.Attributes["value"].InnerText;
                        else
                        if (property.Attributes["name"].InnerText == "endPositionZ" && property.Attributes["value"].InnerText != "-")
                            endPositionZ = (Convert.ToDouble(property.Attributes["value"].InnerText) + (offset/10)) / 2 + "";
                        else
                        if (property.Attributes["name"].InnerText == "isDoorToArea")
                            if (property.Attributes["value"].InnerText == "-")
                                isDoorToArea = "x";
                            else
                                isDoorToArea = property.Attributes["value"].InnerText;
                        else
                        if (property.Attributes["name"].InnerText == "isDoorToLevel")
                            if( property.Attributes["value"].InnerText == "-")
                                isDoorToLevel = "x";
                            else
                                isDoorToLevel = property.Attributes["value"].InnerText;
                        else
                        if (property.Attributes["name"].InnerText == "isVisible" && property.Attributes["value"].InnerText != "-")
                            isVisible = property.Attributes["value"].InnerText;
                        else
                        if (property.Attributes["name"].InnerText == "isSlippery" && property.Attributes["value"].InnerText != "-")
                            isSlippery = property.Attributes["value"].InnerText;
                        else
                        if (property.Attributes["name"].InnerText == "size" && property.Attributes["value"].InnerText != "")
                            size = property.Attributes["value"].InnerText;
                    }

                    Object newObj = new Object();

                    newObj.type = type;
                    newObj.id = id;
                    newObj.posX = posX;
                    newObj.posY = posY;
                    newObj.posZ = posZ;
                    newObj.doorArea = isDoorToArea;
                    newObj.doorLevel = isDoorToLevel;
                    newObj.isSlippery = Convert.ToBoolean(isSlippery);
                    newObj.isVisible = Convert.ToBoolean(isVisible);
                    newObj.endPosX = endPositionX;
                    newObj.endPosY = endPositionY;
                    newObj.endPosZ = endPositionZ;
                    newObj.size = size;

                    if (objName == "-")
                    {
                        statics.Add(newObj);
                        objectID++;
                    }
                    else
                        dynamics.Add(newObj);
                }
            }
        }
    }
}
