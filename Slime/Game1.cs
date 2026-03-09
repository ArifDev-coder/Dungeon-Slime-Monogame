using System;
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
    /// Tekstur logo "Se" yang ditampilkan di atas layar.
    /// </summary>
    private Texture2D _logo_se;

    /// <summary>
    /// Tekstur karakter slime yang akan dimainkan dalam permainan.
    /// </summary>
    private AnimatedSprite _slime;

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

        TextureAtlas slimeMove = TextureAtlas.FromFile(Content, "atlas-definition.xml");

        // _slime0 = atlas.GetRegion("slime0");
        // _slime1 = atlas.GetRegion("slime1");

        _slime = slimeMove.CreateAnimatedSprite("slime_move");
        _slime.Scale = new Vector2(4.0f, 4.0f);

        _logo_se = Content.Load<Texture2D>("images/logo");
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

        base.Update(gameTime);
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

        SpriteBatch.Draw(_logo_se,
            new Vector2(
                Window.ClientBounds.Width,
                10) * 0.5f,
            null,
            Color.White,
            0.0f,
            new Vector2(
                _logo_se.Width,
                0) * 0.5f,
            0.4f,
            SpriteEffects.None,
            0.0f
        );

        _slime.Draw(SpriteBatch, new Vector2(_slime.Width + 10, _slime.Height + 10));

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
