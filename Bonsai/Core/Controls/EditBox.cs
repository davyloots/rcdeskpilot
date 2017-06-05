using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>A basic edit box</summary>
    public class EditBox : Control
    {
        #region Element layers
        public const int TextLayer = 0;
        public const int TopLeftBorder = 1;
        public const int TopBorder = 2;
        public const int TopRightBorder = 3;
        public const int LeftBorder = 4;
        public const int RightBorder = 5;
        public const int LowerLeftBorder = 6;
        public const int LowerBorder = 7;
        public const int LowerRightBorder = 8;
        #endregion

        #region Event code
        public event EventHandler Changed;
        public event EventHandler Enter;
        /// <summary>Raises the changed event</summary>
        protected void RaiseChangedEvent(EditBox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            if (Changed != null)
                Changed(sender, System.EventArgs.Empty);
        }
        /// <summary>Raises the Enter event</summary>
        protected void RaiseEnterEvent(EditBox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            if (Enter != null)
                Enter(sender, System.EventArgs.Empty);
        }
        #endregion

        #region Class Data
        protected System.Windows.Forms.RichTextBox textData; // Text data
        protected int border; // Border of the window
        protected int spacing; // Spacing between the text and the edge of border
        protected System.Drawing.Rectangle textRect; // Bounding rectangle for the text
        protected System.Drawing.Rectangle[] elementRects = new System.Drawing.Rectangle[9];
        protected double blinkTime; // Caret blink time in milliseconds
        protected double lastBlink; // Last timestamp of caret blink
        protected bool isCaretOn; // Flag to indicate whether caret is currently visible
        protected int caretPosition; // Caret position, in characters
        protected bool isInsertMode; // If true, control is in insert mode. Else, overwrite mode.
        protected int firstVisible;  // First visible character in the edit control
        protected ColorValue textColor; // Text color
        protected ColorValue selectedTextColor; // Selected Text color
        protected ColorValue selectedBackColor; // Selected background color
        protected ColorValue caretColor; // Caret color

        // Mouse-specific
        protected bool isMouseDragging; // True to indicate the drag is in progress

        protected static bool isHidingCaret; // If true, we don't render the caret.

        #endregion

        #region Simple overrides/properties/methods
        /// <summary>Can the edit box have focus</summary>
        public override bool CanHaveFocus { get { return (IsVisible && IsEnabled); } }
        /// <summary>Update the spacing</summary>
        public void SetSpacing(int space) { spacing = space; UpdateRectangles(); }
        /// <summary>Update the border</summary>
        public void SetBorderWidth(int b) { border = b; UpdateRectangles(); }
        /// <summary>Update the text color</summary>
        public void SetTextColor(ColorValue color) { textColor = color; }
        /// <summary>Update the text selected color</summary>
        public void SetSelectedTextColor(ColorValue color) { selectedTextColor = color; }
        /// <summary>Update the selected background color</summary>
        public void SetSelectedBackColor(ColorValue color) { selectedBackColor = color; }
        /// <summary>Update the caret color</summary>
        public void SetCaretColor(ColorValue color) { caretColor = color; }

        /// <summary>Get or sets the text</summary>
        public string Text { get { return textData.Text; } set { SetText(value, false); } }
        /// <summary>Gets a copy of the text</summary>
        public string GetTextCopy() { return string.Copy(textData.Text); }
        #endregion

        /// <summary>Creates a new edit box control</summary>
        public EditBox(Dialog parent)
            : base(parent)
        {
            controlType = ControlType.EditBox;
            parentDialog = parent;

            border = 5; // Default border
            spacing = 4; // default spacing
            isCaretOn = true;

            textData = new System.Windows.Forms.RichTextBox();
            // Create the control
            textData.Visible = true;
            textData.Font = new System.Drawing.Font("Arial", 8.0f);
            textData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            textData.Multiline = false;
            textData.Text = string.Empty;
            textData.MaxLength = ushort.MaxValue; // 65k characters should be plenty
            textData.WordWrap = false;
            // Now create the control
            textData.CreateControl();

            isHidingCaret = false;
            firstVisible = 0;
            blinkTime = NativeMethods.GetCaretBlinkTime() * 0.001f;
            lastBlink = FrameworkTimer.GetAbsoluteTime();
            textColor = new ColorValue(0.06f, 0.06f, 0.06f, 1.0f);
            selectedTextColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            selectedBackColor = new ColorValue(0.15f, 0.196f, 0.36f, 1.0f);
            caretColor = new ColorValue(0, 0, 0, 1.0f);
            caretPosition = textData.SelectionStart = 0;
            isInsertMode = true;
            isMouseDragging = false;
        }

        /// <summary>Set the caret to a character position, and adjust the scrolling if necessary</summary>
        protected void PlaceCaret(int pos)
        {
            // Store caret position
            caretPosition = pos;

            // First find the first visible char
            for (int i = 0; i < textData.Text.Length; i++)
            {
                System.Drawing.Point p = textData.GetPositionFromCharIndex(i);
                if (p.X >= 0)
                {
                    firstVisible = i; // This is the first visible character
                    break;
                }
            }

            // if the new position is smaller than the first visible char 
            // we'll need to scroll
            if (firstVisible > caretPosition)
                firstVisible = caretPosition;
        }

        /// <summary>Clears the edit box</summary>
        public void Clear()
        {
            textData.Text = string.Empty;
            PlaceCaret(0);
            textData.SelectionStart = 0;
        }
        /// <summary>Sets the text for the control</summary>
        public void SetText(string text, bool selected)
        {
            if (text == null)
                text = string.Empty;

            textData.Text = text;
            textData.SelectionStart = text.Length;
            // Move the care to the end of the text
            PlaceCaret(text.Length);
            textData.SelectionStart = (selected) ? 0 : caretPosition;
            FocusText();
        }
        /// <summary>Deletes the text that is currently selected</summary>
        protected void DeleteSelectionText()
        {
            int first = Math.Min(caretPosition, textData.SelectionStart);
            int last = Math.Max(caretPosition, textData.SelectionStart);
            // Update caret and selection
            PlaceCaret(first);
            // Remove the characters
            textData.Text = textData.Text.Remove(first, (last - first));
            textData.SelectionStart = caretPosition;
            FocusText();
        }
        /// <summary>Updates the rectangles used by the control</summary>
        protected override void UpdateRectangles()
        {
            // Get the bounding box first
            base.UpdateRectangles();

            // Update text rect
            textRect = boundingBox;
            // First inflate by border to compute render rects
            textRect.Inflate(-border, -border);

            // Update the render rectangles
            elementRects[0] = textRect;
            elementRects[1] = new System.Drawing.Rectangle(boundingBox.Left, boundingBox.Top, (textRect.Left - boundingBox.Left), (textRect.Top - boundingBox.Top));
            elementRects[2] = new System.Drawing.Rectangle(textRect.Left, boundingBox.Top, textRect.Width, (textRect.Top - boundingBox.Top));
            elementRects[3] = new System.Drawing.Rectangle(textRect.Right, boundingBox.Top, (boundingBox.Right - textRect.Right), (textRect.Top - boundingBox.Top));
            elementRects[4] = new System.Drawing.Rectangle(boundingBox.Left, textRect.Top, (textRect.Left - boundingBox.Left), textRect.Height);
            elementRects[5] = new System.Drawing.Rectangle(textRect.Right, textRect.Top, (boundingBox.Right - textRect.Right), textRect.Height);
            elementRects[6] = new System.Drawing.Rectangle(boundingBox.Left, textRect.Bottom, (textRect.Left - boundingBox.Left), (boundingBox.Bottom - textRect.Bottom));
            elementRects[7] = new System.Drawing.Rectangle(textRect.Left, textRect.Bottom, textRect.Width, (boundingBox.Bottom - textRect.Bottom));
            elementRects[8] = new System.Drawing.Rectangle(textRect.Right, textRect.Bottom, (boundingBox.Right - textRect.Right), (boundingBox.Bottom - textRect.Bottom));

            // Inflate further by spacing
            textRect.Inflate(-spacing, -spacing);

            // Make the underlying rich text box the same size
            textData.Size = textRect.Size;
        }

        /// <summary>Copy the selected text to the clipboard</summary>
        protected void CopyToClipboard()
        {
            // Copy the selection text to the clipboard
            if (caretPosition != textData.SelectionStart)
            {
                int first = Math.Min(caretPosition, textData.SelectionStart);
                int last = Math.Max(caretPosition, textData.SelectionStart);
                // Set the text to the clipboard
                System.Windows.Forms.Clipboard.SetDataObject(textData.Text.Substring(first, (last - first)));
            }

        }
        /// <summary>Paste the clipboard data to the control</summary>
        protected void PasteFromClipboard()
        {
            // Get the clipboard data
            System.Windows.Forms.IDataObject clipData = System.Windows.Forms.Clipboard.GetDataObject();
            // Does the clipboard have string data?
            if (clipData.GetDataPresent(System.Windows.Forms.DataFormats.StringFormat))
            {
                // Yes, get that data
                string clipString = clipData.GetData(System.Windows.Forms.DataFormats.StringFormat) as string;
                // find any new lines, remove everything after that
                int index;
                if ((index = clipString.IndexOf("\n")) > 0)
                {
                    clipString = clipString.Substring(0, index - 1);
                }

                // Insert that into the text data
                textData.Text = textData.Text.Insert(caretPosition, clipString);
                caretPosition += clipString.Length;
                textData.SelectionStart = caretPosition;
                FocusText();
            }
        }
        /// <summary>Reset's the caret blink time</summary>
        protected void ResetCaretBlink()
        {
            isCaretOn = true;
            lastBlink = FrameworkTimer.GetAbsoluteTime();
        }

        /// <summary>Update the caret when focus is in</summary>
        public override void OnFocusIn()
        {
            base.OnFocusIn();
            ResetCaretBlink();
        }

        /// <summary>Updates focus to the backing rich textbox so it updates it's state</summary>
        private void FocusText()
        {
            // Because of a design issue with the rich text box control that is used as 
            // the backing store for this control, the 'scrolling' mechanism built into
            // the control will only work if the control has focus.  Setting focus to the 
            // control here would work, but would cause a bad 'flicker' of the application.

            // Therefore, the automatic horizontal scrolling is turned off by default.  To 
            // enable it turn this define on.
#if (SCROLL_CORRECTLY)
            NativeMethods.SetFocus(textData.Handle);
            NativeMethods.SetFocus(Parent.SampleFramework.Window);
#endif
        }

        /// <summary>Handle keyboard input to the edit box</summary>
        public override bool HandleKeyboard(Bonsai.Core.NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            // Default to not handling the message
            bool isHandled = false;
            if (msg == NativeMethods.WindowMessage.KeyDown)
            {
                switch ((System.Windows.Forms.Keys)wParam.ToInt32())
                {
                    case System.Windows.Forms.Keys.End:
                    case System.Windows.Forms.Keys.Home:
                        // Move the caret
                        if (wParam.ToInt32() == (int)System.Windows.Forms.Keys.End)
                            PlaceCaret(textData.Text.Length);
                        else
                            PlaceCaret(0);
                        if (!NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ShiftKey))
                        {
                            // Shift is not down. Update selection start along with caret
                            textData.SelectionStart = caretPosition;
                            FocusText();
                        }

                        ResetCaretBlink();
                        isHandled = true;
                        break;
                    case System.Windows.Forms.Keys.Insert:
                        if (NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ControlKey))
                        {
                            // Control insert -> Copy to clipboard
                            CopyToClipboard();
                        }
                        else if (NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ShiftKey))
                        {
                            // Shift insert -> Paste from clipboard
                            PasteFromClipboard();
                        }
                        else
                        {
                            // Toggle insert mode
                            isInsertMode = !isInsertMode;
                        }
                        break;
                    case System.Windows.Forms.Keys.Delete:
                        // Check to see if there is a text selection
                        if (caretPosition != textData.SelectionStart)
                        {
                            DeleteSelectionText();
                            RaiseChangedEvent(this, true);
                        }
                        else
                        {
                            if (caretPosition < textData.Text.Length)
                            {
                                // Deleting one character
                                textData.Text = textData.Text.Remove(caretPosition, 1);
                                RaiseChangedEvent(this, true);
                            }
                        }
                        ResetCaretBlink();
                        isHandled = true;
                        break;

                    case System.Windows.Forms.Keys.Left:
                        if (NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ControlKey))
                        {
                            // Control is down. Move the caret to a new item
                            // instead of a character.
                        }
                        else if (caretPosition > 0)
                            PlaceCaret(caretPosition - 1); // Move one to the left

                        if (!NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ShiftKey))
                        {
                            // Shift is not down. Update selection
                            // start along with the caret.
                            textData.SelectionStart = caretPosition;
                            FocusText();
                        }
                        ResetCaretBlink();
                        isHandled = true;
                        break;

                    case System.Windows.Forms.Keys.Right:
                        if (NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ControlKey))
                        {
                            // Control is down. Move the caret to a new item
                            // instead of a character.
                        }
                        else if (caretPosition < textData.Text.Length)
                            PlaceCaret(caretPosition + 1); // Move one to the right
                        if (!NativeMethods.IsKeyDown(System.Windows.Forms.Keys.ShiftKey))
                        {
                            // Shift is not down. Update selection
                            // start along with the caret.
                            textData.SelectionStart = caretPosition;
                            FocusText();
                        }
                        ResetCaretBlink();
                        isHandled = true;
                        break;

                    case System.Windows.Forms.Keys.Up:
                    case System.Windows.Forms.Keys.Down:
                        // Trap up and down arrows so that the dialog
                        // does not switch focus to another control.
                        isHandled = true;
                        break;

                    default:
                        // Let the application handle escape
                        isHandled = ((System.Windows.Forms.Keys)wParam.ToInt32()) == System.Windows.Forms.Keys.Escape;
                        break;
                }
            }

            return isHandled;
        }

        /// <summary>Handle mouse messages</summary>
        public override bool HandleMouse(NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            // We need a new point
            System.Drawing.Point p = pt;
            p.X -= textRect.Left;
            p.Y -= textRect.Top;

            switch (msg)
            {
                case NativeMethods.WindowMessage.LeftButtonDown:
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                    // Get focus first
                    if (!hasFocus)
                        Dialog.RequestFocus(this);

                    if (!ContainsPoint(pt))
                        return false;

                    isMouseDragging = true;
                    Parent.SampleFramework.Window.Capture = true;
                    // Determine the character corresponding to the coordinates
                    int index = textData.GetCharIndexFromPosition(p);

                    System.Drawing.Point startPosition = textData.GetPositionFromCharIndex(index);

                    if (p.X > startPosition.X && index < textData.Text.Length)
                        PlaceCaret(index + 1);
                    else
                        PlaceCaret(index);

                    textData.SelectionStart = caretPosition;
                    FocusText();
                    ResetCaretBlink();
                    return true;

                case NativeMethods.WindowMessage.LeftButtonUp:
                    Parent.SampleFramework.Window.Capture = false;
                    isMouseDragging = false;
                    break;
                case NativeMethods.WindowMessage.MouseMove:
                    if (isMouseDragging)
                    {
                        // Determine the character corresponding to the coordinates
                        int dragIndex = textData.GetCharIndexFromPosition(p);

                        if (dragIndex < textData.Text.Length)
                            PlaceCaret(dragIndex + 1);
                        else
                            PlaceCaret(dragIndex);
                    }
                    break;
            }
            return false;
        }

        /// <summary>Handle all other messages</summary>
        public override bool MsgProc(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false;

            if (msg == NativeMethods.WindowMessage.Character)
            {
                int charKey = wParam.ToInt32();
                switch (charKey)
                {
                    case (int)System.Windows.Forms.Keys.Back:
                        {
                            // If there's a selection, treat this
                            // like a delete key.
                            if (caretPosition != textData.SelectionStart)
                            {
                                DeleteSelectionText();
                                RaiseChangedEvent(this, true);
                            }
                            else if (caretPosition > 0)
                            {
                                // Move the caret and delete the char
                                textData.Text = textData.Text.Remove(caretPosition - 1, 1);
                                PlaceCaret(caretPosition - 1);
                                textData.SelectionStart = caretPosition;
                                FocusText();
                                RaiseChangedEvent(this, true);
                            }

                            ResetCaretBlink();
                            break;
                        }
                    case 24: // Ctrl-X Cut
                    case (int)System.Windows.Forms.Keys.Cancel: // Ctrl-C Copy
                        {
                            CopyToClipboard();

                            // If the key is Ctrl-X, delete the selection too.
                            if (charKey == 24)
                            {
                                DeleteSelectionText();
                                RaiseChangedEvent(this, true);
                            }

                            break;
                        }

                    // Ctrl-V Paste
                    case 22:
                        {
                            PasteFromClipboard();
                            RaiseChangedEvent(this, true);
                            break;
                        }
                    case (int)System.Windows.Forms.Keys.Return:
                        // Invoke the event when the user presses Enter.
                        RaiseEnterEvent(this, true);
                        break;

                    // Ctrl-A Select All
                    case 1:
                        {
                            if (textData.SelectionStart == caretPosition)
                            {
                                textData.SelectionStart = 0;
                                PlaceCaret(textData.Text.Length);
                            }
                            break;
                        }

                    // Junk characters we don't want in the string
                    case 26:  // Ctrl Z
                    case 2:   // Ctrl B
                    case 14:  // Ctrl N
                    case 19:  // Ctrl S
                    case 4:   // Ctrl D
                    case 6:   // Ctrl F
                    case 7:   // Ctrl G
                    case 10:  // Ctrl J
                    case 11:  // Ctrl K
                    case 12:  // Ctrl L
                    case 17:  // Ctrl Q
                    case 23:  // Ctrl W
                    case 5:   // Ctrl E
                    case 18:  // Ctrl R
                    case 20:  // Ctrl T
                    case 25:  // Ctrl Y
                    case 21:  // Ctrl U
                    case 9:   // Ctrl I
                    case 15:  // Ctrl O
                    case 16:  // Ctrl P
                    case 27:  // Ctrl [
                    case 29:  // Ctrl ]
                    case 28:  // Ctrl \ 
                        break;

                    default:
                        {
                            // If there's a selection and the user
                            // starts to type, the selection should
                            // be deleted.
                            if (caretPosition != textData.SelectionStart)
                            {
                                DeleteSelectionText();
                            }
                            // If we are in overwrite mode and there is already
                            // a char at the caret's position, simply replace it.
                            // Otherwise, we insert the char as normal.
                            if (!isInsertMode && caretPosition < textData.Text.Length)
                            {
                                // This isn't the most efficient way to do this, but it's simple
                                // and shows the correct behavior
                                char[] charData = textData.Text.ToCharArray();
                                charData[caretPosition] = (char)wParam.ToInt32();
                                textData.Text = new string(charData);
                            }
                            else
                            {
                                // Insert the char
                                char c = (char)wParam.ToInt32();
                                textData.Text = textData.Text.Insert(caretPosition, c.ToString());
                            }

                            // Move the caret and selection position now
                            PlaceCaret(caretPosition + 1);
                            textData.SelectionStart = caretPosition;
                            FocusText();

                            ResetCaretBlink();
                            RaiseChangedEvent(this, true);
                            break;
                        }
                }
            }
            return false;
        }


        /// <summary>Render the control</summary>
        public override void Render(Device device, float elapsedTime)
        {
            if (!IsVisible)
                return; // Nothing to render

            // Render the control graphics
            for (int i = 0; i <= LowerRightBorder; ++i)
            {
                Element e = elementList[i] as Element;
                e.TextureColor.Blend(ControlState.Normal, elapsedTime);
                parentDialog.DrawSprite(e, elementRects[i]);
            }
            //
            // Compute the X coordinates of the first visible character.
            //
            int xFirst = textData.GetPositionFromCharIndex(firstVisible).X;
            int xCaret = textData.GetPositionFromCharIndex(caretPosition).X;
            int xSel;

            if (caretPosition != textData.SelectionStart)
                xSel = textData.GetPositionFromCharIndex(textData.SelectionStart).X;
            else
                xSel = xCaret;

            // Render the selection rectangle
            System.Drawing.Rectangle selRect = System.Drawing.Rectangle.Empty;
            if (caretPosition != textData.SelectionStart)
            {
                int selLeft = xCaret, selRight = xSel;
                // Swap if left is beigger than right
                if (selLeft > selRight)
                {
                    int temp = selLeft;
                    selLeft = selRight;
                    selRight = temp;
                }
                selRect = System.Drawing.Rectangle.FromLTRB(
                    selLeft, textRect.Top, selRight, textRect.Bottom);
                selRect.Offset(textRect.Left - xFirst, 0);
                selRect.Intersect(textRect);
                Parent.DrawRectangle(selRect, selectedBackColor);
            }

            // Render the text
            Element textElement = elementList[TextLayer] as Element;
            textElement.FontColor.Current = textColor;
            parentDialog.DrawText(textData.Text.Substring(firstVisible), textElement, textRect);

            // Render the selected text
            if (caretPosition != textData.SelectionStart)
            {
                int firstToRender = Math.Max(firstVisible, Math.Min(textData.SelectionStart, caretPosition));
                int numToRender = Math.Max(textData.SelectionStart, caretPosition) - firstToRender;
                textElement.FontColor.Current = selectedTextColor;
                parentDialog.DrawText(textData.Text.Substring(firstToRender, numToRender), textElement, selRect);
            }

            //
            // Blink the caret
            //
            if (FrameworkTimer.GetAbsoluteTime() - lastBlink >= blinkTime)
            {
                isCaretOn = !isCaretOn;
                lastBlink = FrameworkTimer.GetAbsoluteTime();
            }

            //
            // Render the caret if this control has the focus
            //
            if (hasFocus && isCaretOn && !isHidingCaret)
            {
                // Start the rectangle with insert mode caret
                System.Drawing.Rectangle caretRect = textRect;
                caretRect.Width = 2;
                caretRect.Location = new System.Drawing.Point(
                    caretRect.Left - xFirst + xCaret - 1,
                    caretRect.Top);

                // If we are in overwrite mode, adjust the caret rectangle
                // to fill the entire character.
                if (!isInsertMode)
                {
                    // Obtain the X coord of the current character
                    caretRect.Width = 4;
                }

                parentDialog.DrawRectangle(caretRect, caretColor);
            }

        }
    }
}
