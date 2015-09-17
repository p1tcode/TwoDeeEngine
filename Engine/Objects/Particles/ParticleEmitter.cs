using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        Queue<Particle> queuedParticles = new Queue<Particle>();

        public ParticleEffect Effect;

        public ParticleEmitter(Vector2 position, int particlesPerSecond, string targetLayer, bool active)
        {
            Position = position;
            this.particlesPerSecond = particlesPerSecond;
            TimeBetweenParticles = 1.0f / particlesPerSecond;
            TargetLayer = targetLayer;
            Active = active;
        }

        public void AddParticle(Particle p)
        {
            InitializeParticle(p);

            particles.Add(p);
            queuedParticles.Enqueue(p);
        }

        public void InitializeParticle(Particle p)
        {
            p.Position = Position;
            p.Direction = RandomMath.RandomDirection();
            p.Velocity = p.Direction * new Vector2(RandomMath.RandomBetween(Effect.MinInitialSpeed, Effect.MaxInitialSpeed));
            p.Scale = new Vector2(RandomMath.RandomBetween(Effect.MinSize, Effect.MaxSize));
            
        }
    }
}
