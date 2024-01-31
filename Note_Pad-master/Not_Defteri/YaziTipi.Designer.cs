namespace Not_Defteri
{
    partial class YaziTipi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YaziTipi));
            this.txtboxsecilenYaziTipi = new System.Windows.Forms.TextBox();
            this.txtboxsecilenYaziTipiStili = new System.Windows.Forms.TextBox();
            this.txtboxsecilenYaziTipiBoyutu = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOrnek = new System.Windows.Forms.Label();
            this.btnTamam = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.txtboxYaziTipi = new System.Windows.Forms.ListBox();
            this.txtboxYaziTipiStili = new System.Windows.Forms.ListBox();
            this.txtboxYaziTipiBoyutu = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtboxsecilenYaziTipi
            // 
            this.txtboxsecilenYaziTipi.Location = new System.Drawing.Point(12, 43);
            this.txtboxsecilenYaziTipi.Name = "txtboxsecilenYaziTipi";
            this.txtboxsecilenYaziTipi.Size = new System.Drawing.Size(160, 20);
            this.txtboxsecilenYaziTipi.TabIndex = 7;
           
            // 
            // txtboxsecilenYaziTipiStili
            // 
            this.txtboxsecilenYaziTipiStili.Location = new System.Drawing.Point(197, 43);
            this.txtboxsecilenYaziTipiStili.Name = "txtboxsecilenYaziTipiStili";
            this.txtboxsecilenYaziTipiStili.Size = new System.Drawing.Size(130, 20);
            this.txtboxsecilenYaziTipiStili.TabIndex = 8;
            // 
            // txtboxsecilenYaziTipiBoyutu
            // 
            this.txtboxsecilenYaziTipiBoyutu.Location = new System.Drawing.Point(342, 43);
            this.txtboxsecilenYaziTipiBoyutu.Name = "txtboxsecilenYaziTipiBoyutu";
            this.txtboxsecilenYaziTipiBoyutu.Size = new System.Drawing.Size(62, 20);
            this.txtboxsecilenYaziTipiBoyutu.TabIndex = 9;
           
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblOrnek);
            this.groupBox1.Location = new System.Drawing.Point(216, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 99);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Örnek";
            // 
            // lblOrnek
            // 
            this.lblOrnek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrnek.Location = new System.Drawing.Point(3, 16);
            this.lblOrnek.Name = "lblOrnek";
            this.lblOrnek.Size = new System.Drawing.Size(195, 80);
            this.lblOrnek.TabIndex = 0;
            this.lblOrnek.Text = "AaBbYyZz";
            this.lblOrnek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTamam
            // 
            this.btnTamam.Location = new System.Drawing.Point(261, 298);
            this.btnTamam.Name = "btnTamam";
            this.btnTamam.Size = new System.Drawing.Size(75, 23);
            this.btnTamam.TabIndex = 4;
            this.btnTamam.Text = "Tamam";
            this.btnTamam.UseVisualStyleBackColor = true;
            this.btnTamam.Click += new System.EventHandler(this.btnTamam_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(342, 298);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(75, 23);
            this.btnIptal.TabIndex = 5;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // txtboxYaziTipi
            // 
            this.txtboxYaziTipi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtboxYaziTipi.FormattingEnabled = true;
            this.txtboxYaziTipi.Location = new System.Drawing.Point(12, 63);
            this.txtboxYaziTipi.Name = "txtboxYaziTipi";
            this.txtboxYaziTipi.Size = new System.Drawing.Size(160, 95);
            this.txtboxYaziTipi.TabIndex = 1;
            this.txtboxYaziTipi.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.txtboxYaziTipi_DrawItem);
            this.txtboxYaziTipi.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipi_SelectedIndexChanged);
            // 
            // txtboxYaziTipiStili
            // 
            this.txtboxYaziTipiStili.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtboxYaziTipiStili.FormattingEnabled = true;
            this.txtboxYaziTipiStili.Location = new System.Drawing.Point(197, 63);
            this.txtboxYaziTipiStili.Name = "txtboxYaziTipiStili";
            this.txtboxYaziTipiStili.Size = new System.Drawing.Size(130, 95);
            this.txtboxYaziTipiStili.TabIndex = 2;
            this.txtboxYaziTipiStili.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.txtboxYaziTipiStili_DrawItem);
            this.txtboxYaziTipiStili.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipiStili_SelectedIndexChanged);
            // 
            // txtboxYaziTipiBoyutu
            // 
            this.txtboxYaziTipiBoyutu.FormattingEnabled = true;
            this.txtboxYaziTipiBoyutu.Location = new System.Drawing.Point(342, 63);
            this.txtboxYaziTipiBoyutu.Name = "txtboxYaziTipiBoyutu";
            this.txtboxYaziTipiBoyutu.Size = new System.Drawing.Size(62, 82);
            this.txtboxYaziTipiBoyutu.TabIndex = 3;
            this.txtboxYaziTipiBoyutu.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipiBoyutu_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Yazı Tipi:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Yazı Tipi Stili:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Boyut:";
            // 
            // YaziTipi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 333);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtboxYaziTipiBoyutu);
            this.Controls.Add(this.txtboxYaziTipiStili);
            this.Controls.Add(this.txtboxYaziTipi);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnTamam);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtboxsecilenYaziTipiBoyutu);
            this.Controls.Add(this.txtboxsecilenYaziTipiStili);
            this.Controls.Add(this.txtboxsecilenYaziTipi);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(55, 55);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YaziTipi";
            this.ShowIcon = false;
            this.Text = "YaziTipi";
            
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtboxsecilenYaziTipi;
        private System.Windows.Forms.TextBox txtboxsecilenYaziTipiStili;
        private System.Windows.Forms.TextBox txtboxsecilenYaziTipiBoyutu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblOrnek;
        private System.Windows.Forms.Button btnTamam;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.ListBox txtboxYaziTipi;
        private System.Windows.Forms.ListBox txtboxYaziTipiStili;
        private System.Windows.Forms.ListBox txtboxYaziTipiBoyutu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}