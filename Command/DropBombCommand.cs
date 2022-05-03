using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class DropBombCommand : ICommand
    {
        public DropBombCommand()
        {

        }

        public void Execute()
        {
            Player p = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            p.DropBomb();
        }
    }
}
