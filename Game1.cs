using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MAZEGAME;

public class Game1 : Game
{
    public enum Dir
    {
        Down,
        Up,
        Left,
        Right,
        None
    }

    private Texture2D playerImage;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D sprites;
    private Vector2 position;
    private int positionInArrayX;
    private int positionInArrayY;
    private Dir currentDirection;
    private Rectangle currentPacMan;
    private int pacManIteration;
    private Rectangle[] pacManUps = new Rectangle[3];
    private Rectangle[] pacManDowns = new Rectangle[3];
    private Rectangle[] pacManRights = new Rectangle[3];
    private Rectangle[] pacManLefts = new Rectangle[3];
    private int timeElapsed;
    private Tile[,] tileArray = new Tile[28, 31];
    private bool isPacManMoving;

    private int[,] mapDesign = new int[,]{
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,3,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,1,1,1,5,1,1,5,1,1,1,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,2,2,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 0,0,0,0,0,0,0,5,5,5,1,2,2,2,2,2,2,1,5,5,5,0,0,0,0,0,0,0},
            { 1,1,1,1,1,1,0,1,1,5,1,2,2,2,2,2,2,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,5,5,5,5,5,5,5,5,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,1,1,1,1,1,0,1,1,5,1,1,1,1,1,1,1,1,5,1,1,0,1,1,1,1,1,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1},
            { 1,3,0,0,1,1,0,0,0,0,0,0,0,5,5,0,0,0,0,0,0,0,1,1,0,0,3,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1},
            { 1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1},
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 680;
        _graphics.PreferredBackBufferHeight = 780;
        _graphics.ApplyChanges();

        // Initial position and direction of Pacman
        // position = new Vector2(315, 140);
        positionInArrayX = 13;
        positionInArrayY = 5;

        currentDirection = Dir.Right;

        // Position of pacman in the sprite sheet
        currentPacMan = new Rectangle(1419, 3, 39, 39);

        // Inital of pacman (mouth wide open) 
        pacManIteration = 0;
        timeElapsed = 0;
        isPacManMoving = true;

        base.Initialize();
    }

    protected override void LoadContent()
    {   
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        playerImage=Content.Load<Texture2D>("maincharacter");
        sprites = Content.Load<Texture2D>("spriteSheet");

        // Different pacman mouth open/closed in all directions
        pacManDowns[0] = new Rectangle(1371, 147, 39, 39);
        pacManDowns[1] = new Rectangle(1419, 147, 39, 39);
        pacManDowns[2] = new Rectangle(1467, 3, 39, 39);

        pacManUps[0] = new Rectangle(1371, 99, 39, 39);
        pacManUps[1] = new Rectangle(1419, 99, 39, 39);
        pacManUps[2] = new Rectangle(1467, 3, 39, 39);

        pacManLefts[0] = new Rectangle(1371, 51, 39, 39);
        pacManLefts[1] = new Rectangle(1419, 51, 39, 39);
        pacManLefts[2] = new Rectangle(1467, 3, 39, 39);

        pacManRights[0] = new Rectangle(1371, 3, 39, 39);
        pacManRights[1] = new Rectangle(1419, 3, 39, 39);
        pacManRights[2] = new Rectangle(1467, 3, 39, 39);

        SetUpTileArray();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

                    // Change direction of pacman when pressing keyboard keys
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Up))
            {
                currentDirection = Dir.Up;
                // currentPacMan = new Rectangle(1419, 99, 39, 39);
            }
            else if (kState.IsKeyDown(Keys.Down))
            {
                currentDirection = Dir.Down;
                // currentPacMan = new Rectangle(1419, 147, 39, 39);
            }
            else if (kState.IsKeyDown(Keys.Right))
            {
                currentDirection = Dir.Right;
                // currentPacMan = new Rectangle(1419, 3, 39, 39);
            }
            else if (kState.IsKeyDown(Keys.Left))
            {
                currentDirection = Dir.Left;
                // currentPacMan = new Rectangle(1419, 51, 39, 39);
            }

        int toBeX = positionInArrayX;
        int toBeY = positionInArrayY;

        // Don't update pacman every update but every three updates (makes the animation slower)
        if (timeElapsed == 8) 
        {
            timeElapsed = 0;
            // TODO: Add your update logic here

            // Move pacman in the direction we are in currently and change which pacman to use (mouth more open/ more closed)
            if (currentDirection == Dir.Down)
            {
                // position.Y += 5;
                toBeY += 1;
                currentPacMan = pacManDowns[pacManIteration];
            }
            else if (currentDirection == Dir.Up)
            {
                // position.Y -= 5;
                toBeY -= 1;
                currentPacMan = pacManUps[pacManIteration];
            }
            else if (currentDirection == Dir.Right)
            {
                // position.X += 5;
                toBeX += 1;
                // if (position.Y > 340 && position.Y < 360 && position.X > 655)
                // {
                //     position.X = 0;
                // }
                if (toBeY == 14 && toBeX > 27)
                {
                    toBeX = 0;
                }
                currentPacMan = pacManRights[pacManIteration];
            }
            else if (currentDirection == Dir.Left)
            {
                // position.X -= 5;
                toBeX -= 1;
                if (toBeY == 14 && toBeX < 0)
                {
                    toBeX = 27;
                }
                currentPacMan = pacManLefts[pacManIteration];
            }
            pacManIteration = (pacManIteration + 1) % 3;
        }

        if (tileArray[toBeX, toBeY].tileType != Tile.TileType.Wall && tileArray[toBeX, toBeY].tileType != Tile.TileType.GhostHouse)
        {
            positionInArrayX = toBeX;
            positionInArrayY = toBeY;
        }

        timeElapsed += 1;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        // Draw the maze
        _spriteBatch.Draw(sprites, new Vector2(5, 32), new Rectangle(684,0,672,744), Color.White);
        // Draw pacman
        // _spriteBatch.Draw(sprites, position, currentPacMan, Color.White);
        _spriteBatch.Draw(sprites, tileArray[positionInArrayX, positionInArrayY].getPosition(), currentPacMan, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);

    }

    private void SetUpTileArray() 
    {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (mapDesign[y, x] == 0) // small snack
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Snack);
                }
                else if (mapDesign[y, x] == 1) // wall collider
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Wall);
                }
                else if (mapDesign[y, x] == 2) //  ghost house
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.GhostHouse);
                }
                else if (mapDesign[y, x] == 3) // big snack
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.Snack);
                }
                else if (mapDesign[y, x] == 5) // empty
                {
                    tileArray[x, y] = new Tile(new Vector2(x * 24, y * 24 + 27), Tile.TileType.None);
                }
            }
        }
    }


}
