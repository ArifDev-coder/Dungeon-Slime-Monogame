using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using DungeonSlime.UI;

namespace DungeonSlime.Scenes;

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

    // UI
    private SoundEffect _uiSoundEffect;
    private Panel _titleScreenButtonsPanel;
    private Panel _optionsPanel;

    // private Button _optionsButton;
    // private Button _optionsBackButton;
    private AnimatedButton _optionsButton;
    private AnimatedButton _optionsBackButton;
    private TextureAtlas _UI;

    private void CreateTitlePanel()
    {
        _titleScreenButtonsPanel = new Panel();
        _titleScreenButtonsPanel.Dock(Gum.Wireframe.Dock.Fill);
        _titleScreenButtonsPanel.AddToRoot();

        AnimatedButton startButton = new AnimatedButton(_UI);
        startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        startButton.X = 50;
        startButton.Y = -12;
        startButton.Width = 50;
        startButton.Text = "Start";
        startButton.Click += HandleStartClicked;
        _titleScreenButtonsPanel.AddChild(startButton);

        _optionsButton = new AnimatedButton(_UI);
        _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsButton.X = -50;
        _optionsButton.Y = -12;
        _optionsButton.Width = 50;
        _optionsButton.Text = "Options";
        _optionsButton.Click += HandleOptionsClicked;
        _titleScreenButtonsPanel.AddChild(_optionsButton);

        startButton.IsFocused = true;
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        Core.ChangeScene(new GameScene());
    }

    private void HandleOptionsClicked(object sender, EventArgs e)
    {
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        _titleScreenButtonsPanel.IsVisible = false;
        _optionsPanel.IsVisible = true;
        _optionsBackButton.IsFocused = true;
    }

    private void CreateOptionsPanel()
    {
        _optionsPanel = new Panel();
        _optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
        _optionsPanel.IsVisible = false;
        _optionsPanel.AddToRoot();

        TextRuntime optionsText = new TextRuntime();
        optionsText.X = 10;
        optionsText.Y = 10;
        optionsText.Text = "OPTIONS";
        optionsText.UseCustomFont = true;
        optionsText.FontScale = 0.5f;
        optionsText.CustomFontFile = @"fonts/04b_30.fnt";
        _optionsPanel.AddChild(optionsText);

        OptionsSlider musicSlider = new OptionsSlider(_UI);
        musicSlider.Name = "MusicSlider";
        musicSlider.Text = "MUSIC";
        musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
        musicSlider.Y = 30f;
        musicSlider.Minimum = 0;
        musicSlider.Maximum = 1;
        musicSlider.Value = Core.Audio.SongVolume;
        musicSlider.SmallChange = .1;
        musicSlider.LargeChange = .2;
        musicSlider.ValueChanged += HandleMusicsSliderValueChanged;
        musicSlider.ValueChangeCompleted += HandleMusicsSliderValueChangeCompleted;
        _optionsPanel.AddChild(musicSlider);

        OptionsSlider sfxSlider = new OptionsSlider(_UI);
        sfxSlider.Name = "SfxSlider";
        sfxSlider.Text = "SFX";
        sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
        sfxSlider.Y = 93;
        sfxSlider.Minimum = 0;
        sfxSlider.Maximum = 1;
        sfxSlider.Value = Core.Audio.SoundEffectVolume;
        sfxSlider.SmallChange = .1;
        sfxSlider.LargeChange = .2;
        sfxSlider.ValueChanged += HandleSfxSliderChanged;
        sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
        _optionsPanel.AddChild(sfxSlider);

        _optionsBackButton = new AnimatedButton(_UI);
        _optionsBackButton.Text = "BACK";
        _optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsBackButton.X = -28f;
        _optionsBackButton.Y = -10f;
        _optionsBackButton.Click += HandleOptionsButtonBack;
        _optionsPanel.AddChild(_optionsBackButton);
    }

    private void HandleSfxSliderChanged(object sender, EventArgs args)
    {
        var slider = (Slider)sender;

        Core.Audio.SoundEffectVolume = (float)slider.Value;
    }

    private void HandleSfxSliderChangeCompleted(object sender, EventArgs e)
    {
        Core.Audio.PlaySoundEffect(_uiSoundEffect);
    }

    private void HandleMusicsSliderValueChanged(object sender, EventArgs args)
    {
        var slider = (Slider)sender;

        Core.Audio.SongVolume = (float)slider.Value;
    }

    private void HandleMusicsSliderValueChangeCompleted(object sender, EventArgs args)
    {
        Core.Audio.PlaySoundEffect(_uiSoundEffect);
    }

    private void HandleOptionsButtonBack(object sender, EventArgs e)
    {
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        _titleScreenButtonsPanel.IsVisible = true;
        _optionsPanel.IsVisible = false;
        _optionsButton.IsFocused = true;
    }

    private void InitializeUI()
    {
        GumService.Default.Root.Children.Clear();

        CreateTitlePanel();
        CreateOptionsPanel();
    }

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

        InitializeUI();
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        _backgroundPattern = Content.Load<Texture2D>("images/title/bg-pattern");
        _titleGameSong = Content.Load<Song>("audio/theme3");

        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
        _UI = TextureAtlas.FromFile(Core.Content, "ui.xml");
    }

    public override void Update(GameTime gameTime)
    {
        // if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        // {
        //     Core.ChangeScene(new GameScene());
        // }

        float offset = _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.X -= offset;
        _backgroundOffset.Y -= offset;

        _backgroundOffset.X %= _backgroundPattern.Width;
        _backgroundOffset.Y %= _backgroundPattern.Height;

        GumService.Default.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(
            _backgroundPattern,
            _backgroundDestination,
            new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size),
            Color.White * 0.5f
        );
        Core.SpriteBatch.End();

        if (_titleScreenButtonsPanel.IsVisible)
        {
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Color dropShadowColor = Color.Black * 0.5f;

            Core.SpriteBatch.DrawString(
                _font,
                DUNGEON_TEXT,
                _dungeonTextPos + new Vector2(10, 10),
                dropShadowColor,
                0.0f,
                _dungeonTextOrigin,
                titleMultiplyFontSize,
                SpriteEffects.None,
                1.0f
            );

            Core.SpriteBatch.DrawString(
                _font,
                DUNGEON_TEXT,
                _dungeonTextPos,
                Color.White,
                0.0f,
                _dungeonTextOrigin,
                titleMultiplyFontSize,
                SpriteEffects.None,
                1.0f
            );

            Core.SpriteBatch.DrawString(
                _font,
                SLIME_TEXT,
                _slimeTextPos + new Vector2(10, 10),
                dropShadowColor,
                0.0f,
                _slimeTextOrigin,
                titleMultiplyFontSize,
                SpriteEffects.None,
                1.0f
            );

            Core.SpriteBatch.DrawString(
                _font,
                SLIME_TEXT,
                _slimeTextPos,
                Color.White,
                0.0f,
                _slimeTextOrigin,
                titleMultiplyFontSize,
                SpriteEffects.None,
                1.0f
            );

            Core.SpriteBatch.End();
        }

        GumService.Default.Draw();
    }
}

