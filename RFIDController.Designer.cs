namespace FastCheckout
{
    partial class RFIDController
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFIDController));
            notifyIcon = new NotifyIcon(components);
            hintLabel = new Label();
            connectionStatusLabel = new Label();
            inventoryStatusLabel = new Label();
            tagCountLabel = new Label();
            tagsListBox = new ListBox();
            SuspendLayout();
            //
            // notifyIcon
            //
            notifyIcon.Text = "RFID Controller";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            //
            // hintLabel
            //
            hintLabel.AutoSize = false;
            hintLabel.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            hintLabel.ForeColor = Color.DimGray;
            hintLabel.Location = new Point(15, 12);
            hintLabel.Size = new Size(420, 22);
            hintLabel.Text = "Press S anywhere to start / stop inventory";
            //
            // connectionStatusLabel
            //
            connectionStatusLabel.AutoSize = false;
            connectionStatusLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            connectionStatusLabel.ForeColor = Color.Firebrick;
            connectionStatusLabel.Location = new Point(15, 42);
            connectionStatusLabel.Size = new Size(420, 24);
            connectionStatusLabel.Text = "Reader: Disconnected";
            //
            // inventoryStatusLabel
            //
            inventoryStatusLabel.AutoSize = false;
            inventoryStatusLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            inventoryStatusLabel.ForeColor = Color.DimGray;
            inventoryStatusLabel.Location = new Point(15, 70);
            inventoryStatusLabel.Size = new Size(420, 24);
            inventoryStatusLabel.Text = "Inventory: Stopped";
            //
            // tagCountLabel
            //
            tagCountLabel.AutoSize = false;
            tagCountLabel.Font = new Font("Segoe UI", 9F);
            tagCountLabel.ForeColor = Color.Black;
            tagCountLabel.Location = new Point(15, 102);
            tagCountLabel.Size = new Size(420, 20);
            tagCountLabel.Text = "Tags scanned: 0";
            //
            // tagsListBox
            //
            tagsListBox.Font = new Font("Consolas", 9F);
            tagsListBox.IntegralHeight = false;
            tagsListBox.Location = new Point(15, 128);
            tagsListBox.Name = "tagsListBox";
            tagsListBox.Size = new Size(420, 260);
            tagsListBox.TabStop = false;
            //
            // RFIDController
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 400);
            Controls.Add(hintLabel);
            Controls.Add(connectionStatusLabel);
            Controls.Add(inventoryStatusLabel);
            Controls.Add(tagCountLabel);
            Controls.Add(tagsListBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RFIDController";
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RFID Controller";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.Label inventoryStatusLabel;
        private System.Windows.Forms.Label tagCountLabel;
        private System.Windows.Forms.ListBox tagsListBox;
    }
}
