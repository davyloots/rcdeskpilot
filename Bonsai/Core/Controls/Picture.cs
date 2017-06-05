using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Objects.Textures;
using System.Drawing;

namespace Bonsai.Core.Controls
{
    /// <summary>Slider control</summary>
    public class Picture : Control, IDisposable
    {
        public const int PictureLayer = 0;
        #region Instance Data
        protected int buttonX;

        protected bool isPressed;
        protected System.Drawing.Rectangle buttonRect;
        protected TextureBase textureBase;
        protected BlendColor blendColor = new BlendColor();
        protected Rectangle sourceRectangle = Rectangle.Empty;
        protected string textureFile = string.Empty;

        /// <summary>Pictures can't have focus</summary>
        public override bool CanHaveFocus { get { return false; } }
        #endregion

        #region Event code
        public event EventHandler Click;
        /// <summary>Create new button instance</summary>
        protected void RaiseClickEvent(Picture sender, bool wasTriggeredByUser)
        {
            // Discard events triggered programatically if these types of events haven't been
            // enabled
            if (!Parent.IsUsingNonUserEvents && !wasTriggeredByUser)
                return;

            if (Click != null)
                Click(sender, System.EventArgs.Empty);
        }
        #endregion

        #region Public properties
        public Rectangle SourceRectangle
        {
            get
            {
                if (sourceRectangle == Rectangle.Empty)
                    return new Rectangle(0, 0, this.width, this.height);
                else
                    return sourceRectangle;
            }
            set
            {
                sourceRectangle = value;
            }
        }

        /// <summary>
        /// Gets/sets the file to use as picture.
        /// </summary>
        public string TextureFile
        {
            get { return textureFile; }
            set
            {
                if (textureBase != null)
                {
                    textureBase.Dispose();
                    textureBase = null;
                }
                textureBase = new TextureBase(value);
                textureFile = value;
            }
        }

        /// <summary>
        /// Gets/Sets the rotational angle in radians to rotate the image.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }
        #endregion

        /// <summary>Create new button instance</summary>
        public Picture(Dialog parent, string textureFile)
            : base(parent)
        {
            controlType = ControlType.Picture;
            parentDialog = parent;

            isPressed = false;
            textureBase = new TextureBase(textureFile);
            blendColor.Initialize(new ColorValue(1f, 1f, 1f, 1f), new ColorValue(1f, 1f, 1f, 1f), new ColorValue(0f, 0f, 0f, 0f));
        }

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (textureBase != null)
            {
                textureBase.Dispose();
                textureBase = null;
            }            
        }
        #endregion

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
        }

        #region Input
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
                             return true;
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
                             RaiseClickEvent(this, true);
                             return true;
                         }
                         break;
                     }
             }
             return false;
         }
         /*
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
         */
        #endregion

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

            float blendRate = (state == ControlState.Pressed) ? 0.0f : 0.9f;

            //Element e = elementList[Picture.PictureLayer] as Element;
       
            // Blend current color
            blendColor.Blend(state, elapsedTime, blendRate);

            parentDialog.DrawTexture(this.textureBase.Direct3DTexture, SourceRectangle, boundingBox, blendColor, Rotation); 
        }

        public void FadeIn()
        {
            blendColor.Current = new ColorValue(1, 1, 1, 0);
        }

        public void FadeOut()
        {

        }
    }
}
