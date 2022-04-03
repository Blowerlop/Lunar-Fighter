using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LunarFighter
{
    public class Sprite
    {
        public int Xposition;
        public int Yposition;
        public int Velocite;

        public Sprite(int pXposition, int pYposition, int pVelocite)
        {
            Xposition = pXposition;
            Yposition = pYposition;
            Velocite = pVelocite;
        }
    }
    


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Lander lander;

        Random random = new Random();

        List<Sprite> listeEnnemi = new List<Sprite>();
        Texture2D imgEnnemi;

        Missile missile;
        Texture2D imgMissile;
        List<Missile> listeGestionMissiles = new List<Missile>();

        List<Missile> SuppList = new List<Missile>();

        Texture2D imgBackground;
        List<SoundEffect> soundEffects = new List<SoundEffect>();
        Song songTheme;

        KeyboardState previousKeyState;
        KeyboardState currentKeyState;

        MouseState previousMouseState;
        MouseState currentMouseState;

        SpriteFont Font;

        Texture2D imgStartMenu;
        Vector2 positionStartMenu;
        Texture2D imgDeadMenu;
        Vector2 positionDeadMenu;

        bool bStartMenu = true;
        bool bvivant = true;

        int score = 0;

        List<int> SetSpawnAtRandomWall(Texture2D pImg, int pLargeurEcran, int pHauteurEcran)
        {
            int x;
            int y;
            List<int> XandY = new List<int>();

            double hasard = random.NextDouble();
            if (hasard < 0.5) // X priority
            {
                hasard = random.NextDouble();
                if (random.NextDouble() < 0.5)
                {
                    x = random.Next(0 - pImg.Width - 1, 0 - pImg.Width);
                }
                else
                {
                    x = random.Next(pLargeurEcran, pLargeurEcran + 1);
                }
                y = random.Next(0 - pImg.Height, pHauteurEcran);
            }
            else // Y priority
            {
                hasard = random.NextDouble();
                if (hasard < 0.5)
                {
                    y = random.Next(0 - pImg.Height - 1, 0 - pImg.Height);
                }
                else
                {
                    y = random.Next(pHauteurEcran, pHauteurEcran + 1);
                }
                x = random.Next(0 - pImg.Width, pLargeurEcran);
            }
            XandY.Add(x);
            XandY.Add(y);
            return (XandY);           
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1900;
            graphics.PreferredBackBufferHeight = 1000;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            graphics.ApplyChanges();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);



            // TODO: use this.Content to load your game content here
            lander = new Lander();
            lander.img = Content.Load<Texture2D>("ship"); 
            lander.imgEngine = Content.Load<Texture2D>("engine");
            lander.position = new Vector2(GraphicsDevice.Viewport.Width / 2,
                                        GraphicsDevice.Viewport.Height / 2);


            imgEnnemi = Content.Load<Texture2D>("meteorBrown_med1");
            imgMissile = Content.Load<Texture2D>("spaceMissiles_040");

            imgBackground = Content.Load<Texture2D>("starfield");
            soundEffects.Add(Content.Load<SoundEffect>("sfx_wpn_laser8"));
            soundEffects.Add(Content.Load<SoundEffect>("asteroid"));
            soundEffects.Add(Content.Load<SoundEffect>("cursor_style_5"));
            soundEffects.Add(Content.Load<SoundEffect>("FGBS(17)"));
            songTheme = Content.Load<Song>("Yeah");

            imgStartMenu = Content.Load<Texture2D>("blue_button00");
            positionStartMenu = new Vector2(GraphicsDevice.Viewport.Width / 2 - imgStartMenu.Width / 2,
                                            GraphicsDevice.Viewport.Height / 2 - imgStartMenu.Height / 2);
            imgDeadMenu = Content.Load<Texture2D>("blue_button002");
            positionDeadMenu = new Vector2(GraphicsDevice.Viewport.Width / 2 - imgDeadMenu.Width / 2,
                                            GraphicsDevice.Viewport.Height / 2 - imgDeadMenu.Height / 2);

            MediaPlayer.Play(songTheme);
            MediaPlayer.IsRepeating = true;

            Font = Content.Load<SpriteFont>("file");

            
            int totalEnnemi = 5;
            for (int i = 0; i < totalEnnemi; i++)
            {
                List<int> XandY = SetSpawnAtRandomWall(imgEnnemi, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                int x = XandY[0];
                int y = XandY[1];
                int velocite = 2;

                Sprite Mechant = new Sprite(x, y, velocite);
                listeEnnemi.Add(Mechant);
            }
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (bStartMenu)
            {
                MediaPlayer.Volume = 0.1f;
                currentMouseState = Mouse.GetState();
                if (
                    (currentMouseState.Position.X < positionStartMenu.X + imgStartMenu.Width
                     && currentMouseState.Position.X > positionStartMenu.X
                     && currentMouseState.Position.Y < positionStartMenu.Y + imgStartMenu.Height
                     && currentMouseState.Position.Y > positionStartMenu.Y)
                     && 
                     !(previousMouseState.Position.X < positionStartMenu.X + imgStartMenu.Width
                     && previousMouseState.Position.X > positionStartMenu.X
                     && previousMouseState.Position.Y < positionStartMenu.Y + imgStartMenu.Height
                     && previousMouseState.Position.Y > positionStartMenu.Y)
                     )
                {
                    soundEffects[2].Play();
                }
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (currentMouseState.Position.X < positionStartMenu.X + imgStartMenu.Width
                        && currentMouseState.Position.X > positionStartMenu.X
                        && currentMouseState.Position.Y < positionStartMenu.Y + imgStartMenu.Height
                        && currentMouseState.Position.Y > positionStartMenu.Y)
                        
                    {
                        soundEffects[3].Play();
                        bStartMenu = false;
                    }

                }
                previousMouseState = currentMouseState;
            }
            if (bvivant && bStartMenu == false)
            {
                MediaPlayer.Volume = 0.5f;
// --------------------------------------------------------- KEYBORD -----------------------------------------------------------------------

                // Keybord Lander
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    lander.angle += 2;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    lander.angle -= 2;
                }

                // Keybord Moteur
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    lander.engineOn = true;

                    float angle_radian = MathHelper.ToRadians(lander.angle);
                    float force_x = (float)Math.Cos(angle_radian) * lander.speed;
                    float force_y = (float)Math.Sin(angle_radian) * lander.speed;
                    lander.velocity += new Vector2(force_x, force_y);
                }
                else
                {
                    lander.engineOn = false;
                }
                lander.update();

                 
                // Keybord Missile
                currentKeyState = Keyboard.GetState();
                if (currentKeyState.IsKeyDown(Keys.Space) && !previousKeyState.IsKeyDown(Keys.Space))
                {
                    float angle_radian = MathHelper.ToRadians(lander.angle);
                    float force_x = (float)Math.Cos(angle_radian) * 5;
                    float force_y = (float)Math.Sin(angle_radian) * 5;

                    missile = new Missile
                    {
                        VelocityTir = Vector2.Zero,
                        Tir = true,
                        Position = new Vector2(lander.position.X, lander.position.Y)
                    };
                    missile.VelocityTir += new Vector2(force_x, force_y);
                    listeGestionMissiles.Add(missile);
                    soundEffects[0].Play();
                }
                previousKeyState = currentKeyState;
                foreach (Missile eachMissiles in listeGestionMissiles)
                {
                    if (eachMissiles.Tir)
                    {
                        eachMissiles.Update();
                    }
                }

// --------------------------------------------------------- Collission -----------------------------------------------------------------------

                // Ennemi target Lander
                foreach (Sprite item in listeEnnemi)
                {
                    if (item.Xposition < lander.position.X && item.Yposition < lander.position.Y)
                    {
                        item.Xposition += item.Velocite;
                        item.Yposition += item.Velocite;

                    }
                    else if (item.Xposition < lander.position.X && item.Yposition > lander.position.Y)
                    {
                        item.Xposition += item.Velocite;
                        item.Yposition -= item.Velocite;
                    }
                    else if (item.Xposition > lander.position.X && item.Yposition < lander.position.Y)
                    {
                        item.Xposition -= item.Velocite;
                        item.Yposition += item.Velocite;
                    }
                    else
                    {
                        item.Xposition -= item.Velocite;
                        item.Yposition -= item.Velocite;
                    }  
                }


                // Collision Check Lander x Wall
                if (lander.position.X < 0)
                {
                    lander.position = new Vector2(GraphicsDevice.Viewport.Width, lander.position.Y);
                }
                else if (lander.position.X > GraphicsDevice.Viewport.Width)
                {
                    lander.position = new Vector2(0, lander.position.Y);
                }
                else if (lander.position.Y < 0)
                {
                    lander.position = new Vector2(lander.position.X, GraphicsDevice.Viewport.Height);
                }
                else if (lander.position.Y > GraphicsDevice.Viewport.Height)
                {
                    lander.position = new Vector2(lander.position.X, 0);
                }


                // Collision check Ennemi x Lander &&  Ennemi x Wall
                foreach (Sprite eachEnnemi in listeEnnemi)
                {
                    if (lander.position.X < eachEnnemi.Xposition + imgEnnemi.Width &&
                        lander.position.X + lander.img.Width > eachEnnemi.Xposition &&
                        lander.position.Y < eachEnnemi.Yposition + imgEnnemi.Height &&
                        lander.position.Y + lander.img.Height > eachEnnemi.Yposition)
                    {
                        bvivant = false;
                    }

                    
                    else if (eachEnnemi.Xposition < 0)
                    {
                        eachEnnemi.Xposition = -eachEnnemi.Xposition;
                    }
                    else if (eachEnnemi.Xposition + imgEnnemi.Width > GraphicsDevice.Viewport.Width)
                    {
                        eachEnnemi.Xposition = -eachEnnemi.Xposition;
                    }

                    else if (eachEnnemi.Yposition < 0)
                    {
                        eachEnnemi.Yposition = -eachEnnemi.Yposition;
                    }
                    else if (eachEnnemi.Yposition + imgEnnemi.Height > GraphicsDevice.Viewport.Height)
                    {
                        eachEnnemi.Yposition = -eachEnnemi.Yposition;
                    }
                }

                
                // Collision check Missile x Wall
                foreach (Missile eachMissiles in listeGestionMissiles)
                {
                    if (eachMissiles.Position.X < 0)
                    {
                        eachMissiles.Tir = false;
                        SuppList.Add(eachMissiles);
                    }
                    if (eachMissiles.Position.X + imgMissile.Width > GraphicsDevice.Viewport.Width)
                    {
                        eachMissiles.Tir = false;
                        SuppList.Add(eachMissiles);
                    }

                    if (eachMissiles.Position.Y < 0)
                    {
                        eachMissiles.Tir = false;
                        SuppList.Add(eachMissiles);
                    }
                    if (eachMissiles.Position.Y + imgMissile.Height > GraphicsDevice.Viewport.Height)
                    {
                        eachMissiles.Tir = false;
                        SuppList.Add(eachMissiles);
                    }
                }


                //Collision check Missile x Ennemi
                foreach (Sprite eachEnnemi in listeEnnemi)
                {
                    foreach (Missile eachMissiles in listeGestionMissiles)
                    {
                        if (eachMissiles.Position.X < eachEnnemi.Xposition + imgEnnemi.Width &&
                                                eachMissiles.Position.X + imgMissile.Width > eachEnnemi.Xposition &&
                                                eachMissiles.Position.Y < eachEnnemi.Yposition + imgEnnemi.Height &&
                                                eachMissiles.Position.Y + imgMissile.Height > eachEnnemi.Yposition)
                        {
                            eachMissiles.Tir = false;
                            SuppList.Add(eachMissiles);

                            List<int> XandY = SetSpawnAtRandomWall(imgEnnemi, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            eachEnnemi.Xposition = XandY[0];
                            eachEnnemi.Yposition = XandY[1];
                            score++;
                            soundEffects[1].Play();
                        }
                    }
                }
                foreach (Missile item in SuppList)
                {
                    listeGestionMissiles.Remove(item);
                }
            }

            if (bvivant == false)
            {
                MediaPlayer.Volume = 0.1f;
                currentMouseState = Mouse.GetState();
                if (
                    (currentMouseState.Position.X < positionDeadMenu.X + imgDeadMenu.Width
                     && currentMouseState.Position.X > positionDeadMenu.X
                     && currentMouseState.Position.Y < positionDeadMenu.Y + imgDeadMenu.Height
                     && currentMouseState.Position.Y > positionDeadMenu.Y)
                     &&
                     !(previousMouseState.Position.X < positionDeadMenu.X + imgDeadMenu.Width
                     && previousMouseState.Position.X > positionDeadMenu.X
                     && previousMouseState.Position.Y < positionDeadMenu.Y + imgDeadMenu.Height
                     && previousMouseState.Position.Y > positionDeadMenu.Y)
                     )
                {
                    soundEffects[2].Play();
                }
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    foreach (Missile eachMissiles in listeGestionMissiles)
                    {
                        eachMissiles.Tir = false;
                        SuppList.Add(eachMissiles);
                    }
                    foreach (Missile item in SuppList)
                    {
                        listeGestionMissiles.Remove(item);
                    }
                    if (currentMouseState.Position.X < positionDeadMenu.X + imgDeadMenu.Width
                        && currentMouseState.Position.X > positionDeadMenu.X
                        && currentMouseState.Position.Y < positionDeadMenu.Y + imgDeadMenu.Height
                        && currentMouseState.Position.Y > positionDeadMenu.Y)

                    {
                        soundEffects[3].Play();
                        foreach (Sprite eachEnnemi in listeEnnemi)
                        {
                            List<int> XandY = SetSpawnAtRandomWall(imgEnnemi, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            eachEnnemi.Xposition = XandY[0];
                            eachEnnemi.Yposition = XandY[1];
                        }
                        lander.velocity = Vector2.Zero;
                        lander.position = new Vector2(GraphicsDevice.Viewport.Width / 2,
                                                      GraphicsDevice.Viewport.Height / 2);
                        lander.angle = 270;
                        score = 0;
                        bvivant = true;
                    }
     
                }
                previousMouseState = currentMouseState;
            }
         
            base.Update(gameTime);
        }
            
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(imgBackground, Vector2.Zero, Color.White);

            if (bStartMenu)
            {
                spriteBatch.Draw(imgStartMenu, positionStartMenu, Color.White);
            }

            if (bvivant && bStartMenu == false)
            {
                Vector2 originImg = new Vector2(lander.img.Width / 2, lander.img.Height / 2);
                spriteBatch.Draw(lander.img, lander.position, null, Color.White,
                    MathHelper.ToRadians(lander.angle), originImg, new Vector2(1, 1), SpriteEffects.None, 0);

                if (lander.engineOn)
                {
                    Vector2 originImgEngine = new Vector2(lander.imgEngine.Width / 2, lander.imgEngine.Height / 2);
                    spriteBatch.Draw(lander.imgEngine, lander.position, null, Color.White,
                        MathHelper.ToRadians(lander.angle), originImgEngine, new Vector2(1, 1), SpriteEffects.None, 0);
                }

                foreach (Sprite item in listeEnnemi)
                {
                    spriteBatch.Draw(imgEnnemi, new Vector2(item.Xposition, item.Yposition), Color.White);
                }


                foreach (Missile eachMissiles in listeGestionMissiles)
                {
                    if (eachMissiles.Tir)
                    {
                        spriteBatch.Draw(imgMissile, eachMissiles.Position, null, Color.White, MathHelper.ToRadians(lander.angle), new Vector2(imgMissile.Width / 2, imgMissile.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0);
                    }
                }
                spriteBatch.DrawString(Font, "Score : " + score, Vector2.Zero, Color.White);
            }

            if (bvivant == false)
            {
                
                spriteBatch.Draw(imgDeadMenu, positionDeadMenu, Color.White);
                spriteBatch.DrawString(Font, "You lost", new Vector2(GraphicsDevice.Viewport.Width / 2 - 18,
                                                                      GraphicsDevice.Viewport.Height / 2 - imgDeadMenu.Height), Color.White);

            }



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
