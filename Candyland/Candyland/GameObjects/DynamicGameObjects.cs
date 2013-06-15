﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public abstract class DynamicGameObjects : GameObject
    {
        protected float upvelocity;             //beschleinigungsfaktor un y richtung
        protected bool isonground = false;
        protected Vector3 minOld;
        protected Vector3 maxOld;

        protected virtual void fall()
        {
            upvelocity += GameConstants.gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
        }

        protected void preventIntersection(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {


                float m_minX = Math.Min(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_minY = Math.Min(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_minZ = Math.Min(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float m_maxX = Math.Max(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_maxY = Math.Max(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_maxZ = Math.Max(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float minX = Math.Min(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float minY = Math.Min(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float minZ = Math.Min(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);
                float maxX = Math.Max(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float maxY = Math.Max(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float maxZ = Math.Max(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);

                float m_minXold = minOld.X;
                float m_minYold = minOld.Y;
                float m_minZold = minOld.Z;
                float m_maxXold = maxOld.X;
                float m_maxYold = maxOld.Y;
                float m_maxZold = maxOld.Z;

                if (m_minYold >= maxY)
                {
                    isonground = true;

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = maxY + m_boxheight;
                    m_boundingBox.Min.Y = maxY;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                }

                if (m_maxYold <= minY)
                {

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = minY;
                    m_boundingBox.Min.Y = minY - m_boxheight;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                    upvelocity = 0;
                }

                if (m_minXold >= maxX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;


                    m_boundingBox.Max.X = maxX + m_boxwidth;
                    m_boundingBox.Min.X = maxX;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }
                if (m_maxXold <= minX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;


                    m_boundingBox.Max.X = minX;
                    m_boundingBox.Min.X = minX - m_boxwidth;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }

                if (m_minZold >= maxZ
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxdepth = m_maxZ - m_minZ;
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = maxZ + m_boxdepth;
                    m_boundingBox.Min.Z = maxZ;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
                if (m_maxZold <= minZ
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxdepth = m_maxZ - m_minZ;
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = minZ;
                    m_boundingBox.Min.Z = minZ - m_boxdepth;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
            }
        }

    }
}