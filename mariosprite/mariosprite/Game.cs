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

namespace GayMarioGame
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatchMario;
        SpriteBatch spriteBatchTurtleShell;
        SpriteBatch spriteBatchSplash;
        Player mario;
        Menu menu;
        //Enemy shell;
        //bool direction;
        //bool directionchange;
        int elapsedMilliseconds = 0;
        bool directnow = false;
        bool moving = false;
        private bool gamepaused;
        bool jumping = false;
        bool ducking = false;
        bool falling = false;
        double lastupdate;
        double gametime;
        double elapsed = 0;
        Level level;
        enum GameState
        {
            Load,
            StartScreen,
            //MainMenuLoad,
            MainMenu,
            MissionLoad,
            InGame,
            InGameMenu,
            ScoreScreen,
        }
        GameState gameState;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if WINDOWS
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;

#endif
            //graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Ermöglicht dem Spiel, alle Initialisierungen durchzuführen, die es benötigt, bevor die Ausführung gestartet wird.
        /// Hier können erforderliche Dienste abgefragt und alle nicht mit Grafiken
        /// verbundenen Inhalte geladen werden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu

            base.Initialize();
            lastupdate = 0;
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            this.gameState = GameState.Load;
            //TRIAL MODE?
            Guide.SimulateTrialMode = true;

            level = new Level(this, GraphicsDevice);
            level.LoadMap(@"Leveltry");
            level.scrollypos(300);

            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            spriteBatchMario = new SpriteBatch(GraphicsDevice);
            spriteBatchTurtleShell = new SpriteBatch(GraphicsDevice);
            spriteBatchSplash = new SpriteBatch(GraphicsDevice);

            //Menü
            menu = new Menu(this.Content, spriteBatchMario, graphics);

            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Inhalt hier zu laden
            mario = new Player();
            mario.LoadGraphic(1, 3, 60, 35, 8);
            mario.LoadGraphicRight(Content.Load<Texture2D>(@"marioright"));
            mario.LoadGraphicLeft(Content.Load<Texture2D>(@"marioleft"));
            mario.LoadGraphicStanding(Content.Load<Texture2D>(@"mariostanding"));
            mario.LoadGraphicJumping(Content.Load<Texture2D>(@"mariojump"));
            mario.startposy = graphics.GraphicsDevice.Viewport.Height - 100;
            mario.setPosy(mario.startposy);
            mario.startposx = 20;
            mario.setPosx(mario.startposx);
            mario.setGraphics(this.graphics);

            //shell = new Enemy(graphics.GraphicsDevice.Viewport.Width - 20, graphics.GraphicsDevice.Viewport.Height-50);
            //shell.LoadGraphic(1, 2, 15, 15, 2);
            //shell.LoadGraphicLeft(Content.Load<Texture2D>(@"turtleshell"));
            //shell.LoadGraphicRight(Content.Load<Texture2D>(@"turtleshell"));
            //shell.startDirection(false);
            //shell.setGraphics(this.graphics);
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Inhalt hier
        }

        

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            elapsedMilliseconds -= gameTime.ElapsedGameTime.Milliseconds;
            switch (gameState)
            {
                case GameState.Load:
                    gameState = GameState.MainMenu;

                    break;
                case GameState.StartScreen:
                    break;

                case GameState.MissionLoad:
                    break;

                case GameState.MainMenu:

                    if (Math.Abs(gameTime.TotalGameTime.Milliseconds - elapsed) > 400)
                    {
                        if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            menu.Up();
                            elapsed = gameTime.TotalGameTime.Milliseconds;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            menu.Down();
                            elapsed = gameTime.TotalGameTime.Milliseconds;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            gameState = GameState.InGame;
                        }
                    }
                    
                    break;

                case GameState.InGameMenu:

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        if (Math.Abs(gameTime.TotalGameTime.Milliseconds - elapsed) > 280)
                        {
                            StartOver();
                            RemoveSplashScreen(spriteBatchMario);
                            elapsed = gameTime.TotalGameTime.Milliseconds;
                        }
                    }
                    break;

                case GameState.InGame:


                    //falling = false;
                    bool nocollision = false;
                    foreach (Tile tile in level.getTiles())
                    {
                        if (mario.Collideswith(tile.tilePosRect))
                        {
                            if (Math.Abs(tile.posy - mario.getPosy()) > Math.Abs(tile.posx - mario.getPosx()))
                            {
                                if (tile.posy >= mario.getPosy())
                                {
                                    mario.y = new List<int>();
                                    mario.setPosy(tile.posy - tile.tileheight);
                                    nocollision = false;
                                    falling = false;
                                   
                                }
                                else if (tile.posy < mario.getPosy())
                                {
                                    mario.y = new List<int>();
                                    mario.setPosy(tile.posy + tile.tileheight);
                                    nocollision = false;
                                   
                                }
                            }
                            else if (Math.Abs(tile.posy - mario.getPosy()) < Math.Abs(tile.posx - mario.getPosx()))
                            {
                                if (tile.posx >= mario.getPosx())
                                {
                                    mario.x = new List<int>();
                                    mario.setPosx(tile.posx - tile.tilewidth );
                                    nocollision = false;
                                   
                                }
                                else if (tile.posx < mario.getPosx())
                                {
                                    mario.x = new List<int>();
                                    mario.setPosx(tile.posx + tile.tilewidth );
                                    nocollision = false;
                                   
                                }
                            }
                            if (!nocollision)
                                break;
                    

                        }
                        else
                        {
                        //mario.y = new List<int>();
                            nocollision = true;
                
                        }      
                    }
                    if (nocollision)
                    {
                        if (!jumping)
                        {
                            mario.setPosy(mario.getPosy() + 10);
                            falling = true;
                        }
                    }
                    // Ermöglicht ein Beenden des Spiels
                    if (Math.Abs(gameTime.TotalGameTime.Milliseconds - elapsed) > 280)
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        elapsed = gameTime.TotalGameTime.Milliseconds;
                        if (gameState == GameState.InGame)
                        {
                            gameState = GameState.InGameMenu;
                        }
                        else
                        {
                            gameState = GameState.MainMenu;
                        }
                    }    

                    if (!gamepaused)
                    {

                        if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            Left();

                        }
                        if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            Right();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            if (!falling)
                                jumping = Jump(jumping);
                        }
                        else
                        {
                            if (mario.y.Count == 0)
                            {
                                jumping = false;
                                falling = true;
                            }
                            elapsedMilliseconds = 0;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            //ducking = Duck();
                        }
                    }

                    //Mario sprite movement
                    if (jumping)
                        falling = false;
                    mario.Update((float)gameTime.ElapsedGameTime.TotalSeconds, directnow, moving, jumping);
                    moving = false;

                    //Turtle shell movement
                    //shell.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    ////Collision detection
                    //if (mario.Collideswith(shell))
                    //{
                    //    if (mario.getPosy() >= shell.getPosy() + 5)
                    //    {
                    //        shell.kill(mario);
                    //    }
                    //    else
                    //    {
                    //        mario.kill();
                    //        shell.kill();
                    //        //lifelost = true;
                    //        gamepaused = true;
                    //    }
                    //}

                    mario.updatePosition();

                    

                    break;

            }

            base.Update(gameTime);

        }

        private void Left()
        {
            if (mario.getPosx() < graphics.GraphicsDevice.Viewport.Width / 3)
            {
                if (Math.Abs(gametime - lastupdate) >10)
                {
                    level.scroll(true,5);
                    lastupdate = gametime;
                }
            }
            else
                mario.x.Add(-5);
            directnow = false;
            moving = true;
        }

        private void Right()
        {
            if (mario.getPosx() > graphics.GraphicsDevice.Viewport.Width / 3 * 2)
            {
                if (Math.Abs(gametime - lastupdate) > 10)
                {
                    level.scroll(false,5);
                    lastupdate = gametime;
                }
            }
            else mario.x.Add(5);
            directnow = true;
            moving = true;
        }

        private bool Jump(bool jumping)
        {
            if (elapsedMilliseconds <= 0)
            {
                var intlist = new int[] { -10, -15, -15, -20, -20, -30, -20, -20, -5, -2, -1 };//,0,0,0,1,2,5,5,5,5,5,20,30,20,20,15,15,10};
                foreach (var integ in intlist)
                    mario.y.Add(integ);
                jumping = true;
                elapsedMilliseconds = 600;
            }
            return jumping;
        }

        public void StartOver()
        {
            //initialize all objects back at startpoint, directions, etc...
            //shell.startDirection(false);
            gamepaused = false;
        }

        public bool Duck()
        {
            return true;
        }

        public void ShowSplashScreen(SpriteBatch spriteBatch, Texture2D splashScreen)
        {
            spriteBatch.Draw(splashScreen, new Vector2(0, 0), Color.White);
        }

        public void RemoveSplashScreen(SpriteBatch spriteBatch)
        {
            gameState = GameState.InGame;
        }


        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu
            graphics.GraphicsDevice.Clear(Color.White);

            //check if movement list is empty and set default to no movement (0)
            

            //if (shell.x.Count == 0)
            //    shell.x.Add(0);
            //if (shell.y.Count == 0)
            //    shell.y.Add(0);


            //character in screen?/visible?
            //TODO: ...

            

            //shell.posx = shell.posx + shell.x[0];
            //shell.posy = shell.posy + shell.y[0];

                        //level.scroll(false, 1);

            //Draw mario
            spriteBatchMario.Begin();
            switch (gameState)
            {
                case GameState.InGame:
                    level.drawMap(spriteBatchMario);
                    mario.Draw(spriteBatchMario, new Vector2(mario.getPosx(), mario.getPosy()), Color.White);
                    break;
                case GameState.MainMenu:
                    menu.Draw(gameTime);
                    break;
                case GameState.InGameMenu:             
                    ShowSplashScreen(spriteBatchMario, Content.Load<Texture2D>(@"splashscreen"));
                    break;
            }
            spriteBatchMario.End();
            

            //Remove elements of list for movement of character
            //mario.x.RemoveAt(0);
            //mario.y.RemoveAt(0);

            //TurleShell Draw
            //if (shell.x[0] != -999)
            //{
            //    spriteBatchTurtleShell.Begin();
            //    shell.Draw(spriteBatchTurtleShell, new Vector2(shell.posx, shell.posy), Color.LightBlue);
            //    spriteBatchTurtleShell.End();

            //    //Remove elements of list for movement of enemy
            //    shell.x.RemoveAt(0);
            //    shell.y.RemoveAt(0);
            //}

            //if (Guide.IsTrialMode)
            //{
            //    SignedInGamer signedInGamer;
            //    Guide.ShowMarketplace(signedInGamer.PlayerIndex);
            //}
            gametime = gameTime.TotalGameTime.Milliseconds;
            base.Draw(gameTime);
        }
    }
}


