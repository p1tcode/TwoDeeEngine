using Engine.Components;
using Microsoft.Xna.Framework;

namespace Engine.Helpers
{
    public static class PreFabs
    {
        public static void LoadContent_ParticleEffects(ParticleManager particleManager)
        {
            particleManager.AddParticleEffect("Smoke");
            particleManager.AddParticleEffect("Spark");
            particleManager.AddParticleEffect("Fire");
        }

        /// <summary>
        /// Initialize Predefined Particle Effects
        /// </summary>
        /// <param name="particleManager"></param>
        public static void Initialize_ParticleEffects(ParticleManager particleManager)
        {

            particleManager["Smoke"].MinInitialSpeed = 5;
            particleManager["Smoke"].MaxInitialSpeed = 10;
            particleManager["Smoke"].MinGrowth = 0.2f;
            particleManager["Smoke"].MaxGrowth = 0.5f;
            particleManager["Smoke"].Color = Color.SlateGray;
            particleManager["Smoke"].MinTTL = 4f;
            particleManager["Smoke"].MaxTTL = 6f;
            particleManager["Smoke"].MinAlpha = 0.2f;
            particleManager["Smoke"].MaxAlpha = 1f;
            particleManager["Smoke"].MinSize = 0.5f;
            particleManager["Smoke"].MaxSize = 0.7f;
            particleManager["Smoke"].Acceleration = new Vector2(0, -7);

            particleManager["Spark"].MinInitialSpeed = 80;
            particleManager["Spark"].MaxInitialSpeed = 120;
            particleManager["Spark"].MinGrowth = -0.1f;
            particleManager["Spark"].MaxGrowth = -0.05f;
            particleManager["Spark"].Color = Color.Yellow;
            particleManager["Spark"].MinTTL = 2f;
            particleManager["Spark"].MaxTTL = 3f;
            particleManager["Spark"].MinAlpha = 0.7f;
            particleManager["Spark"].MaxAlpha = 1f;
            particleManager["Spark"].MinSize = 0.1f;
            particleManager["Spark"].MaxSize = 0.2f;
            particleManager["Spark"].MinDirection = 0;
            particleManager["Spark"].MaxDirection = 360;
            particleManager["Spark"].Acceleration = new Vector2(0, 100);
        }
    }
}
