using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

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

        protected GameObject m_triggersActionOfObject = null;
        protected String m_triggersActionWithID = null;

        protected BoundingBox m_boundingBox;
        public BoundingBox getBoundingBox() { return this.m_boundingBox; }
        public void setBoundingBox(BoundingBox box) { this.m_boundingBox = box; }

        protected int size = 1;

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
        public void setCurrentSpeed(float value) { currentspeed = value; }

        #endregion

        #region graphics

        protected Model m_model;
        protected Model m_original_model;
        public Model getModel() { return this.m_model; }

        protected bool m_hasBillboard = false;
        public bool getHasBillboard() { return m_hasBillboard; }
        protected Billboard m_bb = null;
        public Billboard getBillboard() { return m_bb; }

        protected Texture2D m_texture;
        protected Texture2D m_original_texture;
        public Texture2D getTexture2D() { return this.m_texture; }

        protected Dictionary<int, Texture2D> m_modelTextures;

        protected Effect effect;
        public Effect getEffect() { return this.effect; }

        protected AnimationPlayer animationPlayer;
        protected SkinningData m_skinningData;

        public struct Material
        {
            public Vector4 ambient;
            public Vector4 diffuse;
            public Vector4 specular;
            public float shiny;
            public Effect effect;
        }
        protected Material m_material;
        public Material getMaterial() { return m_material; }

        public class ModelGroup
        {
            public Model model;
            public Dictionary<int, Texture2D> textures;
            public Material material;
        }

        protected bool m_isAnimated = false;

        public class ModelGroupAnimated : ModelGroup
        {
            public AnimationPlayer animationPlayer;
        }

        #endregion

        #region interaction

        // True when the Object should be visible for the Player
        public bool isVisible { get; set; }
        protected bool original_isVisible;
        public bool getOriginalVisibility() { return original_isVisible; }

        public Vector3 minOld { get; set; }
        public Vector3 maxOld { get; set; }

        #endregion

        #endregion

        #region abstract methods

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

        #region virtual methods

        public virtual void init(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            this.ID = id;
            this.m_position = pos;
            this.m_original_position = pos;
            this.isVisible = visible;
            this.original_isVisible = isVisible;
            this.m_updateInfo = updateInfo;
            this.m_material = new Material();
            this.m_material.ambient = GameConstants.ambient;
            this.m_material.diffuse = GameConstants.diffuse;
            this.m_material.specular = GameConstants.specular;
            this.m_material.shiny = GameConstants.shiny;

            this.m_modelTextures = new Dictionary<int, Texture2D>();
        }

        public virtual void load(ContentManager content)
        {
            m_material.effect = content.Load<Effect>("Shaders/Shader");

            if (m_model == null)
                return;

            foreach (ModelMesh mesh in m_model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect basicEffect = part.Effect as BasicEffect;

                    if (basicEffect != null)
                        m_modelTextures[mesh.GetHashCode()] = basicEffect.Texture;

                    part.Effect = m_material.effect;
                }
            }

            if (!m_modelTextures.ContainsKey(-1))
                m_modelTextures.Add(-1, m_texture);

            // Look up our custom skinning information.
            m_skinningData = m_model.Tag as SkinningData;

            if (m_skinningData == null)
            {
                return;
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            }

            m_isAnimated = true;

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(m_skinningData);
        }

        public virtual void Reset()
        {
            m_position = m_original_position;
            calculateBoundingBox();
            isVisible = original_isVisible;
            m_model = m_original_model;
            direction = original_direction;
            currentspeed = original_currentspeed;
        }

        public virtual ModelGroup GetModelGroup()
        {
            ModelGroup group = new ModelGroup();
            if (m_isAnimated)
            {
                ModelGroupAnimated temp = new ModelGroupAnimated();
                temp.animationPlayer = animationPlayer;
                group = temp;
            }
            group.model = m_model;
            group.textures = m_modelTextures;
            group.material = m_material;
            return group;
        }

        public virtual void endIntersection()
        {
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        /// <summary>
        /// Draws the Game Object, using the View and Projection Matrix of the Camera Class
        /// </summary>
        public virtual Matrix prepareForDrawing()
        {
            Matrix worldMatrix = new Matrix();

            if (isVisible && !(m_model == null))
            {
                Matrix view = m_updateInfo.viewMatrix;
                Matrix projection = m_updateInfo.projectionMatrix;
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[m_model.Bones.Count];
                m_model.CopyAbsoluteBoneTransformsTo(transforms);

                Matrix translateMatrix = Matrix.CreateTranslation(m_position);
                worldMatrix = translateMatrix;

                return worldMatrix;
            }

            return worldMatrix;
        }

        #endregion

        #region actions

        public virtual void setTrigger(String actionID, GameObject triggeredObject)
        {
            m_triggersActionOfObject = triggeredObject;
            m_triggersActionWithID = actionID;
        }

        public virtual void addAction(Action action)
        {
        }

        public virtual void Trigger(String actionID)
        {
        }

        #endregion

        

        protected void calculateBoundingBox()
        {
            if (m_model == null)
                return;
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
    }
}
