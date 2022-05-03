using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class WorldBuilder : IBuilder
    {
        private GameObject gameObject;
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        private void BuildComponents()
        {
            World world = (World)gameObject.AddComponent(World.Instance);
            //world.AddComponent(new Component());
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
