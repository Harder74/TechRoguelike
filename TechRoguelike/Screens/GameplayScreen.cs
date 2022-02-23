using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TechRoguelike.Entities;
using TechRoguelike.StateManagement;
using Microsoft.Xna.Framework.Audio;

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
        private bool _initialShot = false;

        private Texture2D _texture;
        private Texture2D _testing;
        private Texture2D _shot;
        private Texture2D yellowSquare;

        private SoundEffect soundEffect;
        private Vector2 MAX_VELOCITY = new Vector2(400, 400);
        private Vector2 MIN_VELOCITY = new Vector2(-400, -400);

        private List<Enemy> enemies = new List<Enemy>();

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
    
            _player = new PlayerSprite(_playerPosition);
            _player.LoadContent(_content);
            _gameFont = _content.Load<SpriteFont>("gameplayfont");
            _fallingLeaves = _content.Load<Song>("FallingLeavesSong");
            MediaPlayer.Volume = .25f;
            MediaPlayer.Play(_fallingLeaves);
            _texture = _content.Load<Texture2D>("BlueHazmatFull");
            _shot = _content.Load<Texture2D>("BasicShot");
            _testing = _content.Load<Texture2D>("ball");
            soundEffect = _content.Load<SoundEffect>("FullSweep");
            yellowSquare = _content.Load<Texture2D>("YellowSquare");
            SoundEffect.MasterVolume = .25f;


            enemies.Add(new YellowSquare(new Vector2(-50, -50), yellowSquare));
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
                if(MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
                enemies.RemoveAll(x => x.IsDestroyed == true);
                _bullets.RemoveAll(x => x._destroy == true);
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(gameTime, _playerPosition);

                    if (_player.Bounds.CollidesWith(enemy.Bounds))
                    {
                        ExitScreen();
                        LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameScreen[] { new BackgroundScreen(), new MainMenuScreen() });
                        MediaPlayer.Pause();
                    }
                }
                foreach (BulletSprite bullet in _bullets)
                {
                    bullet.Update(gameTime);

                   
                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.Bounds.CollidesWith(bullet.Bounds))
                        {
                            enemy.TakeDamage(_player.damage);
                            bullet._destroy = true;
                        }
                    }
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
            
            var viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                MediaPlayer.Pause();
            }
            else
            {
                
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //adds rotation based on players input
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    if (angularVelocity < 15)
                    {
                        angularVelocity += ANGULAR_ACCELERATION * t;
                    }        
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    if(angularVelocity > -15)
                    {
                        angularVelocity -= ANGULAR_ACCELERATION * t;
                    }
                }

                //player input for shooting
                if(keyboardState.IsKeyDown(Keys.Space))
                {
                    fireRateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (!_initialShot)
                    {
                        soundEffect.Play();
                        _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle));
                        _initialShot = true;
                    }
                    else if (fireRateTimer > FIRE_RATE)
                    {
                        soundEffect.Play();
                        _bullets.Add(new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle));
                        fireRateTimer = 0;
                    }                  
                }
                //checks for player input to reset initial shot
                if (keyboardState.IsKeyUp(Keys.Space))
                {
                    _initialShot = false;
                    fireRateTimer = 0;
                }

                //checks for player input for boost, and slows down player when not boosting
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    _velocity += _direction * LINEAR_ACCELERATION * t;
                    _velocity = Vector2.Clamp(_velocity, MIN_VELOCITY, MAX_VELOCITY);
                    _player.SetIsMoving(true);
                }
                else
                {
                    if(_velocity.X > 0)
                    {
                        _velocity.X -= 100f * t;
                    }
                    else if (_velocity.X < 0)
                    {
                        _velocity.X += 100f * t;
                    }
                    else
                    {
                        _velocity.X = 0f;
                    }

                    if (_velocity.Y > 0)
                    {
                        _velocity.Y -= 100f * t;
                    }
                    else if (_velocity.Y < 0)
                    {
                        _velocity.Y += 100f * t;
                    }
                    else
                    {
                        _velocity.Y = 0f;
                    }
                    _player.SetIsMoving(false);
                }

                //checks if player has stopped inputing a rotation to slow the rotation
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

                //updates parameters based on inputs
                angle += angularVelocity * t;
                _direction.X = (float)Math.Sin(angle);
                _direction.Y = -(float)Math.Cos(angle);
                _playerPosition += _velocity * t;

                // Wrap the ship to keep it on-screen
                if (_playerPosition.Y < 0) _playerPosition.Y = viewport.Height;
                if (_playerPosition.Y > viewport.Height) _playerPosition.Y = 0;
                if (_playerPosition.X < 0) _playerPosition.X = viewport.Width;
                if (_playerPosition.X > viewport.Width) _playerPosition.X = 0;

                //updates playersprite based on updated parameters
                _player.UpdatePlayerPos(_playerPosition);
                _player.SetDirection(_direction);
                _player.SetRotation(angle);
            }
        }

        public override void Draw(GameTime gameTime)
        {

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;
            var _text = "Dodge bullets using W, A, S, D, to survive the game.";
            spriteBatch.Begin();

            
            foreach (BulletSprite bullet in _bullets) bullet.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            foreach (Enemy enemy in enemies) enemy.Draw(gameTime, spriteBatch);
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
