using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slime.Graphics;
using Slime.Input;
using Slime;

namespace Slime;

/// <summary>
/// Class utama permainan "Dungeon Slime".
/// Menampilkan logo dan karakter slime di layar.
/// </summary>
public class Game1 : Core
{
    private AnimatedSprite _slime;
    private AnimatedSprite _bat;

    private Vector2 _slimePosition;
    private Vector2 _batPosition;
    private Vector2 _batVelocity;
    private Tilemap _tilemap;
    private Rectangle _roomBounds;

    private const float MOVEMENT_SPEED = 5.0f;

    // Input Buffer
    private Queue<Vector2> _inputBuffer;
    private const int MAX_BUFFER_SIZE = 2;


    /// <summary>
    /// Konstruktor untuk membuat game "Dungeon Slime" dengan ukuran 1280x720 pixel.
    /// Judul jendela akan menampilkan "Dungeon Slime".
    /// </summary>
    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    /// <summary>
    /// Dijalankan saat game pertama kali dimulai untuk setup awal.
    /// Memanggil method Initialize dari class parent (Core).
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        Rectangle screenBounds = GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new Rectangle(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeigth,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeigth * 2
        );

        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;

        // Init Slime Position at the center tile of the tile map.
        _slimePosition = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeigth);

        // Init Bat Position at the center tile of the tile map.
        _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

        AssignRandomBatVelocity();
    }

    /// <summary>
    /// Memuat semua konten (gambar, suara, dll) yang diperlukan oleh permainan.
    /// Dijalankan sekali pada saat game dimulai.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        TextureAtlas entityAtlas = TextureAtlas.FromFile(Content, "entity.xml");

        // _slime0 = atlas.GetRegion("slime0");
        // _slime1 = atlas.GetRegion("slime1");

        _slime = entityAtlas.CreateAnimatedSprite("slime_idle");
        _slime.Scale = new Vector2(1.0f, 1.0f);

        _bat = entityAtlas.CreateAnimatedSprite("bat_basic");
        _bat.Scale = new Vector2(1.0f, 1.0f);

        _tilemap = Tilemap.FromFile(Content, "tilemap-def.xml");
        _tilemap.Scale = new Vector2(5.0f, 5.0f);
    }

    /// <summary>
    /// Dijalankan setiap frame untuk memperbarui logika permainan (input, gerakan, dll).
    /// Mengecek apakah pemain menekan tombol Escape atau back button untuk keluar dari game.
    /// </summary>
    /// <param name="gameTime">Informasi tentang waktu permainan (delta time, total time)</param>
    protected override void Update(GameTime gameTime)
    {
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        // CheckKeyboardInputWithInputBufferTest();
        CheckKeyboardInput();
        CheckGamePadInput();

        EnemyAI();

        base.Update(gameTime);
    }

    private void EnemyAI()
    {
        // Rectangle screenBounds = new Rectangle(
        //     0,
        //     0,
        //     GraphicsDevice.PresentationParameters.BackBufferWidth,
        //     GraphicsDevice.PresentationParameters.BackBufferHeight
        // );

        Circle slimeBounds = new Circle(
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        // if (slimeBounds.Left < screenBounds.Left)
        if (slimeBounds.Left < _roomBounds.Left)
        {
            // _slimePosition.X = screenBounds.Left;
            _slimePosition.X = _roomBounds.Left;
        }
        // else if (slimeBounds.Right > screenBounds.Right)
        else if (slimeBounds.Right > _roomBounds.Right)
        {
            _slimePosition.X = _roomBounds.Right - _slime.Width;
            // _slimePosition.X = screenBounds.Right - _slime.Width;
        }

        // if (slimeBounds.Top < screenBounds.Top)
        if (slimeBounds.Top < _roomBounds.Top)
        {
            // _slimePosition.Y = screenBounds.Top;
            _slimePosition.Y = _roomBounds.Top;
        }
        // else if (slimeBounds.Bottom > screenBounds.Bottom)
        else if (slimeBounds.Bottom > _roomBounds.Bottom)
        {
            // _slimePosition.Y = screenBounds.Bottom - _slime.Height;
            _slimePosition.Y = _roomBounds.Bottom - _slime.Height;
        }

        Vector2 newBatPosition = _batPosition + _batVelocity;

        Circle batBounds = new Circle(
            (int)(newBatPosition.X + (_bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );

        Vector2 normal = Vector2.Zero;

        if (batBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Left;
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Right - _bat.Width;
        }

        if (batBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Top;
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Bottom - _bat.Height;
        }

        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _batVelocity = Vector2.Reflect(_batVelocity, normal);
        }

        _batPosition = newBatPosition;

        if (slimeBounds.Intersects(batBounds))
        {
            // int totalColums = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            // int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            // int column = Random.Shared.Next(0, totalColums);
            // int row = Random.Shared.Next(0, totalRows);

            int column = Random.Shared.Next(1, _tilemap.Columns - 1);
            int row = Random.Shared.Next(1, _tilemap.Rows - 1);

            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            AssignRandomBatVelocity();
        }
    }

    private void AssignRandomBatVelocity()
    {
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        _batVelocity = direction * MOVEMENT_SPEED;
    }

    private void CheckKeyboardInput()
    {
        Vector2 direction = Vector2.Zero;

        float speed = MOVEMENT_SPEED;

        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        {
            direction.Y -= 1;
        }
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        {
            direction.Y += 1;
        }
        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            direction.X -= 1;
        }
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            direction.X += 1;
        }

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }


        if (Input.Keyboard.WasKeyJustPressed(Keys.Space))
        {
            speed *= 10.0f;
        }
        else
        {
            speed = MOVEMENT_SPEED;
        }

        _slimePosition += direction * speed;
    }

    // ! Deprecated
    private void CheckKeyboardInputWithInputBufferTest()
    {
        _inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);

        KeyboardState keyboardState = Keyboard.GetState();
        Vector2 newDirection = Vector2.Zero;

        float speed = MOVEMENT_SPEED;
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            newDirection = -Vector2.UnitY;
        }
        else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            newDirection = Vector2.UnitY;
        }
        else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            newDirection = -Vector2.UnitX;
        }
        else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            newDirection = Vector2.UnitX;
        }

        if (newDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
        {
            _inputBuffer.Enqueue(newDirection);
        }

        if (_inputBuffer.Count > 0)
        {
            Vector2 nextDirection = _inputBuffer.Dequeue();
            _slimePosition += nextDirection * speed;
        }
    }

    private void CheckGamePadInput()
    {
        GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

        float speed = MOVEMENT_SPEED;
        if (gamePadOne.WasButtonJustPressed(Buttons.A))
        {
            speed *= 1.5f;
            gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
        }
        else
        {
            gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
        }

        if (gamePadOne.LeftThumbStick != Vector2.Zero)
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * speed;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
        }
        else
        {
            if (gamePadOne.IsButtonDown(Buttons.DPadUp))
            {
                _slimePosition.Y -= speed;
            }

            if (gamePadOne.IsButtonDown(Buttons.DPadDown))
            {
                _slimePosition.Y += speed;
            }

            if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
            {
                _slimePosition.X -= speed;
            }

            if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
            {
                _slimePosition.X += speed;
            }
        }
    }

    /// <summary>
    /// Dijalankan setiap frame untuk menggambar semua objek visual ke layar.
    /// Menampilkan logo dan karakter slime pada posisi yang ditentukan.
    /// </summary>
    /// <param name="gameTime">Informasi tentang waktu permainan (delta time, total time)</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        // Rectangle IconSourceRect = new(0, 0, 128, 128);
        // Rectangle WordmarkSourceRect = new(150, 34, 458, 58);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp); // sortMode: SpriteSortMode.BackToFront

        // SpriteBatch.Draw(_logo_mg,                     // Texture
        //     new Vector2(                            // Position
        //         Window.ClientBounds.Width,
        //         Window.ClientBounds.Height) * 0.5f,
        //     IconSourceRect,                                   // SourceRectangle
        //     Color.White * 0.9f,                     // Color
        //     0.0f,                                   // Rotation (MathHelper.ToRadians(0))
        //     new Vector2(
        //         IconSourceRect.Width,
        //         IconSourceRect.Height) * 0.5f,               // Origin
        //     1.0f,                                   // Scale (bisa paka new Vector2(x, y))
        //     SpriteEffects.None,                     // Effects (Flip horizonal or vertical) SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally
        //     1.0f                                    // LayerDepth (seperti z-index di html)
        // );

        // SpriteBatch.Draw(_logo_mg,                     // Texture
        //     new Vector2(                            // Position
        //         Window.ClientBounds.Width,
        //         Window.ClientBounds.Height) * 0.5f,
        //     WordmarkSourceRect,                                   // SourceRectangle
        //     Color.White * 0.9f,                     // Color
        //     0.0f,                                   // Rotation (MathHelper.ToRadians(0))
        //     new Vector2(
        //         WordmarkSourceRect.Width,
        //         WordmarkSourceRect.Height) * 0.5f,               // Origin
        //     1.0f,                                   // Scale (bisa paka new Vector2(x, y))
        //     SpriteEffects.None,                     // Effects (Flip horizonal or vertical) SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally
        //     0.0f                                    // LayerDepth
        // );

        // _slime0.Draw(SpriteBatch, Vector2.Zero, Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 0.0f);
        // _slime1.Draw(
        //     SpriteBatch,
        //     new Vector2(
        //         500, 500
        //     ),
        //     Color.White,
        //     0.0f,
        //     Vector2.One,
        //     4.0f,
        //     SpriteEffects.None,
        //     0.0f
        // );

        _tilemap.Draw(SpriteBatch);

        _slime.Draw(SpriteBatch, _slimePosition);
        _bat.Draw(SpriteBatch, _batPosition);

        // SpriteBatch.Draw(_logo_se,
        //     new Vector2(
        //         Window.ClientBounds.Width,
        //         10) * 0.5f,
        //     null,
        //     Color.White,
        //     0.0f,
        //     new Vector2(
        //         _logo_se.Width,
        //         0) * 0.5f,
        //     0.4f,
        //     SpriteEffects.None,
        //     1.0f
        // );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
