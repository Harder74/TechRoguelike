using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public abstract class Enemy : IBounds
    {
        public float Health;
        public Texture2D Texture;
        public Vector2 Position;
        public float Rotation = 0f;
        public float Scale = 1f;
        public BoundingRectangle bounds;
        public BoundingRectangle BoundingRectangle
        {
            get { return bounds; }
            set
            {
                if (!value.Equals(bounds))
                {
                    bounds = value;
                }
            }
        }
        public Color Color = Color.White;
        public bool IsDestroyed = false;
        public Vector2 LinearVelocity;
        public Vector2 LinearAcceleration;
        public Vector2 Direction;
        public Vector2 MAX_VELOCITY = new Vector2(200, 200);
        public Vector2 MIN_VELOCITY = new Vector2(-200, -200);
        public Vector2 MAX_ACCELERATION = new Vector2(5000, 5000);
        public Vector2 MIN_ACCELERATION = new Vector2(-5000, -5000);
        public float RamDamage;
        public bool BeenHit = false;
        public float Score;

        public abstract void Update(GameTime gameTime, Vector2 playerPos);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }

        
    }
}
