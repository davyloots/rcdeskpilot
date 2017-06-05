using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// Predefined control types
    /// </summary>
    public enum ControlType
    {
        StaticText,
        Button,
        CheckBox,
        RadioButton,
        ComboBox,
        Slider,
        ListBox,
        EditBox,
        Scrollbar,
        Picture
    }

    /// <summary>
    /// Possible states of a control
    /// </summary>
    public enum ControlState
    {
        Normal,
        Disabled,
        Hidden,
        Focus,
        MouseOver,
        Pressed,
        LastState // Should always be last
    }

    /// <summary>
    /// Blends colors
    /// </summary>
    public struct BlendColor
    {
        public ColorValue[] States; // Modulate colors for all possible control states
        public ColorValue Current; // Current color

        /// <summary>Initialize the color blending</summary>
        public void Initialize(ColorValue defaultColor, ColorValue disabledColor, ColorValue hiddenColor)
        {
            // Create the array
            States = new ColorValue[(int)ControlState.LastState];
            for (int i = 0; i < States.Length; i++)
            {
                States[i] = defaultColor;
            }

            // Store the data
            States[(int)ControlState.Disabled] = disabledColor;
            States[(int)ControlState.Hidden] = hiddenColor;
            Current = hiddenColor;
        }
        /// <summary>Initialize the color blending</summary>
        public void Initialize(ColorValue defaultColor) { Initialize(defaultColor, new ColorValue(0.5f, 0.5f, 0.5f, 0.75f), new ColorValue()); }

        /// <summary>Blend the colors together</summary>
        public void Blend(ControlState state, float elapsedTime, float rate)
        {
            if ((States == null) || (States.Length == 0))
                return; // Nothing to do

            ColorValue destColor = States[(int)state];
            Current = ColorOperator.Lerp(Current, destColor, 1.0f - (float)Math.Pow(rate, 30 * elapsedTime));
        }
        /// <summary>Blend the colors together</summary>
        public void Blend(ControlState state, float elapsedTime) { Blend(state, elapsedTime, 0.7f); }
    }

    /// <summary>
    /// Contains all the display information for a given control type
    /// </summary>
    public struct ElementHolder
    {
        public ControlType ControlType;
        public uint ElementIndex;
        public Element Element;
    }

    /// <summary>
    /// Contains all the display tweakables for a sub-control
    /// </summary>
    public class Element : ICloneable
    {
        #region Magic Numbers
        #endregion

        #region Instance Data
        public uint TextureIndex; // Index of the texture for this Element 
        public uint FontIndex; // Index of the font for this Element 
        public DrawTextFormat textFormat; // The Format argument to draw text

        public System.Drawing.Rectangle textureRect; // Bounding rectangle of this element on the composite texture

        public BlendColor TextureColor;
        public BlendColor FontColor;
        #endregion

        /// <summary>Set the texture</summary>
        public void SetTexture(uint tex, System.Drawing.Rectangle texRect, ColorValue defaultTextureColor)
        {
            // Store data
            TextureIndex = tex;
            textureRect = texRect;
            TextureColor.Initialize(defaultTextureColor);
        }
        /// <summary>Set the texture</summary>
        public void SetTexture(uint tex, System.Drawing.Rectangle texRect) { SetTexture(tex, texRect, Dialog.WhiteColorValue); }
        /// <summary>Set the font</summary>
        public void SetFont(uint font, ColorValue defaultFontColor, DrawTextFormat format)
        {
            // Store data
            FontIndex = font;
            textFormat = format;
            FontColor.Initialize(defaultFontColor);
        }
        /// <summary>Set the font</summary>
        public void SetFont(uint font) { SetFont(font, Dialog.WhiteColorValue, DrawTextFormat.Center | DrawTextFormat.VerticalCenter); }
        /// <summary>
        /// Refresh this element
        /// </summary>
        public void Refresh()
        {
            if (TextureColor.States != null)
                TextureColor.Current = TextureColor.States[(int)ControlState.Hidden];
            if (FontColor.States != null)
                FontColor.Current = FontColor.States[(int)ControlState.Hidden];
        }

        #region ICloneable Members
        /// <summary>Clone an object</summary>
        public Element Clone()
        {
            Element e = new Element();
            e.TextureIndex = this.TextureIndex;
            e.FontIndex = this.FontIndex;
            e.textFormat = this.textFormat;
            e.textureRect = this.textureRect;
            e.TextureColor = this.TextureColor;
            e.FontColor = this.FontColor;

            return e;
        }
        /// <summary>Clone an object</summary>
        object ICloneable.Clone() { throw new NotSupportedException("Use the strongly typed clone."); }

        #endregion
    }
}
