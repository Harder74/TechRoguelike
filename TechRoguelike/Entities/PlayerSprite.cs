using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public class PlayerSprite
    {
        private Texture2D _shipBasic;
        private Texture2D _testing;
        private Texture2D _shipBoost1;
        private Texture2D _shipBoost2;
        private Texture2D _texture;
        private Vector2 position;
        private bool _isMoving;
        private Vector2 _direction;
        private PlayerFrames _frames;
        private SpriteEffects _flipEffect;

        private const float ANIMATION_SPEED = 1f/6f;
        private double boostAnimationTimer;
        private int boostAnimationFrame;

        private float _rotation = 0f;

        private double loadAnimationTimer;
        private int loadAnimationFrame;

        private double idleAnimationTimer;
        private int jumpAnimationFrame;

        private float _scale = 3f;

        
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(50, 50), 32,32);
        public BoundingRectangle Bounds => bounds;
        
        public Color Color { get; set; } = Color.White;

        public PlayerSprite(Vector2 pos)
        {
            position = pos;
            _frames = new PlayerFrames();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BlueHazmatFull");
            _shipBasic = content.Load<Texture2D>("ShipBasic");
            _shipBoost1 = content.Load<Texture2D>("ShipBoost1");
            _shipBoost2 = content.Load<Texture2D>("ShipBoost2");
           // _testing = content.Load<Texture2D>("ball");
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (_isMoving)
            {
                
                boostAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (boostAnimationTimer > ANIMATION_SPEED)
                {
                    boostAnimationFrame++;
                    if (boostAnimationFrame > 1) boostAnimationFrame = 0;
                    boostAnimationTimer -= ANIMATION_SPEED;
                }
                if(boostAnimationFrame == 0) spriteBatch.Draw(_shipBoost1, position, null, Color, _rotation, new Vector2(16, 16), _scale, SpriteEffects.None, 1);
                else spriteBatch.Draw(_shipBoost2, position, null, Color, _rotation, new Vector2(16, 16), _scale, SpriteEffects.None, 1);
            }
            else
            {
                /*
                spriteBatch.Draw(_texture, position, _frames.playerSlide[0], Color, 0f, new Vector2(32, 32), 1f, _flipEffect, 0);
                runAnimationFrame = 0;
                runAnimationTimer = 0;
                */
                spriteBatch.Draw(_shipBasic, position, null, Color, _rotation, new Vector2(16,16), _scale, SpriteEffects.None, 1);
            }
            
           /*
            var rect = new Rectangle((int)this.Bounds.X, (int)this.Bounds.Y, (int)this.Bounds.Width, (int)this.Bounds.Height);
            spriteBatch.Draw(_testing, rect, Color.White);
           */

        }
        /// <summary>
        /// lets the gamescreen set player position from outside of this class
        /// </summary>
        /// <param name="pos"></param>
        public void UpdatePlayerPos(Vector2 pos)
        {
            position = pos;
            bounds.X = position.X - 16;
            bounds.Y = position.Y - 16;
        }

        /// <summary>
        /// sets moving flag for player sprite
        /// </summary>
        /// <param name="move"></param>
        public void SetIsMoving(bool move)
        {
            _isMoving = move;
        }
        
        /// <summary>
        /// sets player direction
        /// </summary>
        /// <param name="dir"></param>
        public void SetDirection(Vector2 dir)
        {
            _direction = dir;
        }

        public void SetRotation(float angle)
        {
            _rotation = angle;
        }
    }
}
