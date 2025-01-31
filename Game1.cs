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
    private Dir currentDirection;
    private Rectangle currentPacMan;
    private int pacManIteration;
    private Rectangle[] pacManUps = new Rectangle[3];
    private Rectangle[] pacManDowns = new Rectangle[3];
    private Rectangle[] pacManRights = new Rectangle[3];
    private Rectangle[] pacManLefts = new Rectangle[3];
    private int timeElapsed;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 672;
        _graphics.PreferredBackBufferHeight = 771;
        _graphics.ApplyChanges();

        // Initial position and direction of Pacman
        position = new Vector2(315, 140);
        currentDirection = Dir.Right;

        // Position of pacman in the sprite sheet
        currentPacMan = new Rectangle(1419, 3, 39, 39);

        // Inital of pacman (mouth wide open) 
        pacManIteration = 0;
        timeElapsed = 0;

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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

        // Don't update pacman every update but every three updates (makes the animation slower)
        if (timeElapsed == 3) 
        {
            timeElapsed = 0;
            // TODO: Add your update logic here

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

            // Move pacman in the direction we are in currently and change which pacman to use (mouth more open/ more closed)
            if (currentDirection == Dir.Down)
            {
                position.Y += 5;
                currentPacMan = pacManDowns[pacManIteration];
            }
            else if (currentDirection == Dir.Up)
            {
                position.Y -= 5;
                currentPacMan = pacManUps[pacManIteration];
            }
            else if (currentDirection == Dir.Right)
            {
                position.X += 5;
                currentPacMan = pacManRights[pacManIteration];
            }
            else if (currentDirection == Dir.Left)
            {
                position.X -= 5;
                currentPacMan = pacManLefts[pacManIteration];
            }
            pacManIteration = (pacManIteration + 1) % 3;
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
        _spriteBatch.Draw(sprites, new Vector2(0, 27), new Rectangle(684,0,672,744), Color.White);
        // Draw pacman
        _spriteBatch.Draw(sprites, position, currentPacMan, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);

    }
}
