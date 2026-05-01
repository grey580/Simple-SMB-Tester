using System.Drawing;
using System.Windows.Forms;

namespace SimpleSmbTester
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblVersion;
        private ComboBox comboSmbVersion;
        private Label lblPath;
        private TextBox txtPath;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnTest;
        private Label lblResult;
        private Label lblHeadline;
        private TextBox txtDetails;
        private Label lblNote;
        private PictureBox pictureLogo;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblVersion = new Label();
            this.comboSmbVersion = new ComboBox();
            this.lblPath = new Label();
            this.txtPath = new TextBox();
            this.lblUsername = new Label();
            this.txtUsername = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.btnTest = new Button();
            this.lblResult = new Label();
            this.lblHeadline = new Label();
            this.txtDetails = new TextBox();
            this.lblNote = new Label();
            this.pictureLogo = new PictureBox();
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(704, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(93, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new Point(18, 39);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(177, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Simple SMB Tester";
            // 
            // pictureLogo
            // 
            this.pictureLogo.Location = new Point(622, 39);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new Size(54, 54);
            this.pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureLogo.TabIndex = 14;
            this.pictureLogo.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new Point(21, 94);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(76, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "SMB Version";
            // 
            // comboSmbVersion
            // 
            this.comboSmbVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboSmbVersion.FormattingEnabled = true;
            this.comboSmbVersion.Location = new Point(24, 112);
            this.comboSmbVersion.Name = "comboSmbVersion";
            this.comboSmbVersion.Size = new Size(160, 21);
            this.comboSmbVersion.TabIndex = 2;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new Point(21, 146);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new Size(96, 13);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "UNC Folder Path";
            // 
            // txtPath
            // 
            this.txtPath.Location = new Point(24, 164);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new Size(652, 22);
            this.txtPath.TabIndex = 4;
            this.txtPath.TextChanged += new System.EventHandler(this.InputFields_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(21, 199);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(59, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new Point(24, 217);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(316, 22);
            this.txtUsername.TabIndex = 6;
            this.txtUsername.TextChanged += new System.EventHandler(this.InputFields_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(360, 199);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(57, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new Point(363, 217);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(313, 22);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.TextChanged += new System.EventHandler(this.InputFields_TextChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new Point(561, 108);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new Size(115, 30);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "Test Credentials";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblResult
            // 
            this.lblResult.BorderStyle = BorderStyle.FixedSingle;
            this.lblResult.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new Point(24, 263);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new Size(120, 34);
            this.lblResult.TabIndex = 10;
            this.lblResult.Text = string.Empty;
            this.lblResult.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeadline
            // 
            this.lblHeadline.BorderStyle = BorderStyle.FixedSingle;
            this.lblHeadline.Location = new Point(150, 263);
            this.lblHeadline.Name = "lblHeadline";
            this.lblHeadline.Padding = new Padding(8, 8, 8, 8);
            this.lblHeadline.Size = new Size(526, 34);
            this.lblHeadline.TabIndex = 11;
            this.lblHeadline.Text = "Enter a UNC path, username, and password.";
            this.lblHeadline.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDetails
            // 
            this.txtDetails.Location = new Point(24, 311);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = ScrollBars.Vertical;
            this.txtDetails.Size = new Size(652, 106);
            this.txtDetails.TabIndex = 12;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.ForeColor = Color.DimGray;
            this.lblNote.Location = new Point(21, 429);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new Size(437, 13);
            this.lblNote.TabIndex = 13;
            this.lblNote.Text = "SMB 1 is typically disabled by default on modern Windows Server builds, including Server 2022.";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnTest;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(704, 458);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.lblHeadline);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.comboSmbVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pictureLogo);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Simple SMB Tester";
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
