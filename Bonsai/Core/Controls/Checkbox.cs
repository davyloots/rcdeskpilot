using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// Checkbox control
    /// </summary>
    public class Checkbox : Button
    {
        public const int BoxLayer = 0;
        public const int CheckLayer = 1;
        #region Event code
        public event EventHandler Changed;
        /// <summary>Create new button instance</summary>
        protected void RaiseChangedEvent(Checkbox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            // Fire both the changed and clicked event
            base.RaiseClickEvent(sender, wasTriggeredByUser);
            if (Changed != null)
                Changed(sender, System.EventArgs.Empty);
        }
        #endregion
        protected System.Drawing.Rectangle buttonRect;
        protected System.Drawing.Rectangle textRect;
        protected bool isBoxChecked;

        /// <summary>
        /// Create new checkbox instance
        /// </summary>
        public Checkbox(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.CheckBox;
            isBoxChecked = false;
            parentDialog = parent;
        }

        /// <summary>
        /// Checked property
        /// </summary>
        public virtual bool IsChecked
        {
            get { return isBoxChecked; }
            set { SetCheckedInternal(value, false); }
        }
        /// <summary>
        /// Sets the checked state and fires the event if necessary
        /// </summary>
        protected virtual void SetCheckedInternal(bool ischecked, bool fromInput)
        {
            isBoxChecked = ischecked;
            RaiseChangedEvent(this, fromInput);
        }

        /// <summary>
        /// Override hotkey to fire event
        /// </summary>
        public override void OnHotKey()
        {
            SetCheckedInternal(!isBoxChecked, true);
        }

        /// <summary>
        /// Does the control contain the point?
        /// </summary>
        public override bool ContainsPoint(System.Drawing.Point pt)
        {
            return (boundingBox.Contains(pt) || buttonRect.Contains(pt));
        }
        /// <summary>
        /// Update the rectangles
        /// </summary>
        protected override void UpdateRectangles()
        {
            // Update base first
            base.UpdateRectangles();

            // Update the two rects
            buttonRect = boundingBox;
            buttonRect = new System.Drawing.Rectangle(boundingBox.Location,
                new System.Drawing.Size(boundingBox.Height, boundingBox.Height));

            textRect = boundingBox;
            textRect.Offset((int)(1.25f * buttonRect.Width), 0);
        }

        /// <summary>
        /// Render the checkbox control
        /// </summary>
        public override void Render(Device device, float elapsedTime)
        {
            ControlState state = ControlState.Normal;
            if (IsVisible == false)
                state = ControlState.Hidden;
            else if (IsEnabled == false)
                state = ControlState.Disabled;
            else if (isPressed)
                state = ControlState.Pressed;
            else if (isMouseOver)
                state = ControlState.MouseOver;
            else if (hasFocus)
                state = ControlState.Focus;

            Element e = elementList[Checkbox.BoxLayer] as Element;
            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.8f;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            e.FontColor.Blend(state, elapsedTime, blendRate);

            // Draw sprite/text of checkbox
            parentDialog.DrawSprite(e, buttonRect);
            parentDialog.DrawText(textData, e, textRect);

            if (!isBoxChecked)
                state = ControlState.Hidden;

            e = elementList[Checkbox.CheckLayer] as Element;
            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);

            // Draw sprite of checkbox
            parentDialog.DrawSprite(e, buttonRect);
        }

        /// <summary>
        /// Handle the keyboard for the checkbox
        /// </summary>
        public override bool HandleKeyboard(NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            switch (msg)
            {
                case NativeMethods.WindowMessage.KeyDown:
                    if ((System.Windows.Forms.Keys)wParam.ToInt32() == System.Windows.Forms.Keys.Space)
                    {
                        isPressed = true;
                        return true;
                    }
                    break;
                case NativeMethods.WindowMessage.KeyUp:
                    if ((System.Windows.Forms.Keys)wParam.ToInt32() == System.Windows.Forms.Keys.Space)
                    {
                        if (isPressed)
                        {
                            isPressed = false;
                            SetCheckedInternal(!isBoxChecked, true);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Handle mouse messages from the checkbox
        /// </summary>
        public override bool HandleMouse(NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            switch (msg)
            {
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                case NativeMethods.WindowMessage.LeftButtonDown:
                    {
                        if (ContainsPoint(pt))
                        {
                            // Pressed while inside the control
                            isPressed = true;
                            Parent.SampleFramework.Window.Capture = true;
                            if ((!hasFocus) && (parentDialog.IsUsingKeyboardInput))
                                Dialog.RequestFocus(this);

                            return true;
                        }
                    }
                    break;
                case NativeMethods.WindowMessage.LeftButtonUp:
                    {
                        if (isPressed)
                        {
                            isPressed = false;
                            Parent.SampleFramework.Window.Capture = false;

                            // Button click
                            if (ContainsPoint(pt))
                            {
                                SetCheckedInternal(!isBoxChecked, true);
                            }

                            return true;
                        }
                    }
                    break;
            }

            return false;
        }
    }
}
