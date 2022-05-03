using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    /// <summary>
    /// The class for the player / which is a B-17 bomber
    /// </summary>
    class Player : Component
    {
        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        SpriteRenderer spriteRenderer;

        private Vector2 moveVector;
        private float moveSpeed = 0.1f;
        public float Rotation { get; set; }
        private float turnSpeed = 0.9f;

        private Player()
        {

        }

        public override void Start()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            Rotation = 90;
            moveVector = new Vector2(
                (float)Math.Sin(Rotation * 2 * Math.PI / 360),
                (float)-Math.Cos(Rotation * 2 * Math.PI / 360));
            moveVector.Normalize();
            spriteRenderer.Rotation = (float)(Rotation * 2 * Math.PI / 360);
            spriteRenderer.SetSprite("B-17");
            GameObject.SetPosition(0, World.Instance.WorldSize.Y / 2);
        }

        public override void Update()
        {
            Move();
            UpdateCamera();
            Reload();
        }

        /// <summary>
        /// Reloads the bomber
        /// </summary>
        private void Reload()
        {
            //Don't drop bomb if the delay is 100
            if (bombDelay > 0 && World.CurrentGameState == GameState.PLAY)
            {
                //Decrease the bomb delay
                bombDelay -= (float)GameWorld._GameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void Move()
        {
            if (World.CurrentGameState == GameState.PLAY)
            {
                GameObject.TranslatePosition(moveVector * moveSpeed * GameWorld.DeltaTime);
            }
        }

        public void TurnPlane(string direction)
        {
            if (World.CurrentGameState == GameState.PLAY)
            {
                if (direction == "left")
                {
                    Rotation -= turnSpeed;
                }
                if (direction == "right")
                {
                    Rotation += turnSpeed;
                }
                moveVector = new Vector2(
                    (float)Math.Sin(Rotation * 2 * Math.PI / 360),
                    (float)-Math.Cos(Rotation * 2 * Math.PI / 360));
                spriteRenderer.Rotation = (float)(Rotation * 2 * Math.PI / 360); 
            }
        }

        private float bombDelay = 0;

        /// <summary>
        /// Drops bomb with a delay
        /// </summary>
        public void DropBomb()
        {
            if (World.CurrentGameState == GameState.PLAY && bombDelay <= 0)
            {
                //Drop bomb
                GameWorld.Instance.Instantiate(BombFactory.Instance.Create(""));

                //Reset bomb delay
                bombDelay = 2000; //Reset bombDelay
            }
        }

        private void UpdateCamera()
        {
            GameWorld.Instance.Camera.Position = GameObject.Transform.Position;
        }
    }
}
