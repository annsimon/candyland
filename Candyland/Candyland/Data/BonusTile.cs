using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class BonusTile
    {
        string id;
        string name;
        string bonusType;
        string textureString;
        int price;
        Texture2D texture;
        Rectangle rectangle;
        bool sold;

        #region getter

        /// <summary>
        /// Tells if the bonus is already purchased by the player
        /// </summary>
        public bool isSold
        {
            get { return sold; }
        }

        /// <summary>
        /// Gets the ID of a bonus
        /// </summary>
        public string ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the name of a bonus
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the type of a bonus (ConceptArt, BonusLevel...)
        /// </summary>
        public string Type
        {
            get { return bonusType; }
        }

        /// <summary>
        /// Gets the texture string of a bonus tile
        /// </summary>
        public string TextureString
        {
            get { return textureString; }
        }

        /// <summary>
        /// Gets the price of a bonus
        /// </summary>
        public int Price
        {
            get { return price; }
        }

        /// <summary>
        /// Gets or sets the texture of a bonus tile
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle a bonus tile will be drawn in
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        #endregion

        public BonusTile(string id, string name, string type, int price, string texture)
        {
            this.id = id;
            this.name = name;
            this.bonusType = type;
            this.textureString = texture;
            this.price = price;
        }
    }
}
