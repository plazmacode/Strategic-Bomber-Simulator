using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Strategic_Bomber_Simulator
{
    public class GameWorld : Game
    {
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch { get; set; }

        public SpriteFont Arial { get; set; }

        private FrameCounter _frameCounter = new FrameCounter();


        public Camera2D Camera { get; set; } = new Camera2D();

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyGameObjects = new List<GameObject>();

        public Texture2D Pixel { get; set; }
        public static float DeltaTime { get; private set; }

        public static GameTime _GameTime { get; private set; }

        public List<GameObject> GameObjects { get => gameObjects; set => gameObjects = value; }

        public static Random _Random { get; set; } = new Random();
        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;

            Camera.Position = new Vector2(1000.0f, 1000.0f);

            Director director = new Director();

            director.Builders.Add(new WorldBuilder());

            GameObjects.AddRange(director.Construct());

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Awake();
            }
            base.Initialize();
        }

        public void OnResize(Object sender, EventArgs e)
        {
            Camera.OnResize();
        }

        protected override void LoadContent()
        {
            Camera.SetOrigin();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Arial = Content.Load<SpriteFont>("arial");

            Pixel = Content.Load<Texture2D>("pixel");

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Start();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            InputManager.Instance.Update();
            //Options.Instance.Update();

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update();
            }

            base.Update(gameTime);

            CleanUp();
        }

        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        public void Destroy(GameObject go)
        {
            destroyGameObjects.Add(go);
        }

        private void CleanUp()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                GameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();
            }

            for (int i = 0; i < destroyGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyGameObjects[i]);
            }
            destroyGameObjects.Clear();
            newGameObjects.Clear();
        }

        protected override void Draw(GameTime gameTime)
        {
            ////Update position of camera rectangle
            Instance.Camera.UpdateRenderRect();

            //Draw the renderTargets
            if (!World.Instance.WorldDrawn)
            {
                World.Instance.CreateWorld();
                World.Instance.WorldDrawn = true;
            }

            if (World.Instance.UpdateRenderTargets == "area")
            {
                World.Instance.RedrawRenderTargetArea();
            }

            if (World.Instance.UpdateRenderTargets == "full")
            {
                World.Instance.RedrawTerrain();
            }

            GraphicsDevice.Clear(Color.DarkSlateBlue);


            //Draw Rendertargets
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.GetTransformation(_graphics.GraphicsDevice));
            _spriteBatch.Draw(World.Instance.TerrainRender, Vector2.Zero, Color.White);
            if (Options.Instance.GridEnabled)
            {
                _spriteBatch.Draw(World.Instance.GridRender, Vector2.Zero, Color.White);
            }
            _spriteBatch.End();

            //Draw gameObjects using Camera class 
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, Camera.GetTransformation(_graphics.GraphicsDevice));

            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (!GameObjects[i].HasComponent<Cell>())
                {
                    GameObjects[i].Draw(_spriteBatch);
                }
                //GameObjects[i].Draw(_spriteBatch);

            }
            //_spriteBatch.Draw(Pixel, Instance.Camera.RenderRect, null, new Color(new Color(25, 25, 25), 0.1f), 0, default, SpriteEffects.None, 0.1f);

            _spriteBatch.End();

            //Draw text that is independent of cameraposition
            _spriteBatch.Begin();

            _frameCounter.Update(gameTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            _spriteBatch.DrawString(Arial, fps, new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 100, 0), Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            _spriteBatch.DrawString(Arial, Camera.Position.ToString(), Vector2.Zero, Color.White);
            _spriteBatch.DrawString(Arial, GameObjects.Count.ToString(), new Vector2(0, 20), Color.White);
            _spriteBatch.DrawString(Arial, World.CurrentGameState.ToString(), new Vector2(0, 40), Color.White);
            _spriteBatch.DrawString(Arial, Player.Instance.Rotation.ToString(), new Vector2(0, 60), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in GameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
