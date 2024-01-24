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
            this.txtboxYaziTipi = new System.Windows.Forms.ComboBox();
            this.txtboxYaziTipiStili = new System.Windows.Forms.ComboBox();
            this.txtboxYaziTipiBoyutu = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtboxsecilenYaziTipi
            // 
            this.txtboxsecilenYaziTipi.Location = new System.Drawing.Point(12, 31);
            this.txtboxsecilenYaziTipi.Name = "txtboxsecilenYaziTipi";
            this.txtboxsecilenYaziTipi.Size = new System.Drawing.Size(140, 20);
            this.txtboxsecilenYaziTipi.TabIndex = 0;
            // 
            // txtboxsecilenYaziTipiStili
            // 
            this.txtboxsecilenYaziTipiStili.Location = new System.Drawing.Point(168, 31);
            this.txtboxsecilenYaziTipiStili.Name = "txtboxsecilenYaziTipiStili";
            this.txtboxsecilenYaziTipiStili.Size = new System.Drawing.Size(111, 20);
            this.txtboxsecilenYaziTipiStili.TabIndex = 0;
            // 
            // txtboxsecilenYaziTipiBoyutu
            // 
            this.txtboxsecilenYaziTipiBoyutu.Location = new System.Drawing.Point(294, 31);
            this.txtboxsecilenYaziTipiBoyutu.Name = "txtboxsecilenYaziTipiBoyutu";
            this.txtboxsecilenYaziTipiBoyutu.Size = new System.Drawing.Size(49, 20);
            this.txtboxsecilenYaziTipiBoyutu.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblOrnek);
            this.groupBox1.Location = new System.Drawing.Point(168, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 81);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Örnek";
            // 
            // lblOrnek
            // 
            this.lblOrnek.AutoSize = true;
            this.lblOrnek.Location = new System.Drawing.Point(67, 38);
            this.lblOrnek.Name = "lblOrnek";
            this.lblOrnek.Size = new System.Drawing.Size(35, 13);
            this.lblOrnek.TabIndex = 0;
            this.lblOrnek.Text = "label1";
            // 
            // btnTamam
            // 
            this.btnTamam.Location = new System.Drawing.Point(187, 284);
            this.btnTamam.Name = "btnTamam";
            this.btnTamam.Size = new System.Drawing.Size(75, 23);
            this.btnTamam.TabIndex = 4;
            this.btnTamam.Text = "Tamam";
            this.btnTamam.UseVisualStyleBackColor = true;
            this.btnTamam.Click += new System.EventHandler(this.btnTamam_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(268, 284);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(75, 23);
            this.btnIptal.TabIndex = 4;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // txtboxYaziTipi
            // 
            this.txtboxYaziTipi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtboxYaziTipi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtboxYaziTipi.FormattingEnabled = true;
            this.txtboxYaziTipi.Location = new System.Drawing.Point(12, 51);
            this.txtboxYaziTipi.Name = "txtboxYaziTipi";
            this.txtboxYaziTipi.Size = new System.Drawing.Size(140, 21);
            this.txtboxYaziTipi.TabIndex = 5;
            this.txtboxYaziTipi.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.txtboxYaziTipi_DrawItem);
            this.txtboxYaziTipi.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipi_SelectedIndexChanged);
            // 
            // txtboxYaziTipiStili
            // 
            this.txtboxYaziTipiStili.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtboxYaziTipiStili.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtboxYaziTipiStili.FormattingEnabled = true;
            this.txtboxYaziTipiStili.Location = new System.Drawing.Point(168, 51);
            this.txtboxYaziTipiStili.Name = "txtboxYaziTipiStili";
            this.txtboxYaziTipiStili.Size = new System.Drawing.Size(111, 21);
            this.txtboxYaziTipiStili.TabIndex = 5;
            this.txtboxYaziTipiStili.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.txtboxYaziTipiStili_DrawItem);
            this.txtboxYaziTipiStili.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipiStili_SelectedIndexChanged);
            // 
            // txtboxYaziTipiBoyutu
            // 
            this.txtboxYaziTipiBoyutu.FormattingEnabled = true;
            this.txtboxYaziTipiBoyutu.Location = new System.Drawing.Point(294, 51);
            this.txtboxYaziTipiBoyutu.Name = "txtboxYaziTipiBoyutu";
            this.txtboxYaziTipiBoyutu.Size = new System.Drawing.Size(49, 21);
            this.txtboxYaziTipiBoyutu.TabIndex = 5;
            this.txtboxYaziTipiBoyutu.SelectedIndexChanged += new System.EventHandler(this.txtboxYaziTipiBoyutu_SelectedIndexChanged);
            // 
            // YaziTipi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 319);
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
            this.Text = "YaziTipi";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.ComboBox txtboxYaziTipi;
        private System.Windows.Forms.ComboBox txtboxYaziTipiStili;
        private System.Windows.Forms.ComboBox txtboxYaziTipiBoyutu;
    }
}