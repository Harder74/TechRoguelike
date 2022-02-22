using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TechRoguelike.Entities;
using TechRoguelike.StateManagement;

namespace TechRoguelike.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Random random = new Random();
        private ContentManager _content;
        private SpriteFont _gameFont;

        private PlayerSprite _player;
        private Vector2 _playerPosition = new Vector2(200, 200);
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _angularAcceleration;
        private Vector2 _direction;
        const float LINEAR_ACCELERATION = 150;
        const float ANGULAR_ACCELERATION = 12;

        float angle;
        float angularVelocity;

        private List<BulletSprite> _bullets = new List<BulletSprite>();

        private Song _fallingLeaves;

        private readonly Random _random = new Random();
        
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private const float FIRE_RATE = .5f;
        private double fireRateTimer;

        private Texture2D _texture;
        private Texture2D _testing;
        private Texture2D _shot;


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
            _gameFont = _content.Load<SpriteFont>("gameplayfont");
            _fallingLeaves = _content.Load<Song>("FallingLeavesSong");
            MediaPlayer.Volume = .25f;
            MediaPlayer.Play(_fallingLeaves);

            /*
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
            */
            _texture = _content.Load<Texture2D>("BlueHazmatFull");
            _shot = _content.Load<Texture2D>("BasicShot");
            _testing = _content.Load<Texture2D>("ball");
           
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
                foreach (BulletSprite bullet in _bullets)
                {
                    bullet.Update(gameTime);
                    /*
                    if (_player.Bounds.CollidesWith(bullet.Bounds))
                    {
                        ExitScreen();
                        LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
                    }
                    */
                }
                
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;
            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            fireRateTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
              //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                   // _acceleration += _direction * LINEAR_ACCELERATION;
                  
                    angularVelocity += ANGULAR_ACCELERATION * t;
                   
                    
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    //_acceleration += _direction * LINEAR_ACCELERATION; 
                    angularVelocity -= ANGULAR_ACCELERATION * t;

                }
                if(keyboardState.IsKeyDown(Keys.Space) && fireRateTimer > FIRE_RATE)
                {
                    _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction));
                }
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    _velocity += _direction * LINEAR_ACCELERATION * t;
                    // _velocity += _acceleration * t;
                    _player.SetIsMoving(true);
                }
                else
                {
                    if(_velocity.X > 0)
                    {

                        //_acceleration -=  Vector2.UnitX * 50f * t;
                        _velocity.X -= 100f * t;
                    }
                    else if (_velocity.X < 0)
                    {
                        //_acceleration += Vector2.UnitX * 50f * t;
                        _velocity.X += 100f * t;
                    }
                    else
                    {
                        _velocity.X = 0f;
                    }

                    if (_velocity.Y > 0)
                    {

                        //_acceleration -=  Vector2.UnitX * 50f * t;
                        _velocity.Y -= 100f * t;
                    }
                    else if (_velocity.Y < 0)
                    {
                        //_acceleration += Vector2.UnitX * 50f * t;
                        _velocity.Y += 100f * t;
                    }
                    else
                    {
                        _velocity.Y = 0f;
                    }
                    _player.SetIsMoving(false);
                }
                if(keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D) && angularVelocity != 0f)
                {
                    if (angularVelocity > 0)
                    {
                        angularVelocity -= 5f * t;
                    }
                    if(angularVelocity < 0)
                    {
                        angularVelocity += 5f * t;
                    }
                }

                angle += angularVelocity * t;
                _direction.X = (float)Math.Sin(angle);
                _direction.Y = -(float)Math.Cos(angle);

               
                _playerPosition += _velocity * t;

                // Wrap the ship to keep it on-screen
                var viewport = ScreenManager.Game.GraphicsDevice.Viewport;
                if (_playerPosition.Y < 0) _playerPosition.Y = viewport.Height;
                if (_playerPosition.Y > viewport.Height) _playerPosition.Y = 0;
                if (_playerPosition.X < 0) _playerPosition.X = viewport.Width;
                if (_playerPosition.X > viewport.Width) _playerPosition.X = 0;
                _player.UpdatePlayerPos(_playerPosition);
                _player.SetDirection(_direction);
                _player.SetRotation(angle);
                /*
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
                */
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;
            var _text = "Dodge bullets using W, A, S, D, to survive the game.";
            spriteBatch.Begin();

            _player.Draw(gameTime, spriteBatch);
            foreach (BulletSprite bullet in _bullets) bullet.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(_gameFont, _text, new Vector2(), Color.White, 0, new Vector2(-200,0), 1f, SpriteEffects.None, 0);
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
