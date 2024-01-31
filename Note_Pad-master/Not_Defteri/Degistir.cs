using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
    public partial class Degistir : Form
    {
        public RichTextBox TextBoxReferans { get; set; }
        public Degistir()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Degistir_FormClosing);
            sonrakiniBulButton.Enabled = false;

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Degistir_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetHighlight(); // Form kapatılırken renklendirmeleri sıfırla.
        }

        private void sonrakiniBulButton_Click(object sender, EventArgs e)
        {
            string arananMetin = arananTextBox.Text;
            bool buyukKucukHarfEslestir = buyukKucukHarfCheckBox.Checked;
            RichTextBoxFinds options = buyukKucukHarfEslestir ? RichTextBoxFinds.MatchCase : RichTextBoxFinds.None;
            options |= yukariRadioButton.Checked ? RichTextBoxFinds.Reverse : RichTextBoxFinds.None;

            if (TextBoxReferans != null && !string.IsNullOrEmpty(arananMetin))
            {
                int startIndex, endIndex;
                if (yukariRadioButton.Checked)
                {
                    endIndex = TextBoxReferans.SelectionStart;
                    startIndex = 0;
                }
                else
                {
                    startIndex = TextBoxReferans.SelectionStart + TextBoxReferans.SelectionLength;
                    endIndex = TextBoxReferans.Text.Length;
                }

                int foundIndex = TextBoxReferans.Find(arananMetin, startIndex, endIndex, options);

                if (foundIndex != -1)
                {
                    ResetHighlight(); // Önceki aramalardaki renklendirmeyi sıfırla
                    TextBoxReferans.Select(foundIndex, arananMetin.Length);
                    TextBoxReferans.SelectionBackColor = Color.BlueViolet; // Seçili metnin arka plan rengini sarı yapar.
                    TextBoxReferans.SelectionColor = Color.White; // Seçili metnin rengini kırmızı yapar.
                    TextBoxReferans.ScrollToCaret();

                    if (yukariRadioButton.Checked)
                    {
                        TextBoxReferans.SelectionStart = foundIndex;
                    }
                    else
                    {
                        TextBoxReferans.SelectionStart = foundIndex + arananMetin.Length;
                    }
                }
                else
                {
                    MessageBox.Show("Metin bulunamadı.", "Bul", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ResetHighlight()
        {
            if (TextBoxReferans != null)
            {
                TextBoxReferans.SelectAll();
                TextBoxReferans.SelectionBackColor = TextBoxReferans.BackColor;
                TextBoxReferans.SelectionColor = TextBoxReferans.ForeColor;
                TextBoxReferans.DeselectAll();
            }
        }

        private void arananTextBox_TextChanged(object sender, EventArgs e)
        {
            sonrakiniBulButton.Enabled = !string.IsNullOrEmpty(arananTextBox.Text);
        }

        private void degistirButton_Click(object sender, EventArgs e)
        {
            Degistirr();
        }

        private void tumunuDegistirButton_Click(object sender, EventArgs e)
        {
            TumunuDegistir();
        }

        // Tek bir eşleşmeyi değiştiren metod
        private void Degistirr()
        {// Aranan metin ve yeni metin alınıyor.
            string yeniMetin = yeniDegerTextBox.Text;

            // Seçili metnin başlangıç konumu ve uzunluğunu al
            int selectionStart = TextBoxReferans.SelectionStart;
            int selectionLength = TextBoxReferans.SelectionLength;

            // Seçili metni, yeni metinle değiştir
            TextBoxReferans.Select(selectionStart, selectionLength);
            TextBoxReferans.SelectedText = yeniMetin;

            // Yeni değiştirilen metni seç
            TextBoxReferans.Select(selectionStart, yeniMetin.Length);
        }

       

        // Tüm eşleşmeleri değiştiren metod
        private void TumunuDegistir()
        {
            // RichTextBox içindeki tüm aranan metin örneklerini değiştirir
            string arananMetin = arananTextBox.Text;
            string yeniMetin = yeniDegerTextBox.Text;
            RichTextBoxFinds options = GetOptions();

            // Arama ve değiştirme işlemleri için geçici olarak olayları devre dışı bırak
            TextBoxReferans.TextChanged -= new EventHandler(arananTextBox_TextChanged);

            if (!string.IsNullOrEmpty(arananMetin))
            {
                int index = 0;
                while ((index = TextBoxReferans.Find(arananMetin, index, options)) != -1)
                {
                    TextBoxReferans.Select(index, arananMetin.Length);
                    TextBoxReferans.SelectedText = yeniMetin;
                    index += yeniMetin.Length; // Değiştirilen metnin sonundan devam et
                }
            }

            // Olay işleyicilerini tekrar bağla
            TextBoxReferans.TextChanged += new EventHandler(arananTextBox_TextChanged);

        }

        private RichTextBoxFinds GetOptions()
        {
            RichTextBoxFinds options = RichTextBoxFinds.None;
            if (buyukKucukHarfCheckBox.Checked)
            {
                options |= RichTextBoxFinds.MatchCase;
            }
            return options;
        }
    }
}
