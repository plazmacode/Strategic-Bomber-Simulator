using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public interface IGameListener
    {
        void Notify(GameEvent gameEvent);
    }
}
