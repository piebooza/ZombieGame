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

namespace WindowsGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum ScreenState
        {
            Start,
            Game,
            End,
            Pause
        }

        ScreenState currentScreen = ScreenState.Game;

        //dying to zombies
        //different zombies (some move slow + health, fast + low health)
        //different weapons (damage, fire rate, spread, autofire, reload, etc)
        //ammo drops
        //as game goes on: decrease zombie spawn time, increase zombie speed
        //high scores

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite background;
        Sprite target;
        Sprite DeathScreen;
        List<Zombie> zombies = new List<Zombie>();
        Player player;
        Vector2 screen;
        Random rand = new Random();
        MouseState ms;
        Random random = new Random();
        SpriteFont Text;
        SpriteFont DeathText;
        List<int> Counter;
        int whichZombie;
        int points;
        int zombiespawntime = 1000;
        Texture2D pixel;
        
        TimeSpan zombieSpawn = TimeSpan.Zero;
        TimeSpan timer = TimeSpan.Zero;

        int killCount = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            screen = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Counter = new List<int>();
            int zombieX;
            int zombieY;

            do
            {
                zombieX = rand.Next(-500, GraphicsDevice.Viewport.Width + 500);
                zombieY = rand.Next(-500, GraphicsDevice.Viewport.Height + 500);
            } while (GraphicsDevice.Viewport.Bounds.Contains(zombieX, zombieY));

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<int>(new int[] { int.MaxValue });

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Text = Content.Load<SpriteFont>("SpriteFont1");
            DeathText = Content.Load<SpriteFont>("SpriteFont2");
            Player.BulletImage = Content.Load<Texture2D>("bullet");
            target = new Sprite(new Vector2(0, 0), Content.Load<Texture2D>("Target_Cursor"), Color.Red, new Vector2(.07f, .07f));
            background = new Sprite(new Vector2(screen.X / 2, screen.Y / 2), Content.Load<Texture2D>("Back2"), Color.White, new Vector2(2.4f, 1.3f));
            DeathScreen = new Sprite(new Vector2(screen.X/2, screen.Y/2), Content.Load<Texture2D>("deathscreen"), Color.White, new Vector2(2, 1.1f));
            player = new Player(Player.AnimationState.pistol, new Vector2(screen.X / 2, screen.Y / 2), Content.Load<Texture2D>("Characters"), Color.White, new Vector2(1, 1), 0f);
            zombies.Add(new Zombie(Zombie.ZombieState.Normal, new Vector2(zombieX, zombieY), Content.Load<Texture2D>("Characters"), Color.White, new Vector2(1, 1), 0f));


        }
        protected override void UnloadContent()
        {
        }
        public bool CheckFloat(float x, float y)
        {
            return (x - 10 < y && x + 10 > y);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState lastMs = ms;
            ms = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            if (currentScreen == ScreenState.Game)
            {
                #region Game Logic
                timer += gameTime.ElapsedGameTime;
                if (timer > TimeSpan.FromMilliseconds(zombiespawntime))
                {
                    timer = TimeSpan.Zero;
                }

                zombieSpawn += gameTime.ElapsedGameTime;
            }
            List<int> Counter = new List<int>();
            for (int i = 0; i < zombies.Count; i++)
            {
                for (int j = 0; j < player.Bullets.Count; j++)
                {
                    if (CheckFloat(player.Bullets[j].Pos.X, zombies[i].Pos.X))
                    {
                        if (player.Bullets[j].getHitbox().Intersects(zombies[i].ZombieHitbox()))
                        {
                            Counter.Add(i);
                            player.Bullets.RemoveAt(j);
                            j--;
                            killCount++;
                            points += 50;
                        }
                    }
                }
            }
            for (int i = 0; i < Counter.Count; i++)
            {
                zombies.RemoveAt(Counter[i]);

            }
            if (zombieSpawn > TimeSpan.FromMilliseconds(500))
            {
                zombieSpawn = TimeSpan.Zero;
                int zombieX;
                int zombieY;
                do
                {
                    zombieX = rand.Next(-500, GraphicsDevice.Viewport.Width + 500);
                    zombieY = rand.Next(-500, GraphicsDevice.Viewport.Height + 500);
                } while (GraphicsDevice.Viewport.Bounds.Contains(zombieX, zombieY));

                zombies.Add(new Zombie((Zombie.ZombieState)rand.Next(0, 3), new Vector2(zombieX, zombieY), Content.Load<Texture2D>("Characters"), Color.White, new Vector2(1, 1), 0f));
            }

            whichZombie = random.Next(0, 3);

            //move this foreach inside the zombie class
            foreach (Zombie zombie in zombies)
            {
                Vector2 diff2 = player.Pos - zombie.Pos;
                double rot2 = Math.Atan2(diff2.Y, diff2.X);
                zombie.Rotation = (float)rot2;


                Vector2 diff3 = player.Pos - zombie.Pos;
                diff3.Normalize();
                zombie.Pos += diff3 * new Vector2(.05f, .05f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                bool isHit = false;
                foreach (Zombie z in zombies)
                {
                    if (z.ZombieHitbox().Intersects(player.PlayerHitbox()))
                    {
                        isHit = true;
                        break;
                    }
                }

                if (isHit)
                {
                    currentScreen = ScreenState.End;
                }


            }
            #endregion Game Logic
            player.Update(gameTime, keyboard, ms, lastMs, target, GraphicsDevice.Viewport);



            if (currentScreen == ScreenState.Start)
            {
                //TODO: add logic for start screen.
            }
            if (currentScreen == ScreenState.End)
            {

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            if (currentScreen == ScreenState.Game)
            {
                #region Game Draw

                background.Draw(spriteBatch);
                player.Draw(spriteBatch);
                

                foreach (Zombie zombie in zombies)
                {
                    zombie.Draw(spriteBatch);
                }
               

                spriteBatch.DrawString(Text, String.Format("Zombies Destroyed: {0}", killCount), new Vector2(600, 0), Color.Black);
                spriteBatch.DrawString(Text, String.Format("Points: {0}", points), new Vector2(702, 20), Color.Black);


                target.Draw(spriteBatch);

            }
            else if (currentScreen == ScreenState.Start)
            {
                //TODO: add draw for start screen.
            }

            else if (currentScreen == ScreenState.End)
            {
                DeathScreen.Draw(spriteBatch);
                spriteBatch.DrawString(DeathText, "You Died", new Vector2(screen.X / 2 - 64, screen.Y / 2- 20), Color.White);
            }

            else if (currentScreen == ScreenState.Pause)
            {
                //TODO: add draw for pause screen.
            }

            #endregion Game Draw
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
