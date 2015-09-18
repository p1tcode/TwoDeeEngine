using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Engine.Components;

namespace Engine.Objects
{
    public class ParticleEffect
    {
        List<ParticleEmitter> emitters = new List<ParticleEmitter>();

        string name;
        ContentManager content;
        Game game;

        Texture2D texture;
        public Texture2D Texture
        {
            set
            {
                this.texture = value;
            }
        }

        int numberOfEmitters = 0;
        public int NumberOfEmitters
        {
            get
            {
                numberOfEmitters = 0;
                foreach (ParticleEmitter emitter in emitters)
                {
                    numberOfEmitters++;
                }
                return numberOfEmitters;
            }
        }

        /// <summary>
        /// Used to loop the particle effect or only emit once.
        /// </summary>
        public bool Loop { get; set; }

        public float MinInitialSpeed { get; set; }
        public float MaxInitialSpeed { get; set; }

        public float MinSize { get; set; }
        public float MaxSize { get; set; }

        public float MaxAlpha { get; set; }
        public float MinAlpha { get; set; }

        public float MinDirection { get; set; }
        public float MaxDirection { get; set; }

        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }

        public Color Color { get; set; } 
        
        /// <summary>
        /// Minimum time for a particle to live
        /// </summary>
        public float MinTTL { get; set; }
        /// <summary>
        /// Maximum time for a particle to live
        /// </summary>
        public float MaxTTL { get; set; }

        public int numberOfParticles;

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
        }
        
        #endregion


        public ParticleEffect(string n)
        {
            name = n;

            // Loading default values
            Loop = true;
            MinInitialSpeed = 10;
            MaxInitialSpeed = 10;
            MinSize = 0.3f;
            MaxSize = 0.5f;
            MinAlpha = 0.2f;
            MaxAlpha = 1f;
            MinDirection = 0;
            MaxDirection = 360;
            Rotation = 0;
            RotationSpeed = 0;
            MinTTL = 10f;
            MaxTTL = 10f;
        }

        public void Initialize(Game game)
        {
            content = (ContentManager)game.Services.GetService(typeof(ContentManager));
            this.game = game;

            texture = content.Load<Texture2D>(@"defaultParticle");
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            emitter.Initialize(game, this);

            numberOfParticles = (int)((emitter.ParticlesPerSecond * MaxTTL));

            for (int i = 0; i < numberOfParticles; i++)
            {
                Particle particle = new Particle(texture);
                emitter.AddParticle(particle);
            }
            
            emitters.Add(emitter);
        }

        public void Update(GameTime gameTime)
        {
            foreach (ParticleEmitter emitter in emitters)
            {
                emitter.Update(gameTime);
            }
        }
    }
}
