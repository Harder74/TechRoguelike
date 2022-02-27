using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public class YellowSquare : Enemy
    {
       

        public YellowSquare(Vector2 pos, Texture2D texture, int roundCount)
        {
            Health = 25 + (roundCount*8);
            Position = pos;
            Texture = texture;
            //LinearAcceleration = 450f;
            Scale = 2f;
            bounds = new BoundingRectangle(new Vector2(50, 50), 32, 32);
            RamDamage = 75f + (roundCount * 5f);
            Score = 10f;
        }

        public override void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (Health < 0) IsDestroyed = true;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Direction = playerPos - Position;
            LinearAcceleration += Direction * 100f * t;
            LinearAcceleration = Vector2.Clamp(LinearAcceleration, MIN_ACCELERATION, MAX_ACCELERATION);
            LinearVelocity += LinearAcceleration * t;
            LinearVelocity = Vector2.Clamp(LinearVelocity, MIN_VELOCITY, MAX_VELOCITY);
            /*
            if (LinearAcceleration.X > 200) LinearAcceleration.X -= 25f;
            if (LinearAcceleration.X < -200) LinearAcceleration.X += 25f;
            if (LinearAcceleration.Y < -200) LinearAcceleration.Y += 25f;
            if (LinearAcceleration.Y > 200) LinearAcceleration.Y -= 25f;
            */
            Position += LinearVelocity * t;
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 16;
            

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, new Vector2 (16, 16), Scale, SpriteEffects.None, 0f);
        }
    }
}
