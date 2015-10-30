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

        /// <summary>
        /// Growth Per second
        /// </summary>
        public float MinGrowth { get; set; }
        public float MaxGrowth { get; set; }

        public float MinAlpha { get; set; }
        public float MaxAlpha { get; set; }
        public bool FadeOut { get; set; }

        public float MinDirection { get; set; }
        public float MaxDirection { get; set; }

        public float Rotation { get; set; }
        public float MinRotationSpeed { get; set; }
        public float MaxRotationSpeed { get; set; }

        public Vector2 Acceleration { get; set; }

        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        /// <summary>
        /// Minimum time for a particle to live
        /// </summary>
        public float MinTTL { get; set; }
        /// <summary>
        /// Maximum time for a particle to live
        /// </summary>
        public float MaxTTL { get; set; }

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
            Acceleration = Vector2.Zero;
            MinSize = 0.3f;
            MaxSize = 0.5f;
            MinGrowth = 0;
            MaxGrowth = 0;
            MinAlpha = 1f;
            MaxAlpha = 1f;
            MinDirection = 0;
            MaxDirection = 360;
            Rotation = 0;
            MinRotationSpeed = -45;
            MaxRotationSpeed = +45;
            MinTTL = 1.5f;
            MaxTTL = 3f;
            FadeOut = true;
        }

        public void Initialize(Game game)
        {
            content = (ContentManager)game.Services.GetService(typeof(ContentManager));
            this.game = game;

            Texture = content.Load<Texture2D>(@"defaultParticle");
        }

        public void AddEmitter(ParticleEmitter emitter, ObjectManager objectManager)
        {
            emitter.Initialize(this, objectManager);

            for (int i = 0; i < emitter.ParticlesPerSecond; i++)
            {
                Particle particle = new Particle(Texture);
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
