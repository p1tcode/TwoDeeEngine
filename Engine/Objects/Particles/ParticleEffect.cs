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
        IDrawManager drawManager;

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
        /// The amount of particles the emitter should release per second
        /// </summary>
        public float ParticlesPerSecond { get; set; }

        
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
            MaxInitialSpeed = 0;
            MinSize = 0.3f;
            MaxSize = 1.0f;
            MinAlpha = 0.2f;
            MaxAlpha = 1f;
            MinDirection = MathHelper.ToRadians(0);
            MaxDirection = MathHelper.ToRadians(360);
            Rotation = RandomMath.Random.Next(0,359);
            RotationSpeed = 360;
            ParticlesPerSecond = 100f;
            MinTTL = 1.5f;
            MaxTTL = 3f;
        }

        public void Initialize(Game game)
        {
            content = (ContentManager)game.Services.GetService(typeof(ContentManager));
            drawManager = (IDrawManager)game.Services.GetService(typeof(IDrawManager));

            texture = content.Load<Texture2D>(@"defaultParticle");
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            emitter.Effect = this;

            numberOfParticles = (int)(emitter.ParticlesPerSecond * MaxTTL) + 1;

            for (int i = 0; i < numberOfParticles; i++)
            {
                Particle particle = new Particle(texture);
                emitter.AddParticle(particle);
            }
            
            emitters.Add(emitter);
        }
    }
}
