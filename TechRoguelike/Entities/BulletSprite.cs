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

        private Random random = new Random();


        private Texture2D _texture;
        private Vector2 _position;
        private Texture2D _testing;
        private Texture2D _shot;
        private Vector2 _direction;
        private Viewport viewport;
        public bool _destroy = false;
        private float _rotation;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(50, 50), 16, 16);
        public BoundingRectangle Bounds => bounds;

        public BulletSprite(BulletType bulletType, Vector2 pos, Texture2D shot, Vector2 dir, Viewport view, float rotation)
        {
            _bulletType = bulletType;
            _position = pos;
            _shot = shot;
            _direction = dir;
            viewport = view;
            _rotation = rotation;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BlueHazmatFull");
            _shot = content.Load<Texture2D>("BasicShot");
            _testing = content.Load<Texture2D>("ball");
        }

        public void Update(GameTime gameTime)
        {
            _position += _direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 500f;
            if (_position.Y < -64 || _position.Y > viewport.Height + 64 || _position.X < - 64 || _position.X > viewport.Width + 64) _destroy = true;
            bounds.X = _position.X - 8;
            bounds.Y = _position.Y - 8;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(_texture, _position, _source, Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 0);
            /*
            var rect = new Rectangle((int)this.Bounds.X, (int)this.Bounds.Y, (int)this.Bounds.Width, (int)this.Bounds.Height);
            spriteBatch.Draw(_testing, rect, Color.White);
            */
            spriteBatch.Draw(_shot, _position, null, Color.White, _rotation, new Vector2(16, 16), 3f, SpriteEffects.None, 0.1f);
        }
    }
}
