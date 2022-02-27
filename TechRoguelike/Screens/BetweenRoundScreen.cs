using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TechRoguelike.StateManagement;

namespace TechRoguelike.Screens
{
    public class BetweenRoundScreen : MenuScreen
    {
        private readonly List<PowerUpEntry> powerUpEntries = new List<PowerUpEntry>();
        protected IList<PowerUpEntry> PowerUpEntries => powerUpEntries;
        public BetweenRoundScreen() : base("Pick An Upgrade")
        {
            var firstPower = new PowerUpEntry("test", ScreenManager.PowerUpTextures[0]);
            var secondPower = new PowerUpEntry("test2", ScreenManager.PowerUpTextures[0]);
            var thirdPower = new PowerUpEntry("test3", ScreenManager.PowerUpTextures[0]);

            firstPower.Selected += OnCancel;
            secondPower.Selected += OnCancel;
            thirdPower.Selected += OnCancel;

            PowerUpEntries.Add(firstPower);
            PowerUpEntries.Add(secondPower);
            PowerUpEntries.Add(thirdPower);
        }
        // Allows the screen the chance to position the menu entries. By default,
        // all menu entries are lined up in a vertical list, centered on the screen.
        public override void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            var position = new Vector2(0f, ScreenManager.GraphicsDevice.Viewport.Height / 2);

            // update each menu entry's location in turn
            for(int i = 0; i < powerUpEntries.Count; i++)
            {
                if(i == 0)
                {
                    position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - 32;                    
                }
                else if (i == 1)
                {
                    position.X = ScreenManager.GraphicsDevice.Viewport.Width / 3 - 32;
                }
                else
                {
                    position.X = (2 * ScreenManager.GraphicsDevice.Viewport.Width) / 3 - 32;
                }
                position.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2 - 32;
                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                powerUpEntries[i].Position = position;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            UpdateMenuEntryLocations();

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < powerUpEntries.Count; i++)
            {
                var powerUpEntry = powerUpEntries[i];
                bool isSelected = IsActive && i == _selectedEntry;
                powerUpEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 - 100);
            var titleOrigin = font.MeasureString(_menuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
