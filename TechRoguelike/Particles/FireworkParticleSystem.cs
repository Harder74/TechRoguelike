using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TechRoguelike.Particles
{
    public class FireworkParticleSystem : ParticleSystem
    {
        Color[] colors = new Color[] { Color.Blue, Color.BlueViolet, Color.CornflowerBlue, Color.CadetBlue, Color.Aqua, Color.DarkBlue, Color.LightSkyBlue };
        Color color;

        public FireworkParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "particle";
            minNumParticles = 20;
            maxNumParticles = 25;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 100);
            var lifetime = RandomHelper.NextFloat(0.5f, 1f);
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var acceleration = -velocity / lifetime;
            var scale = RandomHelper.NextFloat(20, 25);
            p.Initialize(where, velocity, acceleration, color: color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;



            particle.Scale = .1f + .25f * normalizedLifetime;
        }

        public void PlaceFirework(Vector2 where)
        {
            color = colors[RandomHelper.Next(colors.Length)];
            AddParticles(where);
        }
    }
}
