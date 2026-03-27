using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Slime.Graphics;

public class Tilemap
{
    private readonly Tileset _tileset;
    private readonly int[] _tiles;

    public int Rows { get; }
    public int Columns { get; }
    public int Count { get; }
    public Vector2 Scale { get; set; }
    public float TileWidth => _tileset.TileWidth * Scale.X;
    public float TileHeigth => _tileset.TileHeigth * Scale.Y;

    public Tilemap(Tileset tileset, int columns, int rows)
    {
        _tileset = tileset;
        Rows = rows;
        Columns = columns;
        Count = Columns * Rows;
        Scale = Vector2.One;
        _tiles = new int[Count];
    }

    public void SetTile(int column, int row, int tilesetID)
    {
        int index = row * Columns + column;
        SetTile(index, tilesetID);
    }

    public TextureRegion GetTile(int column, int row)
    {
        int index= row * Columns + column;
        return GetTile(index);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Count; i++)
        {
            int tilesetIndex = _tiles[i];
            TextureRegion tile = _tileset.GetTile(tilesetIndex);

            int x = i % Columns;
            int y = i / Columns;

            Vector2 position = new Vector2(x * TileWidth, y * TileHeigth);
            tile.Draw(spriteBatch, position, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
        }
    }

    public static Tilemap FromFile(ContentManager content, string filename)
    {
        string filePath = Path.Combine(content.RootDirectory, filename);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // <Tileset> element memiliki informasi tentang tileset
                // digunakan untuk tilemap.
                //
                // Contoh
                // <Tileset region="0 0 100 100" tileWidth="10" tileHeight="10">contentPath</Tilese>
                //
                // Atribut Region merepresentasikan x, y, width, dan height
                //
                // tileWidh dan tileHeight atribut adalah width dan height dari setiap tile dalam tileset.
                //
                // contentPath value adalah contentPath pada tesktur untuk memuata konten tileset
                // ya begitu lah.
                XElement tilesetElement = root.Element("Tileset");

                string regionAttribute = tilesetElement.Attribute("region").Value;
                string[] split = regionAttribute.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                int width = int.Parse(split[2]);
                int height = int.Parse(split[3]);

                int tileWidth = int.Parse(tilesetElement.Attribute("tileWidth"));
                int tileHeight = int.Parse(tilesetElement.Attribute("tileHeight"));
                string contentPath = tilesetElement.Value;

                Texture2D texture = content.Load<Texture2D>(contentPath);

                TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);

                Tileset tileset = new Tileset(textureRegion, tileWidth, tileHeight);
            }
        }
    }
}