using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Objects
{
    public static class PreFabs
    {
        public static void Initialize_ParticleEffects(ParticleManager particleManager)
        {
            particleManager.AddParticleEffect("Smoke");
            particleManager.AddParticleEffect("Spark");
            particleManager.AddParticleEffect("Fire");

            particleManager["Smoke"].MinInitialSpeed = 5;
            particleManager["Smoke"].MaxInitialSpeed = 10;
            particleManager["Smoke"].MinGrowth = 0.2f;
            particleManager["Smoke"].MaxGrowth = 0.5f;
            particleManager["Smoke"].Color = Color.DimGray;
            particleManager["Smoke"].MinTTL = 2f;
            particleManager["Smoke"].MaxTTL = 4f;
            particleManager["Smoke"].MinAlpha = 0.2f;
            particleManager["Smoke"].MaxAlpha = 1f;
            particleManager["Smoke"].MinSize = 0.5f;
            particleManager["Smoke"].MaxSize = 0.7f;
            particleManager["Smoke"].MinRotationSpeed = 0;
            particleManager["Smoke"].MaxRotationSpeed = 0;
            particleManager["Smoke"].Acceleration = new Vector2(0, -7);

            particleManager["Spark"].MinInitialSpeed = 80;
            particleManager["Spark"].MaxInitialSpeed = 120;
            particleManager["Spark"].MinGrowth = -0.1f;
            particleManager["Spark"].MaxGrowth = -0.05f;
            particleManager["Spark"].Color = Color.Orange;
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

        public static void Initialize_Input(InputManger inputManager)
        {
            inputManager.AddAction("Next");
            inputManager["Next"].Add(Keys.Space);
        }

        public static void Layers(ObjectManager objectManager)
        {
            // Add default layers
            objectManager.AddLayer("Background");
            objectManager.AddLayer("Player");
            objectManager.AddLayer("Foreground");
            objectManager.AddLayer("Debug");
            objectManager.AddLayer("Text");
        }
    }
}
