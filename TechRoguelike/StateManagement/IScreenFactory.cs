using System;
using System.Collections.Generic;
using System.Text;

namespace TechRoguelike.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
