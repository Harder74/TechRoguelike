using Microsoft.Xna.Framework.Graphics;
namespace TechRoguelike.Screens
{
    public class FireRatePowerUp : PowerUpEntry
    {
        public FireRatePowerUp(Texture2D powerUp) : base(powerUp)
        {
            Text = "Fire Rate";
            _powerUp = powerUp;
            FireRateUpgrade = .05f;
        }
    }
}
