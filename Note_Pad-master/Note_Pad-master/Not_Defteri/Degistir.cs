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
        public Action<Action> PreserveEditorState { get; set; }
        private int highlightedStart = -1;
        private int highlightedLength = 0;

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
                    HighlightMatch(foundIndex, arananMetin.Length);
                }
                else
                {
                    MessageBox.Show("Metin bulunamadı.", "Bul", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ResetHighlight()
        {
            RunWithoutDirty(() =>
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
            });
        }

        private void HighlightMatch(int start, int length)
        {
            RunWithoutDirty(() =>
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
            });
        }

        private void RunWithoutDirty(Action action)
        {
            if (PreserveEditorState != null)
            {
                PreserveEditorState(action);
            }
            else
            {
                action();
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
            string arananMetin = arananTextBox.Text;
            string yeniMetin = yeniDegerTextBox.Text;

            if (!string.IsNullOrEmpty(arananMetin) && TextBoxReferans.SelectionLength > 0)
            {
                string seciliMetin = TextBoxReferans.SelectedText;
                StringComparison comparison = buyukKucukHarfCheckBox.Checked ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

                if (seciliMetin.Equals(arananMetin, comparison))
                {
                    highlightedStart = -1;
                    highlightedLength = 0;
                    // Seçili metni, yeni metinle değiştir
                    TextBoxReferans.SelectedText = yeniMetin;
                }
                else
                {
                    MessageBox.Show("Seçili metin, aranan metinle eşleşmiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen değiştirilecek metni seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       

        // Tüm eşleşmeleri değiştiren metod
        private void TumunuDegistir()
        {
            // RichTextBox içindeki tüm aranan metin örneklerini değiştirir
            string arananMetin = arananTextBox.Text;
            string yeniMetin = yeniDegerTextBox.Text;
            RichTextBoxFinds options = GetOptions();

            if (TextBoxReferans == null || string.IsNullOrEmpty(arananMetin))
            {
                return;
            }

            TextBoxReferans.SuspendLayout();
            try
            {
                int index = 0;
                while ((index = TextBoxReferans.Find(arananMetin, index, options)) != -1)
                {
                    TextBoxReferans.Select(index, arananMetin.Length);
                    TextBoxReferans.SelectedText = yeniMetin;
                    index += yeniMetin.Length; // Değiştirilen metnin sonundan devam et
                }
            }
            finally
            {
                TextBoxReferans.ResumeLayout();
            }

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
