using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TechRoguelike.Entities
{
    public class BulletTypeFrame
    {
        public Rectangle[] _thickBullet =
        {
            new Rectangle(384, 2496, 64, 64)
        };

        public Rectangle GetBullet(BulletType bulletType)
        {
            if (bulletType == BulletType.Thick)
            {
                return _thickBullet[0];
            }
            return _thickBullet[0];
        }
    }
}
