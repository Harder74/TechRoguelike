using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public class YellowSquare : Enemy
    {
       

        public YellowSquare(Vector2 pos, Texture2D texture)
        {
            Health = 50;
            Position = pos;
            Texture = texture;
            LinearAcceleration = 450f;
            Scale = 2f;
            Bounds = new BoundingRectangle(new Vector2(50, 50), 32, 32);
        }

        public override void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (Health < 0) IsDestroyed = true;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Direction = Vector2.Normalize(playerPos - (Position));
            LinearVelocity += Direction * LinearAcceleration * t;
            LinearVelocity = Vector2.Clamp(LinearVelocity, MIN_VELOCITY, MAX_VELOCITY);
            if (LinearVelocity.X > 200) LinearVelocity.X -= 25f;
            if (LinearVelocity.X < -200) LinearVelocity.X += 25f;
            if (LinearVelocity.Y < -200) LinearVelocity.Y += 25f;
            if (LinearVelocity.Y > 200) LinearVelocity.Y -= 25f;
            Position += LinearVelocity * t;
            Bounds.X = Position.X - 16;
            Bounds.Y = Position.Y - 16;
            

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, new Vector2 (16, 16), Scale, SpriteEffects.None, 0f);
        }
    }
}
