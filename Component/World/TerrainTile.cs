using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class TerrainTile : Component
    {
        private string sprite;
        private Color color;

        public TerrainTile(string sprite, Color color)
        {
            this.sprite = sprite;
            this.color = color;
        }

        public override void Start()
        {
            SpriteRenderer sr = new SpriteRenderer();
            sr.SetSprite(sprite);
            sr.Color = color;
            sr.Scale = World.Instance.Grid.GridSize / 10;
            sr.Origin = Vector2.Zero;
            GameObject.AddComponent(sr);
        }
    }
}
