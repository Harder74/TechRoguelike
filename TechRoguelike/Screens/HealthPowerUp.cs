using Microsoft.Xna.Framework.Graphics;

namespace TechRoguelike.Screens
{
    public class HealthPowerUp : PowerUpEntry
    {

        public HealthPowerUp(Texture2D powerUp) : base(powerUp)
        {
            Text = "Health";
            _powerUp = powerUp;
            HealthUpgrade = 25;
        }
    }
}
