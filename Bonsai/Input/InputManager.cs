using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Bonsai.Utils;
using System.Collections.Generic;

namespace Bonsai.Input
{
    public class InputManager : IDisposable
    {
        #region Private fields
        private Device device = null;
        private Device joyStickDevice = null;
        private KeyboardState state = null;
        private JoystickState joystickState;
        private Control control = null;
        #endregion

        #region Public enumerations
        public enum JoyStickAxis
        {
            X,
            Y,
            Z,
            Rx,
            Ry,
            Rz,
            Slider1,
            Slider2,
            POV1,
            POV2,
            POV3,
            POV4,
            None
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the current state of the keyboard.
        /// </summary>
        public KeyboardState KeyBoardState
        {
            get { return state; }
        }

        /// <summary>
        /// Gets the current state of the joystick.
        /// </summary>
        public JoystickState JoyStickState
        {
            get { return joystickState; }
        }

        /// <summary>
        /// Gets whether a joystick is available.
        /// </summary>
        public bool IsJoyStickAvailable
        {
            get { return joyStickDevice != null; }
        }
        #endregion

        #region public constructor
        public InputManager(Control control)
        {
            Tracer.Trace("InputManager constructor", "Creating InputManager");
            this.control = control;
            AcquireDevice();
            AcquireJoyStickDevice(null);
            ReadConfigSettings();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Tracer.Trace("InputManager.Dispose", "Disposing InputManager...");
            FreeDevice();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Returns a list of connected joysticks
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableJocksticks()
        {
            List<string> result = new List<string>();
            // Enumerate Joysticks in the system.
            foreach (DeviceInstance instance in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
            {
                result.Add(instance.InstanceName);
            }
            return result;
        }

        /// <summary>
        /// Updates the input state. Call once every frame.
        /// </summary>
        public void Update()
        {
            ReadKeyBoardData();
            ReadJoyStickData();
        }

        /// <summary>
        /// Updates the input state, but only the joystick.
        /// </summary>
        public void UpdateJoystick()
        {
            ReadJoyStickData();
        }

        public void SetDefaultKey(string function, Key key)
        {
            if (Settings.KeyExists(function) == false)
                Settings.SetKey(function, key);
        }

        public void SetDefaultAxis(string function, JoyStickAxis axis, bool inverted)
        {
            if (Settings.AxisExists(function) == false)
                Settings.SetAxis(function, axis, inverted);
        }

        public void SetAxis(string function, JoyStickAxis axis, bool inverted)
        {
            Settings.SetAxis(function, axis, inverted);
        }

        public bool GetValue(string function)
        {
            if (state != null)
                return state[Settings.GetKey(function)];
            else
                return false;
        }

        public int GetAxisValue(string function)
        {
            if (joyStickDevice == null)
                return 0;

            bool inverted = false;
            switch (Settings.GetAxis(function, out inverted))
            {
                case JoyStickAxis.X:
                    return inverted ? -joystickState.X : joystickState.X;
                case JoyStickAxis.Y:
                    return inverted ? -joystickState.Y : joystickState.Y;
                case JoyStickAxis.Z:
                    return inverted ? -joystickState.Z : joystickState.Z;
                case JoyStickAxis.Rx:
                    return inverted ? -joystickState.Rx : joystickState.Rx;
                case JoyStickAxis.Ry:
                    return inverted ? -joystickState.Ry : joystickState.Ry;
                case JoyStickAxis.Rz:
                    return inverted ? -joystickState.Rz : joystickState.Rz;
                case JoyStickAxis.Slider1:
                    if (joystickState.GetSlider() != null && joystickState.GetSlider().GetLength(0) > 0)
                    {
                        return inverted ? -joystickState.GetSlider()[0] : joystickState.GetSlider()[0];
                    }
                    else
                        return 0;
                case JoyStickAxis.Slider2:
                    if (joystickState.GetSlider() != null && joystickState.GetSlider().GetLength(0) > 1)
                    {
                        return inverted ? -joystickState.GetSlider()[1] : joystickState.GetSlider()[1];
                    }
                    else
                        return 0;
                case JoyStickAxis.POV1:
                    return inverted ? -joystickState.GetPointOfView()[0] : joystickState.GetPointOfView()[0];
                case JoyStickAxis.POV2:
                    return inverted ? -joystickState.GetPointOfView()[1] : joystickState.GetPointOfView()[1];
                case JoyStickAxis.POV3:
                    return inverted ? -joystickState.GetPointOfView()[2] : joystickState.GetPointOfView()[2];
                case JoyStickAxis.POV4:
                    return inverted ? -joystickState.GetPointOfView()[3] : joystickState.GetPointOfView()[3];
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Get the axis configured for the given function.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="inverted"></param>
        /// <returns></returns>
        public JoyStickAxis GetAxis(string function, out bool inverted)
        {
            if (Settings.AxisExists(function) != false)
                return Settings.GetAxis(function, out inverted);
            else
            {
                inverted = false;
                return JoyStickAxis.X;
            }
        }

        /// <summary>
        /// Get the axis configured for the given function.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="inverted"></param>
        /// <returns></returns>
        public JoyStickAxis GetAxis(string function, out bool inverted, JoyStickAxis defaultAxis)
        {
            if (Settings.AxisExists(function) != false)
                return Settings.GetAxis(function, out inverted);
            else
            {
                inverted = false;
                return defaultAxis;
            }
        }


        public void AcquireJoystick(string deviceName)
        {
            AcquireJoyStickDevice(deviceName);
        }
        #endregion

        #region Private methods
        private void ReadConfigSettings()
        {

        }

        /// <summary>
        /// Free the DirectInput device.
        /// </summary>
        private void FreeDevice()
        {
            if (device != null)
            {
                device.Unacquire();
                device.Dispose();
                device = null;
            }
        }

        /// <summary>
        /// Acquires the DirectInput device.
        /// </summary>
        private void AcquireDevice()
        {
            if (device != null)
            {
                FreeDevice();
            }

            CooperativeLevelFlags coopFlags;
            // Set exclusive mode.
            coopFlags = CooperativeLevelFlags.NonExclusive;
            // Set foreground mode.
            coopFlags |= CooperativeLevelFlags.Foreground;
            // Create the device.
            try
            {
                // Obtain an instantiated system Keyboard device.
                device = new Device(SystemGuid.Keyboard);
            }
            catch (InputException ex)
            {
                Tracer.Error("InputManager.AcquireDevice", "Error creating device", ex);
                FreeDevice();
                return;
            }

            // Set the cooperative level to let DirectInput know how
            // this device should interact with the system and with other
            // DirectInput applications.
            try
            {
                device.SetCooperativeLevel(control, coopFlags);
            }
            catch (InputException ex)
            {
                Tracer.Error("InputManager.AcquireDevice", "Error setting cooperative level", ex);
                FreeDevice();
                return;
            }
            try
            {
                device.Acquire();
            }
            catch (InputException ex)
            {
                Tracer.Error("InputManager.AcquireDevice", "Error acquiring device", ex);
                FreeDevice();
                return;
            }
        }

        ///<summary>
        /// Read the input device's state when in immediate mode and display it.
        ///</summary>
        private void ReadKeyBoardData()
        {
            if (device == null)
                AcquireDevice();
            if (device == null)
                return;

            // Get the input's device state, and store it.
            InputException ie = null;
            try
            {
                state = device.GetCurrentKeyboardState();
            }
            catch (DirectXException)
            {
                // DirectInput may be telling us that the input stream has been
                // interrupted.  We aren't tracking any state between polls, so
                // we don't have any special reset that needs to be done.
                // We just re-acquire and try again.

                // If input is lost then acquire and keep trying.

                int loop = 10;
                do
                {
                    try
                    {
                        device.Acquire();
                        loop = 0;
                    }
                    catch (InputLostException)
                    {
                        loop--;
                    }
                    catch (InputException inputException)
                    {
                        ie = inputException;
                        loop = 0;
                    }
                } while (loop > 0);

                // Update the dialog text 
                //if (ie is OtherApplicationHasPriorityException || ie is NotAcquiredException)
                //       dataLabel.Text = "Unacquired";

                // Exception may be OtherApplicationHasPriorityException or other exceptions.
                // This may occur when the app is minimized or in the process of 
                // switching, so just try again later.
                return;
            }

            // Make a string of the index values of the keys that are down.
            for (Key k = Key.Escape; k <= Key.MediaSelect; k++)
            {
                //if (state[k])
                //    textNew += k.ToString() + " ";
            }
        }

        private void AcquireJoyStickDevice(string preferredDevice)
        {
            if (joyStickDevice != null)
            {
                joyStickDevice.Unacquire();
                joyStickDevice.Dispose();
                joyStickDevice = null;
            }

            // Enumerate Joysticks in the system.
            foreach (DeviceInstance instance in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
            {
                if ((instance.InstanceName == preferredDevice) || (preferredDevice == null))
                {
                    // Create the device.  Just pick the first one
                    joyStickDevice = new Device(instance.InstanceGuid);
                    break;
                }
            }

            if (null == joyStickDevice)
            {
                return;
            }

            // Set the data format to the c_dfDIJoystick pre-defined format.
            joyStickDevice.SetDataFormat(DeviceDataFormat.Joystick);
            // Set the cooperative level for the device.
            joyStickDevice.SetCooperativeLevel(control, CooperativeLevelFlags.Exclusive | CooperativeLevelFlags.Background);
            // Enumerate all the objects on the device.
            foreach (DeviceObjectInstance d in joyStickDevice.Objects)
            {
                // For axes that are returned, set the DIPROP_RANGE property for the
                // enumerated axis in order to scale min/max values.

                if ((0 != (d.ObjectId & (int)DeviceObjectTypeFlags.Axis)))
                {
                    // Set the range for the axis.
                    joyStickDevice.Properties.SetRange(ParameterHow.ById, d.ObjectId, new InputRange(-100, +100));
                }
            }
        }

        private void FreeJoyStickDevice()
        {
            if (null != joyStickDevice)
            {
                joyStickDevice.Unacquire();
                joyStickDevice.Dispose();
                joyStickDevice = null;
            }
        }

        private void ReadJoyStickData()
        {
            // Make sure there is a valid device.
            if (null == joyStickDevice)
                return;

            try
            {
                // Poll the device for info.
                joyStickDevice.Poll();
            }
            catch (InputException inputex)
            {
                if ((inputex is NotAcquiredException) || (inputex is InputLostException))
                {
                    // Check to see if either the app
                    // needs to acquire the device, or
                    // if the app lost the device to another
                    // process.
                    try
                    {
                        // Acquire the device.
                        joyStickDevice.Acquire();
                    }
                    catch (InputException)
                    {
                        // Failed to acquire the device.
                        // This could be because the app
                        // doesn't have focus.
                        return;
                    }
                }

            } //catch(InputException inputex)

            // Get the state of the device.
            try { joystickState = joyStickDevice.CurrentJoystickState; }
            // Catch any exceptions. None will be handled here, 
            // any device re-aquisition will be handled above.  
            catch (InputException)
            {
                return;
            }
        }
        #endregion
    }
}
