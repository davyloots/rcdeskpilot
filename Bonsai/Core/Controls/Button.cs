using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{

    /// <summary>
    /// Button control
    /// </summary>
    public class Button : StaticText
    {
        public const int ButtonLayer = 0;
        public const int FillLayer = 1;
        protected bool isPressed;
        #region Event code
        public event EventHandler Click;
        /// <summary>Create new button instance</summary>
        protected void RaiseClickEvent(Button sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            if (Click != null)
                Click(sender, System.EventArgs.Empty);
        }
        #endregion

        /// <summary>Create new button instance</summary>
        public Button(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.Button;
            parentDialog = parent;
            isPressed = false;
            hotKey = 0;
        }

        /// <summary>Can the button have focus</summary>
        public override bool CanHaveFocus { get { return IsVisible && IsEnabled; } }
        /// <summary>The hotkey for this button was pressed</summary>
        public override void OnHotKey()
        {
            RaiseClickEvent(this, true);
        }

        /// <summary>
        /// Will handle the keyboard strokes
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
                        isPressed = false;
                        RaiseClickEvent(this, true);

                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Handle mouse messages from the buttons
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
                            if (!hasFocus)
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
                            if (!parentDialog.IsUsingKeyboardInput)
                                Dialog.ClearFocus();

                            // Button click
                            if (ContainsPoint(pt))
                                RaiseClickEvent(this, true);
                        }
                    }
                    break;
            }

            return false;
        }

        /// <summary>Render the button</summary>
        public override void Render(Device device, float elapsedTime)
        {
            int offsetX = 0;
            int offsetY = 0;

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
                offsetX = 1;
                offsetY = 2;
            }
            else if (isMouseOver)
            {
                state = ControlState.MouseOver;
                offsetX = -1;
                offsetY = -2;
            }
            else if (hasFocus)
            {
                state = ControlState.Focus;
            }

            // Background fill layer
            Element e = elementList[Button.ButtonLayer] as Element;
            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.8f;

            System.Drawing.Rectangle buttonRect = boundingBox;
            buttonRect.Offset(offsetX, offsetY);

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            e.FontColor.Blend(state, elapsedTime, blendRate);

            // Draw sprite/text of button
            parentDialog.DrawSprite(e, buttonRect);
            parentDialog.DrawText(textData, e, buttonRect);

            // Main button
            e = elementList[Button.FillLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            e.FontColor.Blend(state, elapsedTime, blendRate);

            parentDialog.DrawSprite(e, buttonRect);
            parentDialog.DrawText(textData, e, buttonRect);
        }

    }
}
