using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public enum GameState
    {
        PLAY,
        PAUSE
    }

    /// <summary>
    /// Class for making the world. Or should i say Game World
    /// This is in a separate class to make GameWorld less bloated
    /// </summary>
    public class World : Component, IGameListener
    {
        private static World instance;

        public static GameState CurrentGameState { get; set; } = GameState.PAUSE;

        public int WorldNumber { get; set; } = 0;

        public bool WorldRemoved { get; set; } = false;

        public Grid Grid { get; set; }
        private List<Cell> cellsDrawn;

        public Vector2 WorldSize { get; set; }

        public bool WorldDrawn { get; set; } = false;

        public string UpdateRenderTargets { get; set; } = "";

        private World()
        {

        }

        public static World Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new World();
                }
                return instance;
            }
        }

        public RenderTarget2D TerrainRender;
        public RenderTarget2D GridRender;
        private GraphicsDevice GD;

        public void DrawGrid()
        {
            GD.SetRenderTarget(GridRender);
            GD.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };


            GD.Clear(Color.Transparent);
            for (int i = 0; i < cellsDrawn.Count; i++)
            {
                cellsDrawn[i].DrawBorder();
            }
        }

        public void InitializeRenderTargets()
        {
            GD = GameWorld.Instance.GraphicsDevice;
            TerrainRender = new RenderTarget2D(
                GD,
                (int)WorldSize.X,
                (int)WorldSize.Y,
                false,
                GD.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            GridRender = new RenderTarget2D(
                GD,
                (int)WorldSize.X,
                (int)WorldSize.Y,
                false,
                GD.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
        }

        public void RemoveWorld()
        {
            //Grid.Cells.Clear();
            foreach (GameObject gameObject in GameWorld.Instance.GameObjects)
            {
                //Remove gameObject that do not have a cell
                if (gameObject.GetComponent<Cell>() as Cell == null)
                {
                    GameWorld.Instance.Destroy(gameObject);
                }
            }
            WorldRemoved = true;
            //WorldDrawn = false;
        }

        public Cell FindCell(Vector2 position)
        {
            Vector2 GridPos = new Vector2(MathF.Round(position.X / Grid.GridSize), MathF.Round(position.Y / Grid.GridSize));
            Cell c = Grid.Cells[GridPos];

            return c;
        }

        public void CreateWorld()
        {
            WorldSize = new Vector2(5000, 5000);
            WorldNumber++;
            InitializeRenderTargets();
            GD.SetRenderTarget(TerrainRender);

            GD.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GD.Clear(Color.Red);

            GameWorld.Instance._spriteBatch.Begin(SpriteSortMode.Immediate);
            GameWorld.Instance.Camera.Position = new Vector2(0, WorldSize.Y / 2);

            Grid = new Grid(50);
            Grid.BuildGrid();
            CreateTerrain();
            PopulateWorld();
            DrawGrid();
            GameWorld.Instance._spriteBatch.End();
            GD.SetRenderTarget(null);
        }
         
        public void RedrawTerrain()
        {
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                if (GameWorld.Instance.GameObjects[i].HasComponent<Cell>())
                {
                    GameWorld.Instance.GameObjects[i].Draw(GameWorld.Instance._spriteBatch);
                }
            }
        }

        public void RedrawRenderTargetArea(List<GameObject> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Draw(GameWorld.Instance._spriteBatch);
            }
        }

        /// <summary>
        /// Adds terrainTiles to the Cells. and draw the entire map to the TerrainRender
        /// </summary>
        public void CreateTerrain()
        {
            cellsDrawn = new List<Cell>();
            Vector2 WorldGrid = new Vector2(WorldSize.X / Grid.GridSize, WorldSize.Y / Grid.GridSize);
            for (int y = 0; y <WorldGrid.Y; y++)
            {
                for (int x = 0; x < WorldGrid.X; x++)
                {
                    Vector2 current = new Vector2(x, y);
                    GameObject currentGameObject = Grid.Cells[current].GameObject;
                    cellsDrawn.Add(Grid.Cells[current]);

                    int randomCoastDistance = GameWorld._Random.Next((int)WorldGrid.X / 2 - 5, (int)WorldGrid.X / 2 - 2);
                    int randomGrassDistance = GameWorld._Random.Next((int)WorldGrid.X / 2 - 9, (int)WorldGrid.X / 2 - 6);

                    if (Vector2.Distance(WorldGrid / 2, current) > randomCoastDistance)
                    {
                        currentGameObject.AddComponent(new TerrainTile("pixel", NoiseColor(Color.DarkSlateBlue, -5, 5)));
                    }
                    else if (Vector2.Distance(WorldGrid / 2, current) < randomGrassDistance) {
                        currentGameObject.AddComponent(new TerrainTile("pixel", NoiseColor(Color.Green, -15, 15)));
                    }
                    else
                    {
                        currentGameObject.AddComponent(new TerrainTile("pixel", NoiseColor(Color.SandyBrown, -5, 5)));
                    }
                    //Start the newly created TerrainTile components
                    //This adds their SpriteRenderers so that they can be drawn
                    currentGameObject.Start();
                    //Draw their SpriteRenderers
                    currentGameObject.Draw(GameWorld.Instance._spriteBatch);
                }
            }
        }

        public void UpdateTerrainRect()
        {

        }

        /// <summary>
        /// Takes a color and returns a slightly different one
        /// </summary>
        /// <param name="color"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Color NoiseColor(Color color, int min, int max)
        {
            Color c = new Color(color.R, color.G+GameWorld._Random.Next(min,max), color.B);

            return c;
        }

        public void PopulateWorld()
        {
            Director director = new Director();
            director.Builders.Add(new PlayerBuilder()); //Unused if the player is a singleton
            for (int i = 0; i < director.Builders.Count; i++)
            {
                director.Builders[i].BuildGameObject();
                GameWorld.Instance.Instantiate(director.Builders[i].GetResult());
            }
            WorldRemoved = false;
        }

        public override void Start()
        {

        }

        public static float tickSpeed = 500;

        float nextTick = 0;

        public override void Update()
        {
            if (CurrentGameState == GameState.PLAY)
            {
                if (GameWorld._GameTime.TotalGameTime.TotalMilliseconds > nextTick)
                {
                    nextTick = (float)GameWorld._GameTime.TotalGameTime.TotalMilliseconds + tickSpeed;
                    Tick = true;
                }
                else
                {
                    Tick = false;
                }
            }
        }

        public static bool Tick { get; set; } = false;

        public void Notify(GameEvent gameEvent)
        {

        }
    }
}