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
        IDrawManager drawManager;

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
            drawManager = (IDrawManager)game.Services.GetService(typeof(IDrawManager));
        }

        public void AddParticle(Particle p)
        {
            particles.Add(p);
            freeParticles.Enqueue(p);
        }

        public void InitializeParticle(Particle p)
        {
            p.Position = Position;
            p.Direction = RandomMath.RandomDirection();
            p.Velocity = p.Direction * new Vector2(RandomMath.RandomBetween(Effect.MinInitialSpeed, Effect.MaxInitialSpeed));
            p.Scale = new Vector2(RandomMath.RandomBetween(Effect.MinSize, Effect.MaxSize));
            p.Alive = true;
            p.TimeToLive = RandomMath.RandomBetween(Effect.MinTTL, Effect.MaxTTL);
            drawManager[TargetLayer].Add(p);
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastParticle -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastParticle <= 0)
            {
                Particle p = freeParticles.Dequeue();
                InitializeParticle(p);
                timeSinceLastParticle = TimeBetweenParticles;
            }

            foreach (Particle p in particles)
            {
                if (p.Alive)
                {
                    p.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (!p.Alive)
                    {
                        drawManager[TargetLayer].Remove(p);
                        freeParticles.Enqueue(p);
                    }
                }
            }

        }
    }
}
