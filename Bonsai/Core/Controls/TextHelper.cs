using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// Manages the intertion point when drawing text
    /// </summary>
    public struct TextHelper
    {
        private Font textFont; // Used to draw the text
        private Sprite textSprite; // Used to cache the drawn text
        private int color; // Color to draw the text
        private System.Drawing.Point point; // Where to draw the text
        private int lineHeight; // Height of the lines

        /// <summary>
        /// Create a new instance of the text helper class
        /// </summary>
        public TextHelper(Font f, Sprite s, int l)
        {
            textFont = f;
            textSprite = s;
            lineHeight = l;
            color = unchecked((int)0xffffffff);
            point = System.Drawing.Point.Empty;
        }

        /// <summary>
        /// Draw a line of text
        /// </summary>
        public void DrawTextLine(string text)
        {
            if (textFont == null)
            {
                throw new InvalidOperationException("You cannot draw text.  There is no font object.");
            }
            // Create the rectangle to draw to
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(point, System.Drawing.Size.Empty);
            textFont.DrawText(textSprite, text, rect, DrawTextFormat.NoClip, color);

            // Increase the line height
            point.Y += lineHeight;
        }

        /// <summary>
        /// Draw a line of text
        /// </summary>
        public void DrawTextLine(string text, params object[] args)
        {
            // Simply format the string and pass it on
            DrawTextLine(string.Format(text, args));
        }

        /// <summary>
        /// Insertion point of the text
        /// </summary>
        public void SetInsertionPoint(System.Drawing.Point p) { point = p; }
        public void SetInsertionPoint(int x, int y) { point.X = x; point.Y = y; }

        /// <summary>
        /// The color of the text
        /// </summary>
        public void SetForegroundColor(int c) { color = c; }
        public void SetForegroundColor(System.Drawing.Color c) { color = c.ToArgb(); }

        /// <summary>
        /// Begin the sprite rendering
        /// </summary>
        public void Begin()
        {
            if (textSprite != null)
            {
                textSprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortTexture);
            }
        }

        /// <summary>
        /// End the sprite
        /// </summary>
        public void End()
        {
            if (textSprite != null)
            {
                textSprite.End();
            }
        }
    }
}
