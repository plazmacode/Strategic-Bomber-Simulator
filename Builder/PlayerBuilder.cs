using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    internal class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        private void BuildComponents()
        {
            Player player = (Player)gameObject.AddComponent(Player.Instance);
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
