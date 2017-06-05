using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// Radio button control
    /// </summary>
    public class RadioButton : Checkbox
    {
        protected uint buttonGroupIndex;
        /// <summary>
        /// Create new radio button instance
        /// </summary>
        public RadioButton(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.RadioButton;
            parentDialog = parent;
        }

        /// <summary>
        /// Button Group property
        /// </summary>
        public uint ButtonGroup
        {
            get { return buttonGroupIndex; }
            set { buttonGroupIndex = value; }
        }

        /// <summary>
        /// Sets the check state and potentially clears the group
        /// </summary>
        public void SetChecked(bool ischecked, bool clear)
        {
            SetCheckedInternal(ischecked, clear, false);
        }

        /// <summary>
        /// Sets the checked state and fires the event if necessary
        /// </summary>
        protected virtual void SetCheckedInternal(bool ischecked, bool clearGroup, bool fromInput)
        {
            isBoxChecked = ischecked;
            RaiseChangedEvent(this, fromInput);
        }

        /// <summary>
        /// Override hotkey to fire event
        /// </summary>
        public override void OnHotKey()
        {
            SetCheckedInternal(true, true);
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
                            parentDialog.ClearRadioButtonGroup(buttonGroupIndex);
                            isBoxChecked = !isBoxChecked;

                            RaiseChangedEvent(this, true);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Handle mouse messages from the radio button
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
                                parentDialog.ClearRadioButtonGroup(buttonGroupIndex);
                                isBoxChecked = !isBoxChecked;

                                RaiseChangedEvent(this, true);
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
