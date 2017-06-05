using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Bonsai.Utils
{
    public class SplashScreen : Form
    {
        #region Private fields
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        #endregion

        #region Private classes
        private class FormData
        {
            public Image Image;
            public Icon Icon;
            public string Version;
        }
        #endregion

        #region Generated code
        private void InitializeComponent()
        {
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(12, 146);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(616, 26);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Loading";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(511, 172);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(117, 19);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // SplashScreen
            // 
            this.ClientSize = new System.Drawing.Size(640, 200);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
        #endregion

        #region Private constructor
        private SplashScreen()
        {
            this.Opacity = 0.1;
            InitializeComponent();
            this.ClientSize = new Size(640,200);
            this.timer.Interval = 100;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Enabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.1;
            if (this.Opacity >= 1.0)
                this.timer.Enabled = false;
        }
        #endregion

        #region Static stuff
        static SplashScreen _splashScreen = null;
        private Label labelStatus;
        private Label labelVersion;
        static Thread _thread = null;

        static public void StartSplashScreen(Image image, Icon icon, string version)
        {
            if (_splashScreen != null)
            {
                _splashScreen.Dispose();
                _splashScreen = null;
            }
            if (_splashScreen == null)
            {
                _thread = new Thread(new ParameterizedThreadStart(SplashScreen.ShowForm));
                _thread.IsBackground = true;
                _thread.SetApartmentState(ApartmentState.STA);
                FormData formData = new FormData();
                formData.Image = image;
                formData.Icon = icon;
                formData.Version = version;
                _thread.Start(formData);
                Thread.Sleep(100);
            }                
        }

        static public void HideSplashScreen()
        {
            if (_splashScreen != null)
            {
                try
                {
                    _splashScreen.Invoke(new CloseHandler(SplashScreen.HideForm));
                }
                catch { }
            }
        }



        static public void SetSplashStatus(string status)
        {
            if (_splashScreen != null)
            {
                try
                {
                    _splashScreen.Invoke(new StatusHandler(SplashScreen.SetStatus), status);
                }
                catch { }
            }
        }

        static public void SetSplashBackground(Image image)
        {
            if (_splashScreen != null)
            {
                try
                {
                    _splashScreen.Invoke(new BackgroundHandler(SplashScreen.SetBackground), image);
                }
                catch { }
            }
        }

        static public void SetSplashTextColor(Color color)
        {
            if (_splashScreen != null)
            {
                try
                {
                    _splashScreen.Invoke(new TextColorHandler(SplashScreen.SetTextColor), color);
                }
                catch { }
            }
        }

        static private void ShowForm(object o)
        {
            FormData formData = o as FormData;
            _splashScreen = new SplashScreen();
            if (formData != null)
            {
                if (formData.Image != null)
                    _splashScreen.BackgroundImage = formData.Image;
                if (formData.Icon != null)
                    _splashScreen.Icon = formData.Icon;
                if (formData.Version != null)
                    _splashScreen.labelVersion.Text = formData.Version;
            }
            Application.Run(_splashScreen);
        }

        static private void HideForm()
        {
            _splashScreen.Close();
        }

        static private void SetStatus(string status)
        {
            _splashScreen.labelStatus.Text = status;
        }

        static private void SetBackground(Image image)
        {
            _splashScreen.BackgroundImage = image;
        }

        static private void SetTextColor(Color color)
        {
            _splashScreen.labelStatus.ForeColor = color;
            _splashScreen.labelVersion.ForeColor = color;
        }

        private delegate void CloseHandler();
        private delegate void StatusHandler(string status);
        private delegate void BackgroundHandler(Image image);
        private delegate void TextColorHandler(Color color);
        #endregion
    }
}
