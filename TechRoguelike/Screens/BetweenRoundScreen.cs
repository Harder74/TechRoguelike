using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TechRoguelike.StateManagement;
using TechRoguelike.Entities;

namespace TechRoguelike.Screens
{
    public class BetweenRoundScreen : GameScreen
    {
        private readonly List<PowerUpEntry> powerUpEntries = new List<PowerUpEntry>();
        protected IList<PowerUpEntry> PowerUpEntries => powerUpEntries;
        private readonly string text = "Choose a Power Up";
        public int _selectedEntry;
        private readonly InputAction _menuLeft;
        private readonly InputAction _menuRight;
        private readonly InputAction _menuSelect;
        private PlayerSprite Player;

        public BetweenRoundScreen(ScreenManager screenManager, PlayerSprite player)
        {
            Player = player;
            var firstPower = new HealthPowerUp(screenManager.PowerUpTextures[0]);
            var secondPower = new HealthPowerUp(screenManager.PowerUpTextures[0]);
            var thirdPower = new HealthPowerUp(screenManager.PowerUpTextures[0]);

            firstPower.Selected += OnCancel;
            secondPower.Selected += OnCancel;
            thirdPower.Selected += OnCancel;
            firstPower.Selected += OnSelection;
            secondPower.Selected += OnSelection;
            thirdPower.Selected += OnSelection;

            PowerUpEntries.Add(firstPower);
            PowerUpEntries.Add(secondPower);
            PowerUpEntries.Add(thirdPower);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _menuLeft = new InputAction(
                new[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new[] { Keys.A, Keys.Left}, true);
            _menuRight = new InputAction(
                new[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new[] { Keys.D, Keys.Right}, true);
            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            
        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // For input tests we pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (_menuRight.Occurred(input, ControllingPlayer, out playerIndex))
            {
                _selectedEntry--;

                if (_selectedEntry < 0)
                    _selectedEntry = powerUpEntries.Count - 1;
            }

            if (_menuLeft.Occurred(input, ControllingPlayer, out playerIndex))
            {
                _selectedEntry++;

                if (_selectedEntry >= powerUpEntries.Count)
                    _selectedEntry = 0;
            }

            if (_menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
                OnSelectEntry(_selectedEntry, playerIndex);
            
        }

        // Allows the screen the chance to position the menu entries. By default,
        // all menu entries are lined up in a vertical list, centered on the screen.
        public void UpdateMenuEntryLocations()
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
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 - 256);
            var titleOrigin = font.MeasureString(text) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, text, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        protected void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        // Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < powerUpEntries.Count; i++)
            {
                bool isSelected = IsActive && i == _selectedEntry;
                powerUpEntries[i].Update(this, isSelected, gameTime);
            }
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            powerUpEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        protected void OnSelection(object sender, PlayerIndexEventArgs e)
        {
            if(sender is PowerUpEntry powerUp)
            {
                powerUp.UpdatePlayerStats(Player);
            }
        }
    }
}
