using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }

        public Color Color { get; set; } = Color.White;

        public float Scale { get; set; } = 1.0f;

        public float Layer { get; set; } = 0.5f;

        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public override void Awake()
        {

        }

        public override void Start()
        {
        }

        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle objRect = new Rectangle(
                (int)(GameObject.Transform.Position.X - Sprite.Width / 2 ),
                (int)(GameObject.Transform.Position.Y - Sprite.Height / 2),
                (int)(Sprite.Width * Scale),
                (int)(Sprite.Height * Scale));
            //objRect.Inflate(Scale, Scale);
            if (GameWorld.Instance.Camera.RenderRect.Intersects(objRect) ||1 == 1)
            {
                spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
        }
    }
}