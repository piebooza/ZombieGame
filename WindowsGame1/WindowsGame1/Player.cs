using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Player : Sprite
    {
        public enum AnimationState
        {
            pistol,
            uzi,
            shotgun
        }

        public static Texture2D BulletImage;

        public bool isshooting = false;
        TimeSpan shootDelayTime = TimeSpan.FromMilliseconds(20);
        TimeSpan shootDelayTimer = TimeSpan.Zero;
        
        Dictionary<AnimationState, Rectangle[]> animation = new Dictionary<AnimationState, Rectangle[]>();
        AnimationState Gun;

        public List<Bullet> Bullets = new List<Bullet>();

        public Player(AnimationState gun, Vector2 pos, Texture2D image, Color tint, Vector2 scale, float rotation)
            : base(pos, image, tint, scale)
        {

            Gun = gun;
            Rectangle[] pistol = { new Rectangle(0, 4, 38, 29), new Rectangle(0, 43, 39, 29) };
            Rectangle[] uzi = { new Rectangle(78, 4, 39, 31), new Rectangle(73, 43, 39, 31) };
            Rectangle[] shotgun = { new Rectangle(156, 4, 39, 31), new Rectangle(156, 43, 39, 31) };
            
            animation.Add(AnimationState.pistol, pistol);
            animation.Add(AnimationState.uzi, uzi);
            animation.Add(AnimationState.shotgun, shotgun);


            Rotation = rotation;
            Frame = animation[gun][0];
            Origin = new Vector2(Frame.Width / 2, Frame.Height / 2);
        }

        public Player(Vector2 pos, Texture2D image, Color tint, Vector2 scale)
            : base(pos, image, tint, scale)
        {
        }

        public void Update(GameTime gameTime, KeyboardState keyboard, MouseState ms, MouseState lastMs, Sprite target, Viewport screen)
        {
            target.Pos = new Vector2(ms.X, ms.Y);

            Vector2 diff = new Vector2(ms.X, ms.Y) - Pos;
            double rot = Math.Atan2(diff.Y, diff.X);
            Rotation = (float)rot;



            if (Gun == AnimationState.pistol)
            {
                if (ms.LeftButton == ButtonState.Pressed && lastMs.LeftButton == ButtonState.Released && shootDelayTimer == TimeSpan.Zero)
                {
                    isshooting = true;
                }

            }




            if (isshooting)
            {
                Frame = animation[Gun][1];

                shootDelayTimer += gameTime.ElapsedGameTime;
                if (shootDelayTimer >= shootDelayTime)
                {
                    MouseState mouse = Mouse.GetState();
                    Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

                    Vector2 bulletSpeed = Pos - mousePosition;
                    bulletSpeed.Normalize();

                    Vector2 diff2 = Pos - mousePosition;
                    double rotation = Math.Atan2(diff2.Y, diff2.X) + MathHelper.ToRadians(180);


                    Bullet bullet = new Bullet(new Rectangle(0, 0, BulletImage.Width, BulletImage.Height), Pos, BulletImage, Color.White, Vector2.One, (float)rotation, -(10*bulletSpeed));
                    Bullets.Add(bullet);
                    shootDelayTimer = TimeSpan.Zero;
                    isshooting = false;
                }

            }
            else
            {
                Frame = animation[Gun][0];
            }

            if ((keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) && Pos.Y - Frame.Height / 2 > 0)
            {
                Pos.Y -= 2;
            }
            if ((keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) && Pos.X - Frame.Width / 2 > 0)
            {
                Pos.X -= 2;
            }
            if ((keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) && Pos.X + Frame.Width / 2 < screen.Width)
            {
                Pos.X += 2;
            }
            if ((keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) && Pos.Y + Frame.Height / 2 < screen.Height)
            {
                Pos.Y += 2;
            }



            foreach (Bullet bullet in Bullets)
            {
                bullet.Update(gameTime);
            }


        }

        public Rectangle PlayerHitbox()
        {
            return new Rectangle((int)(Pos.X-20), (int)(Pos.Y-10), (int)(Image.Width - 160), (int)(Image.Height - 80));

        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach(Bullet bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
            
            base.Draw(spriteBatch);
        }
    }
}
