using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;
using System;

namespace Engine.Components
{
    interface IParticleManager
    {
        void AddParticleEffect(string particleEffectName);
        ParticleEffect this[string ParticleEffectName] { get; }
        int NumberOfEffects { get; }
    }

    public class ParticleManager : GameComponent, IParticleManager
    {
        List<ParticleEffect> particleEffects = new List<ParticleEffect>();
       
        #region Properties

        int numberOfEffects;
        public int NumberOfEffects
        {
            get
            {
                numberOfEffects = 0;
                foreach (ParticleEffect effect in particleEffects)
                {
                    numberOfEffects++;
                }
                return numberOfEffects;
            }
        }

        int totalEmitters;
        public int TotalEmitters
        {
            get
            {
                totalEmitters = 0;
                foreach (ParticleEffect effect in particleEffects)
                {
                    totalEmitters += effect.NumberOfEmitters;
                }
                return totalEmitters;
            }
        }

        #endregion

        public ParticleManager(Game game) 
            : base(game)
        {
            game.Services.AddService(typeof(IParticleManager), this);

            AddParticleEffect("Fire");
            AddParticleEffect("Smoke");
        }

        public override void Initialize()
        {
            // Here is a good place to grab services needed for this Manager

            foreach (ParticleEffect particleEffect in particleEffects)
            {
                particleEffect.Initialize(Game);
            }

            base.Initialize();
        }

        /// <summary>
        /// List of ParticleEffects
        /// </summary>
        /// <param name="ParticleEffectName"></param>
        /// <returns></returns>
        public ParticleEffect this[string ParticleEffectName]
        {
            get
            {
                return particleEffects.Find((ParticleEffect a) => { return a.Name == ParticleEffectName; });
            }
        }

        /// <summary>
        /// Adds an action to the layer manager
        /// </summary>
        /// <param name="actionName"></param>
        public void AddParticleEffect(string particleEffectName)
        {
            particleEffects.Add(new ParticleEffect(particleEffectName));
        }

        public override void Update(GameTime gameTime)
        {
            // Loops through and updates all the Particle Effects
            foreach (ParticleEffect effect in particleEffects)
            {
                effect.Update(gameTime);
            }
            base.Update(gameTime);
        }

    }
}
