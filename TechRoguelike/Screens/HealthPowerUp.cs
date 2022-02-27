using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechRoguelike.StateManagement;
using TechRoguelike.Entities;

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
