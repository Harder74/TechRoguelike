using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
        }

        public void Update(GameTime gameTime)
        {
            _position += new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_position.X > 752)
            {
                _position.X = -64;
                _position.Y = (float)random.NextDouble() * 480 - 60;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _source, Color.White);
        }
    }
}
