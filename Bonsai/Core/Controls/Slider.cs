using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>Slider control</summary>
    public class Slider : Control
    {
        public const int TrackLayer = 0;
        public const int ButtonLayer = 1;
        #region Instance Data
        public event EventHandler ValueChanged;
        protected int currentValue;
        protected int maxValue;
        protected int minValue;

        protected int dragX; // Mouse position at the start of the drag
        protected int dragOffset; // Drag offset from the center of the button
        protected int buttonX;

        protected bool isPressed;
        protected System.Drawing.Rectangle buttonRect;

        /// <summary>Slider's can always have focus</summary>
        public override bool CanHaveFocus { get { return true; } }

        /// <summary>Current value of the slider</summary>
        protected void RaiseValueChanged(Slider sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            if (ValueChanged != null)
                ValueChanged(sender, System.EventArgs.Empty);
        }
        /// <summary>Current value of the slider</summary>
        public int Value { get { return currentValue; } set { SetValueInternal(value, false); } }
        /// <summary>Sets the range of the slider</summary>
        public void SetRange(int min, int max)
        {
            minValue = min;
            maxValue = max;
            SetValueInternal(currentValue, false);
        }

        /// <summary>Sets the value internally and fires the event if needed</summary>
        protected void SetValueInternal(int newValue, bool fromInput)
        {
            // Clamp to the range
            newValue = Math.Max(minValue, newValue);
            newValue = Math.Min(maxValue, newValue);
            if (newValue == currentValue)
                return;

            // Update the value, the rects, then fire the events if necessar
            currentValue = newValue;
            UpdateRectangles();
            RaiseValueChanged(this, fromInput);
        }
        #endregion

        /// <summary>Create new button instance</summary>
        public Slider(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.Slider;
            parentDialog = parent;

            isPressed = false;
            minValue = 0;
            maxValue = 100;
            currentValue = 50;
        }

        /// <summary>Does the control contain this point?</summary>
        public override bool ContainsPoint(System.Drawing.Point pt)
        {
            return boundingBox.Contains(pt) || buttonRect.Contains(pt);
        }

        /// <summary>Update the rectangles for the control</summary>
        protected override void UpdateRectangles()
        {
            // First get the bounding box
            base.UpdateRectangles();

            // Create the button rect
            buttonRect = boundingBox;
            buttonRect.Width = buttonRect.Height; // Make it square

            // Offset it 
            buttonRect.Offset(-buttonRect.Width / 2, 0);
            buttonX = (int)((currentValue - minValue) * (float)boundingBox.Width / (maxValue - minValue));
            buttonRect.Offset(buttonX, 0);
        }

        /// <summary>Gets a value from a position</summary>
        public int ValueFromPosition(int x)
        {
            float valuePerPixel = ((float)(maxValue - minValue) / (float)boundingBox.Width);
            return (int)(0.5f + minValue + valuePerPixel * (x - boundingBox.Left));
        }

        /// <summary>Handle mouse input input</summary>
        public override bool HandleMouse(Bonsai.Core.NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            switch (msg)
            {
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                case NativeMethods.WindowMessage.LeftButtonDown:
                    {
                        if (buttonRect.Contains(pt))
                        {
                            // Pressed while inside the control
                            isPressed = true;
                            Parent.SampleFramework.Window.Capture = true;

                            dragX = pt.X;
                            dragOffset = buttonX - dragX;
                            if (!hasFocus)
                                Dialog.RequestFocus(this);

                            return true;
                        }
                        if (boundingBox.Contains(pt))
                        {
                            if (pt.X > buttonX + controlX)
                            {
                                SetValueInternal(currentValue + 1, true);
                                return true;
                            }
                            if (pt.X < buttonX + controlX)
                            {
                                SetValueInternal(currentValue - 1, true);
                                return true;
                            }
                        }

                        break;
                    }
                case NativeMethods.WindowMessage.LeftButtonUp:
                    {
                        if (isPressed)
                        {
                            isPressed = false;
                            Parent.SampleFramework.Window.Capture = false;
                            Dialog.ClearFocus();
                            RaiseValueChanged(this, true);
                            return true;
                        }
                        break;
                    }
                case NativeMethods.WindowMessage.MouseMove:
                    {
                        if (isPressed)
                        {
                            SetValueInternal(ValueFromPosition(controlX + pt.X + dragOffset), true);
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        /// <summary>Handle keyboard input</summary>
        public override bool HandleKeyboard(Bonsai.Core.NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            if (msg == NativeMethods.WindowMessage.KeyDown)
            {
                switch ((System.Windows.Forms.Keys)wParam.ToInt32())
                {
                    case System.Windows.Forms.Keys.Home:
                        SetValueInternal(minValue, true);
                        return true;
                    case System.Windows.Forms.Keys.End:
                        SetValueInternal(maxValue, true);
                        return true;
                    case System.Windows.Forms.Keys.Prior:
                    case System.Windows.Forms.Keys.Left:
                    case System.Windows.Forms.Keys.Up:
                        SetValueInternal(currentValue - 1, true);
                        return true;
                    case System.Windows.Forms.Keys.Next:
                    case System.Windows.Forms.Keys.Right:
                    case System.Windows.Forms.Keys.Down:
                        SetValueInternal(currentValue + 1, true);
                        return true;
                }
            }

            return false;
        }


        /// <summary>Render the slider</summary>
        public override void Render(Device device, float elapsedTime)
        {
            ControlState state = ControlState.Normal;
            if (IsVisible == false)
            {
                state = ControlState.Hidden;
            }
            else if (IsEnabled == false)
            {
                state = ControlState.Disabled;
            }
            else if (isPressed)
            {
                state = ControlState.Pressed;
            }
            else if (isMouseOver)
            {
                state = ControlState.MouseOver;
            }
            else if (hasFocus)
            {
                state = ControlState.Focus;
            }

            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.8f;

            Element e = elementList[Slider.TrackLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, boundingBox);

            e = elementList[Slider.ButtonLayer] as Element;
            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            parentDialog.DrawSprite(e, buttonRect);
        }
    }
}
