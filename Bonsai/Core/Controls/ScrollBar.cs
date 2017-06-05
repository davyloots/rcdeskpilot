using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// A scroll bar control
    /// </summary>
    public class ScrollBar : Control
    {
        public const int TrackLayer = 0;
        public const int UpButtonLayer = 1;
        public const int DownButtonLayer = 2;
        public const int ThumbLayer = 3;
        protected const int MinimumThumbSize = 8;
        #region Instance Data
        protected bool showingThumb;
        protected System.Drawing.Rectangle upButtonRect;
        protected System.Drawing.Rectangle downButtonRect;
        protected System.Drawing.Rectangle trackRect;
        protected System.Drawing.Rectangle thumbRect;
        protected int position; // Position of the first displayed item
        protected int pageSize; // How many items are displayable in one page
        protected int start; // First item
        protected int end; // The index after the last item
        private int thumbOffsetY;
        private bool isDragging;
        #endregion

        /// <summary>
        /// Creates a new instance of the scroll bar class
        /// </summary>
        public ScrollBar(Dialog parent)
            : base(parent)
        {
            // Store parent and control type
            controlType = ControlType.Scrollbar;
            parentDialog = parent;

            // Set default properties
            showingThumb = true;
            upButtonRect = System.Drawing.Rectangle.Empty;
            downButtonRect = System.Drawing.Rectangle.Empty;
            trackRect = System.Drawing.Rectangle.Empty;
            thumbRect = System.Drawing.Rectangle.Empty;

            position = 0;
            pageSize = 1;
            start = 0;
            end = 1;
        }

        /// <summary>
        /// Update all of the rectangles
        /// </summary>
        protected override void UpdateRectangles()
        {
            // Get the bounding box first
            base.UpdateRectangles();

            // Make sure buttons are square
            upButtonRect = new System.Drawing.Rectangle(boundingBox.Location,
                new System.Drawing.Size(boundingBox.Width, boundingBox.Width));

            downButtonRect = new System.Drawing.Rectangle(boundingBox.Left, boundingBox.Bottom - boundingBox.Width,
                boundingBox.Width, boundingBox.Width);

            trackRect = new System.Drawing.Rectangle(upButtonRect.Left, upButtonRect.Bottom,
                upButtonRect.Width, downButtonRect.Top - upButtonRect.Bottom);

            thumbRect = upButtonRect;

            UpdateThumbRectangle();
        }

        /// <summary>
        /// Position of the track
        /// </summary>
        public int TrackPosition
        {
            get { return position; }
            set { position = value; Cap(); UpdateThumbRectangle(); }
        }
        /// <summary>
        /// Size of a 'page'
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; Cap(); UpdateThumbRectangle(); }
        }

        /// <summary>Clips position at boundaries</summary>
        protected void Cap()
        {
            if (position < start || end - start <= pageSize)
            {
                position = start;
            }
            else if (position + pageSize > end)
                position = end - pageSize;
        }

        /// <summary>Compute the dimension of the scroll thumb</summary>
        protected void UpdateThumbRectangle()
        {
            if (end - start > pageSize)
            {
                int thumbHeight = Math.Max(trackRect.Height * pageSize / (end - start), MinimumThumbSize);
                int maxPosition = end - start - pageSize;
                thumbRect.Location = new System.Drawing.Point(thumbRect.Left,
                    trackRect.Top + (position - start) * (trackRect.Height - thumbHeight) / maxPosition);
                thumbRect.Size = new System.Drawing.Size(thumbRect.Width, thumbHeight);
                showingThumb = true;
            }
            else
            {
                // No content to scroll
                thumbRect.Height = 0;
                showingThumb = false;
            }
        }

        /// <summary>Scrolls by delta items.  A positive value scrolls down, while a negative scrolls down</summary>
        public void Scroll(int delta)
        {
            // Perform scroll
            position += delta;
            // Cap position
            Cap();
            // Update thumb rectangle
            UpdateThumbRectangle();
        }

        /// <summary>Shows an item</summary>
        public void ShowItem(int index)
        {
            // Cap the index
            if (index < 0)
                index = 0;

            if (index >= end)
                index = end - 1;

            // Adjust the position to show this item
            if (position > index)
                position = index;
            else if (position + pageSize <= index)
                position = index - pageSize + 1;

            // Update thumbs again
            UpdateThumbRectangle();
        }

        /// <summary>Sets the track range</summary>
        public void SetTrackRange(int startRange, int endRange)
        {
            start = startRange; end = endRange;
            Cap();
            UpdateThumbRectangle();
        }

        /// <summary>Render the scroll bar control</summary>
        public override void Render(Device device, float elapsedTime)
        {
            ControlState state = ControlState.Normal;
            if (IsVisible == false)
                state = ControlState.Hidden;
            else if ((IsEnabled == false) || (showingThumb == false))
                state = ControlState.Disabled;
            else if (isMouseOver)
                state = ControlState.MouseOver;
            else if (hasFocus)
                state = ControlState.Focus;

            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.8f;

            // Background track layer
            Element e = elementList[ScrollBar.TrackLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, trackRect);

            // Up arrow
            e = elementList[ScrollBar.UpButtonLayer] as Element;
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, upButtonRect);

            // Down arrow
            e = elementList[ScrollBar.DownButtonLayer] as Element;
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, downButtonRect);

            // Thumb button
            e = elementList[ScrollBar.ThumbLayer] as Element;
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, thumbRect);
        }

        /// <summary>Stores data for a combo box item</summary>
        public override bool HandleMouse(NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            switch (msg)
            {
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                case NativeMethods.WindowMessage.LeftButtonDown:
                    {
                        Parent.SampleFramework.Window.Capture = true;

                        // Check for on up button
                        if (upButtonRect.Contains(pt))
                        {
                            if (position > start)
                                --position;
                            UpdateThumbRectangle();
                            return true;
                        }

                        // Check for on down button
                        if (downButtonRect.Contains(pt))
                        {
                            if (position + pageSize < end)
                                ++position;
                            UpdateThumbRectangle();
                            return true;
                        }

                        // Check for click on thumb
                        if (thumbRect.Contains(pt))
                        {
                            isDragging = true;
                            thumbOffsetY = pt.Y - thumbRect.Top;
                            return true;
                        }

                        // check for click on track
                        if (thumbRect.Left <= pt.X &&
                            thumbRect.Right > pt.X)
                        {
                            if (thumbRect.Top > pt.Y &&
                                trackRect.Top <= pt.Y)
                            {
                                Scroll(-(pageSize - 1));
                                return true;
                            }
                            else if (thumbRect.Bottom <= pt.Y &&
                                trackRect.Bottom > pt.Y)
                            {
                                Scroll(pageSize - 1);
                                return true;
                            }
                        }

                        break;
                    }
                case NativeMethods.WindowMessage.LeftButtonUp:
                    {
                        isDragging = false;
                        Parent.SampleFramework.Window.Capture = false;
                        UpdateThumbRectangle();
                        break;
                    }

                case NativeMethods.WindowMessage.MouseMove:
                    {
                        if (isDragging)
                        {
                            // Calculate new bottom and top of thumb rect
                            int bottom = thumbRect.Bottom + (pt.Y - thumbOffsetY - thumbRect.Top);
                            int top = pt.Y - thumbOffsetY;
                            thumbRect = new System.Drawing.Rectangle(thumbRect.Left, top, thumbRect.Width, bottom - top);
                            if (thumbRect.Top < trackRect.Top)
                                thumbRect.Offset(0, trackRect.Top - thumbRect.Top);
                            else if (thumbRect.Bottom > trackRect.Bottom)
                                thumbRect.Offset(0, trackRect.Bottom - thumbRect.Bottom);

                            // Compute first item index based on thumb position
                            int maxFirstItem = end - start - pageSize; // Largest possible index for first item
                            int maxThumb = trackRect.Height - thumbRect.Height; // Largest possible thumb position

                            position = start + (thumbRect.Top - trackRect.Top +
                                maxThumb / (maxFirstItem * 2)) * // Shift by half a row to avoid last row covered
                                maxFirstItem / maxThumb;

                            return true;
                        }
                        break;
                    }
            }

            // Was not handled
            return false;
        }

    }
}
