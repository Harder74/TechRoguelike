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
using TechRoguelike.Particles;

namespace TechRoguelike.Screens
{
    public class GameplayScreen : GameScreen
    {
        private Random random = new Random();
        private ContentManager _content;
        private SpriteFont _gameFont;
        private GameplayStates gameplayState = GameplayStates.Loading;

        private PlayerSprite _player;
        private Vector2 _playerPosition = new Vector2(0,0);
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _angularAcceleration;
        private Vector2 _direction;
        const float LINEAR_ACCELERATION = 250;
        const float ANGULAR_ACCELERATION = 12;
        //private List<IBounds> collisionObjects;

        float angle;
        float angularVelocity;

        private List<BulletSprite> _bullets = new List<BulletSprite>();

        private Song _fallingLeaves;

        private readonly Random _random = new Random();
        
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        
        private double fireRateTimer;
        private double shotTimer;
        private double roundSwitchTimer;
        private bool _initialShot = false;

        private Texture2D _texture;
        private Texture2D _testing;
        private Texture2D _shot;
        private Texture2D yellowSquare;

        private SoundEffect soundEffect;
        private Vector2 MAX_VELOCITY = new Vector2(400, 400);
        private Vector2 MIN_VELOCITY = new Vector2(-400, -400);

        private List<Enemy> enemies = new List<Enemy>();

        private int roundCount = 1;
        private Texture2D topHealth;
        private Texture2D bottomHealth;

        private double invincibleTimer;
        private float score;
        FireworkParticleSystem _fireworks;
        bool _shaking = false;
        float _shakeTime;

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
            var viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            _playerPosition.X = viewport.Width / 2;
            _playerPosition.Y = viewport.Height / 2;
            _player = new PlayerSprite(_playerPosition);
            _player.LoadContent(_content);
            //collisionObjects.Add((IBounds)_player);
            _gameFont = _content.Load<SpriteFont>("gameplayfont");
            _fallingLeaves = _content.Load<Song>("FallingLeavesSong");
            MediaPlayer.Volume = .25f;
            MediaPlayer.Play(_fallingLeaves);
            _texture = _content.Load<Texture2D>("BlueHazmatFull");
            _shot = _content.Load<Texture2D>("BasicShot");
            _testing = _content.Load<Texture2D>("ball");
            soundEffect = _content.Load<SoundEffect>("FullSweep");
            yellowSquare = _content.Load<Texture2D>("YellowSquare");
            topHealth = _content.Load<Texture2D>("TopLayerHealth");
            bottomHealth = _content.Load<Texture2D>("BottomLayerHealth");
            SoundEffect.MasterVolume = .25f;
            _fireworks = new FireworkParticleSystem(ScreenManager.Game, 25);
            ScreenManager.Game.Components.Add(_fireworks);
            ScreenManager.Game.ResetElapsedTime();

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
                var viewport = ScreenManager.Game.GraphicsDevice.Viewport;
                foreach (BulletSprite bullet in _bullets)
                {
                    bullet.Update(gameTime);

                    
                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.BoundingRectangle.CollidesWith(bullet.BoundingRectangle))
                        {
                            _fireworks.PlaceFirework(enemy.Position);
                            enemy.TakeDamage(_player.Damage);
                            score += _player.Damage * roundCount * (1 / 5f);
                            bullet._destroy = true;
                        }
                    }
                }
                

               
                
                switch (gameplayState)
                {
                    case GameplayStates.Loading:
                        roundSwitchTimer += gameTime.ElapsedGameTime.TotalSeconds;
                        if (roundSwitchTimer > 5f)
                        {                           
                            roundSwitchTimer = 0;
                            gameplayState = GameplayStates.Start;
                        }
                        break;
                    case GameplayStates.Start:
                        roundSwitchTimer += gameTime.ElapsedGameTime.TotalSeconds;                       
                        if (roundSwitchTimer > 5f)
                        {
                            for (int i = 0; i < roundCount; i++)
                            {
                                int updown = random.Next(0, 2);
                                float x = (float)random.NextDouble() * viewport.Width;
                                float y = (float)random.NextDouble() * viewport.Height;
                                if (x < viewport.Width && x > viewport.X)
                                {
                                    if(y > viewport.Y && y < viewport.Height && updown == 0)
                                    {
                                        x = Math.Clamp(x, -256, -32);
                                    }
                                    else if(y > viewport.Y && y < viewport.Height && updown == 1)
                                    {
                                        y = Math.Clamp(y, viewport.Y - 256, viewport.Y - 32);
                                    }
                                    else
                                    {
                                        y = Math.Clamp(y, viewport.Height + 32, viewport.Height + 256);
                                    }
                                }
                                else
                                {
                                    if (y > viewport.Y && y < viewport.Height && updown == 0)
                                    {
                                        x = Math.Clamp(x, viewport.Width + 32, viewport.Width + 256);
                                    }
                                    else if (y > viewport.Y && y < viewport.Height && updown == 1)
                                    {
                                        y = Math.Clamp(y, viewport.Y - 256, viewport.Y - 32);
                                    }
                                    else
                                    {
                                        y = Math.Clamp(y, viewport.Height + 32, viewport.Height + 256);
                                    }
                                }
                                var tempVec = new Vector2(x, y);
                                var temp = new YellowSquare(tempVec, yellowSquare, roundCount);
                                enemies.Add(temp);
                                //collisionObjects.Add((IBounds)temp);
                            }
                            roundSwitchTimer = 0;
                            gameplayState = GameplayStates.During;
                        }
                        break;
                    case GameplayStates.During:
                        
                        foreach (Enemy enemy in enemies)
                        {
                            enemy.Update(gameTime, _playerPosition);

                            if (_player.BoundingRectangle.CollidesWith(enemy.BoundingRectangle) && enemy.BeenHit == false)
                            {
                                if(_player.BeenHit == false)
                                {
                                    _player.BeenHit = true;
                                    _player.TakeDamage(enemy.RamDamage);
                                    _fireworks.PlaceFirework(_playerPosition);
                                    _shaking = true;
                                }
                                enemy.TakeDamage(_player.RamDamage);
                                score += _player.Damage * roundCount * (1 / 5f);
                                enemy.BeenHit = true;
                            }
                            if (!_player.BoundingRectangle.CollidesWith(enemy.BoundingRectangle) && enemy.BeenHit == true)
                            {
                                enemy.BeenHit = false;

                            }
                        }
                        if (_player.BeenHit)
                        {
                            invincibleTimer += gameTime.ElapsedGameTime.TotalSeconds;
                            _player.Color = Color.Gray;
                        }
                        if(invincibleTimer >= 1.5f)
                        {
                            invincibleTimer = 0f;
                            _player.BeenHit = false;
                            _player.Color = Color.White;
                        }
                        
                        if (_player.Health <= 0)
                        {
                            gameplayState = GameplayStates.Gameover;
                        }
                        if (enemies.Count == 0)
                        {
                            roundCount++;
                            gameplayState = GameplayStates.End;
                        }
                        break;
                    case GameplayStates.End:
                        invincibleTimer = 0f;
                        _player.BeenHit = false;
                        _player.Color = Color.White;
                        ScreenManager.AddScreen(new BetweenRoundScreen(ScreenManager, _player), ControllingPlayer);
                        _player.Health += .05f * _player.MAX_HEALTH;
                        gameplayState = GameplayStates.Start;
                        // LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new BetweenRoundScreen());
                        break;
                    case GameplayStates.Gameover:
                        ExitScreen();
                        LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameScreen[] { new BackgroundScreen(), new MainMenuScreen() });
                        MediaPlayer.Pause();
                        break;
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

                    /*
                    fireRateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (!_initialShot)
                    {
                        soundEffect.Play();
                        var bullet = new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle);
                        _bullets.Add(bullet);
                        //collisionObjects.Add(bullet);
                        _initialShot = true;
                    }
                    else if (fireRateTimer > FIRE_RATE)
                    {
                        soundEffect.Play();
                        var bullet = new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle);
                        _bullets.Add(bullet);
                        //collisionObjects.Add(bullet);
                        fireRateTimer = 0;
                    }
                    */

                   
                    if (!_initialShot)
                    {
                        soundEffect.Play();
                        var bullet = new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle);
                        _bullets.Add(bullet);
                        //collisionObjects.Add(bullet);
                        _initialShot = true;
                    }
                    else if (fireRateTimer > _player.FIRE_RATE)
                    {
                        soundEffect.Play();
                        var bullet = new BulletSprite(BulletType.Thick, new Vector2(_playerPosition.X, _playerPosition.Y), _shot, _direction, viewport, angle);
                        _bullets.Add(bullet);
                        //collisionObjects.Add(bullet);
                        fireRateTimer = 0;
                    }
                    else
                    {
                        fireRateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    

                }
                //checks for player input to reset initial shot
                if (keyboardState.IsKeyUp(Keys.Space))
                {
                    
                    shotTimer += gameTime.ElapsedGameTime.TotalSeconds; 
                    if(shotTimer > _player.FIRE_RATE / 2)
                    {
                        _initialShot = false;
                        fireRateTimer = 0;
                        shotTimer = 0;
                    }
                   
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
                    if(_velocity.X > 25f)
                    {
                        _velocity.X -= 100f * t;
                    }
                    else if (_velocity.X < -25f)
                    {
                        _velocity.X += 100f * t;
                    }
                    

                    if (_velocity.Y > 25f)
                    {
                        _velocity.Y -= 100f * t;
                    }
                    else if (_velocity.Y < -25f)
                    {
                        _velocity.Y += 100f * t;
                    }
                    
                    _player.SetIsMoving(false);
                }

                //checks if player has stopped inputing a rotation to slow the rotation
                if(keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D) && angularVelocity != 0f)
                {
                    if (angularVelocity > 0f)
                    {
                        angularVelocity -= 5f * t;
                    }
                    if(angularVelocity < 0f)
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
            var font = ScreenManager.Font;
            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;
            var _text = "Use W to boost, A & D to turn, and Space to shoot";
            var round = $"Round {roundCount}";
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);

            Matrix shakeTransform = Matrix.Identity;
            if (_shaking)
            {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                shakeTransform = Matrix.CreateTranslation(2 * MathF.Sin(_shakeTime), 2 * MathF.Cos(_shakeTime), 0);
                if (_shakeTime > 1000) _shaking = false;
            }
            if (!_shaking)
            {
                _shakeTime = 0;
            }
            
            spriteBatch.Begin(transformMatrix: shakeTransform);
            
            if(gameplayState == GameplayStates.Loading)
            {
                var textSize = font.MeasureString(_text);
                var textPosition = (viewportSize - textSize) / 2;
                textPosition.Y = 100;
                spriteBatch.DrawString(_gameFont, _text, textPosition, Color.White, 0, new Vector2(0,0), 1f, SpriteEffects.None, 0);
            }
            else if(gameplayState == GameplayStates.Start)
            {
                var textSize = font.MeasureString(round);
                var textPosition = (viewportSize - textSize) / 2;
                textPosition.Y= 100;
                spriteBatch.DrawString(_gameFont, round, textPosition, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
            foreach (BulletSprite bullet in _bullets) bullet.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch, viewport);
            foreach (Enemy enemy in enemies) enemy.Draw(gameTime, spriteBatch);
            var rec = new Rectangle(0, 0, (int)(_player.Health / _player.MAX_HEALTH * 128), 128);
            spriteBatch.Draw(bottomHealth, new Vector2(viewport.Width / 2, viewport.Height - 75), null, Color.White, 0f, new Vector2(64,64), 4f, SpriteEffects.None, 0);
            spriteBatch.Draw(topHealth, new Vector2(viewport.Width / 2, viewport.Height - 75), rec, Color.White, 0f, new Vector2(64, 64), 4f, SpriteEffects.None, 0);
            var scoreText = $"Score: {score}";
            spriteBatch.DrawString(_gameFont, scoreText, new Vector2(15, 15), Color.White, 0, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0);

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
