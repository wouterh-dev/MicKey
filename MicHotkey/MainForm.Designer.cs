namespace MicHotkey
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toggleButton = new System.Windows.Forms.Button();
            this.button_hotkey = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_hotkey = new System.Windows.Forms.Label();
            this.timer_update = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toggleButton
            // 
            this.toggleButton.Location = new System.Drawing.Point(80, 3);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(75, 23);
            this.toggleButton.TabIndex = 0;
            this.toggleButton.Text = "Toggle Mic";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button_hotkey
            // 
            this.button_hotkey.Location = new System.Drawing.Point(3, 3);
            this.button_hotkey.Name = "button_hotkey";
            this.button_hotkey.Size = new System.Drawing.Size(71, 23);
            this.button_hotkey.TabIndex = 1;
            this.button_hotkey.Text = "Set hotkey";
            this.button_hotkey.UseVisualStyleBackColor = true;
            this.button_hotkey.Click += new System.EventHandler(this.button_hotkey_Click);
            this.button_hotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.button2_KeyUp);
            this.button_hotkey.Leave += new System.EventHandler(this.button_hotkey_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.button_hotkey, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toggleButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_hotkey, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(158, 42);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label_hotkey
            // 
            this.label_hotkey.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label_hotkey, 2);
            this.label_hotkey.Location = new System.Drawing.Point(3, 29);
            this.label_hotkey.Name = "label_hotkey";
            this.label_hotkey.Size = new System.Drawing.Size(65, 13);
            this.label_hotkey.TabIndex = 2;
            this.label_hotkey.Text = "Hotkey: ???";
            // 
            // timer_update
            // 
            this.timer_update.Interval = 1000;
            this.timer_update.Tick += new System.EventHandler(this.Timer_update_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "MicKey";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(234, 86);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(175, 85);
            this.Name = "MainForm";
            this.Text = "MicKey";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.Button button_hotkey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer timer_update;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label label_hotkey;
    }
}

