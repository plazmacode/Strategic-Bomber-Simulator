using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public enum BUTTONSTATE { UP, DOWN }
    class ButtonEvent : GameEvent
    {
        public Keys Key { get; private set; }

        public BUTTONSTATE State { get; private set; }

        public void Notify(Keys key, BUTTONSTATE state)
        {
            this.Key = key;
            this.State = state;
            base.Notify();
        }
    }
}
