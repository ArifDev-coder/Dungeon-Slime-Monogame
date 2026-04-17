using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime.Graphics;

public class TextureRegion
{
    /// <summary>
    /// Menyimpan tekstur gambar (gambar 2D) yang akan digambar di layar.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Menyimpan area atau bagian dari tekstur yang ingin ditampilkan.
    /// Ini memungkinkan kami hanya menampilkan sebagian kecil dari tekstur besar.
    /// </summary>
    public Rectangle SourceRectangle { get; set; }

    /// <summary>
    /// Mendapatkan lebar (width) dari area yang akan ditampilkan.
    /// </summary>
    public int Width => SourceRectangle.Width;

    /// <summary>
    /// Mendapatkan tinggi (height) dari area yang akan ditampilkan.
    /// </summary>
    public int Height => SourceRectangle.Height;

    public float TopTextureCoordinate => SourceRectangle.Top / (float)Texture.Height;

    public float BottomTextureCoordinate => SourceRectangle.Bottom / (float)Texture.Height;

    public float LeftTextureCoodinate => SourceRectangle.Left / (float)Texture.Width;

    public float RightTextureCoordinate => SourceRectangle.Right / (float)Texture.Width;

    /// <summary>
    /// Konstruktor kosong. Gunakan ini jika ingin membuat TextureRegion tanpa data awal.
    /// </summary>
    public TextureRegion() { }

    /// <summary>
    /// Membuat TextureRegion dengan tekstur dan area yang ditentukan.
    /// </summary>
    /// <param name="texture">Gambar tekstur yang akan digunakan</param>
    /// <param name="x">Posisi horizontal (dari kiri) di dalam tekstur</param>
    /// <param name="y">Posisi vertikal (dari atas) di dalam tekstur</param>
    /// <param name="width">Lebar area yang ingin ditampilkan</param>
    /// <param name="height">Tinggi area yang ingin ditampilkan</param>
    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new Rectangle(x, y, width, height);
    }

    /// <summary>
    /// Menggambar tekstur ke layar dengan posisi dan warna yang ditentukan.
    /// Cara paling sederhana untuk menggambar tanpa efek tambahan.
    /// </summary>
    /// <param name="spriteBatch">Alat untuk menggambar sprite di layar</param>
    /// <param name="position">Posisi (x, y) di layar tempat gambar akan ditampilkan</param>
    /// <param name="color">Warna yang akan diterapkan pada gambar</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    /// Menggambar tekstur dengan efek rotasi dan skala (pembesaran/pengecilan).
    /// Parameter skala akan diterapkan sama untuk lebar dan tinggi.
    /// </summary>
    /// <param name="spriteBatch">Alat untuk menggambar sprite di layar</param>
    /// <param name="position">Posisi (x, y) di layar tempat gambar akan ditampilkan</param>
    /// <param name="color">Warna yang akan diterapkan pada gambar</param>
    /// <param name="rotation">Sudut putaran dalam radian (0 = tidak diputar)</param>
    /// <param name="origin">Titik pusat untuk rotasi dan skala</param>
    /// <param name="scale">Ukuran gambar (1.0 = ukuran normal, 2.0 = 2x lebih besar)</param>
    /// <param name="effects">Efek khusus seperti flip horizontal atau vertikal</param>
    /// <param name="layerDepth">Kedalaman layer (0 = depan, 1 = belakang) untuk menentukan urutan gambar</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        Draw(spriteBatch, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
    }

    /// <summary>
    /// Menggambar tekstur dengan kontrol penuh terhadap semua aspek visual.
    /// Ini adalah method utama yang sebenarnya melakukan penggambaran.
    /// </summary>
    /// <param name="spriteBatch">Alat untuk menggambar sprite di layar</param>
    /// <param name="position">Posisi (x, y) di layar tempat gambar akan ditampilkan</param>
    /// <param name="color">Warna yang akan diterapkan pada gambar</param>
    /// <param name="rotation">Sudut putaran dalam radian (0 = tidak diputar)</param>
    /// <param name="origin">Titik pusat untuk rotasi dan skala</param>
    /// <param name="scale">Ukuran gambar untuk lebar dan tinggi secara terpisah</param>
    /// <param name="effects">Efek khusus seperti flip horizontal atau vertikal</param>
    /// <param name="layerDepth">Kedalaman layer (0 = depan, 1 = belakang) untuk menentukan urutan gambar</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        spriteBatch.Draw(
            Texture,
            position,
            SourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );
    }
}