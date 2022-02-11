using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TechRoguelike.StateManagement;
using TechRoguelike.Entities;

namespace TechRoguelike.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Random random = new Random();
        private ContentManager _content;
        private SpriteFont _gameFont;

        private PlayerSprite _player;
        private Vector2 _playerPosition = new Vector2(200, 200);

        private List<BulletSprite> _bullets = new List<BulletSprite>();
        

        private readonly Random _random = new Random();
        
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back }, true);
            

        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

           // _gameFont = _content.Load<SpriteFont>("gamefont");
            _player = new PlayerSprite(_playerPosition);
            _player.LoadContent(_content);

            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-500, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-600, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-700, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-800, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-1000, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-1200, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-1400, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-1600, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-2000, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-2400, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-2800, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-3200, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-3600, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-4000, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-5000, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-5500, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-5600, (float)random.NextDouble() * 480 - 60)));
            _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(-5700, (float)random.NextDouble() * 480 - 60)));


            foreach (BulletSprite bullet in _bullets) bullet.LoadContent(_content);
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            //ScreenManager.Game.ResetElapsedTime();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
            


            if (IsActive)
            {
                foreach (BulletSprite bullet in _bullets) bullet.Update(gameTime);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];

            
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
              //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.A))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.D))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.W))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.S))
                    movement.Y++;

                
                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 7f;
                _player.SetDirection(movement);
                if (movement.Length() > 0)
                {
                    _player.SetIsMoving(true);
                }
                else
                {
                    _player.SetIsMoving(false);
                }
                _player.UpdatePlayerPos(_playerPosition);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Yellow, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            _player.Draw(gameTime, spriteBatch);
            foreach (BulletSprite bullet in _bullets) bullet.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
