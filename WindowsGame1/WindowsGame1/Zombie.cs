using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Zombie : Sprite
    {

        public enum ZombieState
        {
            Quick,
            Slow,
            Normal
        }


        ZombieState state = ZombieState.Normal;
        Dictionary<ZombieState, Rectangle> zombieAnimation;

        public Zombie(ZombieState state, Vector2 pos, Texture2D image, Color tint, Vector2 scale, float rotation)
            : base(Rectangle.Empty, pos, image, tint, scale, rotation)
        {
            this.state = state;
            zombieAnimation = new Dictionary<ZombieState, Rectangle>();

            Rectangle quick = new Rectangle(0, 82, 45, 30);
            Rectangle normal = new Rectangle(73, 82, 45, 30);
            Rectangle slow = new Rectangle(146, 82, 50, 30);
            zombieAnimation.Add(ZombieState.Quick, quick);
            zombieAnimation.Add(ZombieState.Normal, normal);
            zombieAnimation.Add(ZombieState.Slow, slow);

            Rect = zombieAnimation[state];
            Origin = new Vector2(Rect.Width / 2, Rect.Height / 2);
        }

        public Zombie(Vector2 pos, Texture2D image, Color tint, Vector2 scale)
            : base(pos, image, tint, scale)
        {
            Rotation = 0;
            Rect = new Rectangle(0, 0, image.Width, image.Height);
            Origin = new Vector2(Rect.Width / 2, Rect.Height / 2);
        }

        public Rectangle ZombieHitbox()
        {
            return new Rectangle((int)(Pos.X - Origin.X), (int)(Pos.Y - Origin.Y), (int)(Rect.Width * Scale.X), (int)(Rect.Height * Scale.Y));
        }

    }
}
