﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public abstract class Enemy
    {
        public float Health;
        public Texture2D Texture;
        public Vector2 Position;
        public float Rotation = 0f;
        public float Scale = 1f;
        public BoundingRectangle Bounds;
        public Color Color = Color.White;
        public bool IsDestroyed = false;
        public Vector2 LinearVelocity;
        public float LinearAcceleration;
        public Vector2 Direction;
        public Vector2 MAX_VELOCITY = new Vector2(225, 225);
        public Vector2 MIN_VELOCITY = new Vector2(-225, -225);

        public abstract void Update(GameTime gameTime, Vector2 playerPos);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }
    }
}