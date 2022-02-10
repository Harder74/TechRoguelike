using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TechRoguelike.Entities
{
    public class PlayerSprite
    {
        private Texture2D _texture;
        private Vector2 position;
        private bool _isMoving;
        private Vector2 _direction;
        private PlayerFrames _frames;
        private SpriteEffects _flipEffect;

        private const float ANIMATION_SPEED = 0.33f;
        private double runAnimationTimer;
        private int runAnimationFrame;

        private double loadAnimationTimer;
        private int loadAnimationFrame;

        private double idleAnimationTimer;
        private int jumpAnimationFrame;

        /*
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 16, 200 - 16), 16, 9);
        public BoundingRectangle Bounds => bounds;
        */
        public Color Color { get; set; } = Color.White;

        public PlayerSprite(Vector2 pos)
        {
            position = pos;
            _frames = new PlayerFrames();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BlueHazmatFull");
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
                if(_direction.X < 0)
                {
                    _flipEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    _flipEffect = SpriteEffects.None;
                }
                runAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (runAnimationTimer > ANIMATION_SPEED)
                {
                    runAnimationFrame++;
                    if (runAnimationFrame > 3) runAnimationFrame = 0;
                    runAnimationTimer -= ANIMATION_SPEED;
                }
                spriteBatch.Draw(_texture, position, _frames.playerRunNoWeapon[runAnimationFrame], Color, 0f, new Vector2(0,0), 1f, _flipEffect, 0);
            }
            else
            {
                spriteBatch.Draw(_texture, position, new Rectangle(0, 1280, 64, 64), Color, 0f, new Vector2(0, 0), 1f, _flipEffect, 0);
            }
            
           // spriteBatch.Draw(_texture, position, Color);

        }
        /// <summary>
        /// lets the gamescreen set player position from outside of this class
        /// </summary>
        /// <param name="pos"></param>
        public void UpdatePlayerPos(Vector2 pos)
        {
            position = pos;
            //bounds.X = position.X;
            //bounds.Y = position.Y ;
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
    }
}
