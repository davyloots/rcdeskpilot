using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// StaticText text control
    /// </summary>
    public class StaticText : Control
    {
        public const int TextElement = 0;
        protected string textData; // Window text

        /// <summary>
        /// Create a new instance of a static text control
        /// </summary>
        public StaticText(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.StaticText;
            parentDialog = parent;
            textData = string.Empty;
            elementList.Clear();
        }

        /// <summary>
        /// Render this control
        /// </summary>
        public override void Render(Device device, float elapsedTime)
        {
            if (!IsVisible)
                return; // Nothing to do here

            ControlState state = ControlState.Normal;
            if (!IsEnabled)
                state = ControlState.Disabled;

            // Blend the element colors
            Element e = elementList[TextElement] as Element;
            e.FontColor.Blend(state, elapsedTime);

            // Render with a shadow
            parentDialog.DrawText(textData, e, boundingBox, true);
        }

        /// <summary>
        /// Return a copy of the string
        /// </summary>
        public string GetTextCopy()
        {
            return string.Copy(textData);
        }

        /// <summary>
        /// Sets the updated text for this control
        /// </summary>
        public void SetText(string newText)
        {
            textData = newText;
        }

    }
}
