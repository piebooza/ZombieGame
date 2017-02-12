using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{

    public class Bullet : Sprite
    {
        private Vector2 _speed;

        public Bullet(Rectangle rect, Vector2 pos, Texture2D image, Color tint, Vector2 scale, float rotation, Vector2 speed)
            : base(rect, pos, image, tint, scale, rotation)
        {
            _speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Pos += _speed;
        }

        public Rectangle getHitbox()
        {
            return new Rectangle((int)(Pos.X - Origin.X), (int)(Pos.Y - Origin.Y), (int)(Image.Width * Scale.X), (int)(Image.Height * Scale.Y));
        }
    }
}
