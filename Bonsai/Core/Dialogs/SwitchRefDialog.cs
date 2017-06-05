using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Dialogs
{
    internal class SwitchRefDialog : System.Windows.Forms.Form
    {
        internal const string KeyLocation = @"Software\Microsoft\DirectX 9.0 SDK\ManagedSamples";
        internal const string KeyValueName = "SkipWarning";

        public SwitchRefDialog(string title)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Use the 'question' icon
            this.pictureBox1.Image = System.Drawing.SystemIcons.Question.ToBitmap();
            // Include text
            this.lblInfo.Text = "Switching to the Direct3D reference rasterizer, a software device that implements the entire Direct3D feature set, but runs very slowly.\r\nDo you wish to continue?";
            // UPdate title
            this.Text = title;
        }

        public SwitchRefDialog(string title, string text)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Use the 'question' icon
            this.pictureBox1.Image = System.Drawing.SystemIcons.Question.ToBitmap();
            // Include text
            this.lblInfo.Text = text;
            // UPdate title
            this.Text = title;
        }

        #region Windows Form Designer generated code
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.CheckBox chkShowAgain;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.chkShowAgain = new System.Windows.Forms.CheckBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(16, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Location = new System.Drawing.Point(64, 16);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(328, 48);
            this.lblInfo.TabIndex = 99;
            // 
            // chkShowAgain
            // 
            this.chkShowAgain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkShowAgain.Location = new System.Drawing.Point(8, 104);
            this.chkShowAgain.Name = "chkShowAgain";
            this.chkShowAgain.Size = new System.Drawing.Size(224, 16);
            this.chkShowAgain.TabIndex = 2;
            this.chkShowAgain.Text = "&Don\'t show again";
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnYes.Location = new System.Drawing.Point(117, 72);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(80, 24);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "&Yes";
            this.btnYes.Click += new EventHandler(OnYes);
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNo.Location = new System.Drawing.Point(205, 72);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(80, 24);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "&No";
            this.btnNo.Click += new EventHandler(OnNo);
            // 
            // SwitchRefDialog
            // 
            this.AcceptButton = this.btnYes;
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(402, 134);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.chkShowAgain);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SwitchRefDialog";
            this.Text = "SampleName";
            this.ResumeLayout(false);

        }
        /// <summary>
        /// Fired when the 'Yes' button is clicked
        /// </summary>
        private void OnYes(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// Fired when the 'No' button is clicked
        /// </summary>
        private void OnNo(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        #endregion

        /// <summary>
        /// Dialog is being dismissed, either continue the application, or shutdown.  
        /// Save setting if required.
        /// </summary>
        protected override void OnClosed(System.EventArgs e)
        {
            // Is the box checked?
            if (chkShowAgain.Checked)
            {
                using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(KeyLocation))
                {
                    key.SetValue(KeyValueName, (int)1);
                }
            }
        }
    }
}
