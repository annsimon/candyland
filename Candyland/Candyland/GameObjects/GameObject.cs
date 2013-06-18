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
        #region member variables

        #region general

        protected String ID;
        public String getID() { return this.ID; }

        protected UpdateInfo m_updateInfo;

        protected BoundingBox m_boundingBox;
        public BoundingBox getBoundingBox() { return this.m_boundingBox; }
        public void setBoundingBox(BoundingBox box) { this.m_boundingBox = box; }

        #endregion

        #region position, direction, speed

        protected Vector3 m_position;
        protected Vector3 m_original_position;
        public Vector3 getPosition() { return this.m_position; }
        public void setPosition(float x, float y, float z) { this.m_position = new Vector3(x, y, z); }

        protected Vector3 direction;        //Laufrichtung in x-z Ebene
        protected Vector3 original_direction;        //Laufrichtung in x-z Ebene
        public Vector3 getDirection() { return direction; }

        protected float currentspeed;       //Momentane geschwindigkeit
        protected float original_currentspeed;       //Momentane geschwindigkeit
        public float getCurrentSpeed() { return currentspeed; }

        #endregion

        #region graphics

        protected Model m_model;
        protected Model m_original_model;
        public Model getModel() { return this.m_model; }

        protected Texture2D m_texture;
        protected Texture2D m_original_texture;
        public Texture2D getTexture2D() { return this.m_texture; }

        protected Effect effect;
        public Effect getEffect() { return this.effect; }

        #endregion

        #region interaction

        // True at times when the Object is taking an active role in the Game (like a selected Player or moving Objects)
        protected bool isActive;
        protected bool original_isActive;
        public bool getActive() { return this.isActive; }
        public void setActive(bool value) { this.isActive = value; }

        public Vector3 minOld { get; set; }
        public Vector3 maxOld { get; set; }

        public bool isDestroyed { get; set; }

        #endregion

        #endregion

        #region abstract methods

        public abstract void load(ContentManager content);

        public abstract void collide(GameObject obj);

        public abstract void hasCollidedWith(GameObject obj);
        public abstract void isNotCollidingWith(GameObject obj);

        #endregion

        /// <summary>
        /// sets the Position of a Game Object to the specified Point and translates the BoundingBox
        /// </summary>
        public void setPosition(Vector3 newVector)
        {
            Vector3 translate = newVector - m_position;
            this.m_position = newVector;
            this.m_boundingBox.Min += translate;
            this.m_boundingBox.Max += translate;
        }

        public virtual void init(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.m_original_position = pos;
            this.isActive = false;
            this.original_isActive = false;
            this.m_updateInfo = updateInfo;
        }

        public virtual void Reset()
        {
            m_position = m_original_position;
            calculateBoundingBox();
            isActive = original_isActive;
            m_model = m_original_model;
            direction = original_direction;
            currentspeed = original_currentspeed;
            isDestroyed = false;
        }

        public virtual void endIntersection()
        {
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

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
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                        effect.Parameters["World"].SetValue(worldMatrix * mesh.ParentBone.Transform);
                        effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0,0,1));
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                        effect.Parameters["WorldInverseTranspose"].SetValue(
                        Matrix.Transpose(Matrix.Invert(worldMatrix * mesh.ParentBone.Transform)));
                        effect.Parameters["Texture"].SetValue(m_texture);
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
