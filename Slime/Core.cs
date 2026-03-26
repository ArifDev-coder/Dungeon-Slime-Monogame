using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slime.Input;

namespace Slime;

public class Core : Game
{
    internal static Core slime_instance;

    public static Core Instance => slime_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }

    public static new GraphicsDevice GraphicsDevice { get; private set; }

    public static SpriteBatch SpriteBatch { get; private set; }

    public static new ContentManager Content { get; private set; }

    public static InputManager Input { get; private set; }

    public static bool ExitOnEscape { get; set; }

    public Core(string title, int width, int height, bool fullscreen)
    {
        if (slime_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        slime_instance = this;

        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = fullscreen
        };

        Graphics.ApplyChanges();

        Window.Title = title;

        Content = base.Content;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;

        ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;

        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Input = new InputManager();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);

        GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape) || ExitOnEscape && gamePadOne.IsButtonDown(Buttons.Back))
        {
            Exit();
        }

        base.Update(gameTime);
    }
}
