using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Engine;
using Engine.Objects;
using Engine.Components;
using Engine.Helpers;
using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;

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
        ScreenManager screenManager;
        Camera2D camera;
        World physicsWorld;

        Sprite fallingSprite;
        

        Text debugText;

        FpsCounter fps;

        TimeSpan elapsedTime;

        Random rand = new Random();

        ParticleEmitter emitterSmoke;
        ParticleEmitter emitterSpark;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);

            graphics.IsFullScreen = false;

            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = 2560;
                graphics.PreferredBackBufferHeight = 1440;
                //graphics.PreferredBackBufferWidth = 1920;
                //graphics.PreferredBackBufferHeight = 1080;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
            }
            
            
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;

            IsMouseVisible = true;

            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

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
            PreFabs.LoadContent_ParticleEffects(particleManager);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);


            fps = new FpsCounter(this, Content.Load<SpriteFont>(@"default"));
            Components.Add(fps);

            physicsWorld = new World(new Vector2(0, 980f));
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D tex = Content.Load<Texture2D>(@"crate");

            Body bodyDynamic = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
            fallingSprite = new Sprite(tex, bodyDynamic);
            fallingSprite.Position = new Vector2(500, 200);
            fallingSprite.Body.BodyType = BodyType.Dynamic;
            objectManager["Player"].Add(fallingSprite);

            //PhysicsSprite fallingSprite2;
            tex = Content.Load<Texture2D>(@"crate");
            
            for (int i = 0; i < 40; i++)
            {
                Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(64*i + 32, 1048);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                objectManager["Player"].Add(staticSprite);
            }
            for (int i = 0; i < 25; i++)
            {
                Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(32, 1048- 64 * i);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                objectManager["Player"].Add(staticSprite);
            }
            for (int i = 0; i < 25; i++)
            {
                Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
                Sprite staticSprite = new Sprite(tex, body);
                staticSprite.Position = new Vector2(29*64 + 32, 1048 - 64 * i);
                staticSprite.Body.BodyType = BodyType.Static;
                staticSprite.Body.Friction = 1f;
                objectManager["Player"].Add(staticSprite);
            }


            debugText = new Text(Content.Load<SpriteFont>(@"default"));
            debugText.Color = Color.Red;
            debugText.Position = new Vector2(10, 45);
            objectManager["Text"].AddText(debugText);

            inputManager.AddAction("MouseTrigger");
            inputManager["MouseTrigger"].Add(MouseButtons.Left);

            inputManager.AddAction("MouseRightClick");
            //inputManager["MouseRightClick"].Add(MouseButtons.Right);
            inputManager["MouseRightClick"].Add(Keys.Space);

            PreFabs.Initialize_ParticleEffects(particleManager);
            emitterSmoke = new ParticleEmitter(Vector2.Zero, 1, "Foreground", true, false);
            particleManager["Smoke"].AddEmitter(emitterSmoke);
            emitterSpark = new ParticleEmitter(Vector2.Zero, 1, "Foreground", true, false);
            particleManager["Spark"].AddEmitter(emitterSpark);
            

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

            physicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            #region Input

            if (inputManager["MouseTrigger"].IsClicked)
            {
                Texture2D tex = Content.Load<Texture2D>(@"crate");
                Body body = BodyFactory.CreateRectangle(physicsWorld, tex.Width, tex.Height, 1);
                fallingSprite = new Sprite(tex, body);
                fallingSprite.Position = inputManager.MouseWorldPosition;
                fallingSprite.Body.Friction = 1f;
                fallingSprite.Body.BodyType = BodyType.Dynamic;
                objectManager["Player"].Add(fallingSprite);
            }

            


            inputManager.MouseGrabWorld("MouseRightClick");

            //emitterSmoke.Position = inputManager.MouseWorldPosition;
            //emitterSpark.Position = inputManager.MouseWorldPosition;

            
            foreach (var item in physicsWorld.ContactList)
            {
                Vector2 normal;
                FixedArray2<Vector2> worldPoints;
                

                item.GetWorldManifold(out normal, out worldPoints);

                emitterSmoke.Position = worldPoints[0];
                emitterSpark.Position = worldPoints[0];

                if ((item.FixtureA.Body.LinearVelocity.X > 1f) ||
                    (item.FixtureA.Body.LinearVelocity.X < -1f) ||
                    (item.FixtureA.Body.LinearVelocity.Y > 1f) ||
                    (item.FixtureA.Body.LinearVelocity.Y < -1f))
                    
                {
                    emitterSmoke.Trigger(0.01f);
                }
                if ((item.FixtureA.Body.LinearVelocity.X > 0.5f) ||
                    (item.FixtureA.Body.LinearVelocity.X < -0.5f) ||
                    (item.FixtureA.Body.LinearVelocity.Y > 0.5f) ||
                    (item.FixtureA.Body.LinearVelocity.Y < -0.5f) ||
                    (item.FixtureA.Body.AngularVelocity > 0.5f) ||
                    (item.FixtureA.Body.AngularVelocity < -0.5f))

                {
                    emitterSpark.Trigger(0.2f);
                }


            }
            /*
            if (inputManager["MouseTrigger"].IsDown)
            {
                emitterSmoke.Active = true;
                emitterSpark.Active = true;
            }
            else
            {
                emitterSmoke.Active = false;
                emitterSpark.Active = false;
            }*/
 
            //sprite.Position += inputManager.LeftTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            //sprite.Position -= inputManager.RightTrigger(PlayerIndex.One) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            #endregion

            //camera.SetTarget(sprite.Position);

            

            debugText.OutputText = String.Format("Sprites: {0} \nMousePos: {1}, {2} \nVisibleSprites: {3} \nScrollWheelValue: {4} \nNumberOfParticleEffects: {5} \nNumberOfEmitters: {6}, \nNumbersOfParticles: {7}, \nNumberOfFreeParticles: {8}",
                                                    objectManager.NumberOfSprites.ToString(),
                                                    inputManager.MousePosition.X,
                                                    inputManager.MousePosition.Y,
                                                    objectManager.VisibleSprites.ToString(),
                                                    inputManager.ScrollWheelValue.ToString(),
                                                    particleManager.NumberOfEffects.ToString(),
                                                    particleManager.TotalEmitters.ToString(),
                                                    emitterSmoke.NumbersOfParticles.ToString(),
                                                    emitterSmoke.NumbersOfFreeParticles.ToString());
            
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
