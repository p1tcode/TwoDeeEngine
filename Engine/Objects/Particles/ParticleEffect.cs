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

        public float StartSpeed { get; set; }
        public float EndSpeed { get; set; }

        public float StartSize { get; set; }
        public float EndSize { get; set; }

        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }

        public float MinAngle { get; set; }
        public float MaxAngle { get; set; }

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
            StartSpeed = 10;
            EndSpeed = 0;
            StartSize = 1;
            EndSize = 0;
            StartAlpha = 1;
            EndAlpha = 0;
            MinAngle = MathHelper.ToRadians(0);
            MaxAngle = MathHelper.ToRadians(360);
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

        public void AddEmitter(Vector2 position, string targetLayer, bool active)
        {
            numberOfParticles = (int)(ParticlesPerSecond * MaxTTL) + 1;

            ParticleEmitter emit = new ParticleEmitter(position, ParticlesPerSecond, targetLayer, active);
            for (int i = 0; i < numberOfParticles; i++)
            {
                Particle particle = new Particle(texture);
                particle.Position = position;
                emit.AddParticle(particle);                
            }
            emitters.Add(emit);
        }
    }
}
