using System;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using Gum.Graphics.Animation;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.GueDeriving;
using Slime.Graphics;

namespace Slime.UI;


internal class AnimatedButton : Button
{
    public AnimatedButton(TextureAtlas atlas)
    {
        ButtonVisual buttonVisual = (ButtonVisual)Visual;

        buttonVisual.Height = 14f;
        buttonVisual.HeightUnits = DimensionUnitType.Absolute;
        buttonVisual.Width = 21f;
        buttonVisual.WidthUnits = DimensionUnitType.RelativeToChildren;

        NineSliceRuntime background = buttonVisual.Background;
        background.Texture = atlas.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.Color = Color.White;

        TextRuntime  textInstance = buttonVisual.TextInstance;
        textInstance.Text = "START";
        textInstance.Blue = 130;
        textInstance.Green = 86;
        textInstance.Red = 70;
        textInstance.UseCustomFont = true;
        textInstance.CustomFontFile = "fonts/04b_30.fnt";
        textInstance.FontScale = 0.25f;
        textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        textInstance.Width = 0;
        textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;

        TextureRegion unfocusedTextureRegion = atlas.GetRegion("unfocused-button");

        AnimationChain unfocusedAnimation = new AnimationChain();
        unfocusedAnimation.Name = nameof(unfocusedAnimation);
        AnimationFrame unfocusedFrame = new AnimationFrame
        {
            TopCoordinate = unfocusedTextureRegion.TopTextureCoordinate,
            BottomCoordinate = unfocusedTextureRegion.BottomTextureCoordinate,
            LeftCoordinate = unfocusedTextureRegion.LeftTextureCoodinate,
            RightCoordinate = unfocusedTextureRegion.RightTextureCoordinate,
            FrameLength = .3f,
            Texture = unfocusedTextureRegion.Texture
        };
        unfocusedAnimation.Add(unfocusedFrame);

        Animation focusedAtlasAnimation = atlas.GetAnimation("focused-button-animation");

        AnimationChain focusedAnimation = new AnimationChain();
        focusedAnimation.Name = nameof(focusedAnimation);
        foreach (TextureRegion region in focusedAtlasAnimation.Frames)
        {
            AnimationFrame frame = new AnimationFrame
            {
                TopCoordinate = region.TopTextureCoordinate,
                BottomCoordinate = region.BottomTextureCoordinate,
                LeftCoordinate = region.LeftTextureCoodinate,
                RightCoordinate = region.RightTextureCoordinate,
                FrameLength = (float)focusedAtlasAnimation.Delay.TotalSeconds,
                Texture = region.Texture
            };

            focusedAnimation.Add(frame);
        }

        background.AnimationChains = new AnimationChainList
        {
            unfocusedAnimation,
            focusedAnimation
        };

        buttonVisual.ButtonCategory.ResetAllStates();

        StateSave enableState = buttonVisual.States.Enabled;
        enableState.Apply = () =>
        {
            background.CurrentChainName = unfocusedAnimation.Name;
        };

        StateSave focusedState = buttonVisual.States.Focused;
        focusedState.Apply = () =>
        {
            background.CurrentChainName = focusedAnimation.Name;
            background.Animate = true;
        };

        StateSave highlightedFocused = buttonVisual.States.Highlighted;
        highlightedFocused.Apply = focusedState.Apply;

        StateSave highlighted = buttonVisual.States.Highlighted;
        highlighted.Apply = enableState.Apply;

        KeyDown += HandleKeyDown;

        buttonVisual.RollOn += HandleRollOn;
    }

    private void HandleKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Keys.Left)
        {
            HandleTab(TabDirection.Up, loop: true);
        }

        if (e.Key == Keys.Right)
        {
            HandleTab(TabDirection.Down, loop: true);
        }
    }

    private void HandleRollOn(object sender, EventArgs e)
    {
        IsFocused = true;
    }
}