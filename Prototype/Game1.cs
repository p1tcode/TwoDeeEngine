﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Engine;
using Engine.Objects;
using Engine.Components;
using System;
using System.Threading;

namespace Prototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawManager drawManager;
        InputManger inputManager;
        ParticleManager particleManager;
        Camera2D camera;

        Sprite sprite;
        Animation player;

        Sprite spriteDebug;
        Text debugText;

        FpsCounter fps;

        TimeSpan elapsedTime;
        bool on = true;

        Random rand = new Random();

        int speed;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsMouseVisible = true;

            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
                        
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);   

            camera = new Camera2D(this, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            camera.JumpToTarget(camera.Origin);
            Components.Add(camera);

            drawManager = new DrawManager(this);
            Components.Add(drawManager);

            inputManager = new InputManger(this);
            Components.Add(inputManager);

            particleManager = new ParticleManager(this);
            Components.Add(particleManager);


            fps = new FpsCounter(this, Content.Load<SpriteFont>(@"default"));
            Components.Add(fps);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            sprite = new Sprite(Content.Load<Texture2D>(@"crate"));
            sprite.Position = new Vector2(200, 200);
            sprite.Color = Color.Red;
            drawManager["Player"].Add(sprite);

            player = new Animation(Content.Load<Texture2D>(@"alice"), 64, 64, 0.2f);
            drawManager["Player"].Add(player);
            player.Position = new Vector2(500, 500);

            spriteDebug = new Sprite(Content.Load<Texture2D>(@"crate"));
            spriteDebug.Position = Vector2.Zero;
            spriteDebug.Color = Color.Aqua;
            drawManager["Foreground"].Add(spriteDebug);

            debugText = new Text(Content.Load<SpriteFont>(@"default"));
            debugText.Color = Color.Red;
            debugText.Position = new Vector2(10, 45);
            drawManager["Text"].AddText(debugText);

            inputManager.AddAction("MoveItem");
            inputManager["MoveItem"].Add(MouseButtons.Left);

            inputManager.AddAction("MouseRightClick");
            inputManager["MouseRightClick"].Add(MouseButtons.Right);

            particleManager["Fire"].AddEmitter(new Vector2(200, 200), "Player", true);
            particleManager["Fire"].AddEmitter(new Vector2(300, 300), "Player", true);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            elapsedTime += gameTime.ElapsedGameTime;

            #region Blinking Crate
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                if (on)
                {
                    sprite.Color = Color.Blue;
                    on = false;
                }
                else
                {
                    sprite.Color = Color.Red;
                    on = true;
                }
                elapsedTime -= TimeSpan.FromSeconds(1);
            }
            #endregion

            #region Input
            speed = 150;
            if (inputManager["Down"].IsDown)
                sprite.Position += new Vector2(0, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);    
            if (inputManager["Up"].IsDown)
                sprite.Position += new Vector2(0, -speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (inputManager["Left"].IsDown)
                sprite.Position += new Vector2(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            if (inputManager["Right"].IsDown)
                sprite.Position += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            inputManager.MouseGrabSprite(sprite, "MoveItem");
            inputManager.MouseGrabWorld("MouseRightClick");
 
            sprite.Position.X += inputManager.LeftStick(PlayerIndex.One).X * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            sprite.Position.Y += inputManager.LeftStick(PlayerIndex.One).Y * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            sprite.Position.Y += inputManager.LeftTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            sprite.Position.Y -= inputManager.RightTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            #endregion

            //camera.SetTarget(sprite.Position);

            debugText.OutputText = String.Format("Sprites: {0} \nMousePos: {1}, {2} \nVisibleSprites: {3} \nScrollWheelValue: {4} \nNumberOfParticleEffects: {5} \nNumberOfEmitters: {6}",
                                                    drawManager.NumberOfSprites.ToString(),
                                                    inputManager.MousePosition.X,
                                                    inputManager.MousePosition.Y,
                                                    drawManager.VisibleSprites.ToString(),
                                                    inputManager.ScrollWheelValue.ToString(),
                                                    particleManager.NumberOfEffects.ToString(),
                                                    particleManager.TotalEmitters.ToString());
            
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

            base.Draw(gameTime);

        }
    }
}
