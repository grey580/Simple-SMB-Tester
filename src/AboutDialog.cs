using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace SimpleSmbTester
{
    internal sealed class AboutDialog : Form
    {
        private const string DonationUrl = "https://www.paypal.com/donate/?hosted_button_id=SLSRN79D6MDCA";

        public AboutDialog(Image logo)
        {
            Text = "About Simple SMB Tester";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, 520);

            var logoBox = new PictureBox
            {
                Location = new Point(20, 18),
                Size = new Size(64, 64),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = logo == null ? null : new Bitmap(logo)
            };

            var titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(100, 22),
                Text = "Simple SMB Tester"
            };

            var descriptionLabel = new Label
            {
                Location = new Point(20, 102),
                Size = new Size(380, 96),
                Text = "Simple SMB Tester validates whether a UNC share or folder can be reached with the credentials you provide. It supports SMB 1, SMB 2, and SMB 3 testing and is especially useful for troubleshooting legacy scanners, copiers, and mixed Windows file-sharing environments."
            };

            var qrLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 214),
                Text = "Support this app"
            };

            var qrBox = new PictureBox
            {
                Location = new Point(100, 240),
                Size = new Size(220, 220),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = GenerateDonationQrCode()
            };

            var donateButton = new Button
            {
                Location = new Point(140, 476),
                Size = new Size(140, 30),
                Text = "Donate with PayPal"
            };
            donateButton.Click += donateButton_Click;

            Controls.Add(logoBox);
            Controls.Add(titleLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(qrLabel);
            Controls.Add(qrBox);
            Controls.Add(donateButton);
        }

        private static Image GenerateDonationQrCode()
        {
            using (var generator = new QRCodeGenerator())
            using (var data = generator.CreateQrCode(DonationUrl, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(data))
            {
                return qrCode.GetGraphic(8, Color.FromArgb(20, 48, 76), Color.White, true);
            }
        }

        private void donateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(DonationUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to open donation link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
