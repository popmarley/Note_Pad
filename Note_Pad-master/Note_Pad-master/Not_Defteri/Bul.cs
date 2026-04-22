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
	public partial class Bul : Form
	{

		public RichTextBox TextBoxReferans { get; set; }
        private Degistir degistirForm = null;
        private int highlightedStart = -1;
        private int highlightedLength = 0;
      
        public Bul()
		{
			InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Bul_FormClosing);
            sonrakiniBulButton.Enabled = false;

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
                    HighlightMatch(foundIndex, arananMetin.Length);
                }
                else
                {
                    MessageBox.Show("Metin bulunamadı.", "Bul", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void ResetHighlight()
        {
            if (TextBoxReferans == null || highlightedStart < 0)
            {
                return;
            }

            bool wasModified = TextBoxReferans.Modified;
            int selectionStart = TextBoxReferans.SelectionStart;
            int selectionLength = TextBoxReferans.SelectionLength;

            if (highlightedStart < TextBoxReferans.TextLength)
            {
                int safeLength = Math.Min(highlightedLength, TextBoxReferans.TextLength - highlightedStart);
                TextBoxReferans.Select(highlightedStart, safeLength);
                TextBoxReferans.SelectionBackColor = TextBoxReferans.BackColor;
                TextBoxReferans.SelectionColor = TextBoxReferans.ForeColor;
            }

            TextBoxReferans.Select(
                Math.Min(selectionStart, TextBoxReferans.TextLength),
                Math.Min(selectionLength, Math.Max(0, TextBoxReferans.TextLength - selectionStart)));
            TextBoxReferans.Modified = wasModified;
            highlightedStart = -1;
            highlightedLength = 0;
        }

        private void HighlightMatch(int start, int length)
        {
            ResetHighlight();

            bool wasModified = TextBoxReferans.Modified;
            TextBoxReferans.Select(start, length);
            TextBoxReferans.SelectionBackColor = Color.BlueViolet;
            TextBoxReferans.SelectionColor = Color.White;
            TextBoxReferans.ScrollToCaret();
            TextBoxReferans.Select(start, length);
            TextBoxReferans.Modified = wasModified;

            highlightedStart = start;
            highlightedLength = length;
        }

        private void Bul_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetHighlight(); // Form kapatılırken renklendirmeleri sıfırla.
        }

        private void arananTextBox_TextChanged(object sender, EventArgs e)
        {
            sonrakiniBulButton.Enabled = !string.IsNullOrEmpty(arananTextBox.Text);
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            
            if (degistirForm == null || degistirForm.IsDisposed)
            {
                degistirForm = new Degistir();
                degistirForm.TextBoxReferans = this.TextBoxReferans; // richTextBox referansını geçir
            }

            degistirForm.Show();
           this.Close();
            
        }
    }
}
