using System.Windows.Forms;

namespace Not_Defteri
{
	partial class NotDefteri
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotDefteri));
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.yeniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yeniPencereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.acToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.farkliKaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yardımToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.notDefteriHakkindaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox
			// 
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox.Location = new System.Drawing.Point(0, 24);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(784, 426);
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.yardımToolStripMenuItem});
			this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(784, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yeniToolStripMenuItem,
            this.yeniPencereToolStripMenuItem,
            this.acToolStripMenuItem,
            this.kaydetToolStripMenuItem,
            this.farkliKaydetToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(51, 20);
			this.toolStripMenuItem1.Text = "Dosya";
			// 
			// yeniToolStripMenuItem
			// 
			this.yeniToolStripMenuItem.Name = "yeniToolStripMenuItem";
			this.yeniToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.yeniToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.yeniToolStripMenuItem.Text = "Yeni";
			this.yeniToolStripMenuItem.Click += new System.EventHandler(this.yeniToolStripMenuItem_Click);
			// 
			// yeniPencereToolStripMenuItem
			// 
			this.yeniPencereToolStripMenuItem.Name = "yeniPencereToolStripMenuItem";
			this.yeniPencereToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.yeniPencereToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.yeniPencereToolStripMenuItem.Text = "Yeni Pencere";
			this.yeniPencereToolStripMenuItem.Click += new System.EventHandler(this.yeniPencereToolStripMenuItem_Click);
			// 
			// acToolStripMenuItem
			// 
			this.acToolStripMenuItem.Name = "acToolStripMenuItem";
			this.acToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.acToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.acToolStripMenuItem.Text = "Aç...";
			this.acToolStripMenuItem.Click += new System.EventHandler(this.acToolStripMenuItem_Click);
			// 
			// kaydetToolStripMenuItem
			// 
			this.kaydetToolStripMenuItem.Name = "kaydetToolStripMenuItem";
			this.kaydetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.kaydetToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.kaydetToolStripMenuItem.Text = "Kaydet";
			this.kaydetToolStripMenuItem.Click += new System.EventHandler(this.kaydetToolStripMenuItem_Click);
			// 
			// farkliKaydetToolStripMenuItem
			// 
			this.farkliKaydetToolStripMenuItem.Name = "farkliKaydetToolStripMenuItem";
			this.farkliKaydetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.farkliKaydetToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.farkliKaydetToolStripMenuItem.Text = "Farklı Kaydet";
			this.farkliKaydetToolStripMenuItem.Click += new System.EventHandler(this.farkliKaydetToolStripMenuItem_Click);
			// 
			// yardımToolStripMenuItem
			// 
			this.yardımToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notDefteriHakkindaToolStripMenuItem});
			this.yardımToolStripMenuItem.Name = "yardımToolStripMenuItem";
			this.yardımToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.yardımToolStripMenuItem.Text = "Yardım";
			// 
			// notDefteriHakkindaToolStripMenuItem
			// 
			this.notDefteriHakkindaToolStripMenuItem.Name = "notDefteriHakkindaToolStripMenuItem";
			this.notDefteriHakkindaToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.notDefteriHakkindaToolStripMenuItem.Text = "Not Defteri Hakkında";
			this.notDefteriHakkindaToolStripMenuItem.Click += new System.EventHandler(this.notDefteriHakkindaToolStripMenuItem_Click);
			// 
			// NotDefteri
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 450);
			this.Controls.Add(this.richTextBox);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "NotDefteri";
			this.Text = "Not Defteri";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem yeniToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem acToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem kaydetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem yeniPencereToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem farkliKaydetToolStripMenuItem;
		private ToolStripMenuItem yardımToolStripMenuItem;
		private ToolStripMenuItem notDefteriHakkindaToolStripMenuItem;
	}
}