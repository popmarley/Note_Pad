using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
    public partial class HesapMakinasi : Form
    {
        private TextBox txtSonuc;
        private Button btnTopla, btnCikar, btnCarp, btnBol;
        private NumericUpDown num1, num2;

        public HesapMakinasi()
        {
            InitializeComponent();
            InitializeCalculatorComponents();
        }

        private void InitializeCalculatorComponents()
        {
            // Sayı girişleri için NumericUpDown kontrolleri
            num1 = new NumericUpDown { Location = new System.Drawing.Point(10, 10), Width = 100 };
            num2 = new NumericUpDown { Location = new System.Drawing.Point(120, 10), Width = 100 };

            // İşlem butonları
            btnTopla = new Button { Text = "+", Location = new System.Drawing.Point(10, 40), Width = 50 };
            btnCikar = new Button { Text = "-", Location = new System.Drawing.Point(70, 40), Width = 50 };
            btnCarp = new Button { Text = "*", Location = new System.Drawing.Point(130, 40), Width = 50 };
            btnBol = new Button { Text = "/", Location = new System.Drawing.Point(190, 40), Width = 50 };

            // Sonuç için TextBox
            txtSonuc = new TextBox { Location = new System.Drawing.Point(10, 70), Width = 200, ReadOnly = true };

            // Event Handler'ların eklenmesi
            btnTopla.Click += new EventHandler(IslemYap);
            btnCikar.Click += new EventHandler(IslemYap);
            btnCarp.Click += new EventHandler(IslemYap);
            btnBol.Click += new EventHandler(IslemYap);

            // Kontrollerin forma eklenmesi
            this.Controls.Add(num1);
            this.Controls.Add(num2);
            this.Controls.Add(btnTopla);
            this.Controls.Add(btnCikar);
            this.Controls.Add(btnCarp);
            this.Controls.Add(btnBol);
            this.Controls.Add(txtSonuc);
        }

        private void IslemYap(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var n1 = num1.Value;
            var n2 = num2.Value;
            decimal sonuc = 0;

            switch (btn.Text)
            {
                case "+":
                    sonuc = n1 + n2;
                    break;
                case "-":
                    sonuc = n1 - n2;
                    break;
                case "*":
                    sonuc = n1 * n2;
                    break;
                case "/":
                    if (n2 != 0) sonuc = n1 / n2;
                    else MessageBox.Show("0'a bölünemez!");
                    break;
            }

            txtSonuc.Text = sonuc.ToString();
        }
    }
}
