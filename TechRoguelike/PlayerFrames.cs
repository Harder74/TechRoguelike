using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TechRoguelike
{
    public class PlayerFrames
    {
        public readonly Rectangle[] playerLoad = 
        {
            
            new Rectangle(0, 228, 64, 64),
            new Rectangle(96, 228, 64, 64),
            new Rectangle(192, 228, 64, 64),
            new Rectangle(288, 228, 64, 64),

        };

        public readonly Rectangle[] playerSlide = 
        {
            new Rectangle(0, 424, 64,64),
            new Rectangle(96, 424, 64,64),
            new Rectangle(192, 424, 64,64),
            new Rectangle(288, 424, 64,64),
            new Rectangle(384, 424, 64,64),
        };

        public readonly Rectangle[] playerRunNoWeapon =
        {
            new Rectangle(0, 1280, 64,64),
            new Rectangle(96, 1280, 64,64),
            new Rectangle(192, 1280, 64,64),
            new Rectangle(288, 1280, 64,64),
        };
    }
}
