using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TechRoguelike.Screens
{
    class DamagePowerUp : PowerUpEntry
    {
        public DamagePowerUp(Texture2D powerUp) : base(powerUp)
        {
            Text = "Damage";
            _powerUp = powerUp;
            DamageUpgrade = 10;
        }
    }
}
