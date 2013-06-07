using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Parent Class for all Objects that appear in the Game World (Platforms, Obstacles, Characters...)
    /// </summary>
    public abstract class GameObject : GameElement
    {
        protected String ID;
        public String getID() { return this.ID; }

        protected Vector3 m_position;
        public Vector3 getPosition() { return this.m_position; }
        public void setPosition(float x, float y, float z) { this.m_position = new Vector3(x,y,z); }
        public void setPosition(Vector3 newVector) { this.m_position = newVector; }

        protected Vector3 direction;        //Laufrichtung in x-z Ebene
        public Vector3 getDirection() { return direction; }
        protected float currentspeed;       //Momentane geschwindigkeit
        public float getCurrentSpeed() { return currentspeed; }

        protected Model m_model;
        public Model getModel() { return this.m_model; }

        protected BoundingBox m_boundingBox;
        public BoundingBox getBoundingBox() { return this.m_boundingBox; }
        public void setBoundingBox(BoundingBox box) { this.m_boundingBox = box; }

        protected UpdateInfo m_updateInfo;

        // True at times when the Object is taking an active role in the Game (like a selected Player or moving Objects)
        protected bool isActive;
        public bool getActive() { return this.isActive; }
        public void setActive(bool value) { this.isActive = value; }


        public abstract void load(ContentManager content);

        public abstract void collide(GameObject obj);

        public abstract void hasCollidedWith(GameObject obj);
        public abstract void isNotCollidingWith(GameObject obj);

        //TODO test if, this works
        /// <summary>
        /// Calculates the Bounding Box for a Model by looping over all vertices and finding the min and max coordinates
        /// We can probably do this an easier way, if there aren't gonna be such complex models
        /// </summary>
        //protected void calculateBoundingBox()
        //{
        //    // Copy any parent transforms.
        //    Matrix[] transforms = new Matrix[m_model.Bones.Count];
        //    m_model.CopyAbsoluteBoneTransformsTo(transforms);

        //    // Create variables to keep min and max xyz values for the model
        //    Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        //    Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        //    foreach (ModelMesh mesh in m_model.Meshes)
        //    {
        //        //Create variables to hold min and max xyz values for the mesh
        //        Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        //        Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        //        // There may be multiple parts in a mesh (different materials etc.) so loop through each
        //        foreach (ModelMeshPart part in mesh.MeshParts)
        //        {
        //            // The stride is how big, in bytes, one vertex is in the vertex buffer
        //            int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

        //            byte[] vertexData = new byte[stride * part.NumVertices];
        //            part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, 1); // fixed 13/4/11

        //            // Find minimum and maximum xyz values for this mesh part
        //            // We know the position will always be the first 3 float values of the vertex data
        //            Vector3 vertPosition = new Vector3();
        //            for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
        //            {
        //                vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
        //                vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
        //                vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

        //                // update our running values from this vertex
        //                meshMin = Vector3.Min(meshMin, vertPosition);
        //                meshMax = Vector3.Max(meshMax, vertPosition);
        //            }
        //        }

        //        // transform by mesh bone transforms
        //        meshMin = Vector3.Transform(meshMin, transforms[mesh.ParentBone.Index]);
        //        meshMax = Vector3.Transform(meshMax, transforms[mesh.ParentBone.Index]);

        //        // Expand model extents by the ones from this mesh
        //        modelMin = Vector3.Min(modelMin, meshMin);
        //        modelMax = Vector3.Max(modelMax, meshMax);
        //    }

        //    // Create the Bounding Box with calculated meshMin and meshMax
        //    this.m_boundingBox = new BoundingBox(this.m_position + modelMin, this.m_position + modelMax);
        //    //Console.WriteLine("Min " + meshMin + " Max " + meshMax);
        //}

        protected void calculateBoundingBox()
        {
            // found on http://gamedev.stackexchange.com/questions/2438/how-do-i-create-bounding-boxes-with-xna-4-0
            // slightly changed, because I deleted the transformation into world coordinates, seemed unnecessary

            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 minVertex = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxVertex = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in this.m_model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {

                //ModelMeshPart meshPart = mesh.MeshParts.ElementAt(2);
                    //int vertexOffset = meshPart.VertexOffset;

                    //Console.WriteLine("Offset" + vertexOffset);

                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride; //number of bytes for each vertex
                    int vertexBufferSize = meshPart.NumVertices * vertexStride; // vertex buffer size in bytes

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    //meshPart.VertexBuffer.GetData<float>(meshPart.VertexOffset * vertexStride,vertexData,0,meshPart.NumVertices,vertexStride);
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 vertexPosition = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                        minVertex = Vector3.Min(minVertex, vertexPosition);
                        maxVertex = Vector3.Max(maxVertex, vertexPosition);
                    }
                }
            }

            // Create the Bounding Box with calculated minVertex and maxVertex
            this.m_boundingBox = new BoundingBox(this.m_position + minVertex, this.m_position + maxVertex);
        }

        /// <summary>
        /// Draws the Game Object, using the View and Projection Matrix of the Camera Class
        /// </summary>
        /// <param name="view">Camera.viewMatrix</param>
        /// <param name="projection">Camera.projectionMatrix</param>
        public virtual void draw()
        {
            Matrix view = m_updateInfo.viewMatrix;
            Matrix projection = m_updateInfo.projectionMatrix;
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix translateMatrix = Matrix.CreateTranslation(m_position);
            Matrix worldMatrix = translateMatrix;

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in m_model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        worldMatrix * transforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

            /***************************************************************************************
             * For Debugging Purposes */
       
            // Render a Primitive to show the BoundingBox
            BoundingBoxRenderer.Render(this.m_boundingBox,m_updateInfo.graphics,view,projection,Color.White);

            /***************************************************************************************/
        }


    }
}
