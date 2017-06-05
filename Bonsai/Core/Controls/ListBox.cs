using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{

    /// <summary>Style of the list box</summary>
    public enum ListBoxStyle
    {
        SingleSelection,
        Multiselection,
    }

    /// <summary>Stores data for a list box item</summary>
    public struct ListBoxItem
    {
        public string ItemText;
        public object ItemData;
        public System.Drawing.Rectangle ItemRect;
        public bool IsItemSelected;
    }
    /// <summary>List box control</summary>
    public class ListBox : Control
    {
        public const int MainLayer = 0;
        public const int SelectionLayer = 1;

        #region Event code
        public event EventHandler ContentsChanged;
        public event EventHandler DoubleClick;
        public event EventHandler Selection;
        /// <summary>Raises the contents changed event</summary>
        protected void RaiseContentsChangedEvent(ListBox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            // Fire the event
            if (ContentsChanged != null)
                ContentsChanged(sender, System.EventArgs.Empty);
        }
        /// <summary>Raises the double click event</summary>
        protected void RaiseDoubleClickEvent(ListBox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            // Fire the event
            if (DoubleClick != null)
                DoubleClick(sender, System.EventArgs.Empty);
        }
        /// <summary>Raises the selection event</summary>
        protected void RaiseSelectionEvent(ListBox sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            // Fire the event
            if (Selection != null)
                Selection(sender, System.EventArgs.Empty);
        }
        #endregion

        #region Instance data
        private bool isScrollBarInit;
        protected System.Drawing.Rectangle textRect; // Text rendering bound
        protected System.Drawing.Rectangle selectionRect; // Selection box bound
        protected ScrollBar scrollbarControl;
        protected int scrollWidth;
        protected int border;
        protected int margin;
        protected int textHeight; // Height of a single line of text
        protected int selectedIndex;
        protected int selectedStarted;
        protected bool isDragging;
        protected ListBoxStyle style;

        protected ArrayList itemList;
        #endregion

        /// <summary>Create a new list box control</summary>
        public ListBox(Dialog parent)
            : base(parent)
        {
            // Store control type and parent dialog
            controlType = ControlType.ListBox;
            parentDialog = parent;
            // Create the scrollbar control too
            scrollbarControl = new ScrollBar(parent);

            // Set some default items
            style = ListBoxStyle.SingleSelection;
            scrollWidth = 16;
            selectedIndex = -1;
            selectedStarted = 0;
            isDragging = false;
            margin = 5;
            border = 6;
            textHeight = 0;
            isScrollBarInit = false;

            // Create the item list array
            itemList = new ArrayList();
        }

        /// <summary>Update the rectangles for the list box control</summary>
        protected override void UpdateRectangles()
        {
            // Get bounding box
            base.UpdateRectangles();

            // Calculate the size of the selection rectangle
            selectionRect = boundingBox;
            selectionRect.Width -= scrollWidth;
            selectionRect.Inflate(-border, -border);
            textRect = selectionRect;
            textRect.Inflate(-margin, 0);

            // Update the scroll bars rects too
            scrollbarControl.SetLocation(boundingBox.Right - scrollWidth, boundingBox.Top);
            scrollbarControl.SetSize(scrollWidth, height);
            FontNode fNode = DialogResourceManager.GetGlobalInstance().GetFontNode((int)(elementList[0] as Element).FontIndex);
            if ((fNode != null) && (fNode.Height > 0))
            {
                scrollbarControl.PageSize = (int)(textRect.Height / fNode.Height);

                // The selected item may have been scrolled off the page.
                // Ensure that it is in page again.
                scrollbarControl.ShowItem(selectedIndex);
            }
        }
        /// <summary>Sets the scroll bar width of this control</summary>
        public void SetScrollbarWidth(int width) { scrollWidth = width; UpdateRectangles(); }
        /// <summary>Can this control have focus</summary>
        public override bool CanHaveFocus { get { return (IsVisible && IsEnabled); } }
        /// <summary>Sets the style of the listbox</summary>
        public ListBoxStyle Style { get { return style; } set { style = value; } }
        /// <summary>Number of items current in the list</summary>
        public int NumberItems { get { return itemList.Count; } }
        /// <summary>Indexer for items in the list</summary>
        public ListBoxItem this[int index]
        {
            get { return (ListBoxItem)itemList[index]; }
        }

        /// <summary>Initialize the scrollbar control here</summary>
        public override void OnInitialize()
        {
            parentDialog.InitializeControl(scrollbarControl);
        }


        /// <summary>Called when the control needs to handle the keyboard</summary>
        public override bool HandleKeyboard(NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
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
                            case System.Windows.Forms.Keys.Up:
                            case System.Windows.Forms.Keys.Down:
                            case System.Windows.Forms.Keys.Next:
                            case System.Windows.Forms.Keys.Prior:
                            case System.Windows.Forms.Keys.Home:
                            case System.Windows.Forms.Keys.End:
                                {
                                    // If no items exists, do nothing
                                    if (itemList.Count == 0)
                                        return true;

                                    int oldSelected = selectedIndex;

                                    // Adjust selectedIndex
                                    switch ((System.Windows.Forms.Keys)wParam.ToInt32())
                                    {
                                        case System.Windows.Forms.Keys.Up: --selectedIndex; break;
                                        case System.Windows.Forms.Keys.Down: ++selectedIndex; break;
                                        case System.Windows.Forms.Keys.Next: selectedIndex += scrollbarControl.PageSize - 1; break;
                                        case System.Windows.Forms.Keys.Prior: selectedIndex -= scrollbarControl.PageSize - 1; break;
                                        case System.Windows.Forms.Keys.Home: selectedIndex = 0; break;
                                        case System.Windows.Forms.Keys.End: selectedIndex = itemList.Count - 1; break;
                                    }

                                    // Clamp the item
                                    if (selectedIndex < 0)
                                        selectedIndex = 0;
                                    if (selectedIndex >= itemList.Count)
                                        selectedIndex = itemList.Count - 1;

                                    // Did the selection change?
                                    if (oldSelected != selectedIndex)
                                    {
                                        if (style == ListBoxStyle.Multiselection)
                                        {
                                            // Clear all selection
                                            for (int i = 0; i < itemList.Count; i++)
                                            {
                                                ListBoxItem lbi = (ListBoxItem)itemList[i];
                                                lbi.IsItemSelected = false;
                                                itemList[i] = lbi;
                                            }

                                            // Is shift being held down?
                                            bool shiftDown = ((NativeMethods.GetAsyncKeyState
                                                ((int)System.Windows.Forms.Keys.ShiftKey) & 0x8000) != 0);

                                            if (shiftDown)
                                            {
                                                // Select all items from the start selection to current selected index
                                                int end = Math.Max(selectedStarted, selectedIndex);
                                                for (int i = Math.Min(selectedStarted, selectedIndex); i <= end; ++i)
                                                {
                                                    ListBoxItem lbi = (ListBoxItem)itemList[i];
                                                    lbi.IsItemSelected = true;
                                                    itemList[i] = lbi;
                                                }
                                            }
                                            else
                                            {
                                                ListBoxItem lbi = (ListBoxItem)itemList[selectedIndex];
                                                lbi.IsItemSelected = true;
                                                itemList[selectedIndex] = lbi;

                                                // Update selection start
                                                selectedStarted = selectedIndex;
                                            }

                                        }
                                        else // Update selection start
                                            selectedStarted = selectedIndex;

                                        // adjust scrollbar
                                        scrollbarControl.ShowItem(selectedIndex);
                                        RaiseSelectionEvent(this, true);
                                    }
                                }
                                return true;
                        }
                        break;
                    }
            }

            return false;
        }

        /// <summary>Called when the control should handle the mouse</summary>
        public override bool HandleMouse(NativeMethods.WindowMessage msg, System.Drawing.Point pt, IntPtr wParam, IntPtr lParam)
        {
            const int ShiftModifier = 0x0004;
            const int ControlModifier = 0x0008;

            if (!IsEnabled || !IsVisible)
                return false; // Nothing to do

            // First acquire focus
            if (msg == NativeMethods.WindowMessage.LeftButtonDown)
                if (!hasFocus)
                    Dialog.RequestFocus(this);


            // Let the scroll bar handle it first
            if (scrollbarControl.HandleMouse(msg, pt, wParam, lParam))
                return true;

            // Ok, scrollbar didn't handle it, move on
            switch (msg)
            {
                case NativeMethods.WindowMessage.LeftButtonDoubleClick:
                case NativeMethods.WindowMessage.LeftButtonDown:
                    {
                        // Check for clicks in the text area
                        if (itemList.Count > 0 && selectionRect.Contains(pt))
                        {
                            // Compute the index of the clicked item
                            int clicked = 0;
                            if (textHeight > 0)
                                clicked = scrollbarControl.TrackPosition + (pt.Y - textRect.Top) / textHeight;
                            else
                                clicked = -1;

                            // Only proceed if the click falls ontop of an item
                            if (clicked >= scrollbarControl.TrackPosition &&
                                clicked < itemList.Count &&
                                clicked < scrollbarControl.TrackPosition + scrollbarControl.PageSize)
                            {
                                Parent.SampleFramework.Window.Capture = true;
                                isDragging = true;

                                // If this is a double click, fire off an event and exit
                                // since the first click would have taken care of the selection
                                // updating.
                                if (msg == NativeMethods.WindowMessage.LeftButtonDoubleClick)
                                {
                                    RaiseDoubleClickEvent(this, true);
                                    return true;
                                }

                                selectedIndex = clicked;
                                if ((wParam.ToInt32() & ShiftModifier) == 0)
                                    selectedStarted = selectedIndex; // Shift isn't down

                                // If this is a multi-selection listbox, update per-item
                                // selection data.
                                if (style == ListBoxStyle.Multiselection)
                                {
                                    // Determine behavior based on the state of Shift and Ctrl
                                    ListBoxItem selectedItem = (ListBoxItem)itemList[selectedIndex];
                                    if ((wParam.ToInt32() & (ShiftModifier | ControlModifier)) == ControlModifier)
                                    {
                                        // Control click, reverse the selection
                                        selectedItem.IsItemSelected = !selectedItem.IsItemSelected;
                                        itemList[selectedIndex] = selectedItem;
                                    }
                                    else if ((wParam.ToInt32() & (ShiftModifier | ControlModifier)) == ShiftModifier)
                                    {
                                        // Shift click. Set the selection for all items
                                        // from last selected item to the current item.
                                        // Clear everything else.
                                        int begin = Math.Min(selectedStarted, selectedIndex);
                                        int end = Math.Max(selectedStarted, selectedIndex);

                                        // Unselect everthing before the beginning
                                        for (int i = 0; i < begin; ++i)
                                        {
                                            ListBoxItem lb = (ListBoxItem)itemList[i];
                                            lb.IsItemSelected = false;
                                            itemList[i] = lb;
                                        }
                                        // unselect everything after the end
                                        for (int i = end + 1; i < itemList.Count; ++i)
                                        {
                                            ListBoxItem lb = (ListBoxItem)itemList[i];
                                            lb.IsItemSelected = false;
                                            itemList[i] = lb;
                                        }

                                        // Select everything between
                                        for (int i = begin; i <= end; ++i)
                                        {
                                            ListBoxItem lb = (ListBoxItem)itemList[i];
                                            lb.IsItemSelected = true;
                                            itemList[i] = lb;
                                        }
                                    }
                                    else if ((wParam.ToInt32() & (ShiftModifier | ControlModifier)) == (ShiftModifier | ControlModifier))
                                    {
                                        // Control-Shift-click.

                                        // The behavior is:
                                        //   Set all items from selectedStarted to selectedIndex to
                                        //     the same state as selectedStarted, not including selectedIndex.
                                        //   Set selectedIndex to selected.
                                        int begin = Math.Min(selectedStarted, selectedIndex);
                                        int end = Math.Max(selectedStarted, selectedIndex);

                                        // The two ends do not need to be set here.
                                        bool isLastSelected = ((ListBoxItem)itemList[selectedStarted]).IsItemSelected;

                                        for (int i = begin + 1; i < end; ++i)
                                        {
                                            ListBoxItem lb = (ListBoxItem)itemList[i];
                                            lb.IsItemSelected = isLastSelected;
                                            itemList[i] = lb;
                                        }

                                        selectedItem.IsItemSelected = true;
                                        itemList[selectedIndex] = selectedItem;

                                        // Restore selectedIndex to the previous value
                                        // This matches the Windows behavior

                                        selectedIndex = selectedStarted;
                                    }
                                    else
                                    {
                                        // Simple click.  Clear all items and select the clicked
                                        // item.
                                        for (int i = 0; i < itemList.Count; ++i)
                                        {
                                            ListBoxItem lb = (ListBoxItem)itemList[i];
                                            lb.IsItemSelected = false;
                                            itemList[i] = lb;
                                        }
                                        selectedItem.IsItemSelected = true;
                                        itemList[selectedIndex] = selectedItem;
                                    }
                                } // End of multi-selection case
                                RaiseSelectionEvent(this, true);
                            }
                            return true;
                        }
                        break;
                    }
                case NativeMethods.WindowMessage.LeftButtonUp:
                    {
                        Parent.SampleFramework.Window.Capture = false;
                        isDragging = false;

                        if (selectedIndex != -1)
                        {
                            // Set all items between selectedStarted and selectedIndex to
                            // the same state as selectedStarted
                            int end = Math.Max(selectedStarted, selectedIndex);
                            for (int i = Math.Min(selectedStarted, selectedIndex) + 1; i < end; ++i)
                            {
                                ListBoxItem lb = (ListBoxItem)itemList[i];
                                lb.IsItemSelected = ((ListBoxItem)itemList[selectedStarted]).IsItemSelected;
                                itemList[i] = lb;
                            }
                            ListBoxItem lbs = (ListBoxItem)itemList[selectedIndex];
                            lbs.IsItemSelected = ((ListBoxItem)itemList[selectedStarted]).IsItemSelected;
                            itemList[selectedIndex] = lbs;

                            // If selectedStarted and selectedIndex are not the same,
                            // the user has dragged the mouse to make a selection.
                            // Notify the application of this.
                            if (selectedIndex != selectedStarted)
                                RaiseSelectionEvent(this, true);
                        }
                        break;
                    }
                case NativeMethods.WindowMessage.MouseWheel:
                    {
                        int lines = System.Windows.Forms.SystemInformation.MouseWheelScrollLines;
                        int scrollAmount = (int)(NativeMethods.HiWord((uint)wParam.ToInt32()) / Dialog.WheelDelta * lines);
                        scrollbarControl.Scroll(-scrollAmount);
                        break;
                    }

                case NativeMethods.WindowMessage.MouseMove:
                    {
                        if (isDragging)
                        {
                            // compute the index of the item below the cursor
                            int itemIndex = -1;
                            if (textHeight > 0)
                                itemIndex = scrollbarControl.TrackPosition + (pt.Y - textRect.Top) / textHeight;

                            // Only proceed if the cursor is on top of an item
                            if (itemIndex >= scrollbarControl.TrackPosition &&
                                itemIndex < itemList.Count &&
                                itemIndex < scrollbarControl.TrackPosition + scrollbarControl.PageSize)
                            {
                                selectedIndex = itemIndex;
                                RaiseSelectionEvent(this, true);
                            }
                            else if (itemIndex < scrollbarControl.TrackPosition)
                            {
                                // User drags the mouse above window top
                                scrollbarControl.Scroll(-1);
                                selectedIndex = scrollbarControl.TrackPosition;
                                RaiseSelectionEvent(this, true);
                            }
                            else if (itemIndex >= scrollbarControl.TrackPosition + scrollbarControl.PageSize)
                            {
                                // User drags the mouse below the window bottom
                                scrollbarControl.Scroll(1);
                                selectedIndex = Math.Min(itemList.Count, scrollbarControl.TrackPosition + scrollbarControl.PageSize - 1);
                                RaiseSelectionEvent(this, true);
                            }
                        }
                        break;
                    }
            }

            // Didn't handle it
            return false;
        }

        /// <summary>Called when the control should be rendered</summary>
        public override void Render(Device device, float elapsedTime)
        {
            if (!IsVisible)
                return; // Nothing to render

            Element e = elementList[ListBox.MainLayer] as Element;

            // Blend current color
            e.TextureColor.Blend(ControlState.Normal, elapsedTime);
            e.FontColor.Blend(ControlState.Normal, elapsedTime);

            Element selectedElement = elementList[ListBox.SelectionLayer] as Element;

            // Blend current color
            selectedElement.TextureColor.Blend(ControlState.Normal, elapsedTime);
            selectedElement.FontColor.Blend(ControlState.Normal, elapsedTime);

            parentDialog.DrawSprite(e, boundingBox);

            // Render the text
            if (itemList.Count > 0)
            {
                // Find out the height of a single line of text
                System.Drawing.Rectangle rc = textRect;
                System.Drawing.Rectangle sel = selectionRect;
                rc.Height = (int)(DialogResourceManager.GetGlobalInstance().GetFontNode((int)e.FontIndex).Height);
                textHeight = rc.Height;

                // If we have not initialized the scroll bar page size,
                // do that now.
                if (!isScrollBarInit)
                {
                    if (textHeight > 0)
                        scrollbarControl.PageSize = (int)(textRect.Height / textHeight);
                    else
                        scrollbarControl.PageSize = textRect.Height;

                    isScrollBarInit = true;
                }
                rc.Width = textRect.Width;
                for (int i = scrollbarControl.TrackPosition; i < itemList.Count; ++i)
                {
                    if (rc.Bottom > textRect.Bottom)
                        break;

                    ListBoxItem lb = (ListBoxItem)itemList[i];

                    // Determine if we need to render this item with the
                    // selected element.
                    bool isSelectedStyle = false;

                    if ((selectedIndex == i) && (style == ListBoxStyle.SingleSelection))
                        isSelectedStyle = true;
                    else if (style == ListBoxStyle.Multiselection)
                    {
                        if (isDragging && ((i >= selectedIndex && i < selectedStarted) ||
                            (i <= selectedIndex && i > selectedStarted)))
                        {
                            ListBoxItem selStart = (ListBoxItem)itemList[selectedStarted];
                            isSelectedStyle = selStart.IsItemSelected;
                        }
                        else
                            isSelectedStyle = lb.IsItemSelected;
                    }

                    // Now render the text
                    if (isSelectedStyle)
                    {
                        sel.Location = new System.Drawing.Point(sel.Left, rc.Top);
                        sel.Height = rc.Height;
                        parentDialog.DrawSprite(selectedElement, sel);
                        parentDialog.DrawText(lb.ItemText, selectedElement, rc);
                    }
                    else
                        parentDialog.DrawText(lb.ItemText, e, rc);

                    rc.Offset(0, textHeight);
                }
            }

            // Render the scrollbar finally
            scrollbarControl.Render(device, elapsedTime);
        }


        #region Item Controlling methods
        /// <summary>Adds an item to the list box control</summary>
        public void AddItem(string text, object data)
        {
            if ((text == null) || (text.Length == 0))
                throw new ArgumentNullException("text", "You must pass in a valid item name when adding a new item.");

            // Create a new item and add it
            ListBoxItem newitem = new ListBoxItem();
            newitem.ItemText = text;
            newitem.ItemData = data;
            newitem.IsItemSelected = false;
            itemList.Add(newitem);

            // Update the scrollbar with the new range
            scrollbarControl.SetTrackRange(0, itemList.Count);
        }
        /// <summary>Inserts an item to the list box control</summary>
        public void InsertItem(int index, string text, object data)
        {
            if ((text == null) || (text.Length == 0))
                throw new ArgumentNullException("text", "You must pass in a valid item name when adding a new item.");

            // Create a new item and insert it
            ListBoxItem newitem = new ListBoxItem();
            newitem.ItemText = text;
            newitem.ItemData = data;
            newitem.IsItemSelected = false;
            itemList.Insert(index, newitem);

            // Update the scrollbar with the new range
            scrollbarControl.SetTrackRange(0, itemList.Count);
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

            RaiseSelectionEvent(this, true);
        }

        /// <summary>Removes all items from the control</summary>
        public void Clear()
        {
            // clear the list
            itemList.Clear();

            // Update scroll bar and index
            scrollbarControl.SetTrackRange(0, 1);
            selectedIndex = -1;
        }

        /// <summary>
        /// For single-selection listbox, returns the index of the selected item.
        /// For multi-selection, returns the first selected item after the previousSelected position.
        /// To search for the first selected item, the app passes -1 for previousSelected.  For
        /// subsequent searches, the app passes the returned index back to GetSelectedIndex as.
        /// previousSelected.
        /// Returns -1 on error or if no item is selected.
        /// </summary>
        public int GetSelectedIndex(int previousSelected)
        {
            if (previousSelected < -1)
                return -1;

            if (style == ListBoxStyle.Multiselection)
            {
                // Multiple selections enabled.  Search for the next item with the selected flag
                for (int i = previousSelected + 1; i < itemList.Count; ++i)
                {
                    ListBoxItem lbi = (ListBoxItem)itemList[i];
                    if (lbi.IsItemSelected)
                        return i;
                }

                return -1;
            }
            else
            {
                // Single selection
                return selectedIndex;
            }
        }
        /// <summary>Gets the selected item</summary>
        public ListBoxItem GetSelectedItem(int previousSelected)
        {
            return (ListBoxItem)itemList[GetSelectedIndex(previousSelected)];
        }
        /// <summary>Gets the selected item</summary>
        public ListBoxItem GetSelectedItem() { return GetSelectedItem(-1); }

        /// <summary>Sets the border and margin sizes</summary>
        public void SetBorder(int borderSize, int marginSize)
        {
            border = borderSize;
            margin = marginSize;
            UpdateRectangles();
        }

        /// <summary>Selects this item</summary>
        public void SelectItem(int newIndex)
        {
            if (itemList.Count == 0)
                return; // If no items exist there's nothing to do

            int oldSelected = selectedIndex;

            // Select the new item
            selectedIndex = newIndex;

            // Clamp the item
            if (selectedIndex < 0)
                selectedIndex = 0;
            if (selectedIndex > itemList.Count)
                selectedIndex = itemList.Count - 1;

            // Did the selection change?
            if (oldSelected != selectedIndex)
            {
                if (style == ListBoxStyle.Multiselection)
                {
                    ListBoxItem lbi = (ListBoxItem)itemList[selectedIndex];
                    lbi.IsItemSelected = true;
                    itemList[selectedIndex] = lbi;
                }

                // Update selection start
                selectedStarted = selectedIndex;

                // adjust scrollbar
                scrollbarControl.ShowItem(selectedIndex);
            }
            RaiseSelectionEvent(this, true);
        }
        #endregion
    }

}
