using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using System;

namespace Engine.Components
{
    interface IDrawManager
    {
        void AddLayer(string layerName);
        Layer this[string LayerName] { get; }
        int NumberOfSprites { get; }
        int VisibleSprites { get; }
    }


    public class DrawManager : DrawableGameComponent, IDrawManager
    {
        List<Layer> layers = new List<Layer>();

        ICamera2D camera;
        SpriteBatch spriteBatch;
 
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

        public DrawManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IDrawManager), this); 

            // Add default layers
            AddLayer("Background");
            AddLayer("Player");
            AddLayer("Foreground");
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
        /// <param name="actionName"></param>
        public void AddLayer(string layerName)
        {
            layers.Add(new Layer(layerName));
        }


        public override void Update(GameTime gameTime)
        {
            foreach (Layer layer in layers)
            {
                layer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
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

    }
}
