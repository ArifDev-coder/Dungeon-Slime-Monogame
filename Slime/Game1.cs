using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Slime;
using Slime.Input;
using Slime.Graphics;
using Slime.Scenes;

namespace Slime;

/// <summary>
/// Class utama permainan "Dungeon Slime".
/// Menampilkan logo dan karakter slime di layar.
/// </summary>
public class Game1 : Core
{
    // Audio
    private Song _themeSong;


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

        Audio.PlaySong(_themeSong);

        ChangeScene(new TitleScene());
    }

    /// <summary>
    /// Memuat semua konten (gambar, suara, dll) yang diperlukan oleh permainan.
    /// Dijalankan sekali pada saat game dimulai.
    /// </summary>
    protected override void LoadContent()
    {
        _themeSong = Content.Load<Song>("audio/theme2");
    }
}
