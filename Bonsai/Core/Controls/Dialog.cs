using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// All controls must be assigned to a dialog, which handles
    /// input and rendering for the controls.
    /// </summary>
    public class Dialog
    {
        #region Static Data
        public const int WheelDelta = 120;
        public static readonly ColorValue WhiteColorValue = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        public static readonly ColorValue TransparentWhite = new ColorValue(1.0f, 1.0f, 1.0f, 0.0f);
        public static readonly ColorValue BlackColorValue = new ColorValue(0.0f, 0.0f, 0.0f, 1.0f);
        private static Control controlFocus = null; // The control which has focus
        private static Control controlMouseOver = null; // The control which is hovered over
        private static Control controlMouseDown = null; // The control which the mouse was pressed on

        private static double timeRefresh = 0.0;
        /// <summary>Set the static refresh time</summary>
        public static void SetRefreshTime(float time) { timeRefresh = time; }
        #endregion

        #region Instance Data
        // Sample framework
        private Framework parent = null;
        public Framework SampleFramework { get { return parent; } }

        // Vertex information
        private CustomVertex.TransformedColoredTextured[] vertices;

        // Timing
        private double timeLastRefresh;

        // Control/Elements
        private ArrayList controlList = new ArrayList();
        private ArrayList defaultElementList = new ArrayList();

        // Captions
        private bool hasCaption;
        private string caption;
        private int captionHeight;
        private Element captionElement;
        private bool isDialogMinimized;

        // Dialog information
        private int dialogX, dialogY, width, height;
        // Colors
        private ColorValue topLeftColor, topRightColor, bottomLeftColor, bottomRightColor;

        // Fonts/Textures
        private ArrayList textureList = new ArrayList(); // Index into texture cache
        private ArrayList fontList = new ArrayList(); // Index into font cache

        // Dialogs
        private Dialog nextDialog;
        private Dialog prevDialog;

        // User Input control
        private bool usingNonUserEvents;
        private bool usingKeyboardInput;
        private bool usingMouseInput;
        #endregion

        #region Simple Properties/Methods
        /// <summary>Is the dilaog using non user events</summary>
        public bool IsUsingNonUserEvents { get { return usingNonUserEvents; } set { usingNonUserEvents = value; } }
        /// <summary>Is the dilaog using keyboard input</summary>
        public bool IsUsingKeyboardInput { get { return usingKeyboardInput; } set { usingKeyboardInput = value; } }
        /// <summary>Is the dilaog using mouse input</summary>
        public bool IsUsingMouseInput { get { return usingMouseInput; } set { usingMouseInput = value; } }
        /// <summary>Is the dilaog minimized</summary>
        public bool IsMinimized { get { return isDialogMinimized; } set { isDialogMinimized = value; } }
        /// <summary>Called to set dialog's location</summary>
        public void SetLocation(int x, int y) { dialogX = x; dialogY = y; UpdateVertices(); }
        /// <summary>The dialog's location</summary>
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(dialogX, dialogY); }
            set { dialogX = value.X; dialogY = value.Y; UpdateVertices(); }
        }

        /// <summary>Called to set dialog's size</summary>
        public void SetSize(int w, int h) { width = w; height = h; UpdateVertices(); }
        /// <summary>Dialogs width</summary>
        public int Width { get { return width; } set { width = value; } }
        /// <summary>Dialogs height</summary>
        public int Height { get { return height; } set { height = value; } }
        /// <summary>Called to set dialog's caption</summary>
        public void SetCaptionText(string text) { caption = text; }
        /// <summary>The dialog's caption height</summary>
        public int CaptionHeight { get { return captionHeight; } set { captionHeight = value; } }
        /// <summary>Called to set dialog's caption enabled state</summary>
        public void SetCaptionEnabled(bool isEnabled) { hasCaption = isEnabled; }
        /// <summary>Called to set dialog's border colors</summary>
        public void SetBackgroundColors(ColorValue topLeft, ColorValue topRight, ColorValue bottomLeft, ColorValue bottomRight)
        {
            topLeftColor = topLeft; topRightColor = topRight; bottomLeftColor = bottomLeft; bottomRightColor = bottomRight;
            UpdateVertices();
        }
        /// <summary>Called to set dialog's border colors</summary>
        public void SetBackgroundColors(ColorValue allCorners) { SetBackgroundColors(allCorners, allCorners, allCorners, allCorners); }

        #endregion

        /// <summary>
        /// Create a new instance of the dialog class
        /// </summary>
        public Dialog(Framework sample)
        {
            parent = sample; // store this for later use
            // Initialize to default state
            dialogX = 0; dialogY = 0; width = 0; height = 0;
            hasCaption = false; isDialogMinimized = false;
            caption = string.Empty;
            captionHeight = 18;

            topLeftColor = topRightColor = bottomLeftColor = bottomRightColor = new ColorValue();

            timeLastRefresh = 0.0f;

            nextDialog = this; // Only one dialog
            prevDialog = this;  // Only one dialog

            usingNonUserEvents = false;
            usingKeyboardInput = false;
            usingMouseInput = true;

            InitializeDefaultElements();
        }

        /// <summary>
        /// Initialize the default elements for this dialog
        /// </summary>
        private void InitializeDefaultElements()
        {
            SetTexture(0, "data\\uicontrols.dds");
            SetFont(0, "Arial", 14, FontWeight.Normal);

            //-------------------------------------
            // Element for the caption
            //-------------------------------------
            captionElement = new Element();
            captionElement.SetFont(0, WhiteColorValue, DrawTextFormat.Left | DrawTextFormat.VerticalCenter);
            captionElement.SetTexture(0, System.Drawing.Rectangle.FromLTRB(17, 269, 241, 287));
            captionElement.TextureColor.States[(int)ControlState.Normal] = WhiteColorValue;
            captionElement.FontColor.States[(int)ControlState.Normal] = WhiteColorValue;
            // Pre-blend as we don't need to transition the state
            captionElement.TextureColor.Blend(ControlState.Normal, 10.0f);
            captionElement.FontColor.Blend(ControlState.Normal, 10.0f);

            Element e = new Element();

            //-------------------------------------
            // StaticText
            //-------------------------------------
            e.SetFont(0);
            e.FontColor.States[(int)ControlState.Disabled] = new ColorValue(0.75f, 0.75f, 0.75f, 0.75f);
            // Assign the element
            SetDefaultElement(ControlType.StaticText, StaticText.TextElement, e);

            //-------------------------------------
            // Button - Button
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(0, 0, 136, 54));
            e.SetFont(0);
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(1.0f, 1.0f, 1.0f, 0.55f);
            e.TextureColor.States[(int)ControlState.Pressed] = new ColorValue(1.0f, 1.0f, 1.0f, 0.85f);
            e.FontColor.States[(int)ControlState.MouseOver] = BlackColorValue;
            // Assign the element
            SetDefaultElement(ControlType.Button, Button.ButtonLayer, e);

            //-------------------------------------
            // Button - Fill Layer
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(136, 0, 252, 54), TransparentWhite);
            e.TextureColor.States[(int)ControlState.MouseOver] = new ColorValue(1.0f, 1.0f, 1.0f, 0.6f);
            e.TextureColor.States[(int)ControlState.Pressed] = new ColorValue(0, 0, 0, 0.25f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(1.0f, 1.0f, 1.0f, 0.05f);
            // Assign the element
            SetDefaultElement(ControlType.Button, Button.FillLayer, e);


            //-------------------------------------
            // CheckBox - Box
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(0, 54, 27, 81));
            e.SetFont(0, WhiteColorValue, DrawTextFormat.Left | DrawTextFormat.VerticalCenter);
            e.FontColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(1.0f, 1.0f, 1.0f, 0.55f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(1.0f, 1.0f, 1.0f, 0.8f);
            e.TextureColor.States[(int)ControlState.Pressed] = WhiteColorValue;
            // Assign the element
            SetDefaultElement(ControlType.CheckBox, Checkbox.BoxLayer, e);

            //-------------------------------------
            // CheckBox - Check
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(27, 54, 54, 81));
            // Assign the element
            SetDefaultElement(ControlType.CheckBox, Checkbox.CheckLayer, e);

            //-------------------------------------
            // RadioButton - Box
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(54, 54, 81, 81));
            e.SetFont(0, WhiteColorValue, DrawTextFormat.Left | DrawTextFormat.VerticalCenter);
            e.FontColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(1.0f, 1.0f, 1.0f, 0.55f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(1.0f, 1.0f, 1.0f, 0.8f);
            e.TextureColor.States[(int)ControlState.Pressed] = WhiteColorValue;
            // Assign the element
            SetDefaultElement(ControlType.RadioButton, RadioButton.BoxLayer, e);

            //-------------------------------------
            // RadioButton - Check
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(81, 54, 108, 81));
            // Assign the element
            SetDefaultElement(ControlType.RadioButton, RadioButton.CheckLayer, e);

            //-------------------------------------
            // ComboBox - Main
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(7, 81, 247, 123));
            e.SetFont(0);
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(0.8f, 0.8f, 0.8f, 0.55f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(0.95f, 0.95f, 0.95f, 0.6f);
            e.TextureColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 0.25f);
            e.FontColor.States[(int)ControlState.MouseOver] = new ColorValue(0, 0, 0, 1.0f);
            e.FontColor.States[(int)ControlState.Pressed] = new ColorValue(0, 0, 0, 1.0f);
            e.FontColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            // Assign the element
            SetDefaultElement(ControlType.ComboBox, ComboBox.MainLayer, e);

            //-------------------------------------
            // ComboBox - Button
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(98, 189, 151, 238));
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(1.0f, 1.0f, 1.0f, 0.55f);
            e.TextureColor.States[(int)ControlState.Pressed] = new ColorValue(0.55f, 0.55f, 0.55f, 1.0f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(1.0f, 1.0f, 1.0f, 0.75f);
            e.TextureColor.States[(int)ControlState.Disabled] = new ColorValue(1.0f, 1.0f, 1.0f, 0.25f);
            // Assign the element
            SetDefaultElement(ControlType.ComboBox, ComboBox.ComboButtonLayer, e);

            //-------------------------------------
            // ComboBox - Dropdown
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(13, 123, 241, 160));
            e.SetFont(0, BlackColorValue, DrawTextFormat.Left | DrawTextFormat.Top);
            // Assign the element
            SetDefaultElement(ControlType.ComboBox, ComboBox.DropdownLayer, e);

            //-------------------------------------
            // ComboBox - Selection
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(12, 163, 239, 183));
            e.SetFont(0, WhiteColorValue, DrawTextFormat.Left | DrawTextFormat.Top);
            // Assign the element
            SetDefaultElement(ControlType.ComboBox, ComboBox.SelectionLayer, e);

            //-------------------------------------
            // Slider - Track
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(1, 187, 93, 228));
            e.TextureColor.States[(int)ControlState.Normal] = new ColorValue(1.0f, 1.0f, 1.0f, 0.55f);
            e.TextureColor.States[(int)ControlState.Focus] = new ColorValue(1.0f, 1.0f, 1.0f, 0.75f);
            e.TextureColor.States[(int)ControlState.Disabled] = new ColorValue(1.0f, 1.0f, 1.0f, 0.25f);
            // Assign the element
            SetDefaultElement(ControlType.Slider, Slider.TrackLayer, e);

            //-------------------------------------
            // Slider - Button
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(151, 193, 192, 234));
            // Assign the element
            SetDefaultElement(ControlType.Slider, Slider.ButtonLayer, e);

            //-------------------------------------
            // Scrollbar - Track
            //-------------------------------------
            int scrollBarStartX = 196;
            int scrollBarStartY = 191;
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(scrollBarStartX + 0, scrollBarStartY + 21, scrollBarStartX + 22, scrollBarStartY + 32));
            // Assign the element
            SetDefaultElement(ControlType.Scrollbar, ScrollBar.TrackLayer, e);

            //-------------------------------------
            // Scrollbar - Up Arrow
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(scrollBarStartX + 0, scrollBarStartY + 1, scrollBarStartX + 22, scrollBarStartY + 21));
            e.TextureColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 1.0f);
            // Assign the element
            SetDefaultElement(ControlType.Scrollbar, ScrollBar.UpButtonLayer, e);

            //-------------------------------------
            // Scrollbar - Down Arrow
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(scrollBarStartX + 0, scrollBarStartY + 32, scrollBarStartX + 22, scrollBarStartY + 53));
            e.TextureColor.States[(int)ControlState.Disabled] = new ColorValue(0.8f, 0.8f, 0.8f, 1.0f);
            // Assign the element
            SetDefaultElement(ControlType.Scrollbar, ScrollBar.DownButtonLayer, e);

            //-------------------------------------
            // Scrollbar - Button
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(220, 192, 238, 234));
            // Assign the element
            SetDefaultElement(ControlType.Scrollbar, ScrollBar.ThumbLayer, e);


            //-------------------------------------
            // EditBox
            //-------------------------------------
            // Element assignment:
            //   0 - text area
            //   1 - top left border
            //   2 - top border
            //   3 - top right border
            //   4 - left border
            //   5 - right border
            //   6 - lower left border
            //   7 - lower border
            //   8 - lower right border
            e.SetFont(0, BlackColorValue, DrawTextFormat.Left | DrawTextFormat.Top);

            // Assign the styles
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(14, 90, 241, 113));
            SetDefaultElement(ControlType.EditBox, EditBox.TextLayer, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(8, 82, 14, 90));
            SetDefaultElement(ControlType.EditBox, EditBox.TopLeftBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(14, 82, 241, 90));
            SetDefaultElement(ControlType.EditBox, EditBox.TopBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(241, 82, 246, 90));
            SetDefaultElement(ControlType.EditBox, EditBox.TopRightBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(8, 90, 14, 113));
            SetDefaultElement(ControlType.EditBox, EditBox.LeftBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(241, 90, 246, 113));
            SetDefaultElement(ControlType.EditBox, EditBox.RightBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(8, 113, 14, 121));
            SetDefaultElement(ControlType.EditBox, EditBox.LowerLeftBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(14, 113, 241, 121));
            SetDefaultElement(ControlType.EditBox, EditBox.LowerBorder, e);
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(241, 113, 246, 121));
            SetDefaultElement(ControlType.EditBox, EditBox.LowerRightBorder, e);


            //-------------------------------------
            // Listbox - Main
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(13, 123, 241, 160));
            e.SetFont(0, BlackColorValue, DrawTextFormat.Left | DrawTextFormat.Top);
            // Assign the element
            SetDefaultElement(ControlType.ListBox, ListBox.MainLayer, e);

            //-------------------------------------
            // Listbox - Selection
            //-------------------------------------
            e.SetTexture(0, System.Drawing.Rectangle.FromLTRB(16, 166, 240, 183));
            e.SetFont(0, WhiteColorValue, DrawTextFormat.Left | DrawTextFormat.Top);
            // Assign the element
            SetDefaultElement(ControlType.ListBox, ListBox.SelectionLayer, e);
        }

        /// <summary>Removes all controls from this dialog</summary>
        public void RemoveAllControls()
        {
            controlList.Clear();
            if ((controlFocus != null) && (controlFocus.Parent == this))
                controlFocus = null;

            controlMouseOver = null;
        }

        /// <summary>Clears the radio button group</summary>
        public void ClearRadioButtonGroup(uint groupIndex)
        {
            // Find all radio buttons with the given group number
            foreach (Control c in controlList)
            {
                if (c.ControlType == ControlType.RadioButton)
                {
                    RadioButton rb = c as RadioButton;
                    // Clear the radio button checked setting
                    if (rb.ButtonGroup == groupIndex)
                        rb.SetChecked(false, false);
                }
            }
        }

        /// <summary>Clears the combo box of all items</summary>
        public void ClearComboBox(int id)
        {
            ComboBox comboBox = GetComboBox(id);
            if (comboBox == null)
                return;

            comboBox.Clear();
        }

        #region Message handling
        private static bool isDragging;
        /// <summary>
        /// Handle messages for this dialog
        /// </summary>
        public bool MessageProc(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            // If caption is enable, check for clicks in the caption area.
            if (hasCaption)
            {
                if (msg == NativeMethods.WindowMessage.LeftButtonDown || msg == NativeMethods.WindowMessage.LeftButtonDoubleClick)
                {
                    // Current mouse position
                    short mouseX = NativeMethods.LoWord((uint)lParam.ToInt32());
                    short mouseY = NativeMethods.HiWord((uint)lParam.ToInt32());

                    if (mouseX >= dialogX && mouseX < dialogX + width &&
                        mouseY >= dialogY && mouseY < dialogY + captionHeight)
                    {
                        isDragging = true;
                        NativeMethods.SetCapture(hWnd);
                        return true;
                    }
                }
                else if ((msg == NativeMethods.WindowMessage.LeftButtonUp) && isDragging)
                {
                    // Current mouse position
                    short mouseX = NativeMethods.LoWord((uint)lParam.ToInt32());
                    short mouseY = NativeMethods.HiWord((uint)lParam.ToInt32());

                    if (mouseX >= dialogX && mouseX < dialogX + width &&
                        mouseY >= dialogY && mouseY < dialogY + captionHeight)
                    {
                        NativeMethods.ReleaseCapture();
                        isDragging = false;
                        return true;
                    }
                }
            }

            // If the dialog is minimized, don't send any messages to controls.
            if (isDialogMinimized)
                return false;

            // If a control is in focus, it belongs to this dialog, and it's enabled, then give
            // it the first chance at handling the message.
            if (controlFocus != null &&
                controlFocus.Parent == this &&
                controlFocus.IsEnabled)
            {
                // If the control MsgProc handles it, then we don't.
                if (controlFocus.MsgProc(hWnd, msg, wParam, lParam))
                    return true;
            }

            switch (msg)
            {
                // Call OnFocusIn()/OnFocusOut() of the control that currently has the focus
                // as the application is activated/deactivated.  This matches the Windows
                // behavior.
                case NativeMethods.WindowMessage.ActivateApplication:
                    {
                        if (controlFocus != null &&
                            controlFocus.Parent == this &&
                            controlFocus.IsEnabled)
                        {
                            if (wParam != IntPtr.Zero)
                                controlFocus.OnFocusIn();
                            else
                                controlFocus.OnFocusOut();
                        }
                    }
                    break;

                // Keyboard messages
                case NativeMethods.WindowMessage.KeyDown:
                case NativeMethods.WindowMessage.SystemKeyDown:
                case NativeMethods.WindowMessage.KeyUp:
                case NativeMethods.WindowMessage.SystemKeyUp:
                    {
                        // If a control is in focus, it belongs to this dialog, and it's enabled, then give
                        // it the first chance at handling the message.
                        if (controlFocus != null &&
                            controlFocus.Parent == this &&
                            controlFocus.IsEnabled)
                        {
                            // If the control MsgProc handles it, then we don't.
                            if (controlFocus.HandleKeyboard(msg, wParam, lParam))
                                return true;
                        }

                        // Not yet handled, see if this matches a control's hotkey
                        if (msg == NativeMethods.WindowMessage.KeyUp)
                        {
                            foreach (Control c in controlList)
                            {
                                // Was the hotkey hit?
                                if (c.Hotkey == (System.Windows.Forms.Keys)wParam.ToInt32())
                                {
                                    // Yup!
                                    c.OnHotKey();
                                    return true;
                                }
                            }
                        }
                        if (msg == NativeMethods.WindowMessage.KeyDown)
                        {
                            // If keyboard input is not enabled, this message should be ignored
                            if (!usingKeyboardInput)
                                return false;

                            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)wParam.ToInt32();
                            switch (key)
                            {
                                case System.Windows.Forms.Keys.Right:
                                case System.Windows.Forms.Keys.Down:
                                    if (controlFocus != null)
                                    {
                                        OnCycleFocus(true);
                                        return true;
                                    }
                                    break;
                                case System.Windows.Forms.Keys.Left:
                                case System.Windows.Forms.Keys.Up:
                                    if (controlFocus != null)
                                    {
                                        OnCycleFocus(false);
                                        return true;
                                    }
                                    break;
                                case System.Windows.Forms.Keys.Tab:
                                    if (controlFocus == null)
                                    {
                                        FocusDefaultControl();
                                    }
                                    else
                                    {
                                        bool shiftDown = NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ShiftKey);

                                        OnCycleFocus(!shiftDown);
                                    }
                                    return true;
                            }
                        }
                    }
                    break;

                // Mouse messages
                case NativeMethods.WindowMessage.MouseMove:
                case NativeMethods.WindowMessage.MouseWheel:
                case NativeMethods.WindowMessage.LeftButtonUp:
                case NativeMethods.WindowMessage.LeftButtonDown:
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                case NativeMethods.WindowMessage.RightButtonUp:
                case NativeMethods.WindowMessage.RightButtonDown:
                case NativeMethods.WindowMessage.RightButtonDoubleClick:
                case NativeMethods.WindowMessage.MiddleButtonUp:
                case NativeMethods.WindowMessage.MiddleButtonDown:
                case NativeMethods.WindowMessage.MiddleButtonDoubleClick:
                case NativeMethods.WindowMessage.XButtonUp:
                case NativeMethods.WindowMessage.XButtonDown:
                case NativeMethods.WindowMessage.XButtonDoubleClick:
                    {
                        // If not accepting mouse input, return false to indicate the message should still 
                        // be handled by the application (usually to move the camera).
                        if (!usingMouseInput)
                            return false;

                        // Current mouse position
                        short mouseX = NativeMethods.LoWord((uint)lParam.ToInt32());
                        short mouseY = NativeMethods.HiWord((uint)lParam.ToInt32());
                        System.Drawing.Point mousePoint = new System.Drawing.Point(mouseX, mouseY);
                        // Offset mouse point
                        mousePoint.X -= dialogX;
                        mousePoint.Y -= dialogY;

                        // If caption is enabled, offset the Y coordinate by the negative of its height.
                        if (hasCaption)
                            mousePoint.Y -= captionHeight;

                        // If a control is in focus, it belongs to this dialog, and it's enabled, then give
                        // it the first chance at handling the message.
                        if (controlFocus != null &&
                            controlFocus.Parent == this &&
                            controlFocus.IsEnabled)
                        {
                            // If the control MsgProc handles it, then we don't.
                            if (controlFocus.HandleMouse(msg, mousePoint, wParam, lParam))
                                return true;
                        }

                        // Not yet handled, see if the mouse is over any controls
                        Control control = GetControlAtPoint(mousePoint);
                        if ((control != null) && (control.IsEnabled))
                        {
                            // Let the control handle the mouse if it wants (and return true if it handles it)
                            if (control.HandleMouse(msg, mousePoint, wParam, lParam))
                                return true;
                        }
                        else
                        {
                            // Mouse not over any controls in this dialog, if there was a control
                            // which had focus it just lost it
                            if (msg == NativeMethods.WindowMessage.LeftButtonDown &&
                                controlFocus != null &&
                                controlFocus.Parent == this)
                            {
                                controlFocus.OnFocusOut();
                                controlFocus = null;
                            }
                        }

                        // Still not handled, hand this off to the dialog. Return false to indicate the
                        // message should still be handled by the application (usually to move the camera).
                        switch (msg)
                        {
                            case NativeMethods.WindowMessage.MouseMove:
                                OnMouseMove(mousePoint);
                                return false;
                        }


                    }
                    break;
            }

            // Didn't handle this message
            return false;
        }

        /// <summary>
        /// Handle mouse moves
        /// </summary>
        private void OnMouseMove(System.Drawing.Point pt)
        {
            // If the mouse was previously hovering over a control, it's either
            // still over the control or has left
            if (controlMouseDown != null)
            {
                // If another dialog owns this control then let that dialog handle it
                if (controlMouseDown.Parent != this)
                    return;

                // If the same control is still under the mouse, nothing needs to be done
                if (controlMouseDown.ContainsPoint(pt))
                    return;

                // Mouse has moved outside the control, notify the control and continue
                controlMouseDown.OnMouseExit();
                controlMouseDown = null;
            }

            // Figure out which control the mouse is over now
            Control control = GetControlAtPoint(pt);
            if (control != null)
            {
                controlMouseDown = control;
                controlMouseDown.OnMouseEnter();
            }
        }
        #endregion

        #region Focus
        /// <summary>
        /// Request that this control has focus
        /// </summary>
        public static void RequestFocus(Control control)
        {
            if (controlFocus == control)
                return; // Already does

            if (!control.CanHaveFocus)
                return; // Can't have focus

            if (controlFocus != null)
                controlFocus.OnFocusOut();

            // Set the control focus now
            control.OnFocusIn();
            controlFocus = control;
        }

        /// <summary>
        /// Clears focus of the dialog
        /// </summary>
        public static void ClearFocus()
        {
            if (controlFocus != null)
            {
                controlFocus.OnFocusOut();
                controlFocus = null;
            }
        }
        /// <summary>
        /// Cycles focus to the next available control
        /// </summary>
        private void OnCycleFocus(bool forward)
        {
            // This should only be handled by the dialog which owns the focused control, and 
            // only if a control currently has focus
            if (controlFocus == null || controlFocus.Parent != this)
                return;

            Control control = controlFocus;
            // Go through a bunch of controls
            for (int i = 0; i < 0xffff; i++)
            {
                control = (forward) ? GetNextControl(control) : GetPreviousControl(control);

                // If we've gone in a full circle, focus won't change
                if (control == controlFocus)
                    return;

                // If the dialog accepts keybord input and the control can have focus then
                // move focus
                if (control.Parent.IsUsingKeyboardInput && control.CanHaveFocus)
                {
                    controlFocus.OnFocusOut();
                    controlFocus = control;
                    controlFocus.OnFocusIn();
                    return;
                }
            }

            throw new InvalidOperationException("Multiple dialogs are improperly chained together.");
        }

        /// <summary>
        /// Gets the next control
        /// </summary>
        private static Control GetNextControl(Control control)
        {
            int index = (int)control.index + 1;

            Dialog dialog = control.Parent;

            // Cycle through dialogs in the loop to find the next control. Note
            // that if only one control exists in all looped dialogs it will
            // be the returned 'next' control.
            while (index >= (int)dialog.controlList.Count)
            {
                dialog = dialog.nextDialog;
                index = 0;
            }

            return dialog.controlList[index] as Control;
        }
        /// <summary>
        /// Gets the previous control
        /// </summary>
        private static Control GetPreviousControl(Control control)
        {
            int index = (int)control.index - 1;

            Dialog dialog = control.Parent;

            // Cycle through dialogs in the loop to find the next control. Note
            // that if only one control exists in all looped dialogs it will
            // be the returned 'previous' control.
            while (index < 0)
            {
                dialog = dialog.prevDialog;
                if (dialog == null)
                    dialog = control.Parent;

                index = dialog.controlList.Count - 1;
            }

            return dialog.controlList[index] as Control;
        }
        /// <summary>
        /// Sets focus to the default control of a dialog
        /// </summary>
        private void FocusDefaultControl()
        {
            // Check for a default control in this dialog
            foreach (Control c in controlList)
            {
                if (c.isDefault)
                {
                    // Remove focus from the current control
                    ClearFocus();

                    // Give focus to the default control
                    controlFocus = c;
                    controlFocus.OnFocusIn();
                    return;
                }
            }
        }
        #endregion

        #region Controls Methods/Properties
        /// <summary>Sets the control enabled property</summary>
        public void SetControlEnable(int id, bool isenabled)
        {
            Control c = GetControl(id);
            if (c == null)
                return; // No control to set

            c.IsEnabled = isenabled;
        }
        /// <summary>Gets the control enabled property</summary>
        public bool GetControlEnable(int id)
        {
            Control c = GetControl(id);
            if (c == null)
                return false; // No control to get

            return c.IsEnabled;
        }

        /// <summary>Returns the control located at a point (if one exists)</summary>
        public Control GetControlAtPoint(System.Drawing.Point pt)
        {
            foreach (Control c in controlList)
            {
                if (c == null)
                    continue;

                if (c.IsEnabled && c.IsVisible && c.ContainsPoint(pt))
                    return c;
            }

            return null;
        }
        /// <summary>Returns the control located at this index(if one exists)</summary>
        public Control GetControl(int id)
        {
            foreach (Control c in controlList)
            {
                if (c == null)
                    continue;

                if (c.ID == id)
                    return c;
            }

            return null;
        }
        /// <summary>Returns the control located at this index of this type(if one exists)</summary>
        public Control GetControl(int id, ControlType typeControl)
        {
            foreach (Control c in controlList)
            {
                if (c == null)
                    continue;

                if ((c.ID == id) && (c.ControlType == typeControl))
                    return c;
            }

            return null;
        }

        /// <summary>Returns the static text control located at this index(if one exists)</summary>
        public StaticText GetStaticText(int id) { return GetControl(id, ControlType.StaticText) as StaticText; }
        /// <summary>Returns the button control located at this index(if one exists)</summary>
        public Button GetButton(int id) { return GetControl(id, ControlType.Button) as Button; }
        /// <summary>Returns the checkbox control located at this index(if one exists)</summary>
        public Checkbox GetCheckbox(int id) { return GetControl(id, ControlType.CheckBox) as Checkbox; }
        /// <summary>Returns the radio button control located at this index(if one exists)</summary>
        public RadioButton GetRadioButton(int id) { return GetControl(id, ControlType.RadioButton) as RadioButton; }
        /// <summary>Returns the combo box control located at this index(if one exists)</summary>
        public ComboBox GetComboBox(int id) { return GetControl(id, ControlType.ComboBox) as ComboBox; }
        /// <summary>Returns the slider control located at this index(if one exists)</summary>
        public Slider GetSlider(int id) { return GetControl(id, ControlType.Slider) as Slider; }
        /// <summary>Returns the listbox control located at this index(if one exists)</summary>
        public ListBox GetListBox(int id) { return GetControl(id, ControlType.ListBox) as ListBox; }
        #endregion

        #region Default Elements
        /// <summary>
        /// Sets the default element
        /// </summary>
        public void SetDefaultElement(ControlType ctype, uint index, Element e)
        {
            // If this element already exists, just update it
            for (int i = 0; i < defaultElementList.Count; i++)
            {
                ElementHolder holder = (ElementHolder)defaultElementList[i];
                if ((holder.ControlType == ctype) &&
                    (holder.ElementIndex == index))
                {
                    // Found it, update it
                    holder.Element = e.Clone();
                    defaultElementList[i] = holder;
                    return;
                }
            }

            // Couldn't find it, add a new entry
            ElementHolder newEntry = new ElementHolder();
            newEntry.ControlType = ctype;
            newEntry.ElementIndex = index;
            newEntry.Element = e.Clone();

            // Add it now
            defaultElementList.Add(newEntry);
        }
        /// <summary>
        /// Gets the default element
        /// </summary>
        public Element GetDefaultElement(ControlType ctype, uint index)
        {
            for (int i = 0; i < defaultElementList.Count; i++)
            {
                ElementHolder holder = (ElementHolder)defaultElementList[i];
                if ((holder.ControlType == ctype) &&
                    (holder.ElementIndex == index))
                {
                    // Found it, return it
                    return holder.Element;
                }
            }
            return null;
        }
        #endregion

        #region Texture/Font Resources
        /// <summary>
        /// Shared resource access. Indexed fonts and textures are shared among
        /// all the controls.
        /// </summary>
        public void SetFont(uint index, string faceName, uint height, FontWeight weight)
        {
            // Make sure the list is at least big enough to hold this index
            for (uint i = (uint)fontList.Count; i <= index; i++)
                fontList.Add((int)(-1));

            int fontIndex = DialogResourceManager.GetGlobalInstance().AddFont(faceName, height, weight);
            fontList[(int)index] = fontIndex;
        }
        /// <summary>
        /// Shared resource access. Indexed fonts and textures are shared among
        /// all the controls.
        /// </summary>
        public FontNode GetFont(uint index)
        {
            return DialogResourceManager.GetGlobalInstance().GetFontNode((int)fontList[(int)index]);
        }
        /// <summary>
        /// Shared resource access. Indexed fonts and textures are shared among
        /// all the controls.
        /// </summary>
        public void SetTexture(uint index, string filename)
        {
            // Make sure the list is at least big enough to hold this index
            for (uint i = (uint)textureList.Count; i <= index; i++)
                textureList.Add((int)(-1));

            int textureIndex = DialogResourceManager.GetGlobalInstance().AddTexture(filename);
            textureList[(int)index] = textureIndex;
        }
        /// <summary>
        /// Shared resource access. Indexed fonts and textures are shared among
        /// all the controls.
        /// </summary>
        public TextureNode GetTexture(uint index)
        {
            return DialogResourceManager.GetGlobalInstance().GetTextureNode((int)textureList[(int)index]);
        }
        #endregion

        #region Control Creation
        /// <summary>
        /// Initializes a control
        /// </summary>
        public void InitializeControl(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control", "You cannot pass in a null control to initialize");

            // Set the index
            control.index = (uint)controlList.Count;

            // Look for a default element entires
            for (int i = 0; i < defaultElementList.Count; i++)
            {
                // Find any elements for this control
                ElementHolder holder = (ElementHolder)defaultElementList[i];
                if (holder.ControlType == control.ControlType)
                    control[holder.ElementIndex] = holder.Element;
            }

            // Initialize the control
            control.OnInitialize();
        }
        /// <summary>
        /// Adds a control to the dialog
        /// </summary>
        public void AddControl(Control control)
        {
            // Initialize the control first
            InitializeControl(control);

            // Add this to the control list
            controlList.Add(control);
        }

        /// <summary>
        /// Removes a control from the dialog
        /// </summary>
        /// <param name="control"></param>
        public void RemoveControl(Control control)
        {
            controlList.Remove(control);
        }

        /// <summary>Adds a static text control to the dialog</summary>
        public StaticText AddStatic(int id, string text, int x, int y, int w, int h, bool isDefault)
        {
            // First create the static
            StaticText s = new StaticText(this);

            // Now call the add control method
            AddControl(s);

            // Set the properties of the static now
            s.ID = id;
            s.SetText(text);
            s.SetLocation(x, y);
            s.SetSize(w, h);
            s.isDefault = isDefault;

            return s;
        }        
        /// <summary>Adds a static text control to the dialog</summary>
        public StaticText AddStatic(int id, string text, int x, int y, int w, int h) { return AddStatic(id, text, x, y, w, h, false); }

        /// <summary>Adds a picture control to the dialog</summary>
        public Picture AddPicture(int id, string textureFile, int x, int y, int w, int h)
        {
            // Create the picture
            Picture p = new Picture(this, textureFile);

            // Add the control
            AddControl(p);

            // Set the properties
            p.ID = id;
            p.SetLocation(x, y);
            p.SetSize(w, h);
            p.isDefault = false;

            return p;
        }
        /// <summary>Adds a button control to the dialog</summary>
        public Button AddButton(int id, string text, int x, int y, int w, int h, System.Windows.Forms.Keys hotkey, bool isDefault)
        {
            // First create the button
            Button b = new Button(this);

            // Now call the add control method
            AddControl(b);

            // Set the properties of the button now
            b.ID = id;
            b.SetText(text);
            b.SetLocation(x, y);
            b.SetSize(w, h);
            b.Hotkey = hotkey;
            b.isDefault = isDefault;

            return b;
        }
        /// <summary>Adds a button control to the dialog</summary>
        public Button AddButton(int id, string text, int x, int y, int w, int h) { return AddButton(id, text, x, y, w, h, 0, false); }
        /// <summary>Adds a checkbox to the dialog</summary>
        public Checkbox AddCheckBox(int id, string text, int x, int y, int w, int h, bool ischecked, System.Windows.Forms.Keys hotkey, bool isDefault)
        {
            // First create the checkbox
            Checkbox c = new Checkbox(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the button now
            c.ID = id;
            c.SetText(text);
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.Hotkey = hotkey;
            c.isDefault = isDefault;
            c.IsChecked = ischecked;

            return c;
        }
        /// <summary>Adds a checkbox control to the dialog</summary>
        public Checkbox AddCheckBox(int id, string text, int x, int y, int w, int h, bool ischecked) { return AddCheckBox(id, text, x, y, w, h, ischecked, 0, false); }
        /// <summary>Adds a radiobutton to the dialog</summary>
        public RadioButton AddRadioButton(int id, uint groupId, string text, int x, int y, int w, int h, bool ischecked, System.Windows.Forms.Keys hotkey, bool isDefault)
        {
            // First create the RadioButton
            RadioButton c = new RadioButton(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the button now
            c.ID = id;
            c.ButtonGroup = groupId;
            c.SetText(text);
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.Hotkey = hotkey;
            c.isDefault = isDefault;
            c.IsChecked = ischecked;

            return c;
        }
        /// <summary>Adds a radio button control to the dialog</summary>
        public RadioButton AddRadioButton(int id, uint groupId, string text, int x, int y, int w, int h, bool ischecked) { return AddRadioButton(id, groupId, text, x, y, w, h, ischecked, 0, false); }
        /// <summary>Adds a combobox control to the dialog</summary>
        public ComboBox AddComboBox(int id, int x, int y, int w, int h, System.Windows.Forms.Keys hotkey, bool isDefault)
        {
            // First create the combo
            ComboBox c = new ComboBox(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the button now
            c.ID = id;
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.Hotkey = hotkey;
            c.isDefault = isDefault;

            return c;
        }
        /// <summary>Adds a combobox control to the dialog</summary>
        public ComboBox AddComboBox(int id, int x, int y, int w, int h) { return AddComboBox(id, x, y, w, h, 0, false); }
        /// <summary>Adds a slider control to the dialog</summary>
        public Slider AddSlider(int id, int x, int y, int w, int h, int min, int max, int initialValue, bool isDefault)
        {
            // First create the slider
            Slider c = new Slider(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the button now
            c.ID = id;
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.isDefault = isDefault;
            c.SetRange(min, max);
            c.Value = initialValue;

            return c;
        }
        /// <summary>Adds a slider control to the dialog</summary>
        public Slider AddSlider(int id, int x, int y, int w, int h) { return AddSlider(id, x, y, w, h, 0, 100, 50, false); }
        /// <summary>Adds a listbox control to the dialog</summary>
        public ListBox AddListBox(int id, int x, int y, int w, int h, ListBoxStyle style)
        {
            // First create the listbox
            ListBox c = new ListBox(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the button now
            c.ID = id;
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.Style = style;

            return c;
        }
        /// <summary>Adds a listbox control to the dialog</summary>
        public ListBox AddListBox(int id, int x, int y, int w, int h) { return AddListBox(id, x, y, w, h, ListBoxStyle.SingleSelection); }
        /// <summary>Adds an edit box control to the dialog</summary>
        public EditBox AddEditBox(int id, string text, int x, int y, int w, int h, bool isDefault)
        {
            // First create the editbox
            EditBox c = new EditBox(this);

            // Now call the add control method
            AddControl(c);

            // Set the properties of the static now
            c.ID = id;
            c.Text = (text != null) ? text : string.Empty;
            c.SetLocation(x, y);
            c.SetSize(w, h);
            c.isDefault = isDefault;

            return c;
        }
        /// <summary>Adds an edit box control to the dialog</summary>
        public EditBox AddEditBox(int id, string text, int x, int y, int w, int h) { return AddEditBox(id, text, x, y, w, h, false); }
        #endregion

        /// <summary>Render the dialog</summary>
        private void UpdateVertices()
        {
            vertices = new Microsoft.DirectX.Direct3D.CustomVertex.TransformedColoredTextured[] {
                new CustomVertex.TransformedColoredTextured(dialogX,dialogY,0.5f,1.0f,topLeftColor.ToArgb(),0.0f,0.5f),
                new CustomVertex.TransformedColoredTextured(dialogX + width,dialogY,0.5f,1.0f,topRightColor.ToArgb(),1.0f,0.5f),
                new CustomVertex.TransformedColoredTextured(dialogX+width,dialogY+height,0.5f,1.0f,bottomRightColor.ToArgb(),1.0f,1.0f),
                new CustomVertex.TransformedColoredTextured(dialogX,dialogY+height,0.5f,1.0f,bottomLeftColor.ToArgb(),0.0f,1.0f)
            };
        }
        #region Drawing methods
        /// <summary>Render the dialog</summary>
        public void OnRender(float elapsedTime)
        {
            // See if the dialog needs to be refreshed
            if (timeLastRefresh < timeRefresh)
            {
                timeLastRefresh = FrameworkTimer.GetTime();
                Refresh();
            }

            Device device = DialogResourceManager.GetGlobalInstance().Device;

            // Set up a state block here and restore it when finished drawing all the controls
            DialogResourceManager.GetGlobalInstance().StateBlock.Capture();

            // Set some render/texture states
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            device.RenderState.AlphaTestEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.SelectArg2;
            device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
            device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;
            device.TextureState[0].AlphaArgument1 = TextureArgument.Diffuse;
            device.RenderState.ZBufferEnable = false;
            // Clear vertex/pixel shader
            device.VertexShader = null;
            device.PixelShader = null;

            // Render if not minimized
            if (!isDialogMinimized)
            {               
                device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
                device.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, vertices);             
            }

            // Reset states
            device.TextureState[0].ColorOperation = TextureOperation.Modulate;
            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
            device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
            device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
            device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
            device.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;

            device.SamplerState[0].MinFilter = TextureFilter.Linear;

            // Set the texture up, and begin the sprite
            TextureNode tNode = GetTexture(0);
            device.SetTexture(0, tNode.Texture);
            DialogResourceManager.GetGlobalInstance().Sprite.Begin(SpriteFlags.DoNotSaveState);

            // Render the caption if it's enabled.
            if (hasCaption)
            {
                // DrawSprite will offset the rect down by
                // captionHeight, so adjust the rect higher
                // here to negate the effect.
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, -captionHeight, width, 0);
                DrawSprite(captionElement, rect);
                rect.Offset(5, 0); // Make a left margin
                string output = caption + ((isDialogMinimized) ? " (Minimized)" : null);
                DrawText(output, captionElement, rect, true);
            }

            // If the dialog is minimized, skip rendering
            // its controls.
            if (!isDialogMinimized)
            {
                for (int i = 0; i < controlList.Count; i++)
                {
                    // Focused control is drawn last
                    if (controlList[i] == controlFocus)
                        continue;

                    (controlList[i] as Control).Render(device, elapsedTime);
                }

                // Render the focus control if necessary
                if (controlFocus != null && controlFocus.Parent == this)
                    controlFocus.Render(device, elapsedTime);
            }

            // End the sprite and apply the stateblock
            DialogResourceManager.GetGlobalInstance().Sprite.End();
            DialogResourceManager.GetGlobalInstance().StateBlock.Apply();
        }

        /// <summary>
        /// Refresh the dialog
        /// </summary>
        private void Refresh()
        {
            // Reset the controls
            if (controlFocus != null)
                controlFocus.OnFocusOut();

            if (controlMouseOver != null)
                controlMouseOver.OnMouseExit();

            controlFocus = null;
            controlMouseDown = null;
            controlMouseOver = null;

            // Refresh any controls
            foreach (Control c in controlList)
            {
                c.Refresh();
            }

            if (usingKeyboardInput)
                FocusDefaultControl();
        }

        /// <summary>Draw's some text</summary>
        public void DrawText(string text, Element element, System.Drawing.Rectangle rect, bool shadow)
        {
            // No need to draw fully transparant layers
            if (element.FontColor.Current.Alpha == 0)
                return; // Nothing to do

            System.Drawing.Rectangle screenRect = rect;
            screenRect.Offset(dialogX, dialogY);

            // If caption is enabled, offset the Y position by its height.
            if (hasCaption)
                screenRect.Offset(0, captionHeight);

            // Set the identity transform
            DialogResourceManager.GetGlobalInstance().Sprite.Transform = Matrix.Identity;

            // Get the font node here
            FontNode fNode = GetFont(element.FontIndex);
            if (shadow)
            {
                // Render the text shadowed
                System.Drawing.Rectangle shadowRect = screenRect;
                shadowRect.Offset(1, 1);
                fNode.Font.DrawText(DialogResourceManager.GetGlobalInstance().Sprite, text,
                    shadowRect, element.textFormat, unchecked((int)0xff000000));
            }

            fNode.Font.DrawText(DialogResourceManager.GetGlobalInstance().Sprite, text,
                screenRect, element.textFormat, element.FontColor.Current.ToArgb());
        }
        /// <summary>Draw a sprite</summary>
        public void DrawSprite(Element element, System.Drawing.Rectangle rect)
        {
            // No need to draw fully transparant layers
            if (element.TextureColor.Current.Alpha == 0)
                return; // Nothing to do

            System.Drawing.Rectangle texRect = element.textureRect;
            System.Drawing.Rectangle screenRect = rect;
            screenRect.Offset(dialogX, dialogY);

            // If caption is enabled, offset the Y position by its height.
            if (hasCaption)
                screenRect.Offset(0, captionHeight);

            // Get the texture
            TextureNode tNode = GetTexture(element.TextureIndex);
            float scaleX = (float)screenRect.Width / (float)texRect.Width;
            float scaleY = (float)screenRect.Height / (float)texRect.Height;

            // Set the scaling transform
            DialogResourceManager.GetGlobalInstance().Sprite.Transform = Matrix.Scaling(scaleX, scaleY, 1.0f);

            // Calculate the position
            Vector3 pos = new Vector3(screenRect.Left, screenRect.Top, 0.0f);
            pos.X /= scaleX;
            pos.Y /= scaleY;

            // Finally draw the sprite
            DialogResourceManager.GetGlobalInstance().Sprite.Draw(tNode.Texture, texRect, new Vector3(), pos, element.TextureColor.Current.ToArgb());
        }
        /// <summary>Draw a Texture</summary>
        public void DrawTexture(Texture texture, System.Drawing.Rectangle sourceRect, System.Drawing.Rectangle targetRect, BlendColor blendColor)
        {
            DrawTexture(texture, sourceRect, targetRect, blendColor, 0);
        }

        /// <summary>Draw a Texture</summary>
        public void DrawTexture(Texture texture, System.Drawing.Rectangle sourceRect, System.Drawing.Rectangle targetRect, BlendColor blendColor, float rotationAngle)
        {
            System.Drawing.Rectangle screenRect = targetRect;
            screenRect.Offset(dialogX, dialogY);

            // If caption is enabled, offset the Y position by its height.
            if (hasCaption)
                screenRect.Offset(0, captionHeight);

            //float scaleX = (float)screenRect.Width / (float)targetRect.Width;
            //float scaleY = (float)screenRect.Height / (float)targetRect.Height;
            float scaleX = (float)targetRect.Width / (float)sourceRect.Width;
            float scaleY = (float)targetRect.Height / (float)sourceRect.Height;

            // Set the scaling transform
            Vector3 pos = new Vector3(screenRect.Left / scaleX, screenRect.Top / scaleY, 0.0f);
            Vector3 center = new Vector3();
            if (rotationAngle != 0)
            {
                // Calculate the position
                center = new Vector3(sourceRect.Width / 2, sourceRect.Height / 2, 0);

                DialogResourceManager.GetGlobalInstance().Sprite.Transform = Matrix.RotationZ(rotationAngle) * Matrix.Translation(pos + center) * Matrix.Scaling(scaleX, scaleY, 1.0f);

                pos = new Vector3();
            }
            else
            {
                //DialogResourceManager.GetGlobalInstance().Sprite.Transform = Matrix.Scaling(scaleX, scaleY, 1.0f);
                DialogResourceManager.GetGlobalInstance().Sprite.Transform = Matrix.Scaling(scaleX, scaleY, 1.0f);
            }
            // Finally draw the sprite
            DialogResourceManager.GetGlobalInstance().Sprite.Draw(texture, sourceRect, center, pos, blendColor.Current.ToArgb());
        }
        /// <summary>Draw's some text</summary>
        public void DrawText(string text, Element element, System.Drawing.Rectangle rect) { this.DrawText(text, element, rect, false); }
        /// <summary>Draw a rectangle</summary>
        public void DrawRectangle(System.Drawing.Rectangle rect, ColorValue color)
        {
            // Offset the rectangle
            rect.Offset(dialogX, dialogY);

            // If caption is enabled, offset the Y position by its height
            if (hasCaption)
                rect.Offset(0, captionHeight);

            // Get the integer value of the color
            int realColor = color.ToArgb();
            // Create some vertices
            CustomVertex.TransformedColoredTextured[] verts = {
                new CustomVertex.TransformedColoredTextured((float)rect.Left - 0.5f, (float)rect.Top -0.5f, 0.5f, 1.0f, realColor, 0, 0),
                new CustomVertex.TransformedColoredTextured((float)rect.Right - 0.5f, (float)rect.Top -0.5f, 0.5f, 1.0f, realColor, 0, 0),
                new CustomVertex.TransformedColoredTextured((float)rect.Right - 0.5f, (float)rect.Bottom -0.5f, 0.5f, 1.0f, realColor, 0, 0),
                new CustomVertex.TransformedColoredTextured((float)rect.Left - 0.5f, (float)rect.Bottom -0.5f, 0.5f, 1.0f, realColor, 0, 0),
            };

            // Get the device
            Device device = SampleFramework.Device;

            // Since we're doing our own drawing here, we need to flush the sprites
            DialogResourceManager.GetGlobalInstance().Sprite.Flush();
            // Preserve the devices current vertex declaration
            using (VertexDeclaration decl = device.VertexDeclaration)
            {
                // Set the vertex format
                device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;

                // Set some texture states
                device.TextureState[0].ColorOperation = TextureOperation.SelectArg2;
                device.TextureState[0].AlphaOperation = TextureOperation.SelectArg2;

                // Draw the rectangle
                device.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, verts);

                // Reset some texture states
                device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                device.TextureState[0].AlphaOperation = TextureOperation.Modulate;

                // Restore the vertex declaration
                device.VertexDeclaration = decl;
            }
        }
        #endregion
    }
}
