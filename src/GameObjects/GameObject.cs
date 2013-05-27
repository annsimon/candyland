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
        protected Vector3 Position {get; set;}
        protected Model Model;
 //       protected Texture2D Texture;
        protected BoundingBox BoundingBox;
        protected bool isActive;

        public abstract void Load(ContentManager content);

        /// <summary>
        /// Draws the Game Object, using the View and Projection Matrix of the Camera Class
        /// </summary>
        /// <param name="view">Camera.viewMatrix</param>
        /// <param name="projection">Camera.projectionMatrix</param>
        public void Draw(Matrix view, Matrix projection)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix translateMatrix = Matrix.CreateTranslation(Position);
            Matrix worldMatrix = translateMatrix;

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in Model.Meshes)
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
