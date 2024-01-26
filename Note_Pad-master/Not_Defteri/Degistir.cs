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
            TextBoxReferans.SelectAll();
            TextBoxReferans.SelectionBackColor = TextBoxReferans.BackColor;
            TextBoxReferans.SelectionColor = TextBoxReferans.ForeColor;
            TextBoxReferans.DeselectAll();
        }

        private void arananTextBox_TextChanged(object sender, EventArgs e)
        {
            sonrakiniBulButton.Enabled = !string.IsNullOrEmpty(arananTextBox.Text);
        }
    }
}
