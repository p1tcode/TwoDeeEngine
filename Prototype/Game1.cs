using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Engine;
using Engine.Objects;
using Engine.Components;
using System;
using System.Threading;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Prototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ObjectManager objectManager;
        InputManger inputManager;
        ParticleManager particleManager;
        Camera2D camera;

        World physicsWorld;

        Sprite sprite;
        Sprite fallingSprite;
        Animation player;

        Text debugText;

        FpsCounter fps;

        TimeSpan elapsedTime;
        bool on = true;

        Random rand = new Random();

        ParticleEmitter emitter;

        int speed;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

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

            objectManager = new ObjectManager(this);
            Components.Add(objectManager);

            inputManager = new InputManger(this);
            Components.Add(inputManager);

            particleManager = new ParticleManager(this);
            Components.Add(particleManager);
            particleManager.AddParticleEffect("Smoke");

            fps = new FpsCounter(this, Content.Load<SpriteFont>(@"default"));
            Components.Add(fps);

            physicsWorld = new World(new Vector2(0, 250));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D tex = Content.Load<Texture2D>(@"crate");
            Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
            sprite = new Sprite(Content.Load<Texture2D>(@"crate"), body);
            sprite.Body.BodyType = BodyType.Static;
            sprite.Position = new Vector2(200, 200);
            sprite.Color = Color.Red;
            objectManager["Player"].Add(sprite);


            tex = Content.Load<Texture2D>(@"crate");
            body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
            fallingSprite = new Sprite(tex, body);
            fallingSprite.Position = new Vector2(500, 200);
            fallingSprite.Body.BodyType = BodyType.Dynamic;
            objectManager["Player"].Add(fallingSprite);

            //PhysicsSprite fallingSprite2;
            tex = Content.Load<Texture2D>(@"crate");
            body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
            Sprite fallingSprite2 = new Sprite(tex, body);
            fallingSprite2.Position = new Vector2(440, 480);
            fallingSprite2.Body.BodyType = BodyType.Static;
            fallingSprite2.Body.Friction = 100;
            objectManager["Player"].Add(fallingSprite2);


            player = new Animation(Content.Load<Texture2D>(@"alice"), 64, 64, 0.2f);
            objectManager["Player"].Add(player);
            player.Position = new Vector2(500, 500);

            debugText = new Text(Content.Load<SpriteFont>(@"default"));
            debugText.Color = Color.Red;
            debugText.Position = new Vector2(10, 45);
            objectManager["Text"].AddText(debugText);

            inputManager.AddAction("MoveItem");
            inputManager["MoveItem"].Add(MouseButtons.Left);

            inputManager.AddAction("MouseRightClick");
            inputManager["MouseRightClick"].Add(MouseButtons.Right);

           
            emitter = new ParticleEmitter(Vector2.Zero, 1000, "Background", true);
            particleManager["Smoke"].AddEmitter(emitter);
            particleManager["Smoke"].MinInitialSpeed = 5;
            particleManager["Smoke"].MaxInitialSpeed = 10;
            particleManager["Smoke"].MinGrowth = 0.2f;
            particleManager["Smoke"].MaxGrowth = 0.5f;
            particleManager["Smoke"].Color = Color.SlateGray;
            particleManager["Smoke"].MinTTL = 4f;
            particleManager["Smoke"].MaxTTL = 6f;
            particleManager["Smoke"].MinAlpha = 0.2f;
            particleManager["Smoke"].MaxAlpha = 1f;
            particleManager["Smoke"].MinSize = 0.5f;
            particleManager["Smoke"].MaxSize = 0.7f;
            particleManager["Smoke"].Acceleration = new Vector2(0, 9);
            

            objectManager.DrawLine("Debug", new Vector2(400, 400), new Vector2(600, 500), Color.Red, 2);

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

            physicsWorld.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

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
            if (inputManager["MoveItem"].IsClicked)
            {
                Texture2D tex = Content.Load<Texture2D>(@"crate");
                Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
                fallingSprite = new Sprite(tex, body);
                fallingSprite.Position = inputManager.MouseWorldPosition;
                fallingSprite.Body.Friction = 100;
                fallingSprite.Body.BodyType = BodyType.Dynamic;
                objectManager["Player"].Add(fallingSprite);
            }
            inputManager.MouseGrabSprite(sprite, "MoveItem");
            inputManager.MouseGrabWorld("MouseRightClick");

            if (inputManager["MoveItem"].IsDown)
            {
                emitter.Active = true;
            }
            else
            {
                emitter.Active = false;
            }
 
            sprite.Position += inputManager.LeftStick(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            sprite.Position += inputManager.LeftStick(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            //sprite.Position += inputManager.LeftTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            //sprite.Position -= inputManager.RightTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            #endregion

            //camera.SetTarget(sprite.Position);

            emitter.Position = inputManager.MouseWorldPosition;

            debugText.OutputText = String.Format("Sprites: {0} \nMousePos: {1}, {2} \nVisibleSprites: {3} \nScrollWheelValue: {4} \nNumberOfParticleEffects: {5} \nNumberOfEmitters: {6}, \nNumbersOfParticles: {7}, \nNumberOfFreeParticles: {8}",
                                                    objectManager.NumberOfSprites.ToString(),
                                                    inputManager.MousePosition.X,
                                                    inputManager.MousePosition.Y,
                                                    objectManager.VisibleSprites.ToString(),
                                                    inputManager.ScrollWheelValue.ToString(),
                                                    particleManager.NumberOfEffects.ToString(),
                                                    particleManager.TotalEmitters.ToString(),
                                                    emitter.NumbersOfParticles.ToString(),
                                                    emitter.NumbersOfFreeParticles.ToString());
            
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
