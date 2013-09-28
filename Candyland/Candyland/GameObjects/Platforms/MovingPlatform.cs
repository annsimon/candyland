﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class MovingPlatform : Platform
    {
        #region properties

        Vector3 start;
        Vector3 end;
        int sign = 1;
        int m_originalsign = 1;
        bool nowchangingdirection = false;
        int lastDir = 0;

        #endregion

        public MovingPlatform(String id, Vector3 start, Vector3 end, UpdateInfo updateInfo, bool visible, int size) 
        {
            initialize(id, start, end, updateInfo, visible, size);
        }

        #region initialization

        public void initialize(String id, Vector3 start, Vector3 end, UpdateInfo updateInfo, bool visible, int size)
        {
            this.size = size;

            base.init(id, start, updateInfo, visible);

            this.start = start;
            this.end = end;
            currentspeed = 0.01f;
            direction = start - end;
            direction.Normalize();
            direction *= currentspeed;
            original_currentspeed = currentspeed;
            original_direction = direction;
        }

        public override void load(ContentManager content, AssetManager assets)
        {
            switch (size)
            {
                case 1: loadSmall(assets); break;
                case 2: loadMedium(assets); break;
                case 3: loadLarge(assets); break;
                default: loadSmall(assets); break;
            }
            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = assets.commonShader;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content, assets);
        }

        #endregion

        public override void update()
        {
            if (!isVisible)
                return;
            
            nowchangingdirection = false;
            if (lastDir != 1 && (m_position - start).Length() < currentspeed)
            {
                float check = (m_position - start).Length();
                nowchangingdirection = true;
                direction *= -1;
                lastDir = 1;
            }

            else if (lastDir != 2 && (m_position - end).Length() < currentspeed )
            {
                float check = (m_position - start).Length();
                nowchangingdirection = true;
                direction *= -1;
                lastDir = 2;
            }

            m_position += direction;
            m_boundingBox.Min += direction;
            m_boundingBox.Max += direction;
        }

        #region collision

        public override void collide(GameObject obj) { }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj) {
            base.hasCollidedWith(obj);
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            return base.prepareForDrawing();
        }

        public override void Reset()
        {
            lastDir = 0;
            base.Reset();
        }

    }
}
