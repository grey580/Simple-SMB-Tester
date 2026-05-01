using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleSmbTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboSmbVersion.Items.AddRange(new object[] { "SMB 1", "SMB 2", "SMB 3" });
            comboSmbVersion.SelectedIndex = 2;
            pictureLogo.Image = LoadLogoImage();
            var executableIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            if (executableIcon != null)
            {
                Icon = executableIcon;
            }
            SetStatus(false, "Ready to test a UNC path.", "Windows Server 2022 normally supports SMB 2 and SMB 3 by default. SMB 1 is included here for legacy copier checks.");
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPath.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                SetStatus(false, "Enter a UNC path, username, and password.", "Example path: \\\\server\\share or \\\\server\\share\\folder");
                return;
            }

            ToggleInputs(false);
            SetStatus(false, "Testing...", "Connecting to the share and validating the entered folder.");

            try
            {
                var protocol = (SmbProtocolSelection)comboSmbVersion.SelectedIndex;
                var result = await Task.Run(() => SmbTestService.Test(txtPath.Text, txtUsername.Text, txtPassword.Text, protocol));
                SetStatus(result.Success, result.StatusText, result.DetailsText);
            }
            catch (Exception ex)
            {
                SetStatus(false, "Unexpected error.", ex.Message);
            }
            finally
            {
                ToggleInputs(true);
            }
        }

        private void ToggleInputs(bool enabled)
        {
            comboSmbVersion.Enabled = enabled;
            txtPath.Enabled = enabled;
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            btnTest.Enabled = enabled;
        }

        private void SetStatus(bool success, string headline, string details)
        {
            lblResult.Text = success ? "SUCCESS" : "FAILURE";
            lblResult.BackColor = success ? Color.FromArgb(32, 102, 52) : Color.FromArgb(132, 28, 28);
            lblResult.ForeColor = Color.White;
            lblHeadline.Text = headline;
            txtDetails.Text = details;
        }

        private Image LoadLogoImage()
        {
            var resourceName = "SimpleSmbTester.AppLogo.png";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    return Image.FromStream(memoryStream);
                }
            }
        }
    }
}
