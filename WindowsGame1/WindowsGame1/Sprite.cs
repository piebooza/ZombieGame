﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Sprite
    {
        public Vector2 Pos;
        protected Texture2D Image;
        protected Rectangle Frame;
        protected Color Tint;
        protected Vector2 Scale;
        protected Vector2 Origin;
        public float Rotation;
        public Sprite(Rectangle rect, Vector2 pos, Texture2D image, Color tint, Vector2 scale, float rotation)
        {
            Rotation = rotation;
            Frame = rect;
            Image = image;
            Tint = tint;
            Origin = new Vector2(rect.Width / 2, rect.Height / 2);
            Scale = scale;
            Pos = pos;
        }

        public Sprite(Vector2 pos, Texture2D image, Color tint, Vector2 scale)
        {
            Rotation = 0;
            Frame = new Rectangle(0, 0, image.Width, image.Height);
            Image = image;
            Tint = tint;
            Origin = new Vector2(Frame.Width / 2, Frame.Height / 2);
            Scale = scale;
            Pos = pos;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Pos, Frame, Tint, Rotation, Origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
