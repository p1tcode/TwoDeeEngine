using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    class ParticleEmitter
    {
        public Vector2 Position { get; set; }
        public bool Active { get; set; }
        public float TimeBetweenParticles { get; set; }
        public string TargetLayer { get; set; }

        List<Particle> particles = new List<Particle>();

        public ParticleEmitter(Vector2 position, float particlesPerSecond, string targetLayer, bool active)
        {
            Position = position;
            TimeBetweenParticles = 1.0f / particlesPerSecond;
            TargetLayer = targetLayer;
            Active = active;
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
        }
    }
}
