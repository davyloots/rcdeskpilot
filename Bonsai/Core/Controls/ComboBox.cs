using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>Stores data for a combo box item</summary>
    public struct ComboBoxItem
    {
        public string ItemText;
        public object ItemData;
        public System.Drawing.Rectangle ItemRect;
        public bool IsItemVisible;
    }

    /// <summary>Combo box control</summary>
    public class ComboBox : Button
    {
        public const int MainLayer = 0;
        public const int ComboButtonLayer = 1;
        public const int DropdownLayer = 2;
        public const int SelectionLayer = 3;
        #region Event code
        public event EventHandler Changed;
        /// <summary>Create new button instance</summary>
        protected void RaiseChangedEvent(ComboBox sender, bool wasTriggeredByUser)
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
        private bool isScrollBarInit;

        #region Instance data
        protected int selectedIndex;
        protected int focusedIndex;
        protected int dropHeight;
        protected ScrollBar scrollbarControl;
        protected int scrollWidth;
        protected bool isComboOpen;
        protected System.Drawing.Rectangle textRect;
        protected System.Drawing.Rectangle buttonRect;
        protected System.Drawing.Rectangle dropDownRect;
        protected System.Drawing.Rectangle dropDownTextRect;
        protected ArrayList itemList;
        #endregion

        /// <summary>Create new combo box control</summary>
        public ComboBox(Dialog parent)
            : base(parent)
        {
            // Store control type and parent dialog
            controlType = ControlType.ComboBox;
            parentDialog = parent;
            // Create the scrollbar control too
            scrollbarControl = new ScrollBar(parent);

            // Set some default items
            dropHeight = 100;
            scrollWidth = 16;
            selectedIndex = -1;
            focusedIndex = -1;
            isScrollBarInit = false;

            // Create the item list array
            itemList = new ArrayList();
        }

        /// <summary>Update the rectangles for the combo box control</summary>
        protected override void UpdateRectangles()
        {
            // Get bounding box
            base.UpdateRectangles();

            // Update the bounding box for the items
            buttonRect = new System.Drawing.Rectangle(boundingBox.Right - boundingBox.Height, boundingBox.Top,
                boundingBox.Height, boundingBox.Height);

            textRect = boundingBox;
            textRect.Size = new System.Drawing.Size(textRect.Width - buttonRect.Width, textRect.Height);

            dropDownRect = textRect;
            dropDownRect.Offset(0, (int)(0.9f * textRect.Height));
            dropDownRect.Size = new System.Drawing.Size(dropDownRect.Width - scrollWidth, dropDownRect.Height + dropHeight);

            // Scale it down slightly
            System.Drawing.Point loc = dropDownRect.Location;
            System.Drawing.Size size = dropDownRect.Size;

            loc.X += (int)(0.1f * dropDownRect.Width);
            loc.Y += (int)(0.1f * dropDownRect.Height);
            size.Width -= (2 * (int)(0.1f * dropDownRect.Width));
            size.Height -= (2 * (int)(0.1f * dropDownRect.Height));

            dropDownTextRect = new System.Drawing.Rectangle(loc, size);

            // Update the scroll bars rects too
            scrollbarControl.SetLocation(dropDownRect.Right, dropDownRect.Top + 2);
            scrollbarControl.SetSize(scrollWidth, dropDownRect.Height - 2);
            FontNode fNode = DialogResourceManager.GetGlobalInstance().GetFontNode((int)(elementList[2] as Element).FontIndex);
            if ((fNode != null) && (fNode.Height > 0))
            {
                scrollbarControl.PageSize = (int)(dropDownTextRect.Height / fNode.Height);

                // The selected item may have been scrolled off the page.
                // Ensure that it is in page again.
                scrollbarControl.ShowItem(selectedIndex);
            }
        }

        /// <summary>Sets the drop height of this control</summary>
        public void SetDropHeight(int height) { dropHeight = height; UpdateRectangles(); }
        /// <summary>Sets the scroll bar width of this control</summary>
        public void SetScrollbarWidth(int width) { scrollWidth = width; UpdateRectangles(); }
        /// <summary>Can this control have focus</summary>
        public override bool CanHaveFocus { get { return (IsVisible && IsEnabled); } }
        /// <summary>Number of items current in the list</summary>
        public int NumberItems { get { return itemList.Count; } }
        /// <summary>Indexer for items in the list</summary>
        public ComboBoxItem this[int index]
        {
            get { return (ComboBoxItem)itemList[index]; }
        }

        /// <summary>Initialize the scrollbar control here</summary>
        public override void OnInitialize()
        {
            parentDialog.InitializeControl(scrollbarControl);
        }

        /// <summary>Called when focus leaves the control</summary>
        public override void OnFocusOut()
        {
            // Call base first
            base.OnFocusOut();
            isComboOpen = false;
        }
        /// <summary>Called when the control's hotkey is pressed</summary>
        public override void OnHotKey()
        {
            if (isComboOpen)
                return; // Nothing to do yet

            if (selectedIndex == -1)
                return; // Nothing selected

            selectedIndex++;
            if (selectedIndex >= itemList.Count)
                selectedIndex = 0;

            focusedIndex = selectedIndex;
            RaiseChangedEvent(this, true);
        }


        /// <summary>Called when the control needs to handle the keyboard</summary>
        public override bool HandleKeyboard(NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            const uint RepeatMask = (0x40000000);

            if (!IsEnabled || !IsVisible)
                return false;

            // Let the scroll bar have a chance to handle it first
            if (scrollbarControl.HandleKeyboard(msg, wParam, lParam))
                return true;

            switch (msg)
            {
                case NativeMethods.WindowMessage.KeyDown:
                    {
                        switch ((System.Windows.Forms.Keys)wParam.ToInt32())
                        {
                            case System.Windows.Forms.Keys.Return:
                                {
                                    if (isComboOpen)
                                    {
                                        if (selectedIndex != focusedIndex)
                                        {
                                            selectedIndex = focusedIndex;
                                            RaiseChangedEvent(this, true);
                                        }
                                        isComboOpen = false;

                                        if (!Parent.IsUsingKeyboardInput)
                                            Dialog.ClearFocus();

                                        return true;
                                    }
                                    break;
                                }
                            case System.Windows.Forms.Keys.F4:
                                {
                                    // Filter out auto repeats
                                    if ((lParam.ToInt32() & RepeatMask) != 0)
                                        return true;

                                    isComboOpen = !isComboOpen;
                                    if (!isComboOpen)
                                    {
                                        RaiseChangedEvent(this, true);

                                        if (!Parent.IsUsingKeyboardInput)
                                            Dialog.ClearFocus();
                                    }

                                    return true;
                                }
                            case System.Windows.Forms.Keys.Left:
                            case System.Windows.Forms.Keys.Up:
                                {
                                    if (focusedIndex > 0)
                                    {
                                        focusedIndex--;
                                        selectedIndex = focusedIndex;
                                        if (!isComboOpen)
                                            RaiseChangedEvent(this, true);
                                    }
                                    return true;
                                }
                            case System.Windows.Forms.Keys.Right:
                            case System.Windows.Forms.Keys.Down:
                                {
                                    if (focusedIndex + 1 < (int)NumberItems)
                                    {
                                        focusedIndex++;
                                        selectedIndex = focusedIndex;
                                        if (!isComboOpen)
                                            RaiseChangedEvent(this, true);
                                    }
                                    return true;
                                }
                        }
                        break;
                    }
            }

            return false;
        }

        /// <summary>Called when the control should handle the mouse</summary>
        public override bool HandleMouse(NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            if (!IsEnabled || !IsVisible)
                return false; // Nothing to do

            // Let the scroll bar handle it first
            if (scrollbarControl.HandleMouse(msg, pt, wParam, lParam))
                return true;

            // Ok, scrollbar didn't handle it, move on
            switch (msg)
            {
                case NativeMethods.WindowMessage.MouseMove:
                    {
                        if (isComboOpen && dropDownRect.Contains(pt))
                        {
                            // Determine which item has been selected
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                ComboBoxItem cbi = (ComboBoxItem)itemList[i];
                                if (cbi.IsItemVisible && cbi.ItemRect.Contains(pt))
                                {
                                    focusedIndex = i;
                                }
                            }
                            return true;
                        }
                        break;
                    }
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

                            // Toggle dropdown
                            if (hasFocus)
                            {
                                isComboOpen = !isComboOpen;
                                if (!isComboOpen)
                                {
                                    if (!parentDialog.IsUsingKeyboardInput)
                                        Dialog.ClearFocus();
                                }
                            }

                            return true;
                        }

                        // Perhaps this click is within the dropdown
                        if (isComboOpen && dropDownRect.Contains(pt))
                        {
                            // Determine which item has been selected
                            for (int i = scrollbarControl.TrackPosition; i < itemList.Count; i++)
                            {
                                ComboBoxItem cbi = (ComboBoxItem)itemList[i];
                                if (cbi.IsItemVisible && cbi.ItemRect.Contains(pt))
                                {
                                    selectedIndex = focusedIndex = i;
                                    RaiseChangedEvent(this, true);

                                    isComboOpen = false;

                                    if (!parentDialog.IsUsingKeyboardInput)
                                        Dialog.ClearFocus();

                                    break;
                                }
                            }
                            return true;
                        }
                        // Mouse click not on main control or in dropdown, fire an event if needed
                        if (isComboOpen)
                        {
                            focusedIndex = selectedIndex;
                            RaiseChangedEvent(this, true);
                            isComboOpen = false;
                        }

                        // Make sure the control is no longer 'pressed'
                        isPressed = false;

                        // Release focus if appropriate
                        if (!parentDialog.IsUsingKeyboardInput)
                            Dialog.ClearFocus();

                        break;
                    }
                case NativeMethods.WindowMessage.LeftButtonUp:
                    {
                        if (isPressed && ContainsPoint(pt))
                        {
                            // Button click
                            isPressed = false;
                            Parent.SampleFramework.Window.Capture = false;
                            return true;
                        }
                        break;
                    }
                case NativeMethods.WindowMessage.MouseWheel:
                    {
                        int zdelta = (short)NativeMethods.HiWord((uint)wParam.ToInt32()) / Dialog.WheelDelta;
                        if (isComboOpen)
                        {
                            scrollbarControl.Scroll(-zdelta * System.Windows.Forms.SystemInformation.MouseWheelScrollLines);
                        }
                        else
                        {
                            if (zdelta > 0)
                            {
                                if (focusedIndex > 0)
                                {
                                    focusedIndex--;
                                    selectedIndex = focusedIndex;
                                    if (!isComboOpen)
                                    {
                                        RaiseChangedEvent(this, true);
                                    }
                                }
                            }
                            else
                            {
                                if (focusedIndex + 1 < NumberItems)
                                {
                                    focusedIndex++;
                                    selectedIndex = focusedIndex;
                                    if (!isComboOpen)
                                    {
                                        RaiseChangedEvent(this, true);
                                    }
                                }
                            }
                        }
                        return true;
                    }
            }

            // Didn't handle it
            return false;
        }

        /// <summary>Called when the control should be rendered</summary>
        public override void Render(Device device, float elapsedTime)
        {
            ControlState state = ControlState.Normal;
            if (!isComboOpen)
                state = ControlState.Hidden;

            // Dropdown box
            Element e = elementList[ComboBox.DropdownLayer] as Element;

            // If we have not initialized the scroll bar page size,
            // do that now.
            if (!isScrollBarInit)
            {
                FontNode fNode = DialogResourceManager.GetGlobalInstance().GetFontNode((int)e.FontIndex);
                if ((fNode != null) && (fNode.Height > 0))
                    scrollbarControl.PageSize = (int)(dropDownTextRect.Height / fNode.Height);
                else
                    scrollbarControl.PageSize = dropDownTextRect.Height;

                isScrollBarInit = true;
            }

            if (isComboOpen)
                scrollbarControl.Render(device, elapsedTime);

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime);
            e.FontColor.Blend(state, elapsedTime);
            parentDialog.DrawSprite(e, dropDownRect);

            // Selection outline
            Element selectionElement = elementList[ComboBox.SelectionLayer] as Element;
            selectionElement.TextureColor.Current = e.TextureColor.Current;
            selectionElement.FontColor.Current = selectionElement.FontColor.States[(int)ControlState.Normal];

            FontNode font = DialogResourceManager.GetGlobalInstance().GetFontNode((int)e.FontIndex);
            int currentY = dropDownTextRect.Top;
            int remainingHeight = dropDownTextRect.Height;

            for (int i = scrollbarControl.TrackPosition; i < itemList.Count; i++)
            {
                ComboBoxItem cbi = (ComboBoxItem)itemList[i];

                // Make sure there's room left in the dropdown
                remainingHeight -= (int)font.Height;
                if (remainingHeight < 0)
                {
                    // Not visible, store that item
                    cbi.IsItemVisible = false;
                    itemList[i] = cbi; // Store this back in list
                    continue;
                }

                cbi.ItemRect = new System.Drawing.Rectangle(dropDownTextRect.Left, currentY,
                    dropDownTextRect.Width, (int)font.Height);
                cbi.IsItemVisible = true;
                currentY += (int)font.Height;
                itemList[i] = cbi; // Store this back in list

                if (isComboOpen)
                {
                    if (focusedIndex == i)
                    {
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                            dropDownRect.Left, cbi.ItemRect.Top - 2, dropDownRect.Width,
                            cbi.ItemRect.Height + 4);
                        parentDialog.DrawSprite(selectionElement, rect);
                        parentDialog.DrawText(cbi.ItemText, selectionElement, cbi.ItemRect);
                    }
                    else
                    {
                        parentDialog.DrawText(cbi.ItemText, e, cbi.ItemRect);
                    }
                }
            }

            int offsetX = 0;
            int offsetY = 0;

            state = ControlState.Normal;
            if (IsVisible == false)
                state = ControlState.Hidden;
            else if (IsEnabled == false)
                state = ControlState.Disabled;
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
                state = ControlState.Focus;

            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.8f;

            // Button
            e = elementList[ComboBox.ComboButtonLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);

            System.Drawing.Rectangle windowRect = buttonRect;
            windowRect.Offset(offsetX, offsetY);
            // Draw sprite
            parentDialog.DrawSprite(e, windowRect);

            if (isComboOpen)
                state = ControlState.Pressed;

            // Main text box
            e = elementList[ComboBox.MainLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(state, elapsedTime, blendRate);
            e.FontColor.Blend(state, elapsedTime, blendRate);

            // Draw sprite
            parentDialog.DrawSprite(e, textRect);

            if (selectedIndex >= 0 && selectedIndex < itemList.Count)
            {
                try
                {
                    ComboBoxItem cbi = (ComboBoxItem)itemList[selectedIndex];
                    parentDialog.DrawText(cbi.ItemText, e, textRect);
                }
                catch { } // Ignore
            }

        }

        #region Item Controlling methods
        /// <summary>Adds an item to the combo box control</summary>
        public void AddItem(string text, object data)
        {
            if ((text == null) || (text.Length == 0))
                throw new ArgumentNullException("text", "You must pass in a valid item name when adding a new item.");

            // Create a new item and add it
            ComboBoxItem newitem = new ComboBoxItem();
            newitem.ItemText = text;
            newitem.ItemData = data;
            itemList.Add(newitem);

            // Update the scrollbar with the new range
            scrollbarControl.SetTrackRange(0, itemList.Count);

            // If this is the only item in the list, it should be selected
            if (NumberItems == 1)
            {
                selectedIndex = 0;
                focusedIndex = 0;
                RaiseChangedEvent(this, false);
            }
        }

        /// <summary>Removes an item at a particular index</summary>
        public void RemoveAt(int index)
        {
            // Remove the item
            itemList.RemoveAt(index);

            // Update the scrollbar with the new range
            scrollbarControl.SetTrackRange(0, itemList.Count);

            if (selectedIndex >= itemList.Count)
                selectedIndex = itemList.Count - 1;
        }

        /// <summary>Removes all items from the control</summary>
        public void Clear()
        {
            // clear the list
            itemList.Clear();

            // Update scroll bar and index
            scrollbarControl.SetTrackRange(0, 1);
            focusedIndex = selectedIndex = -1;
        }

        /// <summary>Determines whether this control contains an item</summary>
        public bool ContainsItem(string text, int start)
        {
            return (FindItem(text, start) != -1);
        }
        /// <summary>Determines whether this control contains an item</summary>
        public bool ContainsItem(string text) { return ContainsItem(text, 0); }

        /// <summary>Gets the data for the selected item</summary>
        public object GetSelectedData()
        {
            if ((selectedIndex < 0) || (selectedIndex >= itemList.Count))
                return null; // Nothing selected

            ComboBoxItem cbi = (ComboBoxItem)itemList[selectedIndex];
            return cbi.ItemData;
        }

        /// <summary>Gets the selected item</summary>
        public ComboBoxItem GetSelectedItem()
        {
            if (selectedIndex < 0)
                throw new ArgumentOutOfRangeException("selectedIndex", "No item selected.");

            return (ComboBoxItem)itemList[selectedIndex];
        }

        /// <summary>Gets the data for an item</summary>
        public object GetItemData(string text)
        {
            int index = FindItem(text);
            if (index == -1)
                return null; // no item

            ComboBoxItem cbi = (ComboBoxItem)itemList[index];
            return cbi.ItemData;
        }

        /// <summary>Finds an item in the list and returns the index</summary>
        protected int FindItem(string text, int start)
        {
            if ((text == null) || (text.Length == 0))
                return -1;

            for (int i = start; i < itemList.Count; i++)
            {
                ComboBoxItem cbi = (ComboBoxItem)itemList[i];
                if (string.Compare(cbi.ItemText, text, true) == 0)
                {
                    return i;
                }
            }

            // Never found it
            return -1;
        }
        /// <summary>Finds an item in the list and returns the index</summary>
        protected int FindItem(string text) { return FindItem(text, 0); }

        /// <summary>Sets the selected item by index</summary>
        public void SetSelected(int index)
        {
            if (index >= NumberItems)
                throw new ArgumentOutOfRangeException("index", "There are not enough items in the list to select this index.");

            focusedIndex = selectedIndex = index;
            RaiseChangedEvent(this, false);
        }

        /// <summary>Sets the selected item by text</summary>
        public void SetSelected(string text)
        {
            if ((text == null) || (text.Length == 0))
                throw new ArgumentNullException("text", "You must pass in a valid item name when adding a new item.");

            int index = FindItem(text);
            if (index == -1)
                throw new InvalidOperationException("This item could not be found.");

            focusedIndex = selectedIndex = index;
            RaiseChangedEvent(this, false);
        }

        /// <summary>Sets the selected item by data</summary>
        public void SetSelectedByData(object data)
        {
            for (int index = 0; index < itemList.Count; index++)
            {
                ComboBoxItem cbi = (ComboBoxItem)itemList[index];
                if ((cbi.ItemData != null) && (cbi.ItemData.ToString().Equals(data.ToString())))
                {
                    focusedIndex = selectedIndex = index;
                    RaiseChangedEvent(this, false);
                }
            }

            // Could not find this item.  Uncomment line below for debug information
            //System.Diagnostics.Debugger.Log(9,string.Empty, "Could not find an object with this data.\r\n");
        }

        #endregion
    }
}
