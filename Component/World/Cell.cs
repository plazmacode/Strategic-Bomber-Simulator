using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    /// <summary>
    /// Just used for visualisation of the grid
    /// </summary>
    public class Cell : Component
    {
        public Vector2 StartPosition { get; set; }

        public Color Color { get; set; }
        public Color BorderColor { get; set; }
        public float Layer { get; set; }
        public Rectangle Rect { get; set; }
        public int BorderThickness { get; set; }

        private Rectangle topLine, bottomLine, leftLine, rightLine;
        private int gridSize = (int)World.Instance.Grid.GridSize;


        public Cell(Vector2 position)
        {
            StartPosition = position;
            Color = new Color(25, 25, 25); //Transparent gray
            BorderColor = new Color(50, 50, 50);
            BorderThickness = 1;
            Layer = 0.9f;
        }

        public override void Start()
        {
            GameObject.Transform.Position = StartPosition * gridSize;
            Rect = new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y, gridSize, gridSize);
            topLine = new Rectangle(Rect.X + BorderThickness, Rect.Y, gridSize - BorderThickness, BorderThickness);
            bottomLine = new Rectangle(Rect.X, Rect.Y + gridSize - BorderThickness, gridSize - BorderThickness, BorderThickness);
            rightLine = new Rectangle(Rect.X + gridSize - BorderThickness, Rect.Y, BorderThickness, gridSize);
            leftLine = new Rectangle(Rect.X, Rect.Y, BorderThickness, gridSize);
        }

        /// <summary>
        /// Do not put code in Cell Update() there can be several thousands of them
        /// Use a tickbased Update that does not run every frame
        /// </summary>
        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawBorder( )
        {
            //spriteBatch.Draw(GameWorld.Instance.Pixel, Rect, null, new Color(Color, 0.2f), 0, default, SpriteEffects.None, Layer);
            Color BColor = new Color(BorderColor, 0.2f);
            SpriteBatch spriteBatch = GameWorld.Instance._spriteBatch;
            spriteBatch.Draw(GameWorld.Instance.Pixel, topLine, null, BColor, default, default, SpriteEffects.None, Layer + 0.01f);
            spriteBatch.Draw(GameWorld.Instance.Pixel, bottomLine, null, BColor, default, default, SpriteEffects.None, Layer + 0.01f);
            spriteBatch.Draw(GameWorld.Instance.Pixel, leftLine, null, BColor, default, default, SpriteEffects.None, Layer + 0.01f);
            spriteBatch.Draw(GameWorld.Instance.Pixel, rightLine, null, BColor, default, default, SpriteEffects.None, Layer + 0.01f);
        }
    }
}
