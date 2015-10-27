using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using System;


namespace Engine.Components
{
    interface IObjectManager
    {
        void AddLayer(string layerName);
        Layer this[string LayerName] { get; }
        int NumberOfSprites { get; }
        int VisibleSprites { get; }
    }


    public class ObjectManager : DrawableGameComponent, IObjectManager
    {
        List<Layer> layers = new List<Layer>();

        ICamera2D camera;
        SpriteBatch spriteBatch;
        Texture2D pixel;

        #region Properties
        int numberOfSprites = 0;
        public int NumberOfSprites
        {
            get
            {
                numberOfSprites = 0;
                foreach (Layer layer in layers)
                {
                    numberOfSprites += layer.NumberOfSprites;
                }
                return numberOfSprites;
            }
        }

        int visibleSprites = 0;
        public int VisibleSprites
        {
            get
            {
                visibleSprites = 0;
                foreach (Layer layer in layers)
                {
                    visibleSprites += layer.VisibleSprites;
                }
                return visibleSprites;
            }
        }
        #endregion

        public ObjectManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IObjectManager), this); 

            // Add default layers
            AddLayer("Background");
            AddLayer("Player");
            AddLayer("Foreground");
            AddLayer("Debug");
            AddLayer("Text");

        }

        public override void Initialize()
        {
            // Here is a good place to grab services.
            this.camera = (ICamera2D)Game.Services.GetService(typeof(ICamera2D));
            this.spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            base.Initialize();
        }

        /// <summary>
        /// List of layers
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public Layer this[string LayerName]
        {
            get
            {
                return layers.Find((Layer a) => { return a.Name == LayerName; });
            }
        }


        /// <summary>
        /// Adds an layer to the LayerManager
        /// </summary>
        /// <param name="LayerName"></param>
        public void AddLayer(string layerName)
        {
            layers.Add(new Layer(layerName));
        }


        public override void Update(GameTime gameTime)
        {


            foreach (Layer layer in layers)
            {
                layer.Update(gameTime);
            }

            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Layer layer in layers)
            {
                layer.Draw(spriteBatch, (Camera2D)camera);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public void DrawLine(String Layer, Vector2 point1, Vector2 point2, Color color, float thickness)
        {
            
            Sprite line;

            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            
            line = new Sprite(pixel);

            // calculate the distance between the two vectors
            float distance = Vector2.Distance(point1, point2);
            // calculate the angle between the two vectors
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            line.Position = point1;
            line.Origin = Vector2.Zero;
            line.Scale = new Vector2(distance, thickness);
            line.Rotation = angle;
            line.Color = color;

            this[Layer].Add(line);                
        }

    }
}
