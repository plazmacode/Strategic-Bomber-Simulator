using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    internal class BombFactory : Factory
    {
        private static BombFactory instance;
        public static BombFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BombFactory();
                }
                return instance;
            }
        }

        public override GameObject Create(object generic)
        {
            GameObject gameObject = new GameObject();

            gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Bomb());

            return gameObject;
        }
    }
}
