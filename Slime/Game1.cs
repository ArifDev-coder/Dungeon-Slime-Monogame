using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Slime;
using Slime.Scenes;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;

namespace Slime;

/// <summary>
/// Class utama permainan "Dungeon Slime".
/// Menampilkan logo dan karakter slime di layar.
/// </summary>
public class Game1 : Core
{

    private void InitializeGum()
    {
        GumService.Default.Initialize(this, DefaultVisualsVersion.V3);
        GumService.Default.ContentLoader.XnaContentManager = Core.Content;

        FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);
        FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);
        FrameworkElement.TabReverseKeyCombos.Add(
            new KeyCombo() { PushedKey = Keys.Up }
        );
        FrameworkElement.TabKeyCombos.Add(
            new KeyCombo() { PushedKey = Keys.Down }
        );

        GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
        GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
        GumService.Default.Renderer.Camera.Zoom = 4.0f;
    }

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

        InitializeGum();

        ChangeScene(new TitleScene());
    }

    /// <summary>
    /// Memuat semua konten (gambar, suara, dll) yang diperlukan oleh permainan.
    /// Dijalankan sekali pada saat game dimulai.
    /// </summary>
    protected override void LoadContent()
    {
    }
}
