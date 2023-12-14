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
			this.yazdırToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.düzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.geriAlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ileriAlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kopyalaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yapıştırToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.silToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.degistirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tumunuSecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saatTarihToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.görünümToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yakınlaştırToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yakınlastirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.uzaklastirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.varsayilanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.durumcubuguToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yardımToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.notDefteriHakkindaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printDocument1 = new System.Drawing.Printing.PrintDocument();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox
			// 
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox.Location = new System.Drawing.Point(0, 24);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(784, 404);
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			this.richTextBox.SelectionChanged += new System.EventHandler(this.richTextBox_SelectionChanged);
			this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.düzenToolStripMenuItem,
            this.görünümToolStripMenuItem,
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
            this.farkliKaydetToolStripMenuItem,
            this.yazdırToolStripMenuItem});
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
			// yazdırToolStripMenuItem
			// 
			this.yazdırToolStripMenuItem.Name = "yazdırToolStripMenuItem";
			this.yazdırToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.yazdırToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.yazdırToolStripMenuItem.Text = "Yazdır";
			this.yazdırToolStripMenuItem.Click += new System.EventHandler(this.yazdırToolStripMenuItem_Click);
			// 
			// düzenToolStripMenuItem
			// 
			this.düzenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geriAlToolStripMenuItem,
            this.ileriAlToolStripMenuItem,
            this.kesToolStripMenuItem,
            this.kopyalaToolStripMenuItem,
            this.yapıştırToolStripMenuItem,
            this.silToolStripMenuItem,
            this.bulToolStripMenuItem,
            this.degistirToolStripMenuItem,
            this.tumunuSecToolStripMenuItem,
            this.saatTarihToolStripMenuItem});
			this.düzenToolStripMenuItem.Name = "düzenToolStripMenuItem";
			this.düzenToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.düzenToolStripMenuItem.Text = "Düzen";
			// 
			// geriAlToolStripMenuItem
			// 
			this.geriAlToolStripMenuItem.Name = "geriAlToolStripMenuItem";
			this.geriAlToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.geriAlToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.geriAlToolStripMenuItem.Text = "Geri Al";
			this.geriAlToolStripMenuItem.Click += new System.EventHandler(this.geriAlToolStripMenuItem_Click);
			// 
			// ileriAlToolStripMenuItem
			// 
			this.ileriAlToolStripMenuItem.Name = "ileriAlToolStripMenuItem";
			this.ileriAlToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.ileriAlToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.ileriAlToolStripMenuItem.Text = "İleri Al";
			this.ileriAlToolStripMenuItem.Click += new System.EventHandler(this.ileriAlToolStripMenuItem_Click);
			// 
			// kesToolStripMenuItem
			// 
			this.kesToolStripMenuItem.Name = "kesToolStripMenuItem";
			this.kesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.kesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.kesToolStripMenuItem.Text = "Kes";
			this.kesToolStripMenuItem.Click += new System.EventHandler(this.kesToolStripMenuItem_Click);
			// 
			// kopyalaToolStripMenuItem
			// 
			this.kopyalaToolStripMenuItem.Name = "kopyalaToolStripMenuItem";
			this.kopyalaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.kopyalaToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.kopyalaToolStripMenuItem.Text = "Kopyala";
			this.kopyalaToolStripMenuItem.Click += new System.EventHandler(this.kopyalaToolStripMenuItem_Click);
			// 
			// yapıştırToolStripMenuItem
			// 
			this.yapıştırToolStripMenuItem.Name = "yapıştırToolStripMenuItem";
			this.yapıştırToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.yapıştırToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.yapıştırToolStripMenuItem.Text = "Yapıştır";
			this.yapıştırToolStripMenuItem.Click += new System.EventHandler(this.yapıştırToolStripMenuItem_Click);
			// 
			// silToolStripMenuItem
			// 
			this.silToolStripMenuItem.Name = "silToolStripMenuItem";
			this.silToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.silToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.silToolStripMenuItem.Text = "Sil";
			this.silToolStripMenuItem.Click += new System.EventHandler(this.silToolStripMenuItem_Click);
			// 
			// bulToolStripMenuItem
			// 
			this.bulToolStripMenuItem.Name = "bulToolStripMenuItem";
			this.bulToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.bulToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.bulToolStripMenuItem.Text = "Bul...";
			this.bulToolStripMenuItem.Click += new System.EventHandler(this.bulToolStripMenuItem_Click);
			// 
			// degistirToolStripMenuItem
			// 
			this.degistirToolStripMenuItem.Name = "degistirToolStripMenuItem";
			this.degistirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.degistirToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.degistirToolStripMenuItem.Text = "Değiştir...";
			this.degistirToolStripMenuItem.Click += new System.EventHandler(this.degistirToolStripMenuItem_Click);
			// 
			// tumunuSecToolStripMenuItem
			// 
			this.tumunuSecToolStripMenuItem.Name = "tumunuSecToolStripMenuItem";
			this.tumunuSecToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.tumunuSecToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.tumunuSecToolStripMenuItem.Text = "Tümünü Seç";
			this.tumunuSecToolStripMenuItem.Click += new System.EventHandler(this.tumunuSecToolStripMenuItem_Click);
			// 
			// saatTarihToolStripMenuItem
			// 
			this.saatTarihToolStripMenuItem.Name = "saatTarihToolStripMenuItem";
			this.saatTarihToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.saatTarihToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.saatTarihToolStripMenuItem.Text = "Saat/Tarih";
			this.saatTarihToolStripMenuItem.Click += new System.EventHandler(this.saatTarihToolStripMenuItem_Click);
			// 
			// görünümToolStripMenuItem
			// 
			this.görünümToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yakınlaştırToolStripMenuItem,
            this.durumcubuguToolStripMenuItem});
			this.görünümToolStripMenuItem.Name = "görünümToolStripMenuItem";
			this.görünümToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.görünümToolStripMenuItem.Text = "Görünüm";
			// 
			// yakınlaştırToolStripMenuItem
			// 
			this.yakınlaştırToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yakınlastirToolStripMenuItem1,
            this.uzaklastirToolStripMenuItem,
            this.varsayilanToolStripMenuItem});
			this.yakınlaştırToolStripMenuItem.Name = "yakınlaştırToolStripMenuItem";
			this.yakınlaştırToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
			this.yakınlaştırToolStripMenuItem.Text = "Yakınlaştır";
			// 
			// yakınlastirToolStripMenuItem1
			// 
			this.yakınlastirToolStripMenuItem1.Name = "yakınlastirToolStripMenuItem1";
			this.yakınlastirToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
			this.yakınlastirToolStripMenuItem1.Text = "Yakınlaştır";
			// 
			// uzaklastirToolStripMenuItem
			// 
			this.uzaklastirToolStripMenuItem.Name = "uzaklastirToolStripMenuItem";
			this.uzaklastirToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
			this.uzaklastirToolStripMenuItem.Text = "Uzaklaştır";
			// 
			// varsayilanToolStripMenuItem
			// 
			this.varsayilanToolStripMenuItem.Name = "varsayilanToolStripMenuItem";
			this.varsayilanToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
			this.varsayilanToolStripMenuItem.Text = "Varsayılan Yakınlaştırma";
			// 
			// durumcubuguToolStripMenuItem
			// 
			this.durumcubuguToolStripMenuItem.Checked = true;
			this.durumcubuguToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.durumcubuguToolStripMenuItem.Name = "durumcubuguToolStripMenuItem";
			this.durumcubuguToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
			this.durumcubuguToolStripMenuItem.Text = "Durum Çubuğu";
			this.durumcubuguToolStripMenuItem.Click += new System.EventHandler(this.durumcubuguToolStripMenuItem_Click);
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
			// printDocument1
			// 
			this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3});
			this.statusStrip1.Location = new System.Drawing.Point(0, 428);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(784, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(43, 17);
			this.toolStripStatusLabel1.Text = "St , Stn";
			// 
			// toolStripStatusLabel3
			// 
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new System.Drawing.Size(50, 17);
			this.toolStripStatusLabel3.Text = "yüzdelik";
			// 
			// NotDefteri
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 450);
			this.Controls.Add(this.richTextBox);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "NotDefteri";
			this.Text = "Not Defteri";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NotDefteri_FormClosing);
			this.Load += new System.EventHandler(this.NotDefteri_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NotDefteri_KeyDown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
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
		private ToolStripMenuItem düzenToolStripMenuItem;
		private ToolStripMenuItem geriAlToolStripMenuItem;
		private ToolStripMenuItem kesToolStripMenuItem;
		private ToolStripMenuItem kopyalaToolStripMenuItem;
		private ToolStripMenuItem yapıştırToolStripMenuItem;
		private ToolStripMenuItem silToolStripMenuItem;
		private ToolStripMenuItem bulToolStripMenuItem;
		private ToolStripMenuItem degistirToolStripMenuItem;
		private ToolStripMenuItem tumunuSecToolStripMenuItem;
		private ToolStripMenuItem saatTarihToolStripMenuItem;
		private ToolStripMenuItem ileriAlToolStripMenuItem;
		private ToolStripMenuItem yazdırToolStripMenuItem;
		private System.Drawing.Printing.PrintDocument printDocument1;
		private ToolStripMenuItem görünümToolStripMenuItem;
		private ToolStripMenuItem yakınlaştırToolStripMenuItem;
		private ToolStripMenuItem yakınlastirToolStripMenuItem1;
		private ToolStripMenuItem uzaklastirToolStripMenuItem;
		private ToolStripMenuItem varsayilanToolStripMenuItem;
		private ToolStripMenuItem durumcubuguToolStripMenuItem;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripStatusLabel toolStripStatusLabel2;
		private ToolStripStatusLabel toolStripStatusLabel3;
	}
}