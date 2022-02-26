using System;
using System.Collections.Generic;
using System.Text;
using TechRoguelike.Collisions;

namespace TechRoguelike.Entities
{
    public interface IBounds
    {
        public BoundingRectangle BoundingRectangle { get; set; }
    }
}
