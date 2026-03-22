using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Slime.Graphics;

namespace Slime;

/// <summary>
/// Class utama permainan "Dungeon Slime".
/// Menampilkan logo dan karakter slime di layar.
/// </summary>
public class Game1 : Core
{
    /// <summary>
    /// Tekstur karakter slime yang akan dimainkan dalam permainan.
    /// </summary>
    private AnimatedSprite _slime;
    private AnimatedSprite _bat;

    private Vector2 _slimePosition;
    private Vector2 _batPosition;
    private const float MOVEMENT_SPEED = 1.0f;

    // Input Buffer
    private Queue<Vector2> _inputBuffer;
    private const int MAX_BUFFER_SIZE = 2;
    private KeyboardState _previousKeyboardState;

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
        _slime.Scale = new Vector2(4.0f, 4.0f);

        _bat = entityAtlas.CreateAnimatedSprite("bat_basic");
        _bat.Scale = new Vector2(4.0f, 4.0f);
    }

    /// <summary>
    /// Dijalankan setiap frame untuk memperbarui logika permainan (input, gerakan, dll).
    /// Mengecek apakah pemain menekan tombol Escape atau back button untuk keluar dari game.
    /// </summary>
    /// <param name="gameTime">Informasi tentang waktu permainan (delta time, total time)</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

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
        Vector2 targetPosition = _slimePosition;

        _batPosition += (targetPosition - _batPosition) * 0.005f;
    }

    private void CheckKeyboardInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        Vector2 direction = Vector2.Zero;

        float speed = MOVEMENT_SPEED;

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            direction.Y -= 1;
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            direction.Y += 1;
        }
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            direction.X -= 1;
        }
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            direction.X += 1;
        }

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }


        if (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
        {   
            speed *= 100;
        }
        else
        {
            speed = MOVEMENT_SPEED;
        }

        _slimePosition += direction * speed;

        _previousKeyboardState = keyboardState;
    }

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
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        float speed = MOVEMENT_SPEED;
        if (gamePadState.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        if (gamePadState.ThumbSticks.Left != Vector2.Zero)
        {
            _slimePosition.X += gamePadState.ThumbSticks.Left.X * speed;
            _slimePosition.Y -= gamePadState.ThumbSticks.Left.Y * speed;
        }
        else
        {
            if (gamePadState.IsButtonDown(Buttons.DPadUp))
            {
                _slimePosition.Y -= speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadDown))
            {
                _slimePosition.Y += speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                _slimePosition.X -= speed;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
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
