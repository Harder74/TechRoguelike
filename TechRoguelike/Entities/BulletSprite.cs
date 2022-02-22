using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public class BulletSprite
    {
        private BulletType _bulletType;
        private BulletTypeFrame _frame;
        private Random random = new Random();

        private Rectangle _source;
        private Texture2D _texture;
        private Vector2 _position;
        private Texture2D _testing;
        private Texture2D _shot;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(50, 50), 16, 16);
        public BoundingRectangle Bounds => bounds;

        public BulletSprite(BulletType bulletType, Vector2 pos)
        {
            _frame = new BulletTypeFrame();
            _bulletType = bulletType;
            _source = _frame.GetBullet(bulletType);
            _position = pos;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BlueHazmatFull");
            _shot = content.Load<Texture2D>("BasicShot");
            _testing = content.Load<Texture2D>("ball");
        }

        public void Update(GameTime gameTime)
        {
            _position += new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_position.X > 752)
            {
                _position.X = -64;
                _position.Y = (float)random.NextDouble() * 480 - 60;
                
            }

            bounds.X = _position.X;
            bounds.Y = _position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(_texture, _position, _source, Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 0);
            /*
            var rect = new Rectangle((int)this.Bounds.X, (int)this.Bounds.Y, (int)this.Bounds.Width, (int)this.Bounds.Height);
            spriteBatch.Draw(_testing, rect, Color.White);
            */
            spriteBatch.Draw(_shot, _position, null, Color.White, (float)Math.PI/2, new Vector2(16, 16), 3f, SpriteEffects.None, 0);
        }
    }
}
