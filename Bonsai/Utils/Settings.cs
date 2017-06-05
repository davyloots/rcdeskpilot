using System;
using System.Data;
using System.IO;
using Bonsai.Input;


namespace Bonsai.Utils
{
    public static class Settings
    {
        #region Private fields
        private static DataSet dataSet = new DataSet("FrameWorkSettings");
        private const string settingsFile = "frameworkconfig.xml";
        #endregion

        #region Public delegates and events
        public class SettingsEventArgs : EventArgs
        {
            public string Key;
            public string Value;
            public SettingsEventArgs(string key, string value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        public delegate void SettingsEventHandler(object sender, SettingsEventArgs e);
        public static event SettingsEventHandler SettingsChanged;
        #endregion

        #region Constructor
        static Settings()
        {
            try
            {
                if (File.Exists(settingsFile))
                {
                    ReadSettings();
                }
                else
                {
                    CreateDefaultSettings();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Public methods
        public static string GetValue(string key)
	    {
            DataRow row = dataSet.Tables["Application.KeyValues"].Rows.Find(key);
            if (row != null)
                return row["Value"].ToString();
            else
                return null;
	    }

        public static string GetValue(string key, string defaultValue)
        {
            DataRow row = dataSet.Tables["Application.KeyValues"].Rows.Find(key);
            if (row != null)
                return row["Value"].ToString();
            else
            {
                SetValue(key, defaultValue);
                return defaultValue;
            }
        }

        public static void SetValue(string key, string value)
        {
            DataRow row = dataSet.Tables["Application.KeyValues"].Rows.Find(key);
            if (row != null)
            {
                row["Value"] = value;
            }
            else
            {
                dataSet.Tables["Application.KeyValues"].Rows.Add(key, value);
            }
            if (SettingsChanged != null)
                SettingsChanged(null, new SettingsEventArgs(key, value));
            // save changes
            SaveSettings();
        }
        #endregion

        #region Private methods
        private static void CreateDefaultSettings()
        {
            CreateInputDataTables();
            CreateApplicationDataTables();
        }

        private static void ReadSettings()
        {
            CreateInputDataTables();
            CreateApplicationDataTables();
            dataSet.ReadXml(settingsFile);
        }

        private static void SaveSettings()
        {
            dataSet.WriteXml(settingsFile);
        }
        #endregion

        #region Application
        private static void CreateApplicationDataTables()
        {
            dataSet.Tables.Add("Application.KeyValues");
            dataSet.Tables["Application.KeyValues"].Columns.Add("Key", typeof(string));
            dataSet.Tables["Application.KeyValues"].Columns.Add("Value", typeof(string));
            dataSet.Tables["Application.KeyValues"].PrimaryKey = new DataColumn[] { dataSet.Tables["Application.KeyValues"].Columns["Key"] };
        }
        #endregion

        #region InputManager
        private static void CreateInputDataTables()
        {
            dataSet.Tables.Add("Input.Keyboard");
            dataSet.Tables["Input.Keyboard"].Columns.Add("Function", typeof(string));
            dataSet.Tables["Input.Keyboard"].Columns.Add("Key", typeof(Microsoft.DirectX.DirectInput.Key));
            dataSet.Tables["Input.Keyboard"].PrimaryKey = new DataColumn[] { dataSet.Tables["Input.Keyboard"].Columns["Function"] };

            dataSet.Tables.Add("Input.Joystick");
            dataSet.Tables["Input.Joystick"].Columns.Add("Function", typeof(string));
            dataSet.Tables["Input.Joystick"].Columns.Add("Axis", typeof(InputManager.JoyStickAxis));
            dataSet.Tables["Input.Joystick"].Columns.Add("Inverted", typeof(bool));
            dataSet.Tables["Input.Joystick"].PrimaryKey = new DataColumn[] { dataSet.Tables["Input.Joystick"].Columns["Function"] };
        }

        private static void SetInputProperties()
        {
            dataSet.Tables["Input.Keyboard"].PrimaryKey = new DataColumn[] { dataSet.Tables["Input.Keyboard"].Columns["Function"] };
            dataSet.Tables["Input.Joystick"].PrimaryKey = new DataColumn[] { dataSet.Tables["Input.Joystick"].Columns["Function"] };
        }

        internal static Microsoft.DirectX.DirectInput.Key GetKey(string function)
        {
            DataRow row = dataSet.Tables["Input.Keyboard"].Rows.Find(function);
            if (row != null)
                return (Microsoft.DirectX.DirectInput.Key)row["Key"];
            else
                //throw new BaseException();
                return Microsoft.DirectX.DirectInput.Key.Stop;
        }

        internal static void SetKey(string function, Microsoft.DirectX.DirectInput.Key key)
        {
            DataRow row = dataSet.Tables["Input.Keyboard"].Rows.Find(function);
            if (row != null)
            {
                row["Key"] = key;
            }
            else
            {
                dataSet.Tables["Input.Keyboard"].Rows.Add(function, key);
            }
            // save changes
            SaveSettings();
        }

        internal static bool KeyExists(string function)
        {
            DataRow row = dataSet.Tables["Input.Keyboard"].Rows.Find(function);
            if (row != null)
                return true;
            else
                return false;
        }

        internal static InputManager.JoyStickAxis GetAxis(string function, out bool inverted)
        {
            DataRow row = dataSet.Tables["Input.Joystick"].Rows.Find(function);
            if (row != null)
            {
                inverted = Convert.ToBoolean(row["Inverted"]);
                return (InputManager.JoyStickAxis)row["Axis"];
            }
            else
            {
                //throw new BaseException();
                inverted = false;
                return InputManager.JoyStickAxis.X;
            }
        }

        internal static void SetAxis(string function, InputManager.JoyStickAxis axis, Boolean inverted)
        {
            DataRow row = dataSet.Tables["Input.Joystick"].Rows.Find(function);
            if (row != null)
            {
                row["Axis"] = axis;
                row["Inverted"] = inverted;
            }
            else
            {
                dataSet.Tables["Input.Joystick"].Rows.Add(function, axis, inverted);
            }
            // save changes
            SaveSettings();
        }

        internal static bool AxisExists(string function)
        {
            DataRow row = dataSet.Tables["Input.Joystick"].Rows.Find(function);
            if (row != null)
                return true;
            else
                return false;
        }
        #endregion
    }
}
