﻿using System;
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
        protected Vector3 m_position;
        public Vector3 getPosition() { return this.m_position; }
        public void setPosition(float x, float y, float z) { this.m_position = new Vector3(x,y,z); }
        public void setPosition(Vector3 newVector) { this.m_position = newVector; }

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

        //TODO test if, this works
        /// <summary>
        /// Calculates the Bounding Box for a Model by looping over all vertices and finding the min and max coordinates
        /// We can probably do this an easier way, if there aren't gonna be such complex models
        /// </summary>
        protected void calculateBoundingBox()
        {
           //Create variables to hold min and max xyz values for the mesh. Initialise them to extremes
           Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
           Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

           foreach (ModelMesh mesh in this.m_model.Meshes)
           {
              // There may be multiple parts in a mesh (different materials etc.) so loop through each
               foreach (ModelMeshPart part in mesh.MeshParts)
               {
                   // The stride is how big, in bytes, one vertex is in the vertex buffer
                   // We have to use this as we do not know the make up of the vertex
                   int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                   byte[] vertexData = new byte[stride * part.NumVertices];
                   part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, 1);

                   // Find minimum and maximum xyz values for this mesh part
                   // We know the position will always be the first 3 float values of the vertex data
                   Vector3 vertPosition = new Vector3();
                   for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
                   {
                       vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
                       vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
                       vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

                       // update our running values from this vertex
                       meshMin = Vector3.Min(meshMin, vertPosition);
                       meshMax = Vector3.Max(meshMax, vertPosition);
                   }
               }
           }
           this.m_boundingBox = new BoundingBox(this.m_position + meshMin + (meshMax - meshMin)/2, this.m_position + meshMax + (meshMax - meshMin)/2);
        }


        /// <summary>
        /// Draws the Game Object, using the View and Projection Matrix of the Camera Class
        /// </summary>
        /// <param name="view">Camera.viewMatrix</param>
        /// <param name="projection">Camera.projectionMatrix</param>
        public void draw(Matrix view, Matrix projection)
        {
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
        }


    }
}
