using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Components;

namespace Engine.Objects
{
    public class ParticleEmitter
    {
        public Vector2 Position { get; set; }
        public bool Active { get; set; }
        public float TimeBetweenParticles { get; set; }
        public string TargetLayer { get; set; }

        int particlesPerSecond;
        public int ParticlesPerSecond
        {
            get { return particlesPerSecond; }
        }


        List<Particle> particles = new List<Particle>();
        Queue<Particle> freeParticles = new Queue<Particle>();
        float timeSinceLastParticle;

        public ParticleEffect Effect;
        IObjectManager objectManager;

        /// <summary>
        /// Return the number of particles in the current emitter
        /// </summary>
        public int NumbersOfParticles
        {
            get
            {
                return particles.Count;
            }
        }
        /// <summary>
        /// Return the number of free particles in the current emitter
        /// </summary>
        public int NumbersOfFreeParticles
        {
            get
            {
                return freeParticles.Count;
            }
        }



        public ParticleEmitter(Vector2 position, int particlesPerSecond, string targetLayer, bool active)
        {
            Position = position;
            this.particlesPerSecond = particlesPerSecond;
            TimeBetweenParticles = 1.0f / particlesPerSecond;
            timeSinceLastParticle = TimeBetweenParticles;
            TargetLayer = targetLayer;
            Active = active;
        }

        public void Initialize(Game game, ParticleEffect effect)
        {
            Effect = effect;
            objectManager = (IObjectManager)game.Services.GetService(typeof(IObjectManager));
        }

        /// <summary>
        /// Add particles to the particle list and queue them.
        /// </summary>
        /// <param name="p"></param>
        public void AddParticle(Particle p)
        {
            p.Alive = false;
            particles.Add(p);
            freeParticles.Enqueue(p);
        }

        /// <summary>
        /// Initialize a particle and release it.
        /// </summary>
        /// <param name="p"></param>
        public void InitializeParticle(Particle p)
        {
            p.Position = Position;
            p.Direction = RandomMath.RandomDirection(Effect.MinDirection, Effect.MaxDirection);
            p.Velocity = p.Direction * new Vector2(RandomMath.RandomBetween(Effect.MinInitialSpeed, Effect.MaxInitialSpeed));
            p.Scale = new Vector2(RandomMath.RandomBetween(Effect.MinSize, Effect.MaxSize));
            p.Growth = RandomMath.RandomBetween(Effect.MinGrowth, Effect.MaxGrowth);
            p.Alive = true;
            p.TimeToLive = RandomMath.RandomBetween(Effect.MinTTL, Effect.MaxTTL);
            p.RemainingTimeToLive = p.TimeToLive;
            p.InitialAlpha = RandomMath.RandomBetween(Effect.MinAlpha, Effect.MaxAlpha);
            p.Color = Effect.Color * p.InitialAlpha;
            p.OriginalColor = p.Color;
            p.Rotation = RandomMath.RandomBetween(0, 360);
            p.RotationSpeed = RandomMath.RandomBetween(Effect.MinRotationSpeed, Effect.MaxRotationSpeed);
            p.Acceleration = Effect.Acceleration;
        }


        /// <summary>
        /// Update the list of particles that are alive, and requeue dead particles.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

            // Update some particletransitions and remove dead particles.
            // The main update of the particle itself is done in the IObjectManager
            foreach (Particle p in particles)
            {
                if (p.Alive)
                {
                    if (Effect.FadeOut)
                    {
                        float normalizedTimeToLive = (p.RemainingTimeToLive / p.TimeToLive);

                        p.Color = p.OriginalColor * normalizedTimeToLive;
                    }

                    p.Update(gameTime);

                    if (!p.Alive)
                    {
                        objectManager[TargetLayer].Remove(p);
                        freeParticles.Enqueue(p);
                    }
                }
            }

            if (Active)
            {
                // Check if its time to release a new particle
                timeSinceLastParticle -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSinceLastParticle <= 0)
                {
                    if (freeParticles.Count == 0)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Particle newP = new Particle(Effect.Texture);
                            AddParticle(newP);
                        }
                    }
                    Particle p = freeParticles.Dequeue();
                    InitializeParticle(p);
                    objectManager[TargetLayer].Add(p);
                    timeSinceLastParticle = TimeBetweenParticles;
                }
            }
        }
    }
}
