using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.StateManagement;
using TechRoguelike.Entities;


namespace TechRoguelike.Screens
{
    public class PowerUpEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update
        public Texture2D _powerUp;
        public string Text
        {
             get => _text;
            set => _text = value;
        }

        public Texture2D PowerUp
        {
             get => _powerUp;
            set => _powerUp = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public float HealthUpgrade = 0;
        public float FireRateUpgrade = 0;
        public float DamageUpgrade = 0;
        public float RamDamageUpgrade = 0;


        public event EventHandler<PlayerIndexEventArgs> Selected;
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
        }

        public PowerUpEntry(Texture2D powerUp)
        {
            _powerUp = powerUp;
        }

        public virtual void Update(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }


        // This can be overridden to customize the appearance.
        public virtual void Draw(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Gray : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1.5f + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(32,32);
            spriteBatch.Draw(_powerUp, _position, null, color, 0f, origin, scale, SpriteEffects.None, 0f);
            _position.Y += 128;
            origin = new Vector2(font.MeasureString(_text).X/2, font.LineSpacing / 2);
            spriteBatch.DrawString(font, _text, _position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }

        public void UpdatePlayerStats(PlayerSprite player)
        {
            player.MAX_HEALTH += HealthUpgrade;
            player.Health += HealthUpgrade;
            if(player.FIRE_RATE - FireRateUpgrade > 0) player.FIRE_RATE -= FireRateUpgrade;
            player.Damage += DamageUpgrade;
            player.RamDamage += RamDamageUpgrade;
        }
    }
}

