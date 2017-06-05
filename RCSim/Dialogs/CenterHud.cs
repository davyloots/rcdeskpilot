using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Textures;
using Microsoft.DirectX.Direct3D;
using System.Threading;
using Bonsai.Utils;

namespace RCSim.Dialogs
{
    internal class CenterHud : IFrameworkCallback, IDisposable
    {
        private class HudMessage
        {
            public string Text;
            public double StartTime;

            public HudMessage(string text, double startTime)
            {
                Text = text;
                StartTime = startTime;
            }
        }

        #region Private enums
        private enum CenterHudControlIds
        {
            Static = -1,
            CrashPicture,
            MenuButton,
            ResetButton,
            ToggleSmokeButton,
            ViewButton,
            AutozoomButton,
            ZoomInButton,
            ZoomOutButton,
            GameRestartButton,
            LogoPicture,
            MapPicture,
            MapOverlayPicture,
            AltitudeText
        }
        #endregion

        #region Private fields
        private Dialog dialog;
        private Picture crashPicture;
        private Button menuButton;
        private Button resetButton;
        private Button toggleSmokeButton;
        private Button viewButton;
        private Button autozoomButton;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button gameRestartButton;
        private StaticText captionText;
        private StaticText infoText;
        private StaticText gameText;
        private Picture mapPicture;
        private Picture mapOverlay;
        private StaticText altituteText;
#if LOGO
        private Picture logoPicture;
#endif

        private bool crashed = false;
        private bool showInfo = false;
        private double crashTime = 0;
        private double mouseMoveTime = 0;
        private int mouseX = 0;
        private int mouseY = 0;
        private double captionTime = 0;
        private double gameTextTime = 0;
        private double gameTextDuration = 0;
        private Program owner = null;
        private string crashUrl = null;
        private List<StaticText> messages = new List<StaticText>();
        private List<HudMessage> hudMessages = new List<HudMessage>();
        private const string defaultMap = "data/compass.png";
        #endregion

        #region Public properties
        public Dialog Dialog
        {
            get { return dialog; }
        }

        public bool ShowInfo
        {
            get { return showInfo; }
            set 
            { 
                showInfo = value;
                infoText.IsVisible = showInfo;
            }
        }

        public bool RestartButtonVisible
        {
            set
            {
                gameRestartButton.IsVisible = value;
            }
        }

        public bool MapVisible
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CenterHud(Program owner)
        {
            this.owner = owner;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region Public methods
        public void Initialize()
        {
            dialog = new Dialog(Framework.Instance);
            dialog.SetFont(0, "Arial", 14, FontWeight.Normal);
            dialog.SetFont(1, "Arial", 28, FontWeight.Bold);
           
            captionText = dialog.AddStatic((int)CenterHudControlIds.Static, "Hello world", 0, dialog.Height - 50 , dialog.Width, 22);
            Element e = captionText[0];
            e.FontIndex = 1;

            infoText = dialog.AddStatic((int)CenterHudControlIds.Static, "", 5, 5, 630, 40);
            e = infoText[0];
            e.FontIndex = 0;
            e.textFormat = DrawTextFormat.Left;
            infoText.IsVisible = false;

            gameText = dialog.AddStatic((int)CenterHudControlIds.Static, "", 150, 30, dialog.Width - 300, 60);
            e = gameText[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Center | DrawTextFormat.WordBreak;
            
            //crashPicture = dialog.AddPicture((int)CenterHudControlIds.CrashPicture, "/data/crash.png", dialog.Width / 4, dialog.Height / 4, dialog.Width / 2, dialog.Height / 2);
            //crashPicture.SourceRectangle = new System.Drawing.Rectangle(0, 0, 512, 512);
            //crashPicture.IsVisible = crashed;

#if LOGO
            if (Utility.MediaExists("/data/logo.png"))
            {
                logoPicture = dialog.AddPicture((int)CenterHudControlIds.LogoPicture, "/data/logo.png", dialog.Width - 100, dialog.Height - 60, 50, 28);
                logoPicture.SourceRectangle = new System.Drawing.Rectangle(0, 0, 128, 64);
            }
#endif

            messages.Add(AddStaticText(dialog.Height - 70));
            messages.Add(AddStaticText(dialog.Height - 50));
            messages.Add(AddStaticText(dialog.Height - 30));

            int y = 10;

            menuButton = dialog.AddButton((int)CenterHudControlIds.MenuButton, "menu", 10, y += 30, 120, 21);
            menuButton.Click += new EventHandler(menuButton_Click);

            resetButton = dialog.AddButton((int)CenterHudControlIds.ResetButton, "reset (ENTER)", 10, y += 30, 120, 21);
            resetButton.Click += new EventHandler(resetButton_Click);

            toggleSmokeButton = dialog.AddButton((int)CenterHudControlIds.ToggleSmokeButton, "toggle smoke (S)", 10, y += 30, 120, 21);
            toggleSmokeButton.Click += new EventHandler(toggleSmokeButton_Click);

            viewButton = dialog.AddButton((int)CenterHudControlIds.ViewButton, "change view (V)", 10, y += 30, 120, 21);
            viewButton.Click += new EventHandler(viewButton_Click);

            autozoomButton = dialog.AddButton((int)CenterHudControlIds.AutozoomButton, "camera mode (B)", 10, y += 30, 120, 21);
            autozoomButton.Click += new EventHandler(autozoomButton_Click);

            zoomInButton = dialog.AddButton((int)CenterHudControlIds.ZoomInButton, "zoom in (+)", 10, y += 30, 120, 21);
            zoomInButton.Click += new EventHandler(zoomInButton_Click);

            zoomOutButton = dialog.AddButton((int)CenterHudControlIds.ZoomOutButton, "zoom out (-)", 10, y += 30, 120, 21);
            zoomOutButton.Click += new EventHandler(zoomOutButton_Click);

            gameRestartButton = dialog.AddButton((int)CenterHudControlIds.GameRestartButton, "restart challenge", 10, 10, 120, 21);
            gameRestartButton.IsVisible = false;
            gameRestartButton.Click += new EventHandler(gameRestartButton_Click);
            
            mapPicture = dialog.AddPicture((int)CenterHudControlIds.MapPicture, "data/scenery/default/map_1.png", 10, 10, 128, 128);
            mapPicture.SourceRectangle = new System.Drawing.Rectangle(0, 0, 128, 128);

            mapOverlay = dialog.AddPicture((int)CenterHudControlIds.MapOverlayPicture, "data/map_overlay.png", 10, 10, 128, 128);
            mapOverlay.SourceRectangle = new System.Drawing.Rectangle(0, 0, 128, 128);

            altituteText = dialog.AddStatic((int)CenterHudControlIds.AltitudeText, "alt: 0m", 10, 74, 128, 64);
            e = altituteText[0];
            e.FontIndex = 0;
            e.textFormat = DrawTextFormat.Center | DrawTextFormat.WordBreak;

            MapVisible = Convert.ToBoolean(Settings.GetValue("CompassVisible", "true"));

            Framework.Instance.Window.MouseMove += new System.Windows.Forms.MouseEventHandler(Window_MouseMove);
        }

        private StaticText AddStaticText(int y)
        {
            StaticText text = dialog.AddStatic((int)CenterHudControlIds.Static, "", 10, y, dialog.Width, 20);
            Element e = text[0];
            e.FontIndex = 0;
            e.textFormat = DrawTextFormat.Left;
            text.IsVisible = false;
            return text;
        }

        public void SetMapPicture(string filename)
        {
            try
            {
                if (filename != null)
                    mapPicture.TextureFile = filename;
                else
                    mapPicture.TextureFile = defaultMap;
            }
            catch
            {
                mapPicture.TextureFile = defaultMap;
            }
        }

        public void SetCrashPicture(string filename, string url, string folder)
        {
            if (crashPicture != null)
            {
                dialog.RemoveControl(crashPicture);
                crashPicture.Dispose();
                crashPicture = null;
                crashUrl = null;
            }

            if (filename != null)
            {
                string path = Utility.FindMediaFile(filename, folder);
                crashPicture = dialog.AddPicture((int)CenterHudControlIds.CrashPicture, path, dialog.Width / 4, dialog.Height / 4, dialog.Width / 2, dialog.Height / 2);
                crashPicture.SourceRectangle = new System.Drawing.Rectangle(0, 0, 256, 256);
                crashPicture.IsVisible = crashed;
                crashPicture.Click += new EventHandler(crashPicture_Click);
                SetSize(Framework.Instance.Device.PresentationParameters.BackBufferWidth, Framework.Instance.Device.PresentationParameters.BackBufferHeight);
                crashUrl = url;
            }
        }

        private void LaunchWebSiteRun()
        {
            try
            {
                System.Diagnostics.Process.Start(crashUrl);
            }
            catch
            {

            }
        }

        void gameRestartButton_Click(object sender, EventArgs e)
        {
            owner.RestartGame();
        }

        void crashPicture_Click(object sender, EventArgs e)
        {
            if (crashUrl != null)
            {
                Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
                Thread thread = new Thread(new ThreadStart(LaunchWebSiteRun));
                thread.Start();
            }
        }

        void zoomOutButton_Click(object sender, EventArgs e)
        {
            owner.ZoomOut();
        }

        void zoomInButton_Click(object sender, EventArgs e)
        {
            owner.ZoomIn();
        }

        void autozoomButton_Click(object sender, EventArgs e)
        {
            owner.ChangeCameraMode();
        }

        void  viewButton_Click(object sender, EventArgs e)
        {
            owner.ChangeView();
        }

        void menuButton_Click(object sender, EventArgs e)
        {
            owner.ShowMenu();
        }

        void resetButton_Click(object sender, EventArgs e)
        {
            owner.Player.Reset();   
        }

        void toggleSmokeButton_Click(object sender, EventArgs e)
        {
            owner.Player.ToggleSmoke();
        }

        void Window_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.X != mouseX) || (e.Y != mouseY))
            {
                if (!resetButton.IsVisible)
                    System.Windows.Forms.Cursor.Show();
                mouseMoveTime = owner.CurrentTime + 3.0;
                menuButton.IsVisible = true;
                resetButton.IsVisible = true;
                toggleSmokeButton.IsVisible = true;
                viewButton.IsVisible = true;
                autozoomButton.IsVisible = true;
                zoomInButton.IsVisible = true;
                zoomOutButton.IsVisible = true;
                Framework.Instance.Window.Cursor = System.Windows.Forms.Cursors.Default;
            }
            mouseX = e.X;
            mouseY = e.Y;
        }

        public void SetSize(int width, int height)
        {
            dialog.SetSize(width, height);
            if (dialog.Width > dialog.Height)
            {
                if (crashPicture != null)
                {
                    crashPicture.SetSize(dialog.Height / 2, dialog.Height / 2);
                    crashPicture.SetLocation((dialog.Width - dialog.Height) / 2 + dialog.Height / 4, dialog.Height / 4);
                }
            }
            else
            {
                if (crashPicture != null)
                {
                    crashPicture.SetSize(dialog.Width / 2, dialog.Width / 2);
                    crashPicture.SetLocation(dialog.Width / 4, (dialog.Height - dialog.Width) / 2 + dialog.Width / 4);
                }
            }

#if LOGO
            if (logoPicture != null)
            {
                logoPicture.SetSize(50 * dialog.Width / 1024, 29 * dialog.Width / 1024);
                logoPicture.SetLocation(dialog.Width - 75 * dialog.Width / 1024, dialog.Height - 50 * dialog.Width / 1024);
            }
#endif
            int scale = Math.Min(128 * dialog.Width / 1024, 128);
            if (mapPicture != null)
            {
                mapPicture.SetSize(scale, scale);
                mapPicture.SetLocation(10, dialog.Height - scale - 10);
            }
            if (mapOverlay != null)
            {
                mapOverlay.SetSize(scale, scale);
                mapOverlay.SetLocation(10, dialog.Height - scale - 10);
                altituteText.SetLocation(-54 + scale / 2, dialog.Height - 4 * scale / 10 - 10);
            }

            captionText.SetLocation(0, dialog.Height - 50);
            captionText.SetSize(dialog.Width, 50);
            gameText.SetLocation(150, 30);
            gameText.SetSize(dialog.Width - 300, 60);
            for (int i = 0; i < messages.Count; i++)
            {
                StaticText text = messages[i];
                text.SetLocation(10, Dialog.Height - 30 - (messages.Count - i) * 20);
                text.SetSize(Dialog.Width - 10, 20);
            }
        }

        public void Crash(double currentTime)
        {
            crashed = true;
            if (crashPicture != null)
                crashPicture.IsVisible = crashed;
            crashTime = currentTime + 3.0;            
        }

        public void ShowCaption(string text, double currentTime)
        {
            captionText.SetText(text);
            captionText.IsVisible = true;
            captionTime = currentTime + 3.0;
        }

        public void ShowCaption(string text, double currentTime, double duration)
        {
            captionText.SetText(text);
            captionText.IsVisible = true;
            captionTime = currentTime + duration;
        }

        public void ShowGameText(string text, double duration)
        {
            gameText.SetText(text);
            gameText.IsVisible = true;
            gameTextTime = owner.CurrentTime + duration;            
        }

        public void ShowMessage(string text)
        {
            hudMessages.Add(new HudMessage(text, Program.Instance.CurrentTime));
            if (hudMessages.Count > 3)
            {
                hudMessages.RemoveAt(0);
                messages[0].SetText(hudMessages[0].Text);
                messages[1].SetText(hudMessages[1].Text);
                messages[2].SetText(hudMessages[2].Text);
            }
            else
            {
                messages[hudMessages.Count-1].SetText(text);
                messages[hudMessages.Count-1].IsVisible = true;
            }
        }
        #endregion

        private void UpdateMessages(double currentTime)
        {
            if (hudMessages.Count > 0)
            {
                if (hudMessages[0].StartTime + 5 < currentTime)
                {
                    hudMessages.RemoveAt(0);
                    for (int i = 0; i < messages.Count; i++)
                    {
                        if (i < hudMessages.Count)
                        {
                            messages[i].SetText(hudMessages[i].Text);
                            messages[i].IsVisible = true;
                        }
                        else
                            messages[i].IsVisible = false;
                    }
                }
            }
        }

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            UpdateMessages(totalTime);
            if (showInfo)
            {                
#if DEBUG
                float distance = Program.Instance.Player.Position.Length();
                if (distance > 128)
                {
                    distance = Math.Max(0.5f - (distance - 128) / 1024, 0.001f);
                }
                else
                {
                    distance = 1f - distance / 255f;
                }


                infoText.SetText(string.Format("FPS: {0}\t Alt: {1}m\t Speed: {2}m/s\r\n{3}\r\n{4} RGB:{5}",
                    Framework.Instance.FPS.ToString("F00"), (-owner.Player.FlightModel.Z).ToString("F00"), owner.Player.FlightModel.Speed.ToString("F00"),
                    Framework.Instance.DebugString, Program.Instance.Player.Position.Length().ToString("F00"), (distance*255).ToString("F")));


#else
                float distance = Program.Instance.Player.Position.Length();
                if (distance > 128)
                {
                    distance = Math.Max(0.5f - (distance - 128) / 1024, 0.001f);
                }
                else
                {
                    distance = 1f - distance / 255f;
                }

                infoText.SetText(string.Format("FPS: {0}\t Alt: {1}m\t Speed: {2}m/s\nDepthmap value: {3}",
                    Framework.Instance.FPS.ToString("F00"), (-owner.Player.FlightModel.Z).ToString("F00"), owner.Player.FlightModel.Speed.ToString("F00"),
                    (distance*255).ToString("F")));
#endif
            }

            if (MapVisible && Framework.Instance.CurrentCamera == Program.Instance.ObserverCamera)
            {
                mapPicture.Rotation = Program.Instance.ObserverCamera.Direction - (float)Math.PI / 2;
                mapPicture.IsVisible = true;
                mapOverlay.IsVisible = true;
                altituteText.IsVisible = true;
                altituteText.SetText(string.Format("alt: {0}m", (-owner.Player.FlightModel.Z).ToString("F00")));
            }
            else
            {
                mapPicture.IsVisible = false;
                mapOverlay.IsVisible = false;
                altituteText.IsVisible = false;
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {            
            if (crashed && (crashTime < totalTime))
            {
                crashed = false;
                if (crashPicture != null)
                    crashPicture.IsVisible = crashed;
            }
            if (resetButton.IsVisible && (mouseMoveTime < totalTime))
            {
                menuButton.IsVisible = false;
                resetButton.IsVisible = false;
                toggleSmokeButton.IsVisible = false;
                viewButton.IsVisible = false;
                autozoomButton.IsVisible = false;
                zoomInButton.IsVisible = false;
                zoomOutButton.IsVisible = false;
                System.Windows.Forms.Cursor.Hide();
            }
            if (captionText.IsVisible && (captionTime < totalTime))
            {
                captionText.IsVisible = false;
            }
            if (gameText.IsVisible && (gameTextTime < totalTime))
            {
                gameText.IsVisible = false;
            }
            dialog.OnRender(elapsedTime);
        }
        #endregion

        
    }
}
