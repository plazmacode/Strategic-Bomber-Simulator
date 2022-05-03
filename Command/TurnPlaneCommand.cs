using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class TurnPlaneCommand : ICommand
    {
        private string direction;
        public TurnPlaneCommand(string direction)
        {
            this.direction = direction;
        }

        public void Execute()
        {
            Player p = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            p.TurnPlane(direction);
        }
    }
}
