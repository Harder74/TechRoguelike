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

        /*
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 16, 200 - 16), 16, 9);
        public BoundingRectangle Bounds => bounds;
        */
        public Color Color { get; set; } = Color.White;

        public PlayerSprite(Vector2 pos)
        {
            position = pos;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("tilesPacked");
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, position, new Rectangle(225, 97, 30, 30), Color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

        }
    }
}
