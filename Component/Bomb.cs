using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    internal class Bomb : Component
    {
        private SpriteRenderer sr;

        private bool isExploded = false;
        private float fallSpeed = 0.0003f;

        public override void Start()
        {
            Player p = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            GameObject.Transform.Position = p.GameObject.Transform.Position;

            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.Color = Color.Black;
            sr.SetSprite("Bomb");
            sr.Scale = 0.5f;
            sr.Layer = 0.4f;
        }
 
        public override void Update()
        {
            Fall();
            Explode();
            Disappear();
        }

        private float explosionSize = 5.0f;

        private void Explode()
        {
            if (isExploded && !isFading)
            {
                sr.Scale += 0.08f;
            }

            if (sr.Scale >= explosionSize)
            {
                isFading = true;
            }

            Cell c = World.Instance.FindCell(GameObject.Transform.Position);
            c.Color = Color.Black;
        }

        private bool isFading = false;

        private void Disappear()
        {
            if (isExploded && isFading)
            {
                sr.Scale -= fallSpeed * GameWorld.DeltaTime;
                sr.Color = new Color(sr.Scale * 0.2f, 0, 0);
            }
            if (sr.Scale <= 0 && isExploded)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        /// <summary>
        /// Makes the bomb shrink, as it falls closer to the ground
        /// </summary>
        private void Fall()
        {
            if (sr.Scale <= 0 && !isExploded)
            {
                isExploded = true;
                sr.Scale = 2.0f;
                sr.Color = Color.Red;
            }

            if (!isExploded)
            {
                sr.Scale -= fallSpeed * GameWorld.DeltaTime;
            }
        }
    }
}
