﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Engine.Components;

namespace Engine.Objects
{
    public class ParticleEmitter
    {
        public Vector2 Position { get; set; }
        public bool Active { get; set; }
        public float TimeBetweenParticles { get; set; }
        public string TargetLayer { get; set; }
        public bool Continous { get; set; }

        private bool trigger = false;
        private float elapsedTime = 0;
        private float pauseBetweenTrigger  = 0.01f;

        int particlesPerSecond;
        public int ParticlesPerSecond
        {
            get { return particlesPerSecond; }
        }


        List<Particle> particles = new List<Particle>();
        Queue<Particle> freeParticles = new Queue<Particle>();
        float timeSinceLastParticle;

        public ParticleEffect Effect;
        ObjectManager objectManager;

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


        /// <summary>
        /// Create a particle emitter that controls the release off particles
        /// </summary>
        /// <param name="position">Position of released particles</param>
        /// <param name="particlesPerSecond">Numbers of particles released per second if the effect is continous. If not all particles will be released instantly.</param>
        /// <param name="targetLayer">At what layer should the particles be released (ObjectManager)</param>
        /// <param name="active">Is it active or non active</param>
        /// <param name="continous">Should particles be released at once or continous during the active time</param>
        public ParticleEmitter(Vector2 position, int particlesPerSecond, string targetLayer, bool active, bool continous)
        {
            Position = position;
            this.particlesPerSecond = particlesPerSecond;
            TimeBetweenParticles = 1.0f / particlesPerSecond;
            timeSinceLastParticle = TimeBetweenParticles;
            TargetLayer = targetLayer;
            Active = active;
            Continous = continous;
        }

        public void Initialize(ParticleEffect effect, ObjectManager objectManager)
        {
            Effect = effect;
            this.objectManager = objectManager;
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

        public void Trigger(float pauseBetweenTriggers)
        {
            trigger = true;
            this.pauseBetweenTrigger = pauseBetweenTriggers;
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
                if (Continous)
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
                else
                {
                    if (trigger)
                    {
                        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (elapsedTime > pauseBetweenTrigger)
                        {
                            for (int i = 0; i < particlesPerSecond; i++)
                            {
                                if (freeParticles.Count == 0)
                                {
                                    for (int y = 0; y < 10; y++)
                                    {
                                        Particle newP = new Particle(Effect.Texture);
                                        AddParticle(newP);
                                    }
                                }
                                Particle p = freeParticles.Dequeue();
                                InitializeParticle(p);
                                objectManager[TargetLayer].Add(p);
                            }

                            trigger = false;
                            elapsedTime = 0;
                        }
                    }
                }
            }
        }
    }
}
