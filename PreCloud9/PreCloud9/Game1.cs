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
using GameStructure;

namespace PreCloud9
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public struct PlayerData //still no use of the struct. Uses only the Vector2 position 
    {
        public Vector2 Position;
        public bool IsAlive;
        public Color Color;
        public float Angle;
        public float Power;
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        GameManager gm;

        Texture2D backgroundTile;
        Texture2D BricksBox;
        Texture2D StoneBox;
        Texture2D WaterBox;
        Texture2D CoinBox;
        Texture2D LifePackBox;
        Texture2D TankImage;
        Texture2D BulletImage;
        Texture2D sideView;

        SpriteFont font;

        int screenWidth;
        int screenHeight;
        int unitSize;
        float playerScalling;
        Color[] playerColors = new Color[5];

        Vector2 bulletPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            gm = new GameManager();
            //if (gm.state == true)
            //{
            //    gm.automateGammingTimer();
            //}
            if (GameManager.AI_State)
            {
                Console.WriteLine("Here the AI method is calling.................................AIAIAIAIA");
                gm.startAI();
            }
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 950;
            graphics.PreferredBackBufferHeight = 650;
            //unitSize = screenWidth / gm.gridSize;          
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Cloud 9 games alpha 4.2";
            playerScalling = 0.5f;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            backgroundTile = Content.Load<Texture2D>("Tile");
            BricksBox = Content.Load<Texture2D>("Bricks2XNA");
            StoneBox = Content.Load<Texture2D>("StoneXNA");
            WaterBox = Content.Load<Texture2D>("waterXNA");
            LifePackBox = Content.Load<Texture2D>("LifePackXNA");
            CoinBox = Content.Load<Texture2D>("CoinXNA");
            TankImage = Content.Load<Texture2D>("tank_XNA");
            BulletImage = Content.Load<Texture2D>("bulletXNA");
            font = Content.Load<SpriteFont>("myFont");
            sideView = Content.Load<Texture2D>("sideview");

            initializeColors();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            processKeyBoard();
            updateBullet();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            drawBackGroundTiles();
            drawSideView();
            drawMAP();
            drawMytank();
            drawBullet();
            drawText();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawMytank()
        {
            //Vector2 Position1 = new Vector2(gm.gEngine.myTank.Xcod * unitSize + unitSize / 2, gm.gEngine.myTank.Ycod * unitSize + unitSize / 2);
            //spriteBatch.Draw(TankImage, Position, Color.White);
            //spriteBatch.Draw(TankImage, Position1, null, Color.LightBlue, MathHelper.ToRadians(90 * gm.gEngine.myTank.Direction), new Vector2(TankImage.Width / 2, TankImage.Height / 2), playerScalling, SpriteEffects.None, 0);
            List<Tank> tanklist = gm.gEngine.tankList;
            for (int i = 0; i < tanklist.Count; i++)
            {
                Vector2 position = new Vector2(tanklist[i].Xcod * unitSize + unitSize / 2, tanklist[i].Ycod * unitSize + unitSize / 2);
                spriteBatch.Draw(TankImage, position, null, playerColors[i], MathHelper.ToRadians(90 * tanklist[i].Direction), new Vector2(TankImage.Width / 2, TankImage.Height / 2), playerScalling, SpriteEffects.None, 0);
                tanksTakingLPorCoins(tanklist[i].Xcod, tanklist[i].Ycod);
            }            
        }

        private void tanksTakingLPorCoins(int X,int Y)
        {
            if (gm.gEngine.map[Y,X].Equals("C") || gm.gEngine.map[Y,X].Equals("L"))
            {
                gm.gEngine.map[Y,X] = "N";
            }
        }

        private void updateBullet(){
            if (gm.gEngine.blt.State)
            {
                Console.WriteLine("Inside updateBullet method......................");
                if (gm.gEngine.blt.Angel == 0)
                {
                    //gm.gEngine.blt.Ycod -= unitSize;
                    bulletPosition.Y -= 10;
                }
                else if (gm.gEngine.blt.Angel == 1)
                {
                    //gm.gEngine.blt.Xcod += unitSize;
                    bulletPosition.X += 10;
                }
                else if (gm.gEngine.blt.Angel == 2)
                {
                    //gm.gEngine.blt.Ycod += unitSize;
                    bulletPosition.Y += 10;
                }
                else if (gm.gEngine.blt.Angel == 3)
                {
                    //gm.gEngine.blt.Xcod += unitSize;
                    bulletPosition.X -= 10;
                }
                Console.WriteLine("bullet XCod  : " + bulletPosition.X);
                Console.WriteLine("bullet YCod  : " + bulletPosition.Y);
                if (bulletPosition.X > 650 || bulletPosition.Y > 650 || bulletPosition.X < 0 || bulletPosition.Y<0)
                {
                    Console.WriteLine("Bullet state changes here............................");
                    gm.gEngine.blt.State = false;              
                }
                
            }
        }

        private void drawBullet()
        {
            if (gm.gEngine.blt.State)
            {
                //Vector2 pos = new Vector2(gm.gEngine.myTank.Xcod * unitSize + unitSize / 2, gm.gEngine.myTank.Ycod * unitSize + unitSize / 2);
                spriteBatch.Draw(BulletImage, bulletPosition, null, Color.Red, MathHelper.ToRadians(90 * gm.gEngine.blt.Angel), new Vector2(BulletImage.Width / 2, BulletImage.Height / 2), playerScalling, SpriteEffects.None, 0);
                Console.WriteLine("Here drawing the bullet.....................................................................");
            }
        }

        private void drawBackGroundTiles()
        {
            //screenWidth = device.PresentationParameters.BackBufferWidth;
            //screenHeight = device.PresentationParameters.BackBufferHeight;
            screenWidth = 650;
            screenHeight = 650;
            unitSize = screenWidth / gm.gEngine.gridSize;
            for (int i = 0; i < screenHeight; i+=unitSize)
            {
                for (int j = 0; j < screenWidth; j+=unitSize)
                {
                    Rectangle unitRectangle = new Rectangle(j,i, unitSize, unitSize);
                    spriteBatch.Draw(backgroundTile, unitRectangle, Color.White);
                }           
            }
        }

        private void drawSideView()
        {
            Rectangle sideRectange = new Rectangle(650, 0, 300, 650);
            spriteBatch.Draw(sideView, sideRectange, Color.White);
        }

        private void drawMAP()
        {
            String[,] map = gm.gEngine.map;
            int xcod = 0;
            int ycod = 0;
            for (int i = 0; i < gm.gEngine.gridSize; i++)
            {
                ycod = i*unitSize;
                for (int j = 0; j < gm.gEngine.gridSize; j++)
                {                   
                    if (!(map[i, j].Equals("N")))
                    {
                        xcod = j*unitSize;
                        Rectangle imageRectangle = new Rectangle(xcod+2, ycod+2, unitSize-4, unitSize-4);
                        if (map[i, j].Equals("B"))
                        {
                            spriteBatch.Draw(BricksBox, imageRectangle, Color.White);
                        }
                        if (map[i, j].Equals("S"))
                        {
                            spriteBatch.Draw(StoneBox, imageRectangle, Color.White);
                        }
                        if (map[i, j].Equals("W"))
                        {
                            spriteBatch.Draw(WaterBox, imageRectangle, Color.White);
                        }
                        if (map[i, j].Equals("L"))
                        {
                            spriteBatch.Draw(LifePackBox, imageRectangle, Color.White);
                        }
                        if (map[i, j].Equals("C"))
                        {
                            spriteBatch.Draw(CoinBox, imageRectangle, Color.White);
                        }
                    }
                }
            }
        }

        private void drawText()
        {
            //for (int i = 0; i < gm.gEngine.tankList.Count; i++)
            //{
            //    spriteBatch.DrawString(font, "P " + i + "       " + gm.gEngine.tankList[i].Points + "       " + gm.gEngine.tankList[i].Coins + "      " + gm.gEngine.tankList[i].Health, new Vector2(670, 250 + (i * 20)), Color.Black);
            //}
            spriteBatch.DrawString(font, "P", new Vector2(725, 200), Color.YellowGreen);
            spriteBatch.DrawString(font, "C", new Vector2(800, 200), Color.YellowGreen);
            spriteBatch.DrawString(font, "H", new Vector2(875, 200), Color.YellowGreen);

            for (int i = 0; i < gm.gEngine.tankList.Count; i++)
            {
                if (gm.gEngine.tankList[i].PlayerName.Equals(gm.gEngine.myTank.PlayerName))
                {
                    //Console.WriteLine("My Tank Identifies.......................................................");
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].PlayerName + "", new Vector2(670, 250 + (i * 20)), Color.OrangeRed);
                }
                else
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].PlayerName + "", new Vector2(670, 250 + (i * 20)), Color.YellowGreen);
                }
                
            }

            
            for (int i = 0; i < gm.gEngine.tankList.Count; i++)
            {
                if (gm.gEngine.tankList[i].PlayerName.Equals(gm.gEngine.myTank.PlayerName))
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Points + "", new Vector2(725, 250 + (i * 20)), Color.Orange);
                }
                else
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Points + "", new Vector2(725, 250 + (i * 20)), Color.Yellow);
                }
            }

            
            for (int i = 0; i < gm.gEngine.tankList.Count; i++)
            {
                if (gm.gEngine.tankList[i].PlayerName.Equals(gm.gEngine.myTank.PlayerName))
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Coins + "", new Vector2(800, 250 + (i * 20)), Color.Orange);
                }
                else
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Coins + "", new Vector2(800, 250 + (i * 20)), Color.Yellow);
                }
            }

            
            for (int i = 0; i < gm.gEngine.tankList.Count; i++)
            {
                if (gm.gEngine.tankList[i].PlayerName.Equals(gm.gEngine.myTank.PlayerName))
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Health + "", new Vector2(875, 250 + (i * 20)), Color.Orange);
                }
                else
                {
                    spriteBatch.DrawString(font, gm.gEngine.tankList[i].Health + "", new Vector2(875, 250 + (i * 20)), Color.Yellow);
                }
            }
                
        }


        

        private void processKeyBoard()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Up))
            {
                gm.gEngine.con.sendDatatoServer("UP#");
            }
            else if (keybState.IsKeyDown(Keys.Right))
            {
                gm.gEngine.con.sendDatatoServer("RIGHT#");
            }
            else if (keybState.IsKeyDown(Keys.Down))
            {
                gm.gEngine.con.sendDatatoServer("DOWN#");
            }
            else if (keybState.IsKeyDown(Keys.Left))
            {
                gm.gEngine.con.sendDatatoServer("LEFT#");
            }
            else if(keybState.IsKeyDown(Keys.Space)){
                if (!gm.gEngine.blt.State)
                {
                    Console.WriteLine("Calling for shooting.......");
                    gm.gEngine.con.sendDatatoServer("SHOOT#");
                    bulletPosition = new Vector2(gm.gEngine.myTank.Xcod * unitSize + unitSize / 2, gm.gEngine.myTank.Ycod * unitSize + unitSize / 2);
                    gm.gEngine.blt.State = true;
                    //need to call a method to draw the bullet.
                }              
            }
            //else if (keybState.IsKeyDown(Keys.A))
            //{
            //    if (!(gm.state))
            //    {
            //        gm.state = true;
            //        gm.startAI();
            //    }
            //}
        }

        private void initializeColors()
        {
            this.playerColors[0] = Color.LightGreen;
            this.playerColors[1] = Color.LightPink;
            this.playerColors[2] = Color.Gold;
            this.playerColors[3] = Color.LightSkyBlue;
            this.playerColors[4] = Color.LightSalmon;
        }
    }
}
