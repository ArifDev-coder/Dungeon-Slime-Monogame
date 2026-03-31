using Microsoft.Xna.Framework.Graphics;

namespace Slime.Graphics;

public class Tileset
{
    private readonly TextureRegion[] _tiles;

    public int TileWidth { get; }
    public int TileHeight { get; }
    public int Columns { get; }
    public int Rows { get; }
    public int Count { get; }

    public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeigth)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeigth;
        Columns = textureRegion.Width / tileWidth;
        Rows = textureRegion.Height / tileHeigth;
        Count = Columns * Rows;

        _tiles = new TextureRegion[Count];

        for (int i = 0; i < Count; i++)
        {
            int x = i % Columns * tileWidth;
            int y = i / Columns * tileHeigth;
            _tiles[i] = new TextureRegion(textureRegion.Texture2D, textureRegion.SourceRectangle.X + x, textureRegion.SourceRectangle.Y + y, tileWidth, tileHeigth);
        }
    }

    public TextureRegion GetTile(int index) => _tiles[index];

    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }
}