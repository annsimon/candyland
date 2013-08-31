using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Candyland
{
    /**
     * This class is taken directly from http://algorhymes.wordpress.com/2012/10/02/cxna-handling-2d-animations/
     * 
     */
    public class Animation
    {
        Rectangle[] frames;
        float frameLength = 1f / 5f;
        float timer = 0f;
        public bool startUpdate = false;
        int currentFrame = 0;

        public Rectangle CurrentFrame
        {
            get { return frames[currentFrame]; }
        }

        public int FramesPerSecond
        {
            get { return (int)(1f / frameLength); }
            set { frameLength = 1f / (float)value; }
        }

        public Animation(int width, int height, int numFrames, int xOffset, int yOffset)
        {
            frames = new Rectangle[numFrames];

            int frameWidth = width / numFrames;

            for (int i = 0; i < numFrames; i++)
            {
                frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, height);
            }
        }

        public void Update(GameTime gameTime, bool isRepeatable)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameLength)
            {
                timer = 0f;

                if (isRepeatable)
                {
                    ++currentFrame;
                    if (this.frames.Length == currentFrame) currentFrame = 0;
                }
                else
                {
                    if (startUpdate)
                        if (this.frames.Length > currentFrame + 1) ++currentFrame;
                }
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            timer = 0f;
        }
    }

    public class AnimatingSprite
    {
        public Vector2 Position = Vector2.Zero;
        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        public bool isRepeatable = true;
        public Texture2D texture;
        string currentAnimation;
        bool updateAnimation = true;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public string CurrentAnimation
        {
            get { return currentAnimation; }

            set
            {
                if (!Animations.ContainsKey(value))
                    throw new Exception("Invalid animation specified.");

                if (currentAnimation == null || !currentAnimation.Equals(value))
                {
                    currentAnimation = value;

                    if (isRepeatable) Animations[currentAnimation].Reset();
                }
            }
        }

        public void StartAnimation()
        {
            updateAnimation = true;
        }

        public void StopAnimation()
        {
            updateAnimation = false;
        }

        public void Update(GameTime gameTime)
        {
            if (currentAnimation == null)
            {
                if (Animations.Keys.Count == 0)
                    return;

                string[] keys = new string[Animations.Keys.Count];

                Animations.Keys.CopyTo(keys, 0);

                currentAnimation = keys[0];
            }

            if (updateAnimation)
                Animations[currentAnimation].Update(gameTime, isRepeatable);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(
                texture,
                Position,
                Animations[currentAnimation].CurrentFrame,
                Color.White);
        }
    }
}
