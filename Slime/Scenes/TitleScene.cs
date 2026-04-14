using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Slime;
using Slime.Scenes;
using Slime.Audio;
using Slime.Audio;

namespace Slime.Scenes;

public class TitleScene : Scene
{
    private const string DUNGEON_TEXT = "Dungeon";
    private const string SLIME_TEXT = "Slime";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";
    private const float titleMultiplyFontSize = 5.0f;

    private SpriteFont _font;

    private Vector2 _dungeonTextPos;
    private Vector2 _dungeonTextOrigin;
    private Vector2 _slimeTextPos;
    private Vector2 _slimeTextOrigin;
    private Vector2 _pressEnterPos;
    private Vector2 _pressEnterOrigin;

    // Audio
    private Song _titleGameSong;

    private Texture2D _backgroundPattern;
    private Rectangle _backgroundDestination;
    private Vector2 _backgroundOffset;
    private float _scrollSpeed = 50.0f;

    // Audio
    private Song _titleGameSong;

    private Texture2D _backgroundPattern;
    private Rectangle _backgroundDestination;
    private Vector2 _backgroundOffset;
    private float _scrollSpeed = 50.0f;

    public override void Initialize()
    {
        base.Initialize();

        Core.ExitOnEscape = true;

        Vector2 size = _font.MeasureString(DUNGEON_TEXT);
        _dungeonTextPos = new(640, 100);
        _dungeonTextOrigin = size * 0.5f;

        size = _font.MeasureString(SLIME_TEXT);
        _slimeTextPos = new(757, 207);
        _slimeTextOrigin = size * 0.5f;

        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new(640, 620);
        _pressEnterOrigin = size * 0.5f;

        _backgroundOffset = Vector2.Zero;
        _backgroundDestination = Core.GraphicsDevice.PresentationParameters.Bounds;
        Core.Audio.PlaySong(_titleGameSong);
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        _backgroundPattern = Content.Load<Texture2D>("images/title/bg-pattern");
        _titleGameSong = Content.Load<Song>("audio/theme3");

    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }

        float offset = _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.X -= offset;
        _backgroundOffset.Y -= offset;

        _backgroundOffset.X %= _backgroundPattern.Width;
        _backgroundOffset.Y %= _backgroundPattern.Height;
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(_backgroundPattern, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White * 0.5f);
        Core.SpriteBatch.End();

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(_backgroundPattern, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White * 0.5f);
        Core.SpriteBatch.End();

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Color dropShadowColor = Color.Black * 0.5f;

        Core.SpriteBatch.DrawString(_font, DUNGEON_TEXT, _dungeonTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _dungeonTextOrigin, titleMultiplyFontSize, SpriteEffects.None, 1.0f);

        Core.SpriteBatch.DrawString(_font, DUNGEON_TEXT, _dungeonTextPos, Color.White, 0.0f, _dungeonTextOrigin, titleMultiplyFontSize, SpriteEffects.None, 1.0f);

        Core.SpriteBatch.DrawString(_font, SLIME_TEXT, _slimeTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _slimeTextOrigin, titleMultiplyFontSize, SpriteEffects.None, 1.0f);

        Core.SpriteBatch.DrawString(_font, SLIME_TEXT, _slimeTextPos, Color.White, 0.0f, _slimeTextOrigin, titleMultiplyFontSize, SpriteEffects.None, 1.0f);

        Core.SpriteBatch.DrawString(_font, PRESS_ENTER_TEXT, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        Core.SpriteBatch.End();
    }
}